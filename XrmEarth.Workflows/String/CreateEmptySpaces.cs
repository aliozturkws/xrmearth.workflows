using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class CreateEmptySpaces : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            int numberOfSpaces = NumberOfSpaces.Get(activityHelper.CodeActivityContext);

            string emptyString = new string(' ', numberOfSpaces);

            EmptyString.Set(activityHelper.CodeActivityContext, emptyString);
        }

        [RequiredArgument]
        [Input("Number Of Spaces")]
        public InArgument<int> NumberOfSpaces { get; set; }

        [Output("Empty String")]
        public OutArgument<string> EmptyString { get; set; }
    }
}
