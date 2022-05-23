using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class ToUpper : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToUpper = StringToUpper.Get(activityHelper.CodeActivityContext);

            string upperedString = stringToUpper.ToUpper();

            UpperedString.Set(activityHelper.CodeActivityContext, upperedString);
        }

        [RequiredArgument]
        [Input("String To Upper")]
        public InArgument<string> StringToUpper { get; set; }

        [Output("Uppered String")]
        public OutArgument<string> UpperedString { get; set; }
    }
}
