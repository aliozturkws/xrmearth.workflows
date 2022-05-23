using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class RemoveUserFromTeam : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var systemUser = SystemUser.Get<EntityReference>(activityHelper.CodeActivityContext);
            var team = Team.Get<EntityReference>(activityHelper.CodeActivityContext);

            WorkflowHelper.RemoveUserFromTeam(activityHelper.OrganizationService, team.Id, systemUser.Id);
        }

        [RequiredArgument]
        [Input("System User")]
        [ReferenceTarget(EntityNames.SystemUser)]
        public InArgument<EntityReference> SystemUser { get; set; }

        [RequiredArgument]
        [Input("Team")]
        [ReferenceTarget(EntityNames.Team)]
        public InArgument<EntityReference> Team { get; set; }
    }
}
