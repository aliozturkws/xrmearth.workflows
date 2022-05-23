using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using XrmEarth.Core;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class SendEmailFromTemplateToUsersInRole : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var security = SecurityRole.Get(activityHelper.CodeActivityContext);
            var emailTemplate = EmailTemplate.Get(activityHelper.CodeActivityContext);

            WorkflowHelper.SendEmailFromTemplateToUsersInRole(activityHelper.OrganizationService, security, emailTemplate);
        }

        [Input("Security Role")]
        [RequiredArgument]
        [ReferenceTarget("role")]
        public InArgument<EntityReference> SecurityRole { get; set; }

        [Input("Email Template")]
        [RequiredArgument]
        [ReferenceTarget("template")]
        public InArgument<EntityReference> EmailTemplate { get; set; }
    }
}
