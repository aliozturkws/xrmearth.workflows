using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class CopyNote : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            EntityReference noteToCopy = NoteToCopy.Get(activityHelper.CodeActivityContext);
            string recordUrl = RecordUrl.Get<string>(activityHelper.CodeActivityContext);
            bool copyAttachment = CopyAttachment.Get(activityHelper.CodeActivityContext);

            var dup = new DynamicUrlParser(recordUrl);

            string newEntityLogical = dup.GetEntityLogicalName(activityHelper.OrganizationService);

            Entity note = CrmHelper.GetNote(activityHelper.OrganizationService, noteToCopy.Id);
            if (note.GetAttributeValue<EntityReference>("objectid").Id == dup.Id && note.GetAttributeValue<EntityReference>("objectid").LogicalName == newEntityLogical)
            {
                WasNoteCopied.Set(activityHelper.CodeActivityContext, false);
                return;
            }

            Entity newNote = new Entity("annotation");
            newNote["objectid"] = new EntityReference(newEntityLogical, dup.Id);
            newNote["notetext"] = note.GetAttributeValue<string>("notetext");
            newNote["subject"] = note.GetAttributeValue<string>("subject");
            if (copyAttachment)
            {
                newNote["isdocument"] = note.GetAttributeValue<bool>("isdocument");
                newNote["filename"] = note.GetAttributeValue<string>("filename");
                newNote["filesize"] = note.GetAttributeValue<int>("filesize");
                newNote["documentbody"] = note.GetAttributeValue<string>("documentbody");
            }
            else
                newNote["isdocument"] = false;

            activityHelper.OrganizationService.Create(newNote);

            WasNoteCopied.Set(activityHelper.CodeActivityContext, true);
        }

        [RequiredArgument]
        [Input("Note To Copy")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToCopy { get; set; }

        [Input("Record Dynamic Url")]
        [RequiredArgument]
        public InArgument<string> RecordUrl { get; set; }

        [RequiredArgument]
        [Default("True")]
        [Input("Copy Attachment?")]
        public InArgument<bool> CopyAttachment { get; set; }

        [Output("Was Note Copied")]
        public OutArgument<bool> WasNoteCopied { get; set; }
    }
}
