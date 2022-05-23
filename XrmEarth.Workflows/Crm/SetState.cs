using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class SetState : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var stateCode = StateCode.Get<int>(activityHelper.CodeActivityContext);
            var statusCode = StatusCode.Get<int>(activityHelper.CodeActivityContext);
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);

            WorkflowHelper.SetState(activityHelper.OrganizationService, stateCode, statusCode, recordUrl);
        }

        [RequiredArgument]
        [Input("State Code")]
        public InArgument<int> StateCode { get; set; }

        [RequiredArgument]
        [Input("Status Code")]
        public InArgument<int> StatusCode { get; set; }

        [RequiredArgument]
        [Input("Record Url")]
        public InArgument<string> RecordUrl { get; set; }
    }
}
