using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class Length : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringInput = StringInput.Get(activityHelper.CodeActivityContext);

            StringLength.Set(activityHelper.CodeActivityContext, stringInput.Length + 1);
        }

        [RequiredArgument]
        [Input("String")]
        public InArgument<string> StringInput { get; set; }

        [Output("Length")]
        public OutArgument<int> StringLength { get; set; }
    }
}
