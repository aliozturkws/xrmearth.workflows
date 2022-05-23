using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class SendEmail : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var email = Email.Get(activityHelper.CodeActivityContext);

            var ser = activityHelper.OrganizationService.Execute(
                new SendEmailRequest()
                {
                    EmailId = email.Id,
                    IssueSend = true
                }
              ) as SendEmailResponse;

            if (ser != null)
            {
                Subject.Set(activityHelper.CodeActivityContext, ser.Subject);
            }
        }

        [RequiredArgument]
        [Input("Email To Send")]
        [ReferenceTarget("email")]
        public InArgument<EntityReference> Email { get; set; }

        [Output("Email Subject")]
        public OutArgument<string> Subject { get; set; }
    }
}
