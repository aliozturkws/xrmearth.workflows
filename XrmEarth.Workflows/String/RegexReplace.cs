using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Text.RegularExpressions;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public  class RegexReplace : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToSearch = StringToSearch.Get(activityHelper.CodeActivityContext);
            string replacementValue = ReplacementValue.Get(activityHelper.CodeActivityContext);
            string pattern = Pattern.Get(activityHelper.CodeActivityContext);

            if (string.IsNullOrEmpty(replacementValue))
                replacementValue = "";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            string replacedString = regex.Replace(stringToSearch, replacementValue);

            ReplacedString.Set(activityHelper.CodeActivityContext, replacedString);
        }

        [RequiredArgument]
        [Input("String To Search")]
        public InArgument<string> StringToSearch { get; set; }

        [Input("Replacement Value")]
        public InArgument<string> ReplacementValue { get; set; }

        [RequiredArgument]
        [Input("Pattern")]
        public InArgument<string> Pattern { get; set; }

        [Output("Replaced String")]
        public OutArgument<string> ReplacedString { get; set; }
    }
}
