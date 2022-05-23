using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class QueryValues : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string entityName = EntityName.Get(activityHelper.CodeActivityContext);
            string attribute1 = Attribute1.Get(activityHelper.CodeActivityContext);
            string attribute2 = Attribute2.Get(activityHelper.CodeActivityContext);
            string filterAttribute1 = FilterAttribute1.Get(activityHelper.CodeActivityContext);
            string filterAttribute2 = FilterAttribute2.Get(activityHelper.CodeActivityContext);
            string valueAttribute1 = ValueAttribute1.Get(activityHelper.CodeActivityContext);
            string valueAttribute2 = ValueAttribute2.Get(activityHelper.CodeActivityContext);

            var qe = new QueryExpression();
            qe.EntityName = entityName;
            qe.ColumnSet = new ColumnSet();
            if (!string.IsNullOrEmpty(attribute1)) qe.ColumnSet.Columns.Add(attribute1);
            if (!string.IsNullOrEmpty(attribute2)) qe.ColumnSet.Columns.Add(attribute2);

            var filter = new FilterExpression(LogicalOperator.And);
            if (!string.IsNullOrEmpty(filterAttribute1))
            {
                var condition1 = new ConditionExpression();
                condition1.AttributeName = filterAttribute1;
                condition1.Values.Add(valueAttribute1);
                condition1.Operator = ConditionOperator.Equal;
                filter.Conditions.Add(condition1);
            }
            if (!string.IsNullOrEmpty(filterAttribute2))
            {
                var condition2 = new ConditionExpression();
                condition2.AttributeName = filterAttribute2;
                condition2.Values.Add(valueAttribute2);
                condition2.Operator = ConditionOperator.Equal;
                filter.Conditions.Add(condition2);
            }
            qe.Criteria = filter;

            var results = activityHelper.OrganizationService.RetrieveMultiple(qe);

            if (results.Entities.Count > 0)
            {
                if (results.Entities[0].Attributes[attribute1] != null)
                {
                    ResultValue1.Set(activityHelper.CodeActivityContext, results.Entities[0].Attributes[attribute1].ToString());
                }
                if (results.Entities[0].Attributes[attribute2] != null)
                {
                    ResultValue2.Set(activityHelper.CodeActivityContext, results.Entities[0].Attributes[attribute2].ToString());
                }
            }
        }

        [RequiredArgument]
        [Input("EntityName")]
        [Default("")]
        public InArgument<string> EntityName { get; set; }

        [RequiredArgument]
        [Input("Attribute1")]
        [ReferenceTarget("")]
        public InArgument<string> Attribute1 { get; set; }

        [RequiredArgument]
        [Input("Attribute2")]
        [ReferenceTarget("")]
        public InArgument<string> Attribute2 { get; set; }

        [RequiredArgument]
        [Input("FilterAttibute1")]
        [ReferenceTarget("")]
        public InArgument<string> FilterAttribute1 { get; set; }

        [RequiredArgument]
        [Input("ValueAttribute1")]
        [ReferenceTarget("")]
        public InArgument<string> ValueAttribute1 { get; set; }

        [Input("FilterAttribute2")]
        [ReferenceTarget("")]
        public InArgument<string> FilterAttribute2 { get; set; }

        [Input("ValueAttribute2")]
        [ReferenceTarget("")]
        public InArgument<string> ValueAttribute2 { get; set; }

        [Output("ResultValue1")]
        public OutArgument<string> ResultValue1 { get; set; }

        [Output("ResultValue2")]
        public OutArgument<string> ResultValue2 { get; set; }
    }
}
