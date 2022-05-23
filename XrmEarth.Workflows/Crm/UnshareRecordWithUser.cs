using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class UnshareRecordWithUser : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var target = new EntityReference(activityHelper.Context.PrimaryEntityName, activityHelper.Context.PrimaryEntityId);
            var user = User.Get<EntityReference>(activityHelper.CodeActivityContext);
            var recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);

            if (!string.IsNullOrEmpty(recordUrl))
            {
                target = RecordUrlHelper.GetEntityReference(recordUrl, activityHelper.OrganizationService);
                WorkflowHelper.RevokePrivileges(activityHelper.OrganizationService, target, user);
            }
        }

        [RequiredArgument]
        [Input("User")]
        [ReferenceTarget(EntityNames.SystemUser)]
        public InArgument<EntityReference> User { get; set; }

        [Input("RecordUrl")]
        public InArgument<string> RecordUrl { get; set; }
    }
}
