using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class UpdateNoteTitle : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            EntityReference noteToUpdate = NoteToUpdate.Get(activityHelper.CodeActivityContext);
            string newTitle = NewTitle.Get(activityHelper.CodeActivityContext);

            Entity note = new Entity("annotation");
            note.Id = noteToUpdate.Id;
            note["subject"] = newTitle;
            activityHelper.OrganizationService.Update(note);

            UpdatedTitle.Set(activityHelper.CodeActivityContext, newTitle);
        }

        [RequiredArgument]
        [Input("Note To Update")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToUpdate { get; set; }

        [RequiredArgument]
        [Input("New Title")]
        public InArgument<string> NewTitle { get; set; }

        [Output("Updated Title")]
        public OutArgument<string> UpdatedTitle { get; set; }
    }
}
