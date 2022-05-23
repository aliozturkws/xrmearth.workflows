using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class Join : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string string1 = String1.Get(activityHelper.CodeActivityContext);
            string string2 = String2.Get(activityHelper.CodeActivityContext);
            string joiner = Joiner.Get(activityHelper.CodeActivityContext);

            string joinedString = System.String.Join(joiner, string1, string2);

            JoinedString.Set(activityHelper.CodeActivityContext, joinedString);
        }

        [RequiredArgument]
        [Input("String 1")]
        public InArgument<string> String1 { get; set; }

        [RequiredArgument]
        [Input("String 2")]
        public InArgument<string> String2 { get; set; }

        [Input("Joiner")]
        public InArgument<string> Joiner { get; set; }

        [Output("Joined String")]
        public OutArgument<string> JoinedString { get; set; }
    }
}
