using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class AddRoleToTeam : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var team = Team.Get<EntityReference>(activityHelper.CodeActivityContext);
            var roleName = RoleName.Get<string>(activityHelper.CodeActivityContext);

            WorkflowHelper.AddRoleToUser(activityHelper.OrganizationService, team.Id, roleName);
        }

        [RequiredArgument]
        [Input("Team")]
        [ReferenceTarget(EntityNames.Team)]
        public InArgument<EntityReference> Team { get; set; }

        [RequiredArgument]
        [Input("Role Name")]
        public InArgument<string> RoleName { get; set; }
    }
}