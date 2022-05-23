using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Globalization;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class GetWeekNumberOfYear : BaseCodeActivity
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

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            int weekNumberOfYear = cal.GetWeekOfYear(dateToUse, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            WeekNumberOfYear.Set(activityHelper.CodeActivityContext, weekNumberOfYear);
        }

        [RequiredArgument]
        [Input("Date To Use")]
        public InArgument<DateTime> DateToUse { get; set; }

        [RequiredArgument]
        [Input("Evaluate As User Local")]
        [Default("True")]
        public InArgument<bool> EvaluateAsUserLocal { get; set; }

        [Output("Week Number Of Year")]
        public OutArgument<int> WeekNumberOfYear { get; set; }
    }
}
