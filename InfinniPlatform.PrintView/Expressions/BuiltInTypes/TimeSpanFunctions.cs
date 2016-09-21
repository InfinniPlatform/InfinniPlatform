using System;

namespace InfinniPlatform.PrintView.Expressions.BuiltInTypes
{
    internal static class TimeSpanFunctions
    {
        public static readonly TimeSpan Zero = TimeSpan.Zero;
        public static readonly TimeSpan MinValue = TimeSpan.MinValue;
        public static readonly TimeSpan MaxValue = TimeSpan.MaxValue;

        public static TimeSpan Create(int days, int hours, int minutes, int seconds, int milliseconds = 0)
        {
            return new TimeSpan(days, hours, minutes, seconds, milliseconds);
        }

        public static TimeSpan FromDays(dynamic value)
        {
            return TimeSpan.FromDays(value);
        }

        public static TimeSpan FromHours(dynamic value)
        {
            return TimeSpan.FromHours(value);
        }

        public static TimeSpan FromMinutes(dynamic value)
        {
            return TimeSpan.FromMinutes(value);
        }

        public static TimeSpan FromSeconds(dynamic value)
        {
            return TimeSpan.FromSeconds(value);
        }

        public static TimeSpan FromMilliseconds(dynamic value)
        {
            return TimeSpan.FromMilliseconds(value);
        }

        public static int? Days(TimeSpan? value)
        {
            return (value != null) ? value.Value.Days : (int?) null;
        }

        public static double? TotalDays(TimeSpan? value)
        {
            return (value != null) ? value.Value.TotalDays : (double?) null;
        }

        public static int? Hours(TimeSpan? value)
        {
            return (value != null) ? value.Value.Hours : (int?) null;
        }

        public static double? TotalHours(TimeSpan? value)
        {
            return (value != null) ? value.Value.TotalHours : (double?) null;
        }

        public static int? Minutes(TimeSpan? value)
        {
            return (value != null) ? value.Value.Minutes : (int?) null;
        }

        public static double? TotalMinutes(TimeSpan? value)
        {
            return (value != null) ? value.Value.TotalMinutes : (double?) null;
        }

        public static int? Seconds(TimeSpan? value)
        {
            return (value != null) ? value.Value.Seconds : (int?) null;
        }

        public static double? TotalSeconds(TimeSpan? value)
        {
            return (value != null) ? value.Value.TotalSeconds : (double?) null;
        }

        public static int? Milliseconds(TimeSpan? value)
        {
            return (value != null) ? value.Value.Milliseconds : (int?) null;
        }

        public static double? TotalMilliseconds(TimeSpan? value)
        {
            return (value != null) ? value.Value.TotalMilliseconds : (double?) null;
        }

        public static TimeSpan? Negate(TimeSpan? value)
        {
            return (value != null) ? value.Value.Negate() : (TimeSpan?) null;
        }

        public static TimeSpan? Duration(TimeSpan? value)
        {
            return (value != null) ? value.Value.Duration() : (TimeSpan?) null;
        }

        public static TimeSpan? Add(TimeSpan? left, TimeSpan? right)
        {
            return (left != null && right != null) ? left.Value.Add(right.Value) : left;
        }

        public static TimeSpan? Subtract(TimeSpan? left, TimeSpan? right)
        {
            return (left != null && right != null) ? left.Value.Subtract(right.Value) : left;
        }

        public static bool Equals(TimeSpan? left, TimeSpan? right)
        {
            return (left == right) || (left != null && right != null && TimeSpan.Equals(left.Value, right.Value));
        }
    }
}