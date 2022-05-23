using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class GetWeekStartEnd : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime dateToUse = DateToUse.Get(activityHelper.CodeActivityContext);
            bool evaluateAsUserLocal = EvaluateAsUserLocal.Get(activityHelper.CodeActivityContext);

            if (evaluateAsUserLocal)
            {
                int? timeZoneCode = CrmHelper.RetrieveTimeZoneCode(activityHelper.OrganizationService);
                dateToUse = CrmHelper.RetrieveLocalTimeFromUtcTime(activityHelper.OrganizationService, dateToUse, timeZoneCode);
            }

            int diff = dateToUse.DayOfWeek - DayOfWeek.Sunday;
            if (diff < 0)
                diff += 7;

            DateTime weekStartDate = dateToUse.AddDays(-1 * diff).Date;
            weekStartDate = new DateTime(weekStartDate.Year, weekStartDate.Month, weekStartDate.Day, 0, 0, 0);
            DateTime weekEndDate = weekStartDate.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

            WeekStartDate.Set(activityHelper.CodeActivityContext, weekStartDate);
            WeekEndDate.Set(activityHelper.CodeActivityContext, weekEndDate);
        }

        [RequiredArgument]
        [Input("Date To Use")]
        public InArgument<DateTime> DateToUse { get; set; }

        [RequiredArgument]
        [Input("Evaluate As User Local")]
        [Default("True")]
        public InArgument<bool> EvaluateAsUserLocal { get; set; }

        [Output("Week Start Date")]
        public OutArgument<DateTime> WeekStartDate { get; set; }

        [Output("Week End Date")]
        public OutArgument<DateTime> WeekEndDate { get; set; }
    }
}
