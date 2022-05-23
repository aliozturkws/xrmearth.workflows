using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    // Not Supported v9

    //public class CopyToStaticList : BaseCodeActivity
    //{
    //    protected override void OnExecute(CodeActivityHelper activityHelper)
    //    {
    //        var marketingList = MarketingList.Get(activityHelper.CodeActivityContext);

    //        activityHelper.OrganizationService.Execute(new CopyDynamicListToStaticRequest { ListId = marketingList.Id });
    //    }

    //    [RequiredArgument]
    //    [Input("Marketing List")]
    //    [ReferenceTarget(EntityNames.List)]
    //    public InArgument<EntityReference> MarketingList { get; set; }
    //}
}
