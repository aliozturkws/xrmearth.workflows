using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Text;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class DeleteAttachment : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            EntityReference noteWithAttachment = NoteWithAttachment.Get(activityHelper.CodeActivityContext);
            int deleteSizeMax = DeleteSizeMax.Get(activityHelper.CodeActivityContext);
            int deleteSizeMin = DeleteSizeMin.Get(activityHelper.CodeActivityContext);
            string extensions = Extensions.Get(activityHelper.CodeActivityContext);
            bool appendNotice = AppendNotice.Get(activityHelper.CodeActivityContext);

            if (deleteSizeMax == 0) deleteSizeMax = int.MaxValue;
            if (deleteSizeMin > deleteSizeMax)
            {
                activityHelper.TracingService.Trace("Exception: {0}", "Min:" + deleteSizeMin + " Max:" + deleteSizeMax);
                throw new InvalidPluginExecutionException("Minimum Size Cannot Be Greater Than Maximum Size");
            }

            Entity note = CrmHelper.GetNote(activityHelper.OrganizationService, noteWithAttachment.Id);
            if (!CrmHelper.CheckForAttachment(note))
                return;

            string[] filetypes = new string[0];
            if (!string.IsNullOrEmpty(extensions))
                filetypes = extensions.Replace(".", string.Empty).Split(',');

            StringBuilder notice = new StringBuilder();
            int numberOfAttachmentsDeleted = 0;

            bool delete = false;

            if (note.GetAttributeValue<int>("filesize") >= deleteSizeMax)
                delete = true;

            if (note.GetAttributeValue<int>("filesize") <= deleteSizeMin)
                delete = true;

            if (filetypes.Length > 0 && delete)
                delete = CrmHelper.ExtensionMatch(filetypes, note.GetAttributeValue<string>("filename"));

            if (delete)
            {
                numberOfAttachmentsDeleted++;

                if (appendNotice)
                    notice.AppendLine("Deleted Attachment: " + note.GetAttributeValue<string>("filename") + " " +
                                      DateTime.Now.ToShortDateString());

                CrmHelper.UpdateNote(activityHelper.OrganizationService, note, notice.ToString());
            }

            NumberOfAttachmentsDeleted.Set(activityHelper.CodeActivityContext, numberOfAttachmentsDeleted);
        }

        [RequiredArgument]
        [Input("Note With Attachments To Remove")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteWithAttachment { get; set; }

        [Input("Delete >= Than X Bytes (Empty = 2,147,483,647)")]
        public InArgument<int> DeleteSizeMax { get; set; }

        [Input("Delete <= Than X Bytes (Empty = 0)")]
        public InArgument<int> DeleteSizeMin { get; set; }

        [Input("Limit To Extensions (Comma Delimited, Empty = Ignore)")]
        public InArgument<string> Extensions { get; set; }

        [RequiredArgument]
        [Input("Add Delete Notice As Text?")]
        [Default("false")]
        public InArgument<bool> AppendNotice { get; set; }

        [Output("Number Of Attachments Deleted")]
        public OutArgument<int> NumberOfAttachmentsDeleted { get; set; }
    }
}