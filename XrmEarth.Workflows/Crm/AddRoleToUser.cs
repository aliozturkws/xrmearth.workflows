using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    public class AddRoleToUser : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var systemUser = SystemUser.Get<EntityReference>(activityHelper.CodeActivityContext);
            var roleName = RoleName.Get<string>(activityHelper.CodeActivityContext);

            WorkflowHelper.AddRoleToUser(activityHelper.OrganizationService, systemUser.Id, roleName);
        }

        [RequiredArgument]
        [Input("System User")]
        [ReferenceTarget(EntityNames.SystemUser)]
        public InArgument<EntityReference> SystemUser { get; set; }

        [RequiredArgument]
        [Input("Role Name")]
        public InArgument<string> RoleName { get; set; }
    }
}