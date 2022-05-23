using Microsoft.Xrm.Sdk;
using System;

namespace XrmEarth.Core
{
    public class BetweenSpecificHoursHelper
    {
        public DateTime Date { get; set; }

        public bool ExitsTheWeekend { get; set; }
        public bool ExitsTheBusinessClosed { get; set; }

        public TimeSpan BusinessStartTimeSpan { get; set; }
        public TimeSpan BusinessEndTimeSpan { get; set; }
        public TimeSpan NoonHourStartTimeSpan { get; set; }
        public TimeSpan NoonHourEndTimeSpan { get; set; }

        public EntityCollection CalendarRules { get; set; }
        public IOrganizationService service { get; set; }

        public bool IsBetweenSpecificHours()
        {
            int? timeZoneCode = CrmHelper.RetrieveTimeZoneCode(service);
            Date = CrmHelper.RetrieveLocalTimeFromUtcTime(service, Date, timeZoneCode);

            if (ExitsTheWeekend)
            {
                if (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    return false;
                }
            }

            if (ExitsTheBusinessClosed)
            {
                if (IsBusinessClosed(Date, CalendarRules))
                {
                    return false;
                }
            }

            if (!IsBusinessTime(BusinessStartTimeSpan, BusinessEndTimeSpan, NoonHourStartTimeSpan, NoonHourEndTimeSpan, Date))
            {
                return false;
            }

            return true;
        }

        private bool IsBusinessClosed(DateTime date, EntityCollection calendarRules)
        {
            if (calendarRules != null && calendarRules.Entities.Count > 0)
            {
                foreach (var item in calendarRules.Entities)
                {
                    DateTime effectiveintervalstart = item.Contains("effectiveintervalstart") ? ((DateTime)item["effectiveintervalstart"]) : DateTime.MinValue;
                    DateTime effectiveintervalend = item.Contains("effectiveintervalend") ? ((DateTime)item["effectiveintervalend"]) : DateTime.MinValue;

                    if (DateTimeHelper.DateBetween(effectiveintervalstart, effectiveintervalend, date))
                    {
                        //Holiday
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsBusinessTime(TimeSpan businessStartTimeSpan, TimeSpan businessEndTimeSpan, TimeSpan noonHourStartTimeSpan, TimeSpan noonHourEndTimeSpan, DateTime date)
        {
            if (date.TimeOfDay >= businessStartTimeSpan && date.TimeOfDay < businessEndTimeSpan)
            {
                if (noonHourStartTimeSpan.TotalMinutes > 0 && date.TimeOfDay >= noonHourStartTimeSpan && date.TimeOfDay < noonHourEndTimeSpan)
                {
                    //Launch
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
