using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public  class Replace : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToSearch = StringToSearch.Get(activityHelper.CodeActivityContext);
            string valueToReplace = ValueToReplace.Get(activityHelper.CodeActivityContext);
            string replacementValue = ReplacementValue.Get(activityHelper.CodeActivityContext);

            if (string.IsNullOrEmpty(replacementValue))
                replacementValue = "";

            string replacedString = stringToSearch.Replace(valueToReplace, replacementValue);

            ReplacedString.Set(activityHelper.CodeActivityContext, replacedString);
        }

        [RequiredArgument]
        [Input("String To Search")]
        public InArgument<string> StringToSearch { get; set; }

        [RequiredArgument]
        [Input("Value To Replace")]
        public InArgument<string> ValueToReplace { get; set; }

        [Input("Replacement Value")]
        public InArgument<string> ReplacementValue { get; set; }

        [Output("Replaced String")]
        public OutArgument<string> ReplacedString { get; set; }
    }
}