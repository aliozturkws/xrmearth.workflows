using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class AddDays : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime originalDate = OriginalDate.Get(activityHelper.CodeActivityContext);
            int daysToAdd = DaysToAdd.Get(activityHelper.CodeActivityContext);
            DateTime updatedDate = originalDate.AddDays(daysToAdd);

            UpdatedDate.Set(activityHelper.CodeActivityContext, updatedDate);
        }

        [RequiredArgument]
        [Input("Original Date")]
        public InArgument<DateTime> OriginalDate { get; set; }

        [RequiredArgument]
        [Input("Days To Add")]
        public InArgument<int> DaysToAdd { get; set; }

        [Output("Updated Date")]
        public OutArgument<DateTime> UpdatedDate { get; set; }
    }
}