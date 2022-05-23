using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class CheckUserInTeam : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var systemUser = SystemUser.Get<EntityReference>(activityHelper.CodeActivityContext);
            var team = Team.Get<EntityReference>(activityHelper.CodeActivityContext);
            var isCurrentUser = IsCurrentUser.Get<bool>(activityHelper.CodeActivityContext);

            if (systemUser == null && !isCurrentUser)
                throw new InvalidPluginExecutionException("At least one of the User or CurrentUser fields must be full!");

            bool result;

            if (isCurrentUser)
                result = WorkflowHelper.CheckUserInTeam(activityHelper.OrganizationService, activityHelper.Context.InitiatingUserId, team);
            else
                result = WorkflowHelper.CheckUserInTeam(activityHelper.OrganizationService, systemUser.Id, team);

            Result.Set(activityHelper.CodeActivityContext, result);
        }

        [RequiredArgument]
        [Input("Is Current User")]
        public InArgument<bool> IsCurrentUser { get; set; }

        [Input("System User")]
        [ReferenceTarget(EntityNames.SystemUser)]
        public InArgument<EntityReference> SystemUser { get; set; }

        [Input("Team")]
        [ReferenceTarget(EntityNames.Team)]
        public InArgument<EntityReference> Team { get; set; }

        [Output("Result")]
        public OutArgument<bool> Result { get; set; }
    }
}
