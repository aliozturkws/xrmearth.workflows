using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class WordCount : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToCount = StringToCount.Get(activityHelper.CodeActivityContext);

            string[] words = stringToCount.Trim().Split(' ');

            Count.Set(activityHelper.CodeActivityContext, words.Length);
        }

        [RequiredArgument]
        [Input("String To Count")]
        public InArgument<string> StringToCount { get; set; }

        [Output("Word Count")]
        public OutArgument<int> Count { get; set; }
    }
}