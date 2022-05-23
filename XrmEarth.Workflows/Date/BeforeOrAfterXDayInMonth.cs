using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class BeforeOrAfterXDayInMonth : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var date = Date.Get<DateTime>(activityHelper.CodeActivityContext);
            var beforeOrAfter = BeforeOrAfter.Get<bool>(activityHelper.CodeActivityContext);
            var xMonth = XMonth.Get<int>(activityHelper.CodeActivityContext);
            var xDay = XDay.Get<int>(activityHelper.CodeActivityContext);
            var firstDay = FirstDay.Get<bool>(activityHelper.CodeActivityContext);
            var lastDay = LastDay.Get<bool>(activityHelper.CodeActivityContext);

            if ((firstDay || lastDay) && xDay > 0)
                throw new InvalidPluginExecutionException("The day can not be full if the first day or last day is entered!");

            if ((firstDay && lastDay))
                throw new InvalidPluginExecutionException("First Day Of The Month and Last Day Of The Month can not be True at the same time!");

            if ((!firstDay && !lastDay) && xDay == 0)
                throw new InvalidPluginExecutionException("At least one of the First Day Of The Month, Last Day Of The Month, or X Day fields must be full!");

            DateTime checkDate = DateTime.Now.ToLocalTime();
            checkDate = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, 12, 00, 00);
            date = date.ToLocalTime();
            date = new DateTime(date.Year, date.Month, date.Day, 12, 00, 00);

            if (!beforeOrAfter)
            {
                checkDate = checkDate.AddMonths(-xMonth);

                if (firstDay)
                    checkDate = new DateTime(checkDate.Year, checkDate.Month, 1);

                if (lastDay)
                    checkDate = new DateTime(checkDate.Year, checkDate.Month, DateTime.DaysInMonth(checkDate.Year, checkDate.Month));

                if (xDay > 0)
                    checkDate = new DateTime(checkDate.Year, checkDate.Month, xDay);

                if (date < checkDate)
                    Result.Set(activityHelper.CodeActivityContext, false);
                else
                    Result.Set(activityHelper.CodeActivityContext, true);
            }
            else
            {
                checkDate = checkDate.AddMonths(xMonth);

                if (firstDay)
                    checkDate = new DateTime(checkDate.Year, checkDate.Month, 1);

                if (lastDay)
                    checkDate = new DateTime(checkDate.Year, checkDate.Month, DateTime.DaysInMonth(checkDate.Year, checkDate.Month));

                if (xDay > 0)
                    checkDate = new DateTime(checkDate.Year, checkDate.Month, xDay);

                if (date > checkDate)
                    Result.Set(activityHelper.CodeActivityContext, false);
                else
                    Result.Set(activityHelper.CodeActivityContext, true);
            }
        }

        [RequiredArgument]
        [Input("Date")]
        public InArgument<DateTime> Date { get; set; }

        [RequiredArgument]
        [Input("Before (False) Or After (True)")]
        public InArgument<bool> BeforeOrAfter { get; set; }

        [RequiredArgument]
        [Input("X Month (Before Or After)")]
        public InArgument<int> XMonth { get; set; }

        [Input("X. Day")]
        public InArgument<int> XDay { get; set; }

        [Input("First Day Of The Month")]
        public InArgument<bool> FirstDay { get; set; }

        [Input("Last Day Of The Month")]
        public InArgument<bool> LastDay { get; set; }

        [Output("Result")]
        public OutArgument<bool> Result { get; set; }
    }
}
