using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class DateDiffHours : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime startingDate = StartingDate.Get(activityHelper.CodeActivityContext);
            DateTime endingDate = EndingDate.Get(activityHelper.CodeActivityContext);

            TimeSpan difference = startingDate - endingDate;
            int hoursDifference = Math.Abs(Convert.ToInt32(difference.TotalHours));

            HoursDifference.Set(activityHelper.CodeActivityContext, hoursDifference);
        }

        [RequiredArgument]
        [Input("Starting Date")]
        public InArgument<DateTime> StartingDate { get; set; }

        [RequiredArgument]
        [Input("Ending Date")]
        public InArgument<DateTime> EndingDate { get; set; }

        [Output("Hours Difference")]
        public OutArgument<int> HoursDifference { get; set; }
    }
}