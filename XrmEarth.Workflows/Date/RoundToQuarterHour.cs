using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class RoundToQuarterHour : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var dateToRound = DateToRound.Get(activityHelper.CodeActivityContext);
            var roundDown = RoundDown.Get(activityHelper.CodeActivityContext);

            dateToRound = dateToRound.AddSeconds(-dateToRound.Second);
            var roundedDate = dateToRound;
            var minute = dateToRound.Minute;
            if (minute > 0 && minute < 15)
            {
                roundedDate = roundDown
                    ? dateToRound.AddMinutes(-dateToRound.Minute) // 0
                    : dateToRound.AddMinutes(15 - dateToRound.Minute); // 15
            }
            else if (minute > 15 && minute < 30)
            {
                roundedDate = roundDown
                    ? dateToRound.AddMinutes(-(dateToRound.Minute - 15)) // 15
                    : dateToRound.AddMinutes(30 - dateToRound.Minute); // 30
            }
            else if (minute > 30 && minute < 45)
            {
                roundedDate = roundDown
                    ? dateToRound.AddMinutes(-(dateToRound.Minute - 30)) // 30
                    : dateToRound.AddMinutes(45 - dateToRound.Minute); // 45
            }
            else if (minute > 45 && minute < 60)
            {
                roundedDate = roundDown
                    ? dateToRound.AddMinutes(-(dateToRound.Minute - 45)) // 45
                    : dateToRound.AddMinutes(-dateToRound.Minute).AddHours(1); // Next hour
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
