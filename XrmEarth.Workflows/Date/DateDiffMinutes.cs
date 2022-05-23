using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class DateDiffMinutes : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime startingDate = StartingDate.Get(activityHelper.CodeActivityContext);
            DateTime endingDate = EndingDate.Get(activityHelper.CodeActivityContext);

            startingDate = new DateTime(startingDate.Year, startingDate.Month, startingDate.Day, startingDate.Hour,
                startingDate.Minute, 0, startingDate.Kind);

            endingDate = new DateTime(endingDate.Year, endingDate.Month, endingDate.Day, endingDate.Hour,
                endingDate.Minute, 0, endingDate.Kind);

            TimeSpan difference = startingDate - endingDate;
            int minutesDifference = Math.Abs(Convert.ToInt32(difference.TotalMinutes));

            MinutesDifference.Set(activityHelper.CodeActivityContext, minutesDifference);
        }

        [RequiredArgument]
        [Input("Starting Date")]
        public InArgument<DateTime> StartingDate { get; set; }

        [RequiredArgument]
        [Input("Ending Date")]
        public InArgument<DateTime> EndingDate { get; set; }

        [Output("Minutes Difference")]
        public OutArgument<int> MinutesDifference { get; set; }
    }
}