using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class SendEmailToUsersInRole : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var email = Email.Get(activityHelper.CodeActivityContext);
            var securityRoleLookup = SecurityRoleLookup.Get(activityHelper.CodeActivityContext);

            WorkflowHelper.SendEmailToUsersInRole(activityHelper.OrganizationService, securityRoleLookup, email);
        }

        [Input("Security Role")]
        [RequiredArgument]
        [ReferenceTarget("role")]
        public InArgument<EntityReference> SecurityRoleLookup { get; set; }

        [Input("Email")]
        [RequiredArgument]
        [ReferenceTarget("email")]
        public InArgument<EntityReference> Email { get; set; }
    }
}
