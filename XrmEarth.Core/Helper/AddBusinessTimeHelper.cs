using Microsoft.Xrm.Sdk;
using System;

namespace XrmEarth.Core
{
    public class AddBusinessTimeHelper
    {
        public DateTime Date { get; set; }
        public string DurationType { get; set; }
        public decimal Duration { get; set; }

        public bool ExitsTheWeekend { get; set; }
        public bool ExitsTheBusinessClosed { get; set; }

        public TimeSpan BusinessStartTimeSpan { get; set; }
        public TimeSpan BusinessEndTimeSpan { get; set; }
        public TimeSpan NoonHourStartTimeSpan { get; set; }
        public TimeSpan NoonHourEndTimeSpan { get; set; }
        public EntityCollection CalendarRules { get; set; }
        public IOrganizationService service { get; set; }

        public DateTime CalculatedDate()
        {
            int? timeZoneCode = CrmHelper.RetrieveTimeZoneCode(service);
            Date = CrmHelper.RetrieveLocalTimeFromUtcTime(service, Date, timeZoneCode);
            DateTime updatedDate = new DateTime(Date.Year, Date.Month, Date.Day, Date.Hour, Date.Minute, 0);

            if (DurationType == "Day" || DurationType == "Hour" || DurationType == "Minute") { } else { throw new InvalidPluginExecutionException("Duration Type should be day, hour or minute!"); }

            double totalMinuteDiff = 0;

            if (BusinessStartTimeSpan == BusinessEndTimeSpan)
                totalMinuteDiff = 1440; //24 * 60
            else
                totalMinuteDiff = (BusinessEndTimeSpan - BusinessStartTimeSpan).TotalMinutes;

            decimal addedMinute = 0;
            decimal totalMinute = (decimal)(totalMinuteDiff - (NoonHourEndTimeSpan - NoonHourStartTimeSpan).TotalMinutes);

            if (DurationType == "Day")
            {
                addedMinute = totalMinute * Duration;
            }
            else if (DurationType == "Hour")
            {
                addedMinute = Duration * 60;
            }
            else if (DurationType == "Minute")
            {
                addedMinute = Duration;
            }

            while (addedMinute > 0)
            {
                int temp = 0;

                if (ExitsTheWeekend && ExitsTheBusinessClosed)
                {
                    if (updatedDate.DayOfWeek != DayOfWeek.Saturday && updatedDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        temp = GetOnlyBusinessMinute(BusinessStartTimeSpan, BusinessEndTimeSpan, NoonHourStartTimeSpan, NoonHourEndTimeSpan, updatedDate, CalendarRules);
                    }
                }
                else if (!ExitsTheBusinessClosed && ExitsTheWeekend)
                {
                    if (updatedDate.DayOfWeek == DayOfWeek.Saturday || updatedDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        //not process
                    }
                    else
                    {
                        temp = GetOnlyWeekendMinute(BusinessStartTimeSpan, BusinessEndTimeSpan, NoonHourStartTimeSpan, NoonHourEndTimeSpan, updatedDate, CalendarRules);
                    }
                }
                else if (!ExitsTheWeekend && ExitsTheBusinessClosed)
                {
                    temp = GetOnlyBusinessMinute(BusinessStartTimeSpan, BusinessEndTimeSpan, NoonHourStartTimeSpan, NoonHourEndTimeSpan, updatedDate, CalendarRules);
                }
                else
                {
                    temp = 1;
                }

                if (temp > 0)
                {
                    addedMinute -= 1;
                }

                updatedDate = updatedDate.AddMinutes(1);
            }

            return updatedDate;
        }

        private int GetOnlyBusinessMinute(TimeSpan businessStartTimeSpan, TimeSpan businessEndTimeSpan, TimeSpan noonHourStartTimeSpan, TimeSpan noonHourEndTimeSpan, DateTime i, EntityCollection calendarRules)
        {
            int minute = 0;
            if (i.TimeOfDay >= businessStartTimeSpan && i.TimeOfDay < businessEndTimeSpan)
            {
                if (noonHourStartTimeSpan.TotalMinutes > 0 &&
                    i.TimeOfDay >= noonHourStartTimeSpan && i.TimeOfDay < noonHourEndTimeSpan)
                {
                    //Launch
                }
                else
                {
                    minute += GetMinuteBusinessClosed(i, calendarRules);
                }
            }
            return minute;
        }

        private int GetOnlyWeekendMinute(TimeSpan businessStartTimeSpan, TimeSpan businessEndTimeSpan, TimeSpan noonHourStartTimeSpan, TimeSpan noonHourEndTimeSpan, DateTime i, EntityCollection calendarRules)
        {
            int minute = 0;
            if (i.TimeOfDay >= businessStartTimeSpan && i.TimeOfDay < businessEndTimeSpan)
            {
                if (noonHourStartTimeSpan.TotalMinutes > 0 &&
                    i.TimeOfDay >= noonHourStartTimeSpan && i.TimeOfDay < noonHourEndTimeSpan)
                {
                    //Launch
                }
                else
                {
                    minute++;
                }
            }
            return minute;
        }

        private int GetMinuteBusinessClosed(DateTime i, EntityCollection calendarRules)
        {
            bool isHoliday = false;

            if (calendarRules != null && calendarRules.Entities.Count > 0)
            {
                foreach (var item in calendarRules.Entities)
                {
                    DateTime effectiveintervalstart = item.Contains("effectiveintervalstart") ? ((DateTime)item["effectiveintervalstart"]) : DateTime.MinValue;
                    DateTime effectiveintervalend = item.Contains("effectiveintervalend") ? ((DateTime)item["effectiveintervalend"]) : DateTime.MinValue;

                    if (DateTimeHelper.DateBetween(effectiveintervalstart, effectiveintervalend, i))
                    {
                        //Hoiday
                        isHoliday = true;
                    }
                }
            }

            if (!isHoliday)
                return 1;
            else
                return 0;
        }
    }
}
