using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Globalization;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class ToUTCString : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime originalDate = DateToFormat.Get(activityHelper.CodeActivityContext);
            string cultureIn = Culture.Get(activityHelper.CodeActivityContext);

            CultureInfo culture = null;
            if (!string.IsNullOrEmpty(cultureIn))
                culture = new CultureInfo(cultureIn);

            string formattedDateString = originalDate.ToUniversalTime().ToString(culture);

            FormattedDateString.Set(activityHelper.CodeActivityContext, formattedDateString);
        }

        [RequiredArgument]
        [Input("Date To Format")]
        public InArgument<DateTime> DateToFormat { get; set; }

        [Input("Culture")]
        [Default("en-US")]
        public InArgument<string> Culture { get; set; }

        [Output("Formatted Date String")]
        public OutArgument<string> FormattedDateString { get; set; }
    }
}