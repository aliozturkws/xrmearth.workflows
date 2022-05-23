using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class SetTime : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime dateToUpdate = DateToUpdate.Get(activityHelper.CodeActivityContext);
            int hour = Hour.Get(activityHelper.CodeActivityContext);
            if (hour > 23 || hour < 0)
                throw new InvalidPluginExecutionException("Invalid hour");

            int minute = Minute.Get(activityHelper.CodeActivityContext);
            if (minute > 59 || minute < 0)
                throw new InvalidPluginExecutionException("Invalid minute");

            int second = Second.Get(activityHelper.CodeActivityContext);
            if (second > 59 || second < 0)
                throw new InvalidPluginExecutionException("Invalid second");

            DateTime updatedDate = new DateTime(dateToUpdate.Year, dateToUpdate.Month, dateToUpdate.Day, hour, minute, second);

            UpdatedDate.Set(activityHelper.CodeActivityContext, updatedDate);
        }

        [RequiredArgument]
        [Input("Date To Update")]
        public InArgument<DateTime> DateToUpdate { get; set; }

        [RequiredArgument]
        [Input("Hour (24-hour)")]
        [Default("12")]
        public InArgument<int> Hour { get; set; }

        [RequiredArgument]
        [Input("Minute")]
        [Default("0")]
        public InArgument<int> Minute { get; set; }

        [RequiredArgument]
        [Input("Second")]
        [Default("0")]
        public InArgument<int> Second { get; set; }

        [Output("Updated Date")]
        public OutArgument<DateTime> UpdatedDate { get; set; }
    }
}
