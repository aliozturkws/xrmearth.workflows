using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using XrmEarth.Core.Activity;
using XrmEarth.Data.Const;

namespace XrmEarth.Workflows.Date
{
    public class AddBusinessDays : BaseCodeActivity
    {
        protected override void OnExecute(CodeActivityHelper activityHelper)
        {
            DateTime originalDate = OriginalDate.Get(activityHelper.CodeActivityContext);
            int businessDaysToAdd = BusinessDaysToAdd.Get(activityHelper.CodeActivityContext);
            EntityReference holidaySchedule = HolidayClosureCalendar.Get(activityHelper.CodeActivityContext);

            Entity calendar = null;
            EntityCollection calendarRules = null;
            if (holidaySchedule != null)
            {
                calendar = activityHelper.OrganizationService.Retrieve(EntityNames.Calendar, holidaySchedule.Id, new ColumnSet(true));
                if (calendar != null)
                    calendarRules = calendar.GetAttributeValue<EntityCollection>("calendarrules");
            }

            DateTime tempDate = originalDate;

            if (businessDaysToAdd > 0)
            {
                while (businessDaysToAdd > 0)
                {
                    tempDate = tempDate.AddDays(1);
                    if (tempDate.DayOfWeek == DayOfWeek.Sunday || tempDate.DayOfWeek == DayOfWeek.Saturday)
                        continue;

                    if (calendar == null)
                    {
                        businessDaysToAdd--;
                        continue;
                    }

                    bool isHoliday = false;
                    foreach (Entity calendarRule in calendarRules.Entities)
                    {
                        DateTime startTime = calendarRule.GetAttributeValue<DateTime>("starttime");

                        //Not same date
                        if (!startTime.Date.Equals(tempDate.Date))
                            continue;

                        //Not full day event
                        if (startTime.Subtract(startTime.TimeOfDay) != startTime || calendarRule.GetAttributeValue<int>("duration") != 1440)
                            continue;

                        isHoliday = true;
                        break;
                    }
                    if (!isHoliday)
                        businessDaysToAdd--;
                }
            }
            else if (businessDaysToAdd < 0)
            {
                while (businessDaysToAdd < 0)
                {
                    tempDate = tempDate.AddDays(-1);
                    if (tempDate.DayOfWeek == DayOfWeek.Sunday || tempDate.DayOfWeek == DayOfWeek.Saturday)
                        continue;

                    if (calendar == null)
                    {
                        businessDaysToAdd++;
                        continue;
                    }

                    bool isHoliday = false;
                    foreach (Entity calendarRule in calendarRules.Entities)
                    {
                        DateTime startTime = calendarRule.GetAttributeValue<DateTime>("starttime");

                        //Not same date
                        if (!startTime.Date.Equals(tempDate.Date))
                            continue;

                        //Not full day event
                        if (startTime.Subtract(startTime.TimeOfDay) != startTime || calendarRule.GetAttributeValue<int>("duration") != 1440)
                            continue;

                        isHoliday = true;
                        break;
                    }
                    if (!isHoliday)
                        businessDaysToAdd++;
                }
            }

            DateTime updatedDate = tempDate;

            UpdatedDate.Set(activityHelper.CodeActivityContext, updatedDate);
        }

        [RequiredArgument]
        [Input("Original Date")]
        public InArgument<DateTime> OriginalDate { get; set; }

        [RequiredArgument]
        [Input("Business Days To Add")]
        public InArgument<int> BusinessDaysToAdd { get; set; }

        [Input("Holiday/Closure Calendar")]
        [ReferenceTarget(EntityNames.Calendar)]
        public InArgument<EntityReference> HolidayClosureCalendar { get; set; }

        [Output("Updated Date")]
        public OutArgument<DateTime> UpdatedDate { get; set; }
    }
}