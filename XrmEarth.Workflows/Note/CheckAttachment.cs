using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class CheckAttachment : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            EntityReference noteToCheck = NoteToCheck.Get(activityHelper.CodeActivityContext);

            HasAttachment.Set(activityHelper.CodeActivityContext, CrmHelper.CheckForAttachment(activityHelper.OrganizationService, noteToCheck.Id));
        }

        [RequiredArgument]
        [Input("Note To Check")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToCheck { get; set; }

        [Output("Has Attachment")]
        public OutArgument<bool> HasAttachment { get; set; }
    }
}
