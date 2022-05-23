using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class Trim : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToTrim = StringToTrim.Get(activityHelper.CodeActivityContext);

            string trimmedString = stringToTrim.Trim();

            TrimmedString.Set(activityHelper.CodeActivityContext, trimmedString);
        }

        [RequiredArgument]
        [Input("String To Trim")]
        public InArgument<string> StringToTrim { get; set; }

        [Output("Trimmed String")]
        public OutArgument<string> TrimmedString { get; set; }
    }
}
