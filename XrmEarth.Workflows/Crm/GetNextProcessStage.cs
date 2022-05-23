using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class GetNextProcessStage : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var process = Process.Get(activityHelper.CodeActivityContext);
            var processStage = ProcessStage.Get(activityHelper.CodeActivityContext);

            var nextProcessStage = WorkflowHelper.GetNextProcessStage(activityHelper.OrganizationService, process, processStage);

            NextProcessStage.Set(activityHelper.CodeActivityContext, nextProcessStage);
        }

        [RequiredArgument]
        [Input("Process")]
        [ReferenceTarget("workflow")]
        public InArgument<EntityReference> Process { get; set; }

        [RequiredArgument]
        [Input("Process Stage Name")]
        public InArgument<string> ProcessStage { get; set; }

        [Output("Next Process Stage Name")]
        public OutArgument<string> NextProcessStage { get; set; }
    }
}
