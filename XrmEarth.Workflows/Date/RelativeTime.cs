using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class RelativeTime : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime startingDate = StartingDate.Get(activityHelper.CodeActivityContext);
            DateTime endingDate = EndingDate.Get(activityHelper.CodeActivityContext);
            string relativeTimeString;

            if (endingDate < startingDate)
            {
                relativeTimeString = "in the future";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var ts = new TimeSpan(endingDate.Ticks - startingDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
            {
                relativeTimeString = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            if (delta < 2 * minute)
            {
                relativeTimeString = "a minute ago";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            if (delta < 45 * minute)
            {
                relativeTimeString = ts.Minutes + " minutes ago";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            if (delta < 90 * minute)
            {
                relativeTimeString = "an hour ago";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            if (delta < 24 * hour)
            {
                relativeTimeString = ts.Hours + " hours ago";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            if (delta < 48 * hour)
            {
                relativeTimeString = "yesterday";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            if (delta < 30 * day)
            {
                relativeTimeString = ts.Days + " days ago";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                relativeTimeString = months <= 1 ? "one month ago" : months + " months ago";
                RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
                return;
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            relativeTimeString = years <= 1 ? "one year ago" : years + " years ago";
            RelativeTimeString.Set(activityHelper.CodeActivityContext, relativeTimeString);
        }

        [RequiredArgument]
        [Input("Starting Date")]
        public InArgument<DateTime> StartingDate { get; set; }

        [RequiredArgument]
        [Input("Ending Date")]
        public InArgument<DateTime> EndingDate { get; set; }

        [Output("Relative Time String")]
        public OutArgument<string> RelativeTimeString { get; set; }
    }
}
