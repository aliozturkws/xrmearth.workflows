using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class DateDiffDays : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime startingDate = StartingDate.Get(activityHelper.CodeActivityContext);
            DateTime endingDate = EndingDate.Get(activityHelper.CodeActivityContext);

            TimeSpan difference = startingDate - endingDate;
            int daysDifference = Math.Abs(Convert.ToInt32(difference.TotalDays));

            DaysDifference.Set(activityHelper.CodeActivityContext, daysDifference);
        }

        [RequiredArgument]
        [Input("Starting Date")]
        public InArgument<DateTime> StartingDate { get; set; }

        [RequiredArgument]
        [Input("Ending Date")]
        public InArgument<DateTime> EndingDate { get; set; }

        [Output("Days Difference")]
        public OutArgument<int> DaysDifference { get; set; }
    }
}