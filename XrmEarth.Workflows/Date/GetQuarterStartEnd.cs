using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class GetQuarterStartEnd : BaseCodeActivity
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

            int quarterNumber = (dateToUse.Month - 1) / 3 + 1;
            DateTime quarterStartDate = new DateTime(dateToUse.Year, (quarterNumber - 1) * 3 + 1, 1, 0, 0, 0);
            DateTime quarterEndDate = quarterStartDate.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

            QuarterStartDate.Set(activityHelper.CodeActivityContext, quarterStartDate);
            QuarterEndDate.Set(activityHelper.CodeActivityContext, quarterEndDate);
        }

        [RequiredArgument]
        [Input("Date To Use")]
        public InArgument<DateTime> DateToUse { get; set; }

        [RequiredArgument]
        [Input("Evaluate As User Local")]
        [Default("True")]
        public InArgument<bool> EvaluateAsUserLocal { get; set; }

        [Output("Quarter Start Date")]
        public OutArgument<DateTime> QuarterStartDate { get; set; }

        [Output("Quarter End Date")]
        public OutArgument<DateTime> QuarterEndDate { get; set; }
    }
}
