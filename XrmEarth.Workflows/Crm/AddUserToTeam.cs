using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class AddUserToTeam : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var systemUser = SystemUser.Get<EntityReference>(activityHelper.CodeActivityContext);
            var team = Team.Get<EntityReference>(activityHelper.CodeActivityContext);

            WorkflowHelper.AddUserToTeam(activityHelper.OrganizationService, team.Id, systemUser.Id);
        }

        [RequiredArgument]
        [ReferenceTarget(EntityNames.SystemUser)]
        [Input("System User")]
        public InArgument<EntityReference> SystemUser { get; set; }

        [RequiredArgument]
        [ReferenceTarget(EntityNames.Team)]
        [Input("Team")]
        public InArgument<EntityReference> Team { get; set; }
    }
}