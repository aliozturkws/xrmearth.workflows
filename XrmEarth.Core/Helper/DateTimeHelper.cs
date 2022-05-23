using System;

namespace XrmEarth.Core
{
    public class DateTimeHelper
    {
        public static bool DateBetween(DateTime startDate, DateTime enDate, DateTime betweenDate)
        {
            if (betweenDate >= startDate && betweenDate < enDate)
                return true;
            else
                return false;
        }

        public static double DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {

            TimeSpan ts = date1 - date2;

            switch (interval)
            {
                case DateInterval.Year:
                    return date1.Year - date2.Year;
                case DateInterval.Month:
                    return (date1.Month - date2.Month) + (12 * (date1.Year - date2.Year));
                case DateInterval.Weekday:
                    return ts.TotalDays / 7;
                case DateInterval.Day:
                    return ts.TotalDays;
                case DateInterval.Hour:
                    return ts.TotalHours;
                case DateInterval.Minute:
                    return ts.TotalMinutes;
                default:
                    return ts.TotalSeconds;
            }
        }

        public enum DateInterval
        {
            Year,
            Month,
            Weekday,
            Day,
            Hour,
            Minute,
            Second
        }
    }
}
