using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class DeleteNote : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            EntityReference noteToDelete = NoteToDelete.Get(activityHelper.CodeActivityContext);

            activityHelper.OrganizationService.Delete("annotation", noteToDelete.Id);

            WasNoteDeleted.Set(activityHelper.CodeActivityContext, true);
        }

        [RequiredArgument]
        [Input("Note To Delete")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToDelete { get; set; }

        [Output("Was Note Deleted")]
        public OutArgument<bool> WasNoteDeleted { get; set; }
    }
}
