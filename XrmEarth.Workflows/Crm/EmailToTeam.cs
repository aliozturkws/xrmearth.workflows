using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Linq;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class EmailToTeam : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var email = Email.Get(activityHelper.CodeActivityContext);
            var team = Team.Get(activityHelper.CodeActivityContext);

            var teamInUsers = CrmHelper.GetTeamInUsers(activityHelper.OrganizationService, team.Id);

            Entity emailEntity = activityHelper.OrganizationService.Retrieve(EntityNames.Email, email.Id, new ColumnSet("to"));

            var toList = new EntityCollection();

            if (emailEntity.Contains("to"))
            {
                toList = emailEntity.GetAttributeValue<EntityCollection>("to");
            }

            if (teamInUsers.Entities.Count == 0) return;

            foreach (var user in teamInUsers.Entities)
            {
                if (toList.Entities.Where(to => ((EntityReference)to["partyid"]).Id == user.Id).FirstOrDefault() != null)
                    continue;

                var toEntity = new Entity(EntityNames.ActivityParty);
                toEntity["partyid"] = new EntityReference(EntityNames.SystemUser, user.Id);
                toList.Entities.Add(toEntity);
            }

            emailEntity["to"] = toList;
            activityHelper.OrganizationService.Update(emailEntity);
        }

        [RequiredArgument]
        [Input("Email")]
        [ReferenceTarget("email")]
        public InArgument<EntityReference> Email { get; set; }

        [RequiredArgument]
        [Input("Team")]
        [ReferenceTarget("team")]
        public InArgument<EntityReference> Team { get; set; }
    }
}
