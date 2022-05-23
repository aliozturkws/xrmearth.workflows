using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class GetDayNumberOfWeek : BaseCodeActivity
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

            int dayNumberOfWeek = ((int)dateToUse.DayOfWeek + 1);

            DayNumberOfWeek.Set(activityHelper.CodeActivityContext, dayNumberOfWeek);
        }

        [RequiredArgument]
        [Input("Date To Use")]
        public InArgument<DateTime> DateToUse { get; set; }

        [RequiredArgument]
        [Input("Evaluate As User Local")]
        [Default("True")]
        public InArgument<bool> EvaluateAsUserLocal { get; set; }

        [Output("Day Number Of Week")]
        public OutArgument<int> DayNumberOfWeek { get; set; }
    }
}
