using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class IsSameDay : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime firstDate = FirstDate.Get(activityHelper.CodeActivityContext);
            DateTime secondDate = SecondDate.Get(activityHelper.CodeActivityContext);
            bool evaluateAsUserLocal = EvaluateAsUserLocal.Get(activityHelper.CodeActivityContext);

            if (evaluateAsUserLocal)
            {
                int? timeZoneCode = CrmHelper.RetrieveTimeZoneCode(activityHelper.OrganizationService);
                firstDate = CrmHelper.RetrieveLocalTimeFromUtcTime(activityHelper.OrganizationService, firstDate, timeZoneCode);
                secondDate = CrmHelper.RetrieveLocalTimeFromUtcTime(activityHelper.OrganizationService, secondDate, timeZoneCode);
            }

            bool sameDay = firstDate.Date == secondDate.Date;

            SameDay.Set(activityHelper.CodeActivityContext, sameDay);
        }

        [RequiredArgument]
        [Input("First Date")]
        public InArgument<DateTime> FirstDate { get; set; }

        [RequiredArgument]
        [Input("Second Date")]
        public InArgument<DateTime> SecondDate { get; set; }

        [RequiredArgument]
        [Input("Evaluate As User Local")]
        [Default("True")]
        public InArgument<bool> EvaluateAsUserLocal { get; set; }

        [Output("Same Day")]
        public OutArgument<bool> SameDay { get; set; }
    }
}
