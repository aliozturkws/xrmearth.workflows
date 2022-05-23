using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class GetYearStartEnd : BaseCodeActivity
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

            DateTime yearStartDate = new DateTime(dateToUse.Year, 1, 1, 0, 0, 0);
            DateTime yearEndDate = yearStartDate.AddYears(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

            YearStartDate.Set(activityHelper.CodeActivityContext, yearStartDate);
            YearEndDate.Set(activityHelper.CodeActivityContext, yearEndDate);
        }

        [RequiredArgument]
        [Input("Date To Use")]
        public InArgument<DateTime> DateToUse { get; set; }

        [RequiredArgument]
        [Input("Evaluate As User Local")]
        [Default("True")]
        public InArgument<bool> EvaluateAsUserLocal { get; set; }

        [Output("Year Start Date")]
        public OutArgument<DateTime> YearStartDate { get; set; }

        [Output("Year End Date")]
        public OutArgument<DateTime> YearEndDate { get; set; }
    }
}
