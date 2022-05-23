using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class RoundToHour : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var dateToRound = DateToRound.Get(activityHelper.CodeActivityContext);
            var roundDown = RoundDown.Get(activityHelper.CodeActivityContext);

            dateToRound = dateToRound.AddSeconds(-dateToRound.Second);
            var roundedDate = dateToRound;
            var minute = dateToRound.Minute;
            if (minute > 0 && minute < 60)
            {
                roundedDate = roundDown
                    ? dateToRound.AddMinutes(-minute) // 0
                    : dateToRound.AddMinutes(-minute).AddHours(1); // Next hour
            }

            RoundedDate.Set(activityHelper.CodeActivityContext, roundedDate);
        }

        [RequiredArgument]
        [Input("Date To Round")]
        public InArgument<DateTime> DateToRound { get; set; }

        [RequiredArgument]
        [Default("false")]
        [Input("Round Down (Otherwise Round Up)?")]
        public InArgument<bool> RoundDown { get; set; }

        [Output("Rounded Date")]
        public OutArgument<DateTime> RoundedDate { get; set; }
    }
}
