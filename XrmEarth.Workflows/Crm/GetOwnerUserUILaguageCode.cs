using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class GetOwnerUserUILaguageCode : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            UserUILanguageCode.Set(activityHelper.CodeActivityContext, WorkflowHelper.RetrieveUserUILanguageCode(activityHelper.OrganizationService, activityHelper.Context.UserId));
        }

        [Output("OwnerUserUILanguageCode")]
        public OutArgument<int> UserUILanguageCode { get; set; }

    }
}
