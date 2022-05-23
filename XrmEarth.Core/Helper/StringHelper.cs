using System;
using System.Linq;
using XrmEarth.Data.Const;

namespace XrmEarth.Core
{
    public class StringHelper
    {
        public const string RandomStringChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string RandomNumericChars = "0123456789";

        public static string RandomString(int length, string chars)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static TimeSpan GetTimeSpan(string time)
        {
            if (!string.IsNullOrEmpty(time))
            {
                var timeList = time.Split(':');
                var startBusinessHour = Convert.ToInt16(timeList[0]);
                var startBusinessMinute = Convert.ToInt16(timeList[1]);
                return new TimeSpan(startBusinessHour, startBusinessMinute, 0);
            }
            else
            {
                return new TimeSpan();
            }
        }

        public static string GetTimeSpanText(TimeSpan span, string languageCode)
        {
            string formatted = string.Empty;

            if (span.Days > 0)
            {
                if (languageCode == LanguageCode.Turkish)
                    formatted = $"{span.Days} gün ";
                else
                    formatted = $"{span.Days} day ";
            }
            if (span.Hours > 0)
            {
                if (languageCode == LanguageCode.Turkish)
                    formatted += $"{span.Hours} saat ";
                else
                    formatted += $"{span.Hours} hour ";
            }
            if (span.Minutes > 0)
            {
                if (languageCode == LanguageCode.Turkish)
                    formatted += $"{span.Minutes} dakika ";
                else
                    formatted += $"{span.Minutes} minute ";
            }

            if (!string.IsNullOrEmpty(formatted))
                formatted = formatted.Substring(0, formatted.Length - 1);

            return formatted;
        }
    }
}
