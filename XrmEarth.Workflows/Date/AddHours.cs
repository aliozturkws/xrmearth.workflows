using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class AddHours : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime originalDate = OriginalDate.Get(activityHelper.CodeActivityContext);
            int hoursToAdd = HoursToAdd.Get(activityHelper.CodeActivityContext);
            DateTime updatedDate = originalDate.AddHours(hoursToAdd);

            UpdatedDate.Set(activityHelper.CodeActivityContext, updatedDate);
        }

        [RequiredArgument]
        [Input("Original Date")]
        public InArgument<DateTime> OriginalDate { get; set; }

        [RequiredArgument]
        [Input("Hours To Add")]
        public InArgument<int> HoursToAdd { get; set; }

        [Output("Updated Date")]
        public OutArgument<DateTime> UpdatedDate { get; set; }
    }
}