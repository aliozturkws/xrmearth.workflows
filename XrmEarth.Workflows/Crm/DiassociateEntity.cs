using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class DiassociateEntity : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var relationShipName = RelationShipName.Get<string>(activityHelper.CodeActivityContext);
            var relationShipEntityName = RelationShipEntityName.Get<string>(activityHelper.CodeActivityContext);
            var relationRecordUrl = RelationRecordUrl.Get<string>(activityHelper.CodeActivityContext);
            var targetRecordUrl = TargetRecordUrl.Get<string>(activityHelper.CodeActivityContext);

            WorkflowHelper.Diassociate(activityHelper.OrganizationService, targetRecordUrl, relationRecordUrl, relationShipName, relationShipEntityName);
        }

        [RequiredArgument]
        [Input("RelationShip Name")]
        public InArgument<string> RelationShipName { get; set; }

        [RequiredArgument]
        [Input("RelationShip Entity Name")]
        public InArgument<string> RelationShipEntityName { get; set; }

        [RequiredArgument]
        [Input("Target Record Url")]
        public InArgument<string> TargetRecordUrl { get; set; }

        [RequiredArgument]
        [Input("Relation Record Url")]
        public InArgument<string> RelationRecordUrl { get; set; }
    }
}
