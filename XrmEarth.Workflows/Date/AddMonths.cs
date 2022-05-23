using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class AddMonths : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime originalDate = OriginalDate.Get(activityHelper.CodeActivityContext);
            int monthsToAdd = MonthsToAdd.Get(activityHelper.CodeActivityContext);
            DateTime updatedDate = originalDate.AddMonths(monthsToAdd);

            UpdatedDate.Set(activityHelper.CodeActivityContext, updatedDate);
        }

        [RequiredArgument]
        [Input("Original Date")]
        public InArgument<DateTime> OriginalDate { get; set; }

        [RequiredArgument]
        [Input("Months To Add")]
        public InArgument<int> MonthsToAdd { get; set; }

        [Output("Updated Date")]
        public OutArgument<DateTime> UpdatedDate { get; set; }
    }
}