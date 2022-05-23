using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class CheckTeamInRole : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var team = Team.Get<EntityReference>(activityHelper.CodeActivityContext);
            var roleName = RoleName.Get<string>(activityHelper.CodeActivityContext);

            var result = WorkflowHelper.CheckTeamInRole(activityHelper.OrganizationService, team.Id, roleName);

            Result.Set(activityHelper.CodeActivityContext, result);
        }

        [Input("Team")]
        [ReferenceTarget(EntityNames.Team)]
        public InArgument<EntityReference> Team { get; set; }

        [Input("Role Name")]
        public InArgument<string> RoleName { get; set; }

        [Output("Result")]
        public OutArgument<bool> Result { get; set; }
    }
}
