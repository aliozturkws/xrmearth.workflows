using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class RollupFunctions : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);
            var recordUrl2 = RecordUrl2.Get<string>(activityHelper.CodeActivityContext);
            var fetchXml = FetchXML.Get<string>(activityHelper.CodeActivityContext);

            var response = WorkflowHelper.RollupFunctions(activityHelper.OrganizationService, fetchXml, recordUrl, recordUrl2);

            Max.Set(activityHelper.CodeActivityContext, response.Max);
            Min.Set(activityHelper.CodeActivityContext, response.Min);
            Avg.Set(activityHelper.CodeActivityContext, response.Avg);
            Sum.Set(activityHelper.CodeActivityContext, response.Sum);
            Count.Set(activityHelper.CodeActivityContext, response.Count);
        }

        [RequiredArgument]
        [Input("RecordUrl")]
        public InArgument<string> RecordUrl { get; set; }

        [Input("RecordUrl2")]
        public InArgument<string> RecordUrl2 { get; set; }

        [RequiredArgument]
        [Input("FetchXML")]
        public InArgument<string> FetchXML { get; set; }

        [Output("Count")]
        public OutArgument<decimal> Count { get; set; }

        [Output("Sum")]
        public OutArgument<decimal> Sum { get; set; }

        [Output("Max")]
        public OutArgument<decimal> Max { get; set; }

        [Output("Min")]
        public OutArgument<decimal> Min { get; set; }

        [Output("Avg")]
        public OutArgument<decimal> Avg { get; set; }
    }
}

