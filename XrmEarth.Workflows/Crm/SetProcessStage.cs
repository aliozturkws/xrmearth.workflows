using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class SetProcessStage : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var recordUrl = RecordUrl.Get(activityHelper.CodeActivityContext);
            var process = Process.Get(activityHelper.CodeActivityContext);
            var processStage = ProcessStage.Get(activityHelper.CodeActivityContext);

            WorkflowHelper.SetProcessStage(activityHelper.OrganizationService, process, recordUrl, processStage);
        }

        [RequiredArgument]
        [Input("Record Url")]
        public InArgument<string> RecordUrl { get; set; }

        [RequiredArgument]
        [Input("Process")]
        [ReferenceTarget("workflow")]
        public InArgument<EntityReference> Process { get; set; }

        [RequiredArgument]
        [Input("Process Stage Name")]
        public InArgument<string> ProcessStage { get; set; }
    }
}
