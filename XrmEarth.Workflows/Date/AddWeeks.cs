using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class AddWeeks : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime originalDate = OriginalDate.Get(activityHelper.CodeActivityContext);
            int weeksToAdd = WeeksToAdd.Get(activityHelper.CodeActivityContext);
            DateTime updatedDate = originalDate.AddDays(weeksToAdd * 7);

            UpdatedDate.Set(activityHelper.CodeActivityContext, updatedDate);
        }

        [RequiredArgument]
        [Input("Original Date")]
        public InArgument<DateTime> OriginalDate { get; set; }

        [RequiredArgument]
        [Input("Weeks To Add")]
        public InArgument<int> WeeksToAdd { get; set; }

        [Output("Updated Date")]
        public OutArgument<DateTime> UpdatedDate { get; set; }
    }
}