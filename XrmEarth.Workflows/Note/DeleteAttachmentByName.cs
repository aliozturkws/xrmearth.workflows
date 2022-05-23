using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Text;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Note
{
    public class DeleteAttachmentByName : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            EntityReference noteWithAttachment = NoteWithAttachment.Get(activityHelper.CodeActivityContext);
            string fileName = FileName.Get(activityHelper.CodeActivityContext);
            bool appendNotice = AppendNotice.Get(activityHelper.CodeActivityContext);

            Entity note = CrmHelper.GetNote(activityHelper.OrganizationService, noteWithAttachment.Id);
            if (!CrmHelper.CheckForAttachment(note))
                return;

            StringBuilder notice = new StringBuilder();
            int numberOfAttachmentsDeleted = 0;

            if (System.String.Equals(note.GetAttributeValue<string>("filename"), fileName, StringComparison.CurrentCultureIgnoreCase))
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
        [Input("Note With Attachment To Remove")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteWithAttachment { get; set; }

        [RequiredArgument]
        [Input("File Name With Extension")]
        public InArgument<string> FileName { get; set; }

        [RequiredArgument]
        [Input("Add Delete Notice As Text?")]
        [Default("false")]
        public InArgument<bool> AppendNotice { get; set; }

        [Output("Number Of Attachments Deleted")]
        public OutArgument<int> NumberOfAttachmentsDeleted { get; set; }
    }
}
