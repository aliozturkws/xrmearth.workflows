using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class ToLower : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToLower = StringToLower.Get(activityHelper.CodeActivityContext);

            string loweredString = stringToLower.ToLower();

            LoweredString.Set(activityHelper.CodeActivityContext, loweredString);
        }

        [RequiredArgument]
        [Input("String To Lower")]
        public InArgument<string> StringToLower { get; set; }

        [Output("Lowered String")]
        public OutArgument<string> LoweredString { get; set; }
    }
}
