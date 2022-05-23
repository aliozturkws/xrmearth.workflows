using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using XrmEarth.Data.Const;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    // Not Supported v9

    //public class AddMarketingListToCampaign : BaseCodeActivity
    //{
    //    protected override void OnExecute(CodeActivityHelper activityHelper)
    //    {
    //        var marketingList = MarketingList.Get(activityHelper.CodeActivityContext);
    //        var campaign = Campaign.Get(activityHelper.CodeActivityContext);

    //        var request = new AddItemCampaignRequest
    //        {
    //            CampaignId = campaign.Id,
    //            EntityId = marketingList.Id,
    //            EntityName = EntityNames.List,
    //        };
    //        activityHelper.OrganizationService.Execute(request);
    //    }

    //    [RequiredArgument]
    //    [Input("Marketing List")]
    //    [ReferenceTarget(EntityNames.List)]
    //    public InArgument<EntityReference> MarketingList { get; set; }

    //    [RequiredArgument]
    //    [Input("Marketing Campaign")]
    //    [ReferenceTarget(EntityNames.Campaign)]
    //    public InArgument<EntityReference> Campaign { get; set; }
    //}
}
