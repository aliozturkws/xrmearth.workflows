using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class UpdateEntity : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);
            var entityName = EntityName.Get<string>(activityHelper.CodeActivityContext);
            var entityId = EntityId.Get<string>(activityHelper.CodeActivityContext);
            var updateFieldName = UpdateFieldName.Get<string>(activityHelper.CodeActivityContext);
            var valueToSet = ValueToSet.Get<string>(activityHelper.CodeActivityContext);
            var valueToSetLogicalName = ValueToSetLogicalName.Get<string>(activityHelper.CodeActivityContext);

            WorkflowHelper.UpdateEntity(activityHelper.OrganizationService,
                                        activityHelper.Context,
                                        recordUrl,
                                        entityName,
                                        entityId,
                                        updateFieldName,
                                        valueToSet,
                                        valueToSetLogicalName);
        }

        [Input("Record Url")]
        public InArgument<string> RecordUrl { get; set; }

        [Input("EntityName")]
        public InArgument<string> EntityName { get; set; }

        [Input("EntityId Id")]
        public InArgument<string> EntityId { get; set; }

        [RequiredArgument]
        [Input("Update Field Name")]
        public InArgument<string> UpdateFieldName { get; set; }

        [RequiredArgument]
        [Input("Value To Set")]
        public InArgument<string> ValueToSet { get; set; }

        [Input("Value To Set LogicalName")]
        public InArgument<string> ValueToSetLogicalName { get; set; }
    }
}
