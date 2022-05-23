using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class DeleteEntity : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var deleteEntityUrl = DeleteEntityUrl.Get(activityHelper.CodeActivityContext);
            var deleteUsingEntityUrl = DeleteUsingEntityUrl.Get(activityHelper.CodeActivityContext);

            var entityName = EntityName.Get(activityHelper.CodeActivityContext);
            var entityId = EntityId.Get(activityHelper.CodeActivityContext);

            var deleteEntityUrlEntityId = RecordUrlHelper.GetIdByRecordUrl(deleteEntityUrl).ToString();
            var entityEtc = RecordUrlHelper.GetEtcByRecordUrl(deleteEntityUrl);
            var deleteEntityUrlEntityName = CrmHelper.GetEntityLogicalName(activityHelper.OrganizationService, entityEtc);

            if (deleteUsingEntityUrl)
            {
                if (string.IsNullOrEmpty(deleteEntityUrl))
                {
                    throw new InvalidOperationException("ERROR: Delete Entity URL to be deleted missing.");
                }
                activityHelper.OrganizationService.Delete(deleteEntityUrlEntityName, new Guid(deleteEntityUrlEntityId));
            }
            else
            {
                if (string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(entityId))
                {
                    throw new InvalidOperationException("ERROR: Entity Type name or GUID to be deleted missing.");
                }
                activityHelper.OrganizationService.Delete(entityName, new Guid(entityId));
            }
        }

        [RequiredArgument]
        [Input("Delete Using Entity URL")]
        [Default("True")]
        public InArgument<bool> DeleteUsingEntityUrl { get; set; }

        [Input("Entity URL")]
        public InArgument<string> DeleteEntityUrl { get; set; }

        [Input("Entity Name")]
        public InArgument<string> EntityName { get; set; }

        [Input("Entity Id")]
        public InArgument<string> EntityId { get; set; }
    }
}
