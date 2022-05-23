using Microsoft.Xrm.Sdk;
using System;

namespace XrmEarth.Core
{
    public class GetAvailableNextDayHelper
    {
        public DateTime Date { get; set; }

        public bool ExitsTheWeekend { get; set; }
        public bool ExitsTheBusinessClosed { get; set; }

        public TimeSpan HourTimeSpan { get; set; }

        public EntityCollection CalendarRules { get; set; }
        public IOrganizationService service { get; set; }

        public DateTime GetAvailableDate()
        {
            int? timeZoneCode = CrmHelper.RetrieveTimeZoneCode(service);
            Date = CrmHelper.RetrieveLocalTimeFromUtcTime(service, Date, timeZoneCode);

            DateTime firstDate = new DateTime(Date.Year, Date.Month, Date.Day, Date.Hour, Date.Minute, 0);
            DateTime resultDate = new DateTime(Date.Year, Date.Month, Date.Day, HourTimeSpan.Hours, HourTimeSpan.Minutes, 0);

            bool isBusinessDate = false;

            while (!isBusinessDate)
            {
                Date = Date.AddMinutes(1);

                if (ExitsTheWeekend)
                {
                    if (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        continue;
                    }
                }

                if (ExitsTheBusinessClosed)
                {
                    if (IsBusinessClosed(Date, CalendarRules))
                    {
                        continue;
                    }
                }

                resultDate = new DateTime(Date.Year, Date.Month, Date.Day, HourTimeSpan.Hours, HourTimeSpan.Minutes, 0);

                if (firstDate > resultDate) //If there is a previous date on the same day, it is passed.
                {
                    continue;
                }

                if (ExitsTheWeekend)
                {
                    if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        continue;
                    }
                }

                if (ExitsTheBusinessClosed)
                {
                    if (IsBusinessClosed(resultDate, CalendarRules))
                    {
                        continue;
                    }
                }

                isBusinessDate = true;
            }

            return resultDate;
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
    }
}
