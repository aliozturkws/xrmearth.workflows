using Microsoft.Xrm.Sdk;
using System;

namespace XrmEarth.Core
{
    public class DurationOfBusinessHoursHelper
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool ExitsTheWeekend { get; set; }
        public bool ExitsTheBusinessClosed { get; set; }

        public TimeSpan BusinessStartTimeSpan { get; set; }
        public TimeSpan BusinessEndTimeSpan { get; set; }
        public TimeSpan NoonHourStartTimeSpan { get; set; }
        public TimeSpan NoonHourEndTimeSpan { get; set; }
        public EntityCollection CalendarRules { get; set; }
        public IOrganizationService service { get; set; }

        public int DurationOfBusinessHours()
        {
            int? timeZoneCode = CrmHelper.RetrieveTimeZoneCode(service);
            StartDate = CrmHelper.RetrieveLocalTimeFromUtcTime(service, StartDate, timeZoneCode);
            EndDate = CrmHelper.RetrieveLocalTimeFromUtcTime(service, EndDate, timeZoneCode);

            int totalMinute = 0;

            for (var i = StartDate; i < EndDate; i = i.AddMinutes(1))
            {
                if (ExitsTheWeekend && ExitsTheBusinessClosed)
                {
                    if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                    {
                        totalMinute += GetOnlyBusinessMinute(BusinessStartTimeSpan, BusinessEndTimeSpan, NoonHourStartTimeSpan, NoonHourEndTimeSpan, i, CalendarRules);
                    }
                }
                else if (!ExitsTheBusinessClosed && ExitsTheWeekend)
                {
                    if (i.DayOfWeek == DayOfWeek.Saturday || i.DayOfWeek == DayOfWeek.Sunday)
                        continue;
                    else
                        totalMinute += GetOnlyWeekendMinute(BusinessStartTimeSpan, BusinessEndTimeSpan, NoonHourStartTimeSpan, NoonHourEndTimeSpan, i, CalendarRules);
                }
                else if (!ExitsTheWeekend && ExitsTheBusinessClosed)
                {
                    totalMinute += GetOnlyBusinessMinute(BusinessStartTimeSpan, BusinessEndTimeSpan, NoonHourStartTimeSpan, NoonHourEndTimeSpan, i, CalendarRules);
                }
                else
                {
                    totalMinute++;
                }
            }

            return totalMinute;
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
                        //Holiday
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
