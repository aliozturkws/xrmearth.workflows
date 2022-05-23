using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public  class Reverse : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToReverse = StringToReverse.Get(activityHelper.CodeActivityContext);

            char[] letters = stringToReverse.ToCharArray();
            Array.Reverse(letters);
            string reversedString = new string(letters);

            ReversedString.Set(activityHelper.CodeActivityContext, reversedString);
        }

        [RequiredArgument]
        [Input("String To Reverse")]
        public InArgument<string> StringToReverse { get; set; }

        [Output("Reversed String")]
        public OutArgument<string> ReversedString { get; set; }
    }
}
