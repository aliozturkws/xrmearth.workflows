using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class ExecuteWorkflow : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var workflow = Workflow.Get<EntityReference>(activityHelper.CodeActivityContext);
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);

            WorkflowHelper.ExecuteWorkflow(activityHelper.OrganizationService, workflow.Id, recordUrl);
        }

        [RequiredArgument]
        [Input("Workflow")]
        [ReferenceTarget(EntityNames.Workflow)]
        public InArgument<EntityReference> Workflow { get; set; }

        [RequiredArgument]
        [Input("Record Url")]
        public InArgument<string> RecordUrl { get; set; }
    }
}
