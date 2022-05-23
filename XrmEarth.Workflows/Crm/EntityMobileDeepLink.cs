using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class EntityMobileDeepLink : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);
            var recordUrlEntity = RecordUrlHelper.GetEntityReference(recordUrl, activityHelper.OrganizationService);

            string recordUrlEdit = string.Format("ms-dynamicsxrm://?pagetype=entity&etn={0}&id={1}", recordUrlEntity.LogicalName, recordUrlEntity.Id);
            string recordUrlNew = string.Format("ms-dynamicsxrm://?pagetype=create&etn={0}", recordUrlEntity.LogicalName);
            string recordUrlDefaultView = string.Format("ms-dynamicsxrm://?pagetype=view&etn={0}", recordUrlEntity.LogicalName);

            MobileDeepLinkEdit.Set(activityHelper.CodeActivityContext, recordUrlEdit);
            MobileDeepLinkNew.Set(activityHelper.CodeActivityContext, recordUrlNew);
            MobileDeepLinkDefaultView.Set(activityHelper.CodeActivityContext, recordUrlDefaultView);
        }

        [RequiredArgument]
        [Input("Record URL")]
        public InArgument<string> RecordUrl { get; set; }

        [Output("Mobile Deep Link Edit")]
        public OutArgument<string> MobileDeepLinkEdit { get; set; }

        [Output("Mobile Deep Link New")]
        public OutArgument<string> MobileDeepLinkNew { get; set; }

        [Output("Mobile Deep Link Default View")]
        public OutArgument<string> MobileDeepLinkDefaultView { get; set; }
    }
}
