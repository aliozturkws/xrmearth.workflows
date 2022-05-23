using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Text.RegularExpressions;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class RegexExtract : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToSearch = StringToSearch.Get(activityHelper.CodeActivityContext);
            string pattern = Pattern.Get(activityHelper.CodeActivityContext);

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(stringToSearch);

            if (match.Success)
            {
                string extractedString = match.Value;
                ExtractedString.Set(activityHelper.CodeActivityContext, extractedString);
                return;
            }

            ExtractedString.Set(activityHelper.CodeActivityContext, null);
        }

        [RequiredArgument]
        [Input("String To Search")]
        public InArgument<string> StringToSearch { get; set; }

        [RequiredArgument]
        [Input("Pattern")]
        public InArgument<string> Pattern { get; set; }

        [Output("Extracted String")]
        public OutArgument<string> ExtractedString { get; set; }
    }
}
