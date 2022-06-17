using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class CheckAttachmentForEntity : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string recordUrl = RecordUrl.Get(activityHelper.CodeActivityContext);

            EntityReference entityreference = RecordUrlHelper.GetEntityReference(recordUrl, activityHelper.OrganizationService);

            EntityCollection results = CrmHelper.GetAttachments(activityHelper.OrganizationService, entityreference.Id);

            foreach (var entity in results.Entities)
            {
                object oIsDocument;
                bool hasValue = entity.Attributes.TryGetValue("isdocument", out oIsDocument);
                if (hasValue)
                {
                    HasAttachment.Set(activityHelper.CodeActivityContext, hasValue);
                    break;
                }
            }
        }

        [RequiredArgument]
        [Input("Record URL")]
        public InArgument<string> RecordUrl { get; set; }

        [Output("Has Attachment")]
        public OutArgument<bool> HasAttachment { get; set; }
    }
}
