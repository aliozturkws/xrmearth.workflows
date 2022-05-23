using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class UpdateNoteText : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            EntityReference noteToUpdate = NoteToUpdate.Get(activityHelper.CodeActivityContext);
            string newText = NewText.Get(activityHelper.CodeActivityContext);

            Entity note = new Entity("annotation");
            note.Id = noteToUpdate.Id;
            note["notetext"] = newText;
            activityHelper.OrganizationService.Update(note);

            UpdatedText.Set(activityHelper.CodeActivityContext, newText);
        }

        [RequiredArgument]
        [Input("Note To Update")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToUpdate { get; set; }

        [RequiredArgument]
        [Input("New Text")]
        public InArgument<string> NewText { get; set; }

        [Output("Updated Text")]
        public OutArgument<string> UpdatedText { get; set; }
    }
}
