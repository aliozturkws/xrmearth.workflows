using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class MoveNote : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            EntityReference noteToMove = NoteToMove.Get(activityHelper.CodeActivityContext);
            string recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);

            var dup = new DynamicUrlParser(recordUrl);

            string newEntityLogical = dup.GetEntityLogicalName(activityHelper.OrganizationService);

            Entity note = CrmHelper.GetNote(activityHelper.OrganizationService, noteToMove.Id);
            if (note.GetAttributeValue<EntityReference>("objectid").Id == dup.Id && note.GetAttributeValue<EntityReference>("objectid").LogicalName == newEntityLogical)
            {
                WasNoteMoved.Set(activityHelper.CodeActivityContext, false);
                return;
            }

            Entity updateNote = new Entity("annotation");
            updateNote.Id = noteToMove.Id;
            updateNote["objectid"] = new EntityReference(newEntityLogical, dup.Id);

            activityHelper.OrganizationService.Update(updateNote);

            WasNoteMoved.Set(activityHelper.CodeActivityContext, true);
        }

        [RequiredArgument]
        [Input("Note To Move")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToMove { get; set; }

        [Input("Record Dynamic Url")]
        [RequiredArgument]
        public InArgument<string> RecordUrl { get; set; }

        [Output("Was Note Moved")]
        public OutArgument<bool> WasNoteMoved { get; set; }
    }
}
