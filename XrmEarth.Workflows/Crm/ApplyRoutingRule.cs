using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Crm
{
    public class ApplyRoutingRule : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var incidentRecordUrl = IncidentRecordUrl.Get<string>(activityHelper.CodeActivityContext);

            var incidentEntityReference = RecordUrlHelper.GetEntityReference(incidentRecordUrl, activityHelper.OrganizationService);

            var routeRequest = new ApplyRoutingRuleRequest();
            routeRequest.Target = incidentEntityReference;
            var routeResponse = (ApplyRoutingRuleResponse)activityHelper.OrganizationService.Execute(routeRequest);
        }

        [RequiredArgument]
        [Input("Incident Record Url")]
        public InArgument<string> IncidentRecordUrl { get; set; }
    }
}
