using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    // Not Supported v9

    //public class RemoveFromAllMarketingLists : BaseCodeActivity
    //{
    //    protected override void OnExecute(CodeActivityHelper activityHelper)
    //    {
    //        var recordUrl = RecordUrl.Get(activityHelper.CodeActivityContext);

    //        var recordUrlEntity = RecordUrlHelper.GetEntityReference(recordUrl, activityHelper.OrganizationService);

    //        CrmHelper.CheckMarketingListMemberEntityType(recordUrlEntity.LogicalName);

    //        var listMembers = CrmHelper.GetListMembers(activityHelper.OrganizationService, recordUrlEntity.Id);

    //        foreach (var listMember in listMembers.Entities)
    //        {
    //            var ent = (EntityReference)listMember.Attributes["entityid"];
    //            var list = (EntityReference)listMember.Attributes["listid"];
    //            var request = new RemoveMemberListRequest
    //            {
    //                EntityId = ent.Id,
    //                ListId = list.Id
    //            };
    //            activityHelper.OrganizationService.Execute(request);
    //        }
    //    }

    //    [RequiredArgument]
    //    [Input("Record Url")]
    //    public InArgument<string> RecordUrl { get; set; }
    //}
}
