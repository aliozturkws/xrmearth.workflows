using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class GetOwnerTypeByRecordUrl : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);

            EntityReference entityReference = RecordUrlHelper.GetEntityReference(recordUrl, activityHelper.OrganizationService);

            Entity entity = activityHelper.OrganizationService.Retrieve(entityReference.LogicalName, entityReference.Id, new ColumnSet("ownerid"));
            EntityReference ownerEntityReference = (EntityReference)entity["ownerid"];

            LogicalName.Set(activityHelper.CodeActivityContext, ownerEntityReference.LogicalName.ToLower());
            OwnerId.Set(activityHelper.CodeActivityContext, ownerEntityReference.Id.ToString());
        }

        [RequiredArgument]
        [Input("Record URL")]
        [Default("")]
        public InArgument<string> RecordUrl { get; set; }

        [Output("OwnerId")]
        public OutArgument<string> OwnerId { get; set; }

        [Output("LogicalName")]
        public OutArgument<string> LogicalName { get; set; }
    }
}
