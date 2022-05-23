using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public  class ReplaceWithSpace : BaseCodeActivity
	{
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToSearch = StringToSearch.Get(activityHelper.CodeActivityContext);
            string valueToReplace = ValueToReplace.Get(activityHelper.CodeActivityContext);
            int numberOfSpaces = NumberOfSpaces.Get(activityHelper.CodeActivityContext);

            string spaces = "";
            spaces = spaces.PadRight(numberOfSpaces, ' ');

            if (valueToReplace == "#")
                valueToReplace = " ";

            string replacedString = stringToSearch.Replace(valueToReplace, spaces);

            ReplacedString.Set(activityHelper.CodeActivityContext, replacedString);
        }

        [RequiredArgument]
	    [Input("String To Search")]
	    public InArgument<string> StringToSearch { get; set; }

	    [RequiredArgument]
	    [Input("Value To Replace")]
	    public InArgument<string> ValueToReplace { get; set; }

	    [RequiredArgument]
	    [Input("Number Of Spaces")]
	    public InArgument<int> NumberOfSpaces { get; set; }

	    [Output("Replaced String")]
	    public OutArgument<string> ReplacedString { get; set; }
    }
}