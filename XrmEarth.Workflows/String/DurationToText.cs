using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.String
{
    public class DurationToText : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var durationType = DurationType.Get(activityHelper.CodeActivityContext);
            var duration = Duration.Get(activityHelper.CodeActivityContext);
            var languageCode = LanguageCode.Get(activityHelper.CodeActivityContext);

            if (durationType == "Day" || durationType == "Hour" || durationType == "Minute") { } else { throw new InvalidPluginExecutionException("Duration Type should be day, hour or minute!"); }

            TimeSpan time = new TimeSpan();

            if (durationType == "Day")
                time = TimeSpan.FromDays(Convert.ToDouble(duration));
            else if (durationType == "Hour")
                time = TimeSpan.FromHours(Convert.ToDouble(duration));
            else if (durationType == "Minute")
                time = TimeSpan.FromMinutes(Convert.ToDouble(duration));

            DurationText.Set(activityHelper.CodeActivityContext, StringHelper.GetTimeSpanText(time, languageCode));
        }

        [RequiredArgument]
        [Input("Duration Type (Day,Hour or Minute)")]
        [Default("Hour")]
        public InArgument<string> DurationType { get; set; }

        [RequiredArgument]
        [Input("LanguageCode (tr or en)")]
        [Default("tr")]
        public InArgument<string> LanguageCode { get; set; }

        [RequiredArgument]
        [Input("Duration")]
        public InArgument<decimal> Duration { get; set; }

        [Output("Duration Text")]
        public OutArgument<string> DurationText { get; set; }
    }
}
