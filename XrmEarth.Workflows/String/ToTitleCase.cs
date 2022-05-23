using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Globalization;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public  class ToTitleCase : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            string stringToTitleCase = StringToTitleCase.Get(activityHelper.CodeActivityContext);

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

            string titleCasedString = ti.ToTitleCase(stringToTitleCase);

            TitleCasedString.Set(activityHelper.CodeActivityContext, titleCasedString);
        }

        [RequiredArgument]
        [Input("String To Title Case")]
        public InArgument<string> StringToTitleCase { get; set; }

        [Output("Title Cased String")]
        public OutArgument<string> TitleCasedString { get; set; }
    }
}
