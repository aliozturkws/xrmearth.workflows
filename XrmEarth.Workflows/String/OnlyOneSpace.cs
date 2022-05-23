using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Text.RegularExpressions;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class OnlyOneSpace : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            try
            {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);

                string stringToTrim = StringToTrim.Get(activityHelper.CodeActivityContext);

                string trimmedString = regex.Replace(stringToTrim, " ");

                TrimmedString.Set(activityHelper.CodeActivityContext, trimmedString);
            }
            catch (Exception ex)
            {
                activityHelper.TracingService.Trace("Exception: {0}", ex.ToString());
            }
        }

        [RequiredArgument]
        [Input("String To Trim")]
        public InArgument<string> StringToTrim { get; set; }

        [Output("Trimmed String")]
        public OutArgument<string> TrimmedString { get; set; }
    }
}
