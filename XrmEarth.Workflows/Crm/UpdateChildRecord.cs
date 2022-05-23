using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class UpdateChildRecord : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var relationEntityName = RelationEntityName.Get<string>(activityHelper.CodeActivityContext);
            var relationEntityFieldName = RelationEntityFieldName.Get<string>(activityHelper.CodeActivityContext);
            var relationEntityUpdatedFieldName = RelationEntityUpdatedFieldName.Get<string>(activityHelper.CodeActivityContext);
            var valueToSet = ValueToSet.Get<string>(activityHelper.CodeActivityContext);
            var valueToSetLogicalName = ValueToSetLogicalName.Get<string>(activityHelper.CodeActivityContext);
            var mapFieldName = MapFieldName.Get<string>(activityHelper.CodeActivityContext);

            var processCount = WorkflowHelper.UpdateChildRecord(activityHelper.OrganizationService,
                                              activityHelper.Context,
                                              relationEntityName,
                                              relationEntityFieldName,
                                              relationEntityUpdatedFieldName,
                                              valueToSet,
                                              valueToSetLogicalName,
                                              mapFieldName);

            ProcessCount.Set(activityHelper.CodeActivityContext, processCount);
        }

        [RequiredArgument]
        [Input("Relation Entity Name")]
        public InArgument<string> RelationEntityName { get; set; }

        [RequiredArgument]
        [Input("Relation Entity Field Name")]
        public InArgument<string> RelationEntityFieldName { get; set; }

        [RequiredArgument]
        [Input("Relation Entity Updated Field Name")]
        public InArgument<string> RelationEntityUpdatedFieldName { get; set; }

        [Input("Value To Set")]
        public InArgument<string> ValueToSet { get; set; }

        [Input("Value To Set LogicalName")]
        public InArgument<string> ValueToSetLogicalName { get; set; }

        [Input("Map Field Name")]
        public InArgument<string> MapFieldName { get; set; }

        [Output("ProcessCount")]
        public OutArgument<int> ProcessCount { get; set; }
    }
}
