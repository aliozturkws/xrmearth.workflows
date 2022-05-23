using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Geo
{
    public class GeocodeAddress : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var name = Name.Get<string>(activityHelper.CodeActivityContext);
            var apiKey = ApiKey.Get<string>(activityHelper.CodeActivityContext);
            var address = Address.Get<string>(activityHelper.CodeActivityContext);

            var response = WorkflowHelper.GetGeoCode(name, address, apiKey);

            Latitude.Set(activityHelper.CodeActivityContext, response.Latitude);
            Longitude.Set(activityHelper.CodeActivityContext, response.Longitude);
        }

        [RequiredArgument]
        [Input("Name (google or bing)")]
        public InArgument<string> Name { get; set; }

        [RequiredArgument]
        [Input("Key")]
        public InArgument<string> ApiKey { get; set; }

        [RequiredArgument]
        [Input("Address")]
        public InArgument<string> Address { get; set; }

        [Output("Latitude")]
        public OutArgument<double> Latitude { get; set; }

        [Output("Longitude")]
        public OutArgument<double> Longitude { get; set; }
    }
}
