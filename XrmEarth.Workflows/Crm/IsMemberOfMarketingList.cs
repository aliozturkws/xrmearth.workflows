using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    // Not Supported v9

    //public class IsMemberOfMarketingList : BaseCodeActivity
    //{
    //    protected override void OnExecute(CodeActivityHelper activityHelper)
    //    {
    //        var marketingList = MarketingList.Get(activityHelper.CodeActivityContext);
    //        var recordUrl = RecordUrl.Get(activityHelper.CodeActivityContext);

    //        var recordUrlEntity = RecordUrlHelper.GetEntityReference(recordUrl, activityHelper.OrganizationService);

    //        CrmHelper.CheckMarketingListMemberEntityType(recordUrlEntity.LogicalName);

    //        var isMember = CrmHelper.CheckIsMemberOfMarketingList(activityHelper.OrganizationService, marketingList.Id, recordUrlEntity.Id);

    //        MemberOfMarketingList.Set(activityHelper.CodeActivityContext, isMember);
    //    }

    //    [RequiredArgument]
    //    [Input("Marketing List")]
    //    [ReferenceTarget(EntityNames.List)]
    //    public InArgument<EntityReference> MarketingList { get; set; }

    //    [RequiredArgument]
    //    [Input("Record Url")]
    //    public InArgument<string> RecordUrl { get; set; }

    //    [Output("Is Member Of Marketing List")]
    //    public OutArgument<bool> MemberOfMarketingList { get; set; }
    //}
}
