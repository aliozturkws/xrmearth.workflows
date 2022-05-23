using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Text.RegularExpressions;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class RegexMatch : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToSearch = StringToSearch.Get(activityHelper.CodeActivityContext);
            string pattern = Pattern.Get(activityHelper.CodeActivityContext);

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(stringToSearch);
            bool containsPattern = match.Success;

            ContainsPattern.Set(activityHelper.CodeActivityContext, containsPattern);
        }

        [RequiredArgument]
        [Input("String To Search")]
        public InArgument<string> StringToSearch { get; set; }

        [RequiredArgument]
        [Input("Pattern")]
        public InArgument<string> Pattern { get; set; }

        [Output("Contains Pattern")]
        public OutArgument<bool> ContainsPattern { get; set; }
    }
}