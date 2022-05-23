using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class GetAppRecordUrl : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);
            string appModuleUniqueName = AppModuleUniqueName.Get<string>(activityHelper.CodeActivityContext);
            string appRecordUrl = WorkflowHelper.GetAppRecordUrl(activityHelper.OrganizationService, recordUrl, appModuleUniqueName);

            AppRecordUrl.Set(activityHelper.CodeActivityContext, RecordUrlHelper.ChangeEtcToEtnUrl(appRecordUrl, activityHelper.OrganizationService));
        }

        [RequiredArgument]
        [Input("Record URL")]
        [Default("")]
        public InArgument<string> RecordUrl { get; set; }

        [RequiredArgument]
        [Input("Application Unique Name")]
        [Default("")]
        public InArgument<string> AppModuleUniqueName { get; set; }

        [Output("Record URL for App Module")]
        public OutArgument<string> AppRecordUrl { get; set; }
    }
}
