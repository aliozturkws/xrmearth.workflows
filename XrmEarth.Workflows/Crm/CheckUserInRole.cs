using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class CheckUserInRole : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var systemUser = SystemUser.Get<EntityReference>(activityHelper.CodeActivityContext);
            var roleName = RoleName.Get<string>(activityHelper.CodeActivityContext);
            var isCurrentUser = IsCurrentUser.Get<bool>(activityHelper.CodeActivityContext);

            if (systemUser == null && !isCurrentUser)
            {
                throw new InvalidPluginExecutionException("At least one of the User or CurrentUser fields must be full!");
            }

            bool result;

            if (isCurrentUser)
                result = WorkflowHelper.CheckUserInRole(activityHelper.OrganizationService, activityHelper.Context.InitiatingUserId, roleName);
            else
                result = WorkflowHelper.CheckUserInRole(activityHelper.OrganizationService, systemUser.Id, roleName);

            Result.Set(activityHelper.CodeActivityContext, result);
        }

        [RequiredArgument]
        [Input("Is Current User")]
        public InArgument<bool> IsCurrentUser { get; set; }

        [Input("System User")]
        [ReferenceTarget(EntityNames.SystemUser)]
        public InArgument<EntityReference> SystemUser { get; set; }

        [RequiredArgument]
        [Input("Role Name")]
        public InArgument<string> RoleName { get; set; }

        [Output("Result")]
        public OutArgument<bool> Result { get; set; }
    }
}
