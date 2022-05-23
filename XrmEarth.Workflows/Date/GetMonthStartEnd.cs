using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class GetMonthStartEnd : BaseCodeActivity
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

            DateTime monthStartDate = new DateTime(dateToUse.Year, dateToUse.Month, 1, 0, 0, 0);
            DateTime monthEndDate = monthStartDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

            MonthStartDate.Set(activityHelper.CodeActivityContext, monthStartDate);
            MonthEndDate.Set(activityHelper.CodeActivityContext, monthEndDate);
        }

        [RequiredArgument]
        [Input("Date To Use")]
        public InArgument<DateTime> DateToUse { get; set; }

        [RequiredArgument]
        [Input("Evaluate As User Local")]
        [Default("True")]
        public InArgument<bool> EvaluateAsUserLocal { get; set; }

        [Output("Month Start Date")]
        public OutArgument<DateTime> MonthStartDate { get; set; }

        [Output("Month End Date")]
        public OutArgument<DateTime> MonthEndDate { get; set; }
    }
}
