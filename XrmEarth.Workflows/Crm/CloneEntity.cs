using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class CloneEntity : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);

            WorkflowHelper.CloneEntity(activityHelper.OrganizationService, recordUrl);
        }

        [RequiredArgument]
        [Input("Record Url")]
        public InArgument<string> RecordUrl { get; set; }
    }
}
