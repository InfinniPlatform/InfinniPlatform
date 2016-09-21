using System;
using System.Globalization;

namespace InfinniPlatform.PrintView.Expressions.BuiltInTypes
{
    internal static class DateTimeFunctions
    {
        public static readonly DateTime MinValue = DateTime.MinValue;
        public static readonly DateTime MaxValue = DateTime.MaxValue;

        public static DateTime Now()
        {
            return DateTime.Now;
        }

        public static DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public static DateTime Today()
        {
            return DateTime.Today;
        }

        public static DateTime Create(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }

        public static DateTime Create(int year, int month, int day, int hour, int minute, int second)
        {
            return new DateTime(year, month, day, hour, minute, second);
        }

        public static DateTime Create(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }

        public static int? Year(DateTime? value)
        {
            return (value != null) ? value.Value.Year : (int?) null;
        }

        public static int? Month(DateTime? value)
        {
            return (value != null) ? value.Value.Month : (int?) null;
        }

        public static int? Day(DateTime? value)
        {
            return (value != null) ? value.Value.Day : (int?) null;
        }

        public static int? Hour(DateTime? value)
        {
            return (value != null) ? value.Value.Hour : (int?) null;
        }

        public static int? Minute(DateTime? value)
        {
            return (value != null) ? value.Value.Minute : (int?) null;
        }

        public static int? Second(DateTime? value)
        {
            return (value != null) ? value.Value.Second : (int?) null;
        }

        public static int? Millisecond(DateTime? value)
        {
            return (value != null) ? value.Value.Millisecond : (int?) null;
        }

        public static int? DayOfYear(DateTime? value)
        {
            return (value != null) ? value.Value.DayOfYear : (int?) null;
        }

        public static int? DayOfWeek(DateTime? value)
        {
            return (value != null) ? (int) value.Value.DayOfWeek : (int?) null;
        }

        public static DateTime? Add(DateTime? value, TimeSpan? span)
        {
            return (value != null && span != null) ? value.Value.Add(span.Value) : value;
        }

        public static DateTime? AddYears(DateTime? value, int years)
        {
            return (value != null) ? value.Value.AddYears(years) : (DateTime?) null;
        }

        public static DateTime? AddMonths(DateTime? value, int months)
        {
            return (value != null) ? value.Value.AddMonths(months) : (DateTime?) null;
        }

        public static DateTime? AddDays(DateTime? value, int days)
        {
            return (value != null) ? value.Value.AddDays(days) : (DateTime?) null;
        }

        public static DateTime? AddHours(DateTime? value, int hours)
        {
            return (value != null) ? value.Value.AddHours(hours) : (DateTime?) null;
        }

        public static DateTime? AddMinutes(DateTime? value, int minutes)
        {
            return (value != null) ? value.Value.AddMinutes(minutes) : (DateTime?) null;
        }

        public static DateTime? AddSeconds(DateTime? value, int seconds)
        {
            return (value != null) ? value.Value.AddSeconds(seconds) : (DateTime?) null;
        }

        public static DateTime? AddMilliseconds(DateTime? value, int milliseconds)
        {
            return (value != null) ? value.Value.AddMilliseconds(milliseconds) : (DateTime?) null;
        }

        public static TimeSpan? Subtract(DateTime? left, DateTime? right)
        {
            return (left != null && right != null) ? (left - right) : null;
        }

        public static bool Equals(DateTime? left, DateTime? right)
        {
            return (left == right) || (left != null && right != null && DateTime.Equals(left.Value, right.Value));
        }

        public static int DaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        public static string MonthName(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        public static string DayName(int dayOfWeek)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek) dayOfWeek);
        }
    }
}