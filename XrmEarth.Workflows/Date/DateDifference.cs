using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class DateDifference : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            var date1 = Date1.Get<DateTime>(activityHelper.CodeActivityContext);
            var date2 = Date2.Get<DateTime>(activityHelper.CodeActivityContext);
            var dateInterval = DateInterval.Get<string>(activityHelper.CodeActivityContext);

            DateDifferenceResult.Set(activityHelper.CodeActivityContext, WorkflowHelper.DateDifference(date1, date2, dateInterval));
        }

        [RequiredArgument]
        [Input("Date 1")]
        public InArgument<DateTime> Date1 { get; set; }

        [RequiredArgument]
        [Input("Date 2")]
        public InArgument<DateTime> Date2 { get; set; }

        [RequiredArgument]
        [Input("Date Interval")]
        public InArgument<string> DateInterval { get; set; }

        [Output("DateDifferenceResult")]
        public OutArgument<double> DateDifferenceResult { get; set; }
    }
}
