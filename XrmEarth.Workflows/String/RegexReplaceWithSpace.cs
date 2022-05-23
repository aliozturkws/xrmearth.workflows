using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Text.RegularExpressions;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public  class RegexReplaceWithSpace : BaseCodeActivity
	{
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToSearch = StringToSearch.Get(activityHelper.CodeActivityContext);
            int numberOfSpaces = NumberOfSpaces.Get(activityHelper.CodeActivityContext);
            string pattern = Pattern.Get(activityHelper.CodeActivityContext);

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            string spaces = "";
            spaces = spaces.PadRight(numberOfSpaces, ' ');

            string replacedString = regex.Replace(stringToSearch, spaces);

            ReplacedString.Set(activityHelper.CodeActivityContext, replacedString);
        }

        [RequiredArgument]
	    [Input("String To Search")]
	    public InArgument<string> StringToSearch { get; set; }

	    [RequiredArgument]
	    [Input("Number Of Spaces")]
	    public InArgument<int> NumberOfSpaces { get; set; }

	    [RequiredArgument]
	    [Input("Pattern")]
	    public InArgument<string> Pattern { get; set; }

	    [Output("Replaced String")]
	    public OutArgument<string> ReplacedString { get; set; }
    }
}
