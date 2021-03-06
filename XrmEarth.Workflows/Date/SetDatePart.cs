using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class SetDatePart : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime dateToUpdate = DateToUpdate.Get(activityHelper.CodeActivityContext);
            string datePart = DatePart.Get(activityHelper.CodeActivityContext);
            int newValue = NewValue.Get(activityHelper.CodeActivityContext);

            string[] allowedParts = { "HR", "MIN", "SEC", "MON", "DAY", "YR" };
            int pos = Array.IndexOf(allowedParts, datePart.ToUpper());
            if (pos == -1)
                throw new InvalidPluginExecutionException("Invalid date part - must be HR/MIN/SEC MON/DAY/YR");

            DateTime updatedDate = new DateTime();
            switch (pos)
            {
                case 0:
                    if (newValue > 23 || newValue < 0)
                        throw new InvalidPluginExecutionException("Invalid hour");

                    updatedDate = UpdateDate(newValue, dateToUpdate.Minute, dateToUpdate.Second, dateToUpdate.Month,
                        dateToUpdate.Day, dateToUpdate.Year);
                    break;
                case 1:
                    if (newValue > 59 || newValue < 0)
                        throw new InvalidPluginExecutionException("Invalid minute");

                    updatedDate = UpdateDate(dateToUpdate.Hour, newValue, dateToUpdate.Second, dateToUpdate.Month,
                        dateToUpdate.Day, dateToUpdate.Year);
                    break;
                case 2:
                    if (newValue > 59 || newValue < 0)
                        throw new InvalidPluginExecutionException("Invalid second");

                    updatedDate = UpdateDate(dateToUpdate.Hour, dateToUpdate.Minute, newValue, dateToUpdate.Month,
                        dateToUpdate.Day, dateToUpdate.Year);
                    break;
                case 3:
                    if (newValue < 1 || newValue > 12)
                        throw new InvalidPluginExecutionException("Invalid month");

                    updatedDate = UpdateDate(dateToUpdate.Hour, dateToUpdate.Minute, dateToUpdate.Second, newValue,
                        dateToUpdate.Day, dateToUpdate.Year);
                    break;
                case 4:
                    if (newValue > 31 || newValue < 0)
                        throw new InvalidPluginExecutionException("Invalid day");

                    updatedDate = UpdateDate(dateToUpdate.Hour, dateToUpdate.Minute, dateToUpdate.Second, dateToUpdate.Month,
                        newValue, dateToUpdate.Year);
                    break;
                case 5:
                    if (newValue > 9999 || newValue < 1900)
                        throw new InvalidPluginExecutionException("Invalid year");

                    updatedDate = UpdateDate(dateToUpdate.Hour, dateToUpdate.Minute, dateToUpdate.Second, dateToUpdate.Month,
                        dateToUpdate.Day, newValue);
                    break;
            }

            UpdatedDate.Set(activityHelper.CodeActivityContext, updatedDate);
        }

        private static DateTime UpdateDate(int hour, int minute, int second, int month, int day, int year)
        {
            try
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception)
            {
                throw new InvalidPluginExecutionException("Update resulted in invalid date");
            }
        }

        [RequiredArgument]
        [Input("Date To Update")]
        public InArgument<DateTime> DateToUpdate { get; set; }

        [RequiredArgument]
        [Input("Date Part HR/MIN/SEC MON/DAY/YR")]
        public InArgument<string> DatePart { get; set; }

        [RequiredArgument]
        [Input("New Value")]
        public InArgument<int> NewValue { get; set; }

        [Output("Updated Date")]
        public OutArgument<DateTime> UpdatedDate { get; set; }
    }
}
