using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class GetReferenceByRecordUrl : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);

            EntityReference reference = RecordUrlHelper.GetEntityReference(recordUrl, activityHelper.OrganizationService);

            RecordId.Set(activityHelper.CodeActivityContext, reference.Id.ToString());
            RecordLogicalName.Set(activityHelper.CodeActivityContext, reference.LogicalName);
        }

        [RequiredArgument]
        [Input("Record URL")]
        [Default("")]
        public InArgument<string> RecordUrl { get; set; }

        [Output("Record Id")]
        public OutArgument<string> RecordId { get; set; }

        [Output("Record LogicalName")]
        public OutArgument<string> RecordLogicalName { get; set; }
    }
}
