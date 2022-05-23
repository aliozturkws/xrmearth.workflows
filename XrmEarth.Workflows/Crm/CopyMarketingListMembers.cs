using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Crm
{
    // Not Supported v9

    //public class CopyMarketingListMembers : BaseCodeActivity
    //{
    //    protected override void OnExecute(CodeActivityHelper activityHelper)
    //    {
    //        var sourceList = SourceList.Get(activityHelper.CodeActivityContext);
    //        var targetList = TargetList.Get(activityHelper.CodeActivityContext);

    //        var request = new CopyMembersListRequest
    //        {
    //            SourceListId = sourceList.Id,
    //            TargetListId = targetList.Id
    //        };

    //        activityHelper.OrganizationService.Execute(request);
    //    }

    //    [RequiredArgument]
    //    [Input("Source List")]
    //    [ReferenceTarget(EntityNames.List)]
    //    public InArgument<EntityReference> SourceList { get; set; }

    //    [RequiredArgument]
    //    [Input("Target List")]
    //    [ReferenceTarget(EntityNames.List)]
    //    public InArgument<EntityReference> TargetList { get; set; }
    //}
}
