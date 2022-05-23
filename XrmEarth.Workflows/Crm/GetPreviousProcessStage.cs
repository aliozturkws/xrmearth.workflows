using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class GetPreviousProcessStage : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var process = Process.Get(activityHelper.CodeActivityContext);
            var processStage = ProcessStage.Get(activityHelper.CodeActivityContext);

            var previousProcessStage = WorkflowHelper.GetPreviousProcessStage(activityHelper.OrganizationService, process, processStage);

            PreviousProcessStage.Set(activityHelper.CodeActivityContext, previousProcessStage);
        }

        [RequiredArgument]
        [Input("Process")]
        [ReferenceTarget("workflow")]
        public InArgument<EntityReference> Process { get; set; }

        [RequiredArgument]
        [Input("Process Stage Name")]
        public InArgument<string> ProcessStage { get; set; }

        [Output("Previous Process Stage Name")]
        public OutArgument<string> PreviousProcessStage { get; set; }
    }
}
