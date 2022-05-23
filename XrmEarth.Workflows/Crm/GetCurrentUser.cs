using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class GetCurrentUser : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            CurrentUser.Set(activityHelper.CodeActivityContext, new EntityReference("systemuser", activityHelper.Context.InitiatingUserId));
        }

        [Output("CurrentUser")]
        [ReferenceTarget(EntityNames.SystemUser)]
        public OutArgument<EntityReference> CurrentUser { get; set; }
    }
}
