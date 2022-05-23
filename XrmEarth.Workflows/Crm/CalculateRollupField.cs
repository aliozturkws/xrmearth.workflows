using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class CalculateRollupField : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string fieldName = FieldName.Get(activityHelper.CodeActivityContext);
            string parentRecordUrl = ParentRecordUrl.Get(activityHelper.CodeActivityContext);

            if (string.IsNullOrEmpty(parentRecordUrl))
                return;

            int parentObjectTypeCode = RecordUrlHelper.GetEtcByRecordUrl(parentRecordUrl);
            string parentEntityName = CrmHelper.GetEntityLogicalName(activityHelper.OrganizationService, parentObjectTypeCode);
            Guid parentId = RecordUrlHelper.GetIdByRecordUrl(parentRecordUrl);

            var calculateRollup = new CalculateRollupFieldRequest();
            calculateRollup.FieldName = fieldName;
            calculateRollup.Target = new EntityReference(parentEntityName, parentId);
            var response = (CalculateRollupFieldResponse)activityHelper.OrganizationService.Execute(calculateRollup);
        }

        [RequiredArgument]
        [Input("FieldName")]
        [Default("")]
        public InArgument<string> FieldName { get; set; }

        [RequiredArgument]
        [Input("Parent Record URL")]
        [ReferenceTarget("")]
        public InArgument<string> ParentRecordUrl { get; set; }
    }
}
