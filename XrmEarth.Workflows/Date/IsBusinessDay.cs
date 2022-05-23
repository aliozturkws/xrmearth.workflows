using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core;
using XrmEarth.Core.Activity;

namespace XrmEarth.Workflows.Date
{
    public class IsBusinessDay : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime dateToCheck = DateToCheck.Get(activityHelper.CodeActivityContext);
            bool evaluateAsUserLocal = EvaluateAsUserLocal.Get(activityHelper.CodeActivityContext);

            if (evaluateAsUserLocal)
            {
                int? timeZoneCode = CrmHelper.RetrieveTimeZoneCode(activityHelper.OrganizationService);
                dateToCheck = CrmHelper.RetrieveLocalTimeFromUtcTime(activityHelper.OrganizationService, dateToCheck, timeZoneCode);
            }

            EntityReference holidaySchedule = HolidayClosureCalendar.Get(activityHelper.CodeActivityContext);

            bool validBusinessDay = dateToCheck.DayOfWeek != DayOfWeek.Saturday || dateToCheck.DayOfWeek == DayOfWeek.Sunday;

            if (!validBusinessDay)
            {
                ValidBusinessDay.Set(activityHelper.CodeActivityContext, false);
                return;
            }

            if (holidaySchedule != null)
            {
                Entity calendar = activityHelper.OrganizationService.Retrieve("calendar", holidaySchedule.Id, new ColumnSet(true));
                if (calendar == null) return;

                EntityCollection calendarRules = calendar.GetAttributeValue<EntityCollection>("calendarrules");
                foreach (Entity calendarRule in calendarRules.Entities)
                {
                    //Date is not stored as UTC
                    DateTime startTime = calendarRule.GetAttributeValue<DateTime>("starttime");

                    //Not same date
                    if (!startTime.Date.Equals(dateToCheck.Date))
                        continue;

                    //Not full day event
                    if (startTime.Subtract(startTime.TimeOfDay) != startTime || calendarRule.GetAttributeValue<int>("duration") != 1440)
                        continue;

                    ValidBusinessDay.Set(activityHelper.CodeActivityContext, false);
                    return;
                }
            }

            ValidBusinessDay.Set(activityHelper.CodeActivityContext, true);
        }

        [RequiredArgument]
        [Input("Date To Check")]
        public InArgument<DateTime> DateToCheck { get; set; }

        [Input("Holiday/Closure Calendar")]
        [ReferenceTarget("calendar")]
        public InArgument<EntityReference> HolidayClosureCalendar { get; set; }

        [RequiredArgument]
        [Input("Evaluate As User Local")]
        [Default("True")]
        public InArgument<bool> EvaluateAsUserLocal { get; set; }

        [Output("Valid Business Day")]
        public OutArgument<bool> ValidBusinessDay { get; set; }
    }
}