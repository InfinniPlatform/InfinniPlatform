using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using InfinniPlatform.PrintView.Expressions.Parser;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Expressions
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class ExpressionExecutorTest
    {
        private static readonly Dictionary<string, object> ObjectCreationExpressionCases
            = new Dictionary<string, object>
              {
                  { "new int()", new int() },
                  { "new System.Int32()", new int() },
                  { "new System.DateTime(2015, 1, 2)", new DateTime(2015, 1, 2) },
                  { "new System.Collections.Generic.List<int> { 1, 2, 3 }", new List<int> { 1, 2, 3 } },
                  { "new System.Collections.Generic.Dictionary<int, char> { { 1, 'A' }, { 2, 'B' } }", new Dictionary<int, char> { { 1, 'A' }, { 2, 'B' } } }
              };

        private static readonly Dictionary<string, Array> ArrayCreationExpressionCases
            = new Dictionary<string, Array>
              {
                  {
                      "new[]",
                      new object[] { }
                  },
                  {
                      "new[] { }",
                      new object[] { }
                  },
                  {
                      "new[] { 1, 2, 3 }",
                      new[] { 1, 2, 3 }
                  },
                  {
                      "new[] { 'A', 'b', 'c' }",
                      new[] { 'A', 'b', 'c' }
                  },
                  {
                      "new[] { \"A\", \"b\", \"c\", null }",
                      new[] { "A", "b", "c", null }
                  },
                  {
                      "new[] { 1, 1.5, 'A', \"Abc\", null }",
                      new object[] { 1, 1.5, 'A', "Abc", null }
                  },
                  {
                      "new[,] { { 1, 2, 3 }, { 4, 5, 6 } }",
                      new[,] { { 1, 2, 3 }, { 4, 5, 6 } }
                  },
                  {
                      "new[] { new[] { 11 }, new[] { 21, 22 } }",
                      new[] { new[] { 11 }, new[] { 21, 22 } }
                  },
                  {
                      "new int[]",
                      new int[] { }
                  },
                  {
                      "new int[] { }",
                      new int[] { }
                  },
                  {
                      "new int[0]",
                      new int[] { }
                  },
                  {
                      "new int[0] { }",
                      new int[] { }
                  },
                  {
                      "new int[] { 1, 2, 3 }",
                      new[] { 1, 2, 3 }
                  },
                  {
                      "new int[3] { 1, 2, 3 }",
                      new[] { 1, 2, 3 }
                  },
                  {
                      "new int[,] { { 1, 2, 3 }, { 4, 5, 6 } }",
                      new[,] { { 1, 2, 3 }, { 4, 5, 6 } }
                  },
                  {
                      "new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } }",
                      new[,] { { 1, 2, 3 }, { 4, 5, 6 } }
                  },
                  {
                      "new int[][] { new[] { 11 }, new[] { 21, 22 } }",
                      new[] { new[] { 11 }, new[] { 21, 22 } }
                  },
                  {
                      "new int[2][] { new[] { 11 }, new[] { 21, 22 } }",
                      new[] { new[] { 11 }, new[] { 21, 22 } }
                  },
                  {
                      "new[,] { { new[] { 1 }, new[] { 2 }, new[] { 3 } }, { new[] { 4 }, new[] { 5 }, new[] { 6 } } }",
                      new[,] { { new[] { 1 }, new[] { 2 }, new[] { 3 } }, { new[] { 4 }, new[] { 5 }, new[] { 6 } } }
                  },
                  {
                      "new int[,][] { { new[] { 1 }, new[] { 2 }, new[] { 3 } }, { new[] { 4 }, new[] { 5 }, new[] { 6 } } }",
                      new[,] { { new[] { 1 }, new[] { 2 }, new[] { 3 } }, { new[] { 4 }, new[] { 5 }, new[] { 6 } } }
                  },
                  {
                      "new int[2, 3][] { { new[] { 1 }, new[] { 2 }, new[] { 3 } }, { new[] { 4 }, new[] { 5 }, new[] { 6 } } }",
                      new[,] { { new[] { 1 }, new[] { 2 }, new[] { 3 } }, { new[] { 4 }, new[] { 5 }, new[] { 6 } } }
                  },
                  {
                      "new System.Int32[] { 1, 2, 3 }",
                      new[] { 1, 2, 3 }
                  }
              };

        private static readonly Dictionary<string, object> AnonymousObjectCreationExpressionCases
            = new Dictionary<string, object>
              {
                  {
                      "new { Property1 = 1, Property2 = 'A' }",
                      new { Property1 = 1, Property2 = 'A' }
                  },
                  {
                      "new { Property1 = 1, Property2 = new { SubProperty1 = 1.5 } }",
                      new { Property1 = 1, Property2 = new { SubProperty1 = 1.5 } }
                  }
              };

        private static readonly Dictionary<string, object> DateTimeFunctionsCases
            = new Dictionary<string, object>
              {
                  { "DateTime.MinValue", DateTime.MinValue },
                  { "DateTime.MaxValue", DateTime.MaxValue },
                  { "DateTime.Now() != null", true },
                  { "DateTime.UtcNow() != null", true },
                  { "DateTime.Today() != null", true },
                  { "DateTime.Create(2015, 1, 2)", new DateTime(2015, 1, 2) },
                  { "DateTime.Create(2015, 1, 2, 3, 4, 5)", new DateTime(2015, 1, 2, 3, 4, 5) },
                  { "DateTime.Create(2015, 1, 2, 3, 4, 5, 6)", new DateTime(2015, 1, 2, 3, 4, 5, 6) },
                  { "DateTime.Equals(null, null)", true },
                  { "DateTime.Equals(null, DateTime.Create(2015, 1, 2))", false },
                  { "DateTime.Equals(DateTime.Create(2015, 1, 2), null)", false },
                  { "DateTime.Equals(DateTime.Create(2015, 1, 2), DateTime.Create(2015, 1, 3))", false },
                  { "DateTime.DaysInMonth(2015, 1)", 31 },
                  { "DateTime.DaysInMonth(2015, 2)", 28 },
                  { "DateTime.Year(null)", null },
                  { "DateTime.Year(DateTime.Create(2015, 1, 2, 3, 4, 5, 6))", 2015 },
                  { "DateTime.Month(null)", null },
                  { "DateTime.Month(DateTime.Create(2015, 1, 2, 3, 4, 5, 6))", 1 },
                  { "DateTime.Day(null)", null },
                  { "DateTime.Day(DateTime.Create(2015, 1, 2, 3, 4, 5, 6))", 2 },
                  { "DateTime.Hour(null)", null },
                  { "DateTime.Hour(DateTime.Create(2015, 1, 2, 3, 4, 5, 6))", 3 },
                  { "DateTime.Minute(null)", null },
                  { "DateTime.Minute(DateTime.Create(2015, 1, 2, 3, 4, 5, 6))", 4 },
                  { "DateTime.Second(null)", null },
                  { "DateTime.Second(DateTime.Create(2015, 1, 2, 3, 4, 5, 6))", 5 },
                  { "DateTime.Millisecond(null)", null },
                  { "DateTime.Millisecond(DateTime.Create(2015, 1, 2, 3, 4, 5, 6))", 6 },
                  { "DateTime.AddYears(null, 1)", null },
                  { "DateTime.AddYears(DateTime.Create(2015, 1, 2, 3, 4, 5, 6), 1)", new DateTime(2016, 1, 2, 3, 4, 5, 6) },
                  { "DateTime.AddMonths(null, 1)", null },
                  { "DateTime.AddMonths(DateTime.Create(2015, 1, 2, 3, 4, 5, 6), 1)", new DateTime(2015, 2, 2, 3, 4, 5, 6) },
                  { "DateTime.AddDays(null, 1)", null },
                  { "DateTime.AddDays(DateTime.Create(2015, 1, 2, 3, 4, 5, 6), 1)", new DateTime(2015, 1, 3, 3, 4, 5, 6) },
                  { "DateTime.AddHours(null, 1)", null },
                  { "DateTime.AddHours(DateTime.Create(2015, 1, 2, 3, 4, 5, 6), 1)", new DateTime(2015, 1, 2, 4, 4, 5, 6) },
                  { "DateTime.AddMinutes(null, 1)", null },
                  { "DateTime.AddMinutes(DateTime.Create(2015, 1, 2, 3, 4, 5, 6), 1)", new DateTime(2015, 1, 2, 3, 5, 5, 6) },
                  { "DateTime.AddSeconds(null, 1)", null },
                  { "DateTime.AddSeconds(DateTime.Create(2015, 1, 2, 3, 4, 5, 6), 1)", new DateTime(2015, 1, 2, 3, 4, 6, 6) },
                  { "DateTime.AddMilliseconds(null, 1)", null },
                  { "DateTime.AddMilliseconds(DateTime.Create(2015, 1, 2, 3, 4, 5, 6), 1)", new DateTime(2015, 1, 2, 3, 4, 5, 7) },
                  { "DateTime.DayOfYear(null)", null },
                  { "DateTime.DayOfYear(DateTime.Create(2015, 1, 1))", 1 },
                  { "DateTime.DayOfYear(DateTime.Create(2015, 1, 5))", 5 },
                  { "DateTime.DayOfYear(DateTime.Create(2015, 2, 1))", 32 },
                  { "DateTime.DayOfWeek(null)", null },
                  { "DateTime.DayOfWeek(DateTime.Create(2015, 2, 16))", 1 }, // ПН
                  { "DateTime.DayOfWeek(DateTime.Create(2015, 2, 17))", 2 }, // ВТ
                  { "DateTime.DayOfWeek(DateTime.Create(2015, 2, 18))", 3 }, // СР
                  { "DateTime.DayOfWeek(DateTime.Create(2015, 2, 19))", 4 }, // ЧТ
                  { "DateTime.DayOfWeek(DateTime.Create(2015, 2, 20))", 5 }, // ПТ
                  { "DateTime.DayOfWeek(DateTime.Create(2015, 2, 21))", 6 }, // СБ
                  { "DateTime.DayOfWeek(DateTime.Create(2015, 2, 22))", 0 }, // ВС
                  { "DateTime.MonthName(1)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1) },
                  { "DateTime.MonthName(2)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(2) },
                  { "DateTime.MonthName(3)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(3) },
                  { "DateTime.MonthName(4)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(4) },
                  { "DateTime.MonthName(5)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(5) },
                  { "DateTime.MonthName(6)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(6) },
                  { "DateTime.MonthName(7)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(7) },
                  { "DateTime.MonthName(8)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(8) },
                  { "DateTime.MonthName(9)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(9) },
                  { "DateTime.MonthName(10)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(10) },
                  { "DateTime.MonthName(11)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(11) },
                  { "DateTime.MonthName(12)", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(12) },
                  { "DateTime.DayName(1)", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOfWeek.Monday) },
                  { "DateTime.DayName(2)", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOfWeek.Tuesday) },
                  { "DateTime.DayName(3)", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOfWeek.Wednesday) },
                  { "DateTime.DayName(4)", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOfWeek.Thursday) },
                  { "DateTime.DayName(5)", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOfWeek.Friday) },
                  { "DateTime.DayName(6)", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOfWeek.Saturday) },
                  { "DateTime.DayName(0)", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOfWeek.Sunday) },
                  { "DateTime.Subtract(DateTime.Create(2015, 2, 1), DateTime.Create(2015, 1, 1))", TimeSpan.FromDays(31) }
              };

        private static readonly Dictionary<string, object> TimeSpanFunctionsCases
            = new Dictionary<string, object>
              {
                  { "TimeSpan.Zero", TimeSpan.Zero },
                  { "TimeSpan.MinValue", TimeSpan.MinValue },
                  { "TimeSpan.MaxValue", TimeSpan.MaxValue },
                  { "TimeSpan.FromDays(1)", TimeSpan.FromDays(1) },
                  { "TimeSpan.FromDays(1.5)", TimeSpan.FromDays(1.5) },
                  { "TimeSpan.FromHours(1)", TimeSpan.FromHours(1) },
                  { "TimeSpan.FromHours(1.5)", TimeSpan.FromHours(1.5) },
                  { "TimeSpan.FromMinutes(1)", TimeSpan.FromMinutes(1) },
                  { "TimeSpan.FromMinutes(1.5)", TimeSpan.FromMinutes(1.5) },
                  { "TimeSpan.FromSeconds(1)", TimeSpan.FromSeconds(1) },
                  { "TimeSpan.FromSeconds(1.5)", TimeSpan.FromSeconds(1.5) },
                  { "TimeSpan.FromMilliseconds(1)", TimeSpan.FromMilliseconds(1) },
                  { "TimeSpan.FromMilliseconds(1.5)", TimeSpan.FromMilliseconds(1.5) },
                  { "TimeSpan.Create(1, 2, 3, 4)", new TimeSpan(1, 2, 3, 4, 0) },
                  { "TimeSpan.Create(1, 2, 3, 4, 5)", new TimeSpan(1, 2, 3, 4, 5) },
                  { "TimeSpan.Days(null)", null },
                  { "TimeSpan.Days(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).Days },
                  { "TimeSpan.TotalDays(null)", null },
                  { "TimeSpan.TotalDays(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).TotalDays },
                  { "TimeSpan.Hours(null)", null },
                  { "TimeSpan.Hours(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).Hours },
                  { "TimeSpan.TotalHours(null)", null },
                  { "TimeSpan.TotalHours(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).TotalHours },
                  { "TimeSpan.Minutes(null)", null },
                  { "TimeSpan.Minutes(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).Minutes },
                  { "TimeSpan.TotalMinutes(null)", null },
                  { "TimeSpan.TotalMinutes(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).TotalMinutes },
                  { "TimeSpan.Seconds(null)", null },
                  { "TimeSpan.Seconds(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).Seconds },
                  { "TimeSpan.TotalSeconds(null)", null },
                  { "TimeSpan.TotalSeconds(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).TotalSeconds },
                  { "TimeSpan.Milliseconds(null)", null },
                  { "TimeSpan.Milliseconds(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).Milliseconds },
                  { "TimeSpan.TotalMilliseconds(null)", null },
                  { "TimeSpan.TotalMilliseconds(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).TotalMilliseconds },
                  { "TimeSpan.Negate(null)", null },
                  { "TimeSpan.Negate(TimeSpan.Create(1, 2, 3, 4, 5))", new TimeSpan(1, 2, 3, 4, 5).Negate() },
                  { "TimeSpan.Duration(null)", null },
                  { "TimeSpan.Duration(-TimeSpan.Create(1, 2, 3, 4, 5))", (-new TimeSpan(1, 2, 3, 4, 5)).Duration() },
                  { "TimeSpan.Add(null, null)", null },
                  { "TimeSpan.Add(null, TimeSpan.FromDays(2))", null },
                  { "TimeSpan.Add(TimeSpan.FromDays(1), null)", null },
                  { "TimeSpan.Add(TimeSpan.FromDays(1), TimeSpan.FromDays(2))", TimeSpan.FromDays(3) },
                  { "TimeSpan.Subtract(null, null)", null },
                  { "TimeSpan.Subtract(null, TimeSpan.FromDays(2))", null },
                  { "TimeSpan.Subtract(TimeSpan.FromDays(1), null)", null },
                  { "TimeSpan.Subtract(TimeSpan.FromDays(1), TimeSpan.FromDays(2))", TimeSpan.FromDays(-1) },
                  { "TimeSpan.Equals(null, null)", true },
                  { "TimeSpan.Equals(null, TimeSpan.FromDays(1))", false },
                  { "TimeSpan.Equals(TimeSpan.FromDays(1), null)", false },
                  { "TimeSpan.Equals(TimeSpan.FromDays(1), TimeSpan.FromDays(2))", false },
                  { "TimeSpan.Equals(TimeSpan.FromDays(1), TimeSpan.FromDays(1))", true }
              };

        private static readonly Dictionary<string, object> EnumerableFunctionsCases
            = new Dictionary<string, object>
              {
                  { "Enumerable.Empty<int>()", new int[] { } },
                  { "Enumerable.Empty<object>()", new object[] { } },
                  { "Enumerable.Range(1, 5)", new[] { 1, 2, 3, 4, 5 } },
                  { "Enumerable.Repeat(1, 5)", new[] { 1, 1, 1, 1, 1 } },
                  { "Enumerable.All(null, i => i == 1)", true },
                  { "Enumerable.All(new[] { }, i => i == 1)", true },
                  { "Enumerable.All(new[] { 1, 1, 1 }, i => i == 1)", true },
                  { "Enumerable.All(new[] { 1, 1, 2 }, i => i == 1)", false },
                  { "Enumerable.Any(null, i => i != 1)", false },
                  { "Enumerable.Any(new[] { }, i => i != 1)", false },
                  { "Enumerable.Any(new[] { 1, 1, 1 }, i => i != 1)", false },
                  { "Enumerable.Any(new[] { 1, 1, 2 }, i => i != 1)", true },
                  { "Enumerable.Contains(null, 1)", false },
                  { "Enumerable.Contains(new[] { }, 1)", false },
                  { "Enumerable.Contains(new[] { 2 }, 1)", false },
                  { "Enumerable.Contains(new[] { 1, 2 }, 1)", true },
                  { "Enumerable.Equals(null, null)", true },
                  { "Enumerable.Equals(null, new[] { })", false },
                  { "Enumerable.Equals(new[] { }, null)", false },
                  { "Enumerable.Equals(new[] { }, new[] { })", true },
                  { "Enumerable.Equals(new[] { 1, 2, 3 }, new[] { 3, 2, 1 })", false },
                  { "Enumerable.Equals(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })", true },
                  { "Enumerable.Equals(new[] { new { Id = 1 }, new { Id = 2 } }, new[] { new { Id = 1 }, new { Id = 2 } }, (x, y) => x.Id == y.Id)", true },
                  { "Enumerable.Count(null)", 0 },
                  { "Enumerable.Count(new[] { })", 0 },
                  { "Enumerable.Count(new[] { 1, 2, 3 })", 3 },
                  { "Enumerable.Count(new[] { 1, 2.5, 'A' })", 3 },
                  { "Enumerable.Count(new[] { 1, 2, 2, 3, 3, 3 }, i => i == 0)", 0 },
                  { "Enumerable.Count(new[] { 1, 2, 2, 3, 3, 3 }, i => i == 1)", 1 },
                  { "Enumerable.Count(new[] { 1, 2, 2, 3, 3, 3 }, i => i == 2)", 2 },
                  { "Enumerable.Count(new[] { 1, 2, 2, 3, 3, 3 }, i => i == 3)", 3 },
                  { "Enumerable.Average(null)", null },
                  { "Enumerable.Average(new[] { })", null },
                  { "Enumerable.Average(new[] { 1 })", 1.0 },
                  { "Enumerable.Average(new[] { 1, 2 })", (1.0 + 2.0) / 2.0 },
                  { "Enumerable.Average(new[] { 1, 2, 3 })", (1.0 + 2.0 + 3.0) / 3.0 },
                  { "Enumerable.Average(null, null)", null },
                  { "Enumerable.Average(new[] { }, null)", null },
                  { "Enumerable.Average(new[] { 1 }, null)", 1.0 },
                  { "Enumerable.Average(new[] { 1, 2 }, null)", (1.0 + 2.0) / 2.0 },
                  { "Enumerable.Average(new[] { 1, 2, 3 }, null)", (1.0 + 2.0 + 3.0) / 3.0 },
                  { "Enumerable.Average(null, i => i.Value)", null },
                  { "Enumerable.Average(new[] { }, i => i.Value)", null },
                  { "Enumerable.Average(new[] { new { Value = 1 } }, i => i.Value)", 1.0 },
                  { "Enumerable.Average(new[] { new { Value = 1 }, new { Value = 2 } }, i => i.Value)", (1.0 + 2.0) / 2.0 },
                  { "Enumerable.Average(new[] { new { Value = 1 }, new { Value = 2 }, new { Value = 3 } }, i => i.Value)", (1.0 + 2.0 + 3.0) / 3.0 },
                  { "Enumerable.Min(null)", null },
                  { "Enumerable.Min(new[] { })", null },
                  { "Enumerable.Min(new[] { 1 })", 1 },
                  { "Enumerable.Min(new[] { 1, 2 })", 1 },
                  { "Enumerable.Min(new[] { 1, 2, 3 })", 1 },
                  { "Enumerable.Min(new[] { 'A' })", 'A' },
                  { "Enumerable.Min(new[] { 'A', 'B' })", 'A' },
                  { "Enumerable.Min(new[] { 'A', 'B', 'C' })", 'A' },
                  { "Enumerable.Min(null, null)", null },
                  { "Enumerable.Min(new[] { }, null)", null },
                  { "Enumerable.Min(new[] { 1 }, null)", 1 },
                  { "Enumerable.Min(new[] { 1, 2 }, null)", 1 },
                  { "Enumerable.Min(new[] { 1, 2, 3 }, null)", 1 },
                  { "Enumerable.Min(null, i => i.Value)", null },
                  { "Enumerable.Min(new[] { }, i => i.Value)", null },
                  { "Enumerable.Min(new[] { new { Value = 1 } }, i => i.Value)", 1 },
                  { "Enumerable.Min(new[] { new { Value = 1 }, new { Value = 2 } }, i => i.Value)", 1 },
                  { "Enumerable.Min(new[] { new { Value = 1 }, new { Value = 2 }, new { Value = 3 } }, i => i.Value)", 1 },
                  { "Enumerable.Max(null)", null },
                  { "Enumerable.Max(new[] { })", null },
                  { "Enumerable.Max(new[] { 1 })", 1 },
                  { "Enumerable.Max(new[] { 1, 2 })", 2 },
                  { "Enumerable.Max(new[] { 1, 2, 3 })", 3 },
                  { "Enumerable.Max(new[] { 'A' })", 'A' },
                  { "Enumerable.Max(new[] { 'A', 'B' })", 'B' },
                  { "Enumerable.Max(new[] { 'A', 'B', 'C' })", 'C' },
                  { "Enumerable.Max(null, null)", null },
                  { "Enumerable.Max(new[] { }, null)", null },
                  { "Enumerable.Max(new[] { 1 }, null)", 1 },
                  { "Enumerable.Max(new[] { 1, 2 }, null)", 2 },
                  { "Enumerable.Max(new[] { 1, 2, 3 }, null)", 3 },
                  { "Enumerable.Max(null, i => i.Value)", null },
                  { "Enumerable.Max(new[] { }, i => i.Value)", null },
                  { "Enumerable.Max(new[] { new { Value = 1 } }, i => i.Value)", 1 },
                  { "Enumerable.Max(new[] { new { Value = 1 }, new { Value = 2 } }, i => i.Value)", 2 },
                  { "Enumerable.Max(new[] { new { Value = 1 }, new { Value = 2 }, new { Value = 3 } }, i => i.Value)", 3 },
                  { "Enumerable.Aggregate(null, null)", null },
                  { "Enumerable.Aggregate(null, (result, i) => result * i)", null },
                  { "Enumerable.Aggregate(new[] { }, (result, i) => result * i)", null },
                  { "Enumerable.Aggregate(new[] { 1 }, (result, i) => result * i)", 1 },
                  { "Enumerable.Aggregate(new[] { 1, 2 }, (result, i) => result * i)", 1 * 2 },
                  { "Enumerable.Aggregate(new[] { 1, 2, 3 }, (result, i) => result * i)", 1 * 2 * 3 },
                  { "Enumerable.Aggregate(null, null, null)", null },
                  { "Enumerable.Aggregate(null, 10, (result, i) => result * i)", 10 },
                  { "Enumerable.Aggregate(new[] { }, 10, (result, i) => result * i)", 10 },
                  { "Enumerable.Aggregate(new[] { 1 }, 10, (result, i) => result * i)", 10 * 1 },
                  { "Enumerable.Aggregate(new[] { 1, 2 }, 10, (result, i) => result * i)", 10 * 1 * 2 },
                  { "Enumerable.Aggregate(new[] { 1, 2, 3 }, 10, (result, i) => result * i)", 10 * 1 * 2 * 3 },
                  { "Enumerable.ElementAt(null, 0)", null },
                  { "Enumerable.ElementAt(new[] { }, 0)", null },
                  { "Enumerable.ElementAt(new[] { 1, 2, 3 }, -1)", null },
                  { "Enumerable.ElementAt(new[] { 1, 2, 3 }, 0)", 1 },
                  { "Enumerable.ElementAt(new[] { 1, 2, 3 }, 1)", 2 },
                  { "Enumerable.ElementAt(new[] { 1, 2, 3 }, 2)", 3 },
                  { "Enumerable.ElementAt(new[] { 1, 2, 3 }, 3)", null },
                  { "Enumerable.First(null)", null },
                  { "Enumerable.First(new[] { })", null },
                  { "Enumerable.First(new[] { 1, 2, 3 })", 1 },
                  { "Enumerable.First(null, null)", null },
                  { "Enumerable.First(new[] { }, null)", null },
                  { "Enumerable.First(new[] { 1, 2, 3 }, null)", 1 },
                  { "Enumerable.First(new[] { }, i => i => i > 1)", null },
                  { "Enumerable.First(new[] { 1, 2, 3 }, i => i > 1)", 2 },
                  { "Enumerable.Last(null)", null },
                  { "Enumerable.Last(new[] { })", null },
                  { "Enumerable.Last(new[] { 1, 2, 3 })", 3 },
                  { "Enumerable.Last(null, null)", null },
                  { "Enumerable.Last(new[] { }, null)", null },
                  { "Enumerable.Last(new[] { 1, 2, 3 }, null)", 3 },
                  { "Enumerable.Last(new[] { }, i => i => i > 1)", null },
                  { "Enumerable.Last(new[] { 1, 2, 3 }, i => i > 1)", 3 },
                  { "Enumerable.Distinct(null)", null },
                  { "Enumerable.Distinct(new[] { 1, 2, 3 })", new[] { 1, 2, 3 } },
                  { "Enumerable.Distinct(new[] { 1, 1, 2, 2, 3, 3 })", new[] { 1, 2, 3 } },
                  { "Enumerable.OfType<int>(null)", null },
                  { "Enumerable.OfType<int>(new[] { })", new object[] { } },
                  { "Enumerable.OfType<int>(new[] { 1, 1.5, 'A' })", new[] { 1 } },
                  { "Enumerable.OfType<double>(new[] { 1, 1.5, 'A' })", new[] { 1.5 } },
                  { "Enumerable.OfType<char>(new[] { 1, 1.5, 'A' })", new[] { 'A' } },
                  { "Enumerable.Where(null, null)", null },
                  { "Enumerable.Where(new[] { }, null)", null },
                  { "Enumerable.Where(new[] { }, i => i < 3)", new object[] { } },
                  { "Enumerable.Where(new[] { 3, 4, 5 }, i => i < 3)", new object[] { } },
                  { "Enumerable.Where(new[] { 1, 2, 3 }, i => i < 3)", new[] { 1, 2 } },
                  { "Enumerable.Where(new[] { }, (i, index) => index < 2)", new object[] { } },
                  { "Enumerable.Where(new[] { 3, 4, 5 }, (i, index) => index < 2)", new object[] { 3, 4 } },
                  { "Enumerable.Where(new[] { 1, 2, 3 }, (i, index) => index < 2)", new[] { 1, 2 } },
                  { "Enumerable.Skip(null, 0)", null },
                  { "Enumerable.Skip(new[] { }, 0)", new object[] { } },
                  { "Enumerable.Skip(new[] { }, -1)", new object[] { } },
                  { "Enumerable.Skip(new[] { 1, 2, 3 }, -1)", new[] { 1, 2, 3 } },
                  { "Enumerable.Skip(new[] { 1, 2, 3 }, 0)", new[] { 1, 2, 3 } },
                  { "Enumerable.Skip(new[] { 1, 2, 3 }, 1)", new[] { 2, 3 } },
                  { "Enumerable.Skip(new[] { 1, 2, 3 }, 2)", new[] { 3 } },
                  { "Enumerable.Skip(new[] { 1, 2, 3 }, 3)", new object[] { } },
                  { "Enumerable.Skip(new[] { 1, 2, 3 }, 4)", new object[] { } },
                  { "Enumerable.SkipWhile(null, null)", null },
                  { "Enumerable.SkipWhile(null, i => i < 3)", null },
                  { "Enumerable.SkipWhile(new[] { }, null)", null },
                  { "Enumerable.SkipWhile(new[] { }, i => i < 3)", new object[] { } },
                  { "Enumerable.SkipWhile(new[] { 1, 2 }, i => i < 3)", new object[] { } },
                  { "Enumerable.SkipWhile(new[] { 1, 2, 3, 4 }, i => i < 3)", new[] { 3, 4 } },
                  { "Enumerable.SkipWhile(null, (i, index) => index < 2)", null },
                  { "Enumerable.SkipWhile(new[] { }, (i, index) => index < 2)", new object[] { } },
                  { "Enumerable.SkipWhile(new[] { 1, 2 }, (i, index) => index < 2)", new object[] { } },
                  { "Enumerable.SkipWhile(new[] { 1, 2, 3, 4 }, (i, index) => index < 2)", new[] { 3, 4 } },
                  { "Enumerable.Take(null, 0)", null },
                  { "Enumerable.Take(new[] { }, 0)", new object[] { } },
                  { "Enumerable.Take(new[] { }, -1)", new object[] { } },
                  { "Enumerable.Take(new[] { 1, 2, 3 }, -1)", new object[] { } },
                  { "Enumerable.Take(new[] { 1, 2, 3 }, 0)", new object[] { } },
                  { "Enumerable.Take(new[] { 1, 2, 3 }, 1)", new[] { 1 } },
                  { "Enumerable.Take(new[] { 1, 2, 3 }, 2)", new[] { 1, 2 } },
                  { "Enumerable.Take(new[] { 1, 2, 3 }, 3)", new[] { 1, 2, 3 } },
                  { "Enumerable.Take(new[] { 1, 2, 3 }, 4)", new[] { 1, 2, 3 } },
                  { "Enumerable.TakeWhile(null, null)", null },
                  { "Enumerable.TakeWhile(null, i => i < 3)", null },
                  { "Enumerable.TakeWhile(new[] { }, null)", null },
                  { "Enumerable.TakeWhile(new[] { }, i => i < 3)", new object[] { } },
                  { "Enumerable.TakeWhile(new[] { 1, 2 }, i => i < 3)", new object[] { 1, 2 } },
                  { "Enumerable.TakeWhile(new[] { 1, 2, 3, 4 }, i => i < 3)", new[] { 1, 2 } },
                  { "Enumerable.TakeWhile(null, (i, index) => index < 2)", null },
                  { "Enumerable.TakeWhile(new[] { }, (i, index) => index < 2)", new object[] { } },
                  { "Enumerable.TakeWhile(new[] { 1, 2 }, (i, index) => index > 2)", new object[] { } },
                  { "Enumerable.TakeWhile(new[] { 1, 2, 3, 4 }, (i, index) => index < 2)", new[] { 1, 2 } },
                  { "Enumerable.Except(null, null)", null },
                  { "Enumerable.Except(null, new[] { 1, 2, 3 })", null },
                  { "Enumerable.Except(new[] { 1, 2, 3 }, null)", new[] { 1, 2, 3 } },
                  { "Enumerable.Except(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })", new object[] { } },
                  { "Enumerable.Except(new[] { 1, 2, 3 }, new[] { 3, 4, 5 })", new[] { 1, 2 } },
                  { "Enumerable.Except(null, null, null)", null },
                  { "Enumerable.Except(null, new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, (x, y) => x.Id == y.Id)", null },
                  { "Enumerable.Except(new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, null, (x, y) => x.Id == y.Id)", new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } } },
                  { "Enumerable.Except(new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, (x, y) => x.Id == y.Id)", new object[] { } },
                  { "Enumerable.Except(new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, new[] { new { Id = 3 }, new { Id = 4 }, new { Id = 5 } }, (x, y) => x.Id == y.Id)", new[] { new { Id = 1 }, new { Id = 2 } } },
                  { "Enumerable.Intersect(null, null)", null },
                  { "Enumerable.Intersect(null, new[] { 1, 2, 3 })", null },
                  { "Enumerable.Intersect(new[] { 1, 2, 3 }, null)", null },
                  { "Enumerable.Intersect(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })", new[] { 1, 2, 3 } },
                  { "Enumerable.Intersect(new[] { 1, 2, 3 }, new[] { 3, 4, 5 })", new[] { 3 } },
                  { "Enumerable.Intersect(null, null, null)", null },
                  { "Enumerable.Intersect(null, new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, (x, y) => x.Id == y.Id)", null },
                  { "Enumerable.Intersect(new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, null, (x, y) => x.Id == y.Id)", null },
                  {
                      "Enumerable.Intersect(new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, (x, y) => x.Id == y.Id)", new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }
                  },
                  { "Enumerable.Intersect(new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, new[] { new { Id = 3 }, new { Id = 4 }, new { Id = 5 } }, (x, y) => x.Id == y.Id)", new[] { new { Id = 3 } } },
                  { "Enumerable.Concat(null, null)", null },
                  { "Enumerable.Concat(null, new[] { })", new object[] { } },
                  { "Enumerable.Concat(new[] { }, null)", new object[] { } },
                  { "Enumerable.Concat(new[] { }, new[] { })", new object[] { } },
                  { "Enumerable.Concat(new[] { 1 }, new[] { 1 })", new[] { 1, 1 } },
                  { "Enumerable.Concat(new[] { 1 }, new[] { 1, 2, 3 })", new[] { 1, 1, 2, 3 } },
                  { "Enumerable.Union(null, null)", null },
                  { "Enumerable.Union(null, new[] { })", new object[] { } },
                  { "Enumerable.Union(new[] { }, null)", new object[] { } },
                  { "Enumerable.Union(new[] { }, new[] { })", new object[] { } },
                  { "Enumerable.Union(new[] { 1 }, new[] { 1 })", new[] { 1 } },
                  { "Enumerable.Union(new[] { 1 }, new[] { 1, 2, 3 })", new[] { 1, 2, 3 } },
                  { "Enumerable.Union(null, null, null)", null },
                  { "Enumerable.Union(null, new[] { }, (x, y) => x.Id == y.Id)", new object[] { } },
                  { "Enumerable.Union(new[] { }, null, (x, y) => x.Id == y.Id)", new object[] { } },
                  { "Enumerable.Union(new[] { }, new[] { }, (x, y) => x.Id == y.Id)", new object[] { } },
                  { "Enumerable.Union(new[] { new { Id = 1 } }, new[] { new { Id = 1 } }, (x, y) => x.Id == y.Id)", new[] { new { Id = 1 } } },
                  { "Enumerable.Union(new[] { new { Id = 1 } }, new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, (x, y) => x.Id == y.Id)", new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } } },
                  { "Enumerable.Reverse(null)", null },
                  { "Enumerable.Reverse(new[] { })", new object[] { } },
                  { "Enumerable.Reverse(new[] { 1, 2, 3 })", new[] { 3, 2, 1 } },
                  { "Enumerable.Select(null, null)", null },
                  { "Enumerable.Select(new[] { }, null)", null },
                  { "Enumerable.Select(new[] { }, i => i)", new object[] { } },
                  { "Enumerable.Select(new[] { 1, 2, 3 }, i => i)", new[] { 1, 2, 3 } },
                  { "Enumerable.Select(new[] { 1, 2, 3 }, i => (double)i)", new[] { 1.0, 2.0, 3.0 } },
                  { "Enumerable.Select(new[] { 1, 2, 3 }, i => new { Id = i })", new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } } },
                  { "Enumerable.Select(new[] { 1, 2, 3 }, (i, index) => i * index)", new[] { 1 * 0, 2 * 1, 3 * 2 } },
                  { "Enumerable.Select(new[] { 1, 2, 3 }, (i, index) => new { Id = i * index })", new[] { new { Id = 1 * 0 }, new { Id = 2 * 1 }, new { Id = 3 * 2 } } },
                  { "Enumerable.SelectMany(null, null)", null },
                  { "Enumerable.SelectMany(new[] { new[] { 1 }, new[] { 2 }, new[] { 3 } }, i => i)", new[] { 1, 2, 3 } },
                  {
                      "Enumerable.GroupBy(new[] { new { Id = 1, Value = 'A' }, new { Id = 1, Value = 'B' }, new { Id = 2, Value = 'C' } }, i => i.Id)", new[]
                                                                                                                                                        {
                                                                                                                                                            new
                                                                                                                                                            {
                                                                                                                                                                Key = 1,
                                                                                                                                                                Items = new[] { new { Id = 1, Value = 'A' }, new { Id = 1, Value = 'B' } }
                                                                                                                                                            },
                                                                                                                                                            new { Key = 2, Items = new[] { new { Id = 2, Value = 'C' } } }
                                                                                                                                                        }
                  },
                  {
                      "Enumerable.GroupBy(new[] { new { Id = 1, Value = 'A' }, new { Id = 1, Value = 'B' }, new { Id = 2, Value = 'C' } }, i => i.Id, i => i.Value)",
                      new[] { new { Key = 1, Items = new[] { 'A', 'B' } }, new { Key = 2, Items = new[] { 'C' } } }
                  },
                  {
                      "Enumerable.GroupBy(new[] { new { Id = 1, Value = 'A' }, new { Id = 1, Value = 'B' }, new { Id = 2, Value = 'C' } }, i => i.Id, i => i.Value, (x, y) => x == y)",
                      new[] { new { Key = 1, Items = new[] { 'A', 'B' } }, new { Key = 2, Items = new[] { 'C' } } }
                  },
                  { "Enumerable.OrderBy(null)", null },
                  { "Enumerable.OrderBy(new[] { })", new object[] { } },
                  { "Enumerable.OrderBy(new[] { 3, 2, 1 })", new[] { 1, 2, 3 } },
                  { "Enumerable.OrderBy(null, null)", null },
                  { "Enumerable.OrderBy(new[] { }, null)", new object[] { } },
                  { "Enumerable.OrderBy(null, i => i.Id)", null },
                  { "Enumerable.OrderBy(new[] { }, i => i.Id)", new object[] { } },
                  { "Enumerable.OrderBy(new[] { new { Id = 3 }, new { Id = 2 }, new { Id = 1 } }, i => i.Id)", new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } } },
                  { "Enumerable.OrderByDescending(null)", null },
                  { "Enumerable.OrderByDescending(new[] { })", new object[] { } },
                  { "Enumerable.OrderByDescending(new[] { 1, 2, 3 })", new[] { 3, 2, 1 } },
                  { "Enumerable.OrderByDescending(null, null)", null },
                  { "Enumerable.OrderByDescending(new[] { }, null)", new object[] { } },
                  { "Enumerable.OrderByDescending(null, i => i.Id)", null },
                  { "Enumerable.OrderByDescending(new[] { }, i => i.Id)", new object[] { } },
                  { "Enumerable.OrderByDescending(new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } }, i => i.Id)", new[] { new { Id = 3 }, new { Id = 2 }, new { Id = 1 } } },
                  {
                      "Enumerable.OrderByDescending(Enumerable.OrderBy(new[] { new { Key1 = 2, Key2 = 'A' }, new { Key1 = 2, Key2 = 'B' }, new { Key1 = 1, Key2 = 'A' }, new { Key1 = 1, Key2 = 'B' } }, i => i.Key1), i => i.Key2)",
                      new[]
                      {
                          new { Key1 = 1, Key2 = 'B' }, new { Key1 = 1, Key2 = 'A' }, new { Key1 = 2, Key2 = 'B' },
                          new { Key1 = 2, Key2 = 'A' }
                      }
                  },
                  { "Enumerable.Select(new[] { 1, 2, 3 }, delegate(int i) { return i; })", new[] { 1, 2, 3 } }
              };

        private static void AssertExecuteExpression(string expression, object expectedResult, object dataContext = null)
        {
            // When
            var actualResult = ExpressionExecutor.Execute(expression, dataContext);

            // Then
            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(ObjectEqualityComparer.Default));
        }

        [Test]
        [TestCase("1 + 2", 1 + 2)]
        [TestCase("1.5 + 2.5", 1.5 + 2.5)]
        [TestCase("'A' + '\\u1'", 'A' + 1)]
        [TestCase("\"A\" + \"bc\"", "A" + "bc")]
        public void AddExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCaseSource(nameof(AnonymousObjectCreationExpressionCases))]
        public void AnonymousObjectCreationExpression(KeyValuePair<string, object> testCase)
        {
            AssertExecuteExpression(testCase.Key, testCase.Value);
        }

        [Test]
        [TestCaseSource(nameof(ArrayCreationExpressionCases))]
        public void ArrayCreationExpression(KeyValuePair<string, Array> testCase)
        {
            AssertExecuteExpression(testCase.Key, testCase.Value);
        }

        [Test]
        [TestCase("123 as string", null)]
        [TestCase("123 as System.String", null)]
        [TestCase("123 as int", 123)]
        [TestCase("123 as System.Int32", 123)]
        [TestCase("123.456 as string", null)]
        [TestCase("123.456 as System.String", null)]
        [TestCase("123.456 as double", 123.456)]
        [TestCase("123.456 as System.Double", 123.456)]
        [TestCase("'A' as string", null)]
        [TestCase("'A' as System.String", null)]
        [TestCase("'A' as char", 'A')]
        [TestCase("'A' as System.Char", 'A')]
        [TestCase("\"Abc\" as int", null)]
        [TestCase("\"Abc\" as System.Int32", null)]
        [TestCase("\"Abc\" as string", "Abc")]
        [TestCase("\"Abc\" as System.String", "Abc")]
        public void AsExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("0 & 0", 0 & 0)]
        [TestCase("0 & 1", 0 & 1)]
        [TestCase("0 & 2", 0 & 2)]
        [TestCase("0 & 3", 0 & 3)]
        [TestCase("1 & 1", 1 & 1)]
        [TestCase("1 & 2", 1 & 2)]
        [TestCase("1 & 3", 1 & 3)]
        [TestCase("2 & 2", 2 & 2)]
        [TestCase("2 & 3", 2 & 3)]
        [TestCase("3 & 3", 3 & 3)]
        [TestCase("false & false", false & false)]
        [TestCase("false & true", false & true)]
        [TestCase("true & false", true & false)]
        [TestCase("true & true", true & true)]
        public void BitwiseAndExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("0 | 0", 0 | 0)]
        [TestCase("0 | 1", 0 | 1)]
        [TestCase("0 | 2", 0 | 2)]
        [TestCase("0 | 3", 0 | 3)]
        [TestCase("1 | 1", 1 | 1)]
        [TestCase("1 | 2", 1 | 2)]
        [TestCase("1 | 3", 1 | 3)]
        [TestCase("2 | 2", 2 | 2)]
        [TestCase("2 | 3", 2 | 3)]
        [TestCase("3 | 3", 3 | 3)]
        [TestCase("false | false", false | false)]
        [TestCase("false | true", false | true)]
        [TestCase("true | false", true | false)]
        [TestCase("true | true", true | true)]
        public void BitwiseOrExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("(long)1", 1L)]
        [TestCase("(string)1", "1")]
        [TestCase("(System.Int64)1", 1L)]
        [TestCase("(System.String)1", "1")]
        [TestCase("(int)null", default(int))]
        [TestCase("(System.Int32)null", default(int))]
        [TestCase("(bool)null", default(bool))]
        [TestCase("(System.Boolean)null", default(bool))]
        [TestCase("(bool)0", false)]
        [TestCase("(System.Boolean)0", false)]
        [TestCase("(bool)1", true)]
        [TestCase("(System.Boolean)1", true)]
        public void CastExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("null ?? 1234", 1234)]
        [TestCase("5678 ?? 1234", 5678)]
        public void CoalesceExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("false ? 123 : 456", 456)]
        [TestCase("true ? 123 : 456", 123)]
        public void ConditionalExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("Condition.If(true, 1, 2)", 1)]
        [TestCase("Condition.If(false, 1, 2)", 2)]
        [TestCase("Condition.Switch(1, new[] { 1, 2, 3 }, new[] { 'A', 'B', 'C' })", 'A')]
        [TestCase("Condition.Switch(2, new[] { 1, 2, 3 }, new[] { 'A', 'B', 'C' })", 'B')]
        [TestCase("Condition.Switch(3, new[] { 1, 2, 3 }, new[] { 'A', 'B', 'C' })", 'C')]
        [TestCase("Condition.Switch(4, new[] { 1, 2, 3 }, new[] { 'A', 'B', 'C' })", null)]
        [TestCase("Condition.Switch(1, new[] { 1, 2, 3 }, new[] { 'A', 'B', 'C' }, 'D')", 'A')]
        [TestCase("Condition.Switch(2, new[] { 1, 2, 3 }, new[] { 'A', 'B', 'C' }, 'D')", 'B')]
        [TestCase("Condition.Switch(3, new[] { 1, 2, 3 }, new[] { 'A', 'B', 'C' }, 'D')", 'C')]
        [TestCase("Condition.Switch(4, new[] { 1, 2, 3 }, new[] { 'A', 'B', 'C' }, 'D')", 'D')]
        public void ConditionFunctions(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("123", 123)]
        [TestCase("123.456", 123.456)]
        [TestCase("false", false)]
        [TestCase("true", true)]
        [TestCase("null", null)]
        [TestCase("0xAF", 0xAF)]
        [TestCase("'A'", 'A')]
        [TestCase("\"Abc\"", "Abc")]
        [TestCase("@\"A\\bc\"", @"A\bc")]
        [TestCase("System.TypeCode.Boolean", TypeCode.Boolean)]
        public void ConstantExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCaseSource(nameof(DateTimeFunctionsCases))]
        public void DateTimeFunctions(KeyValuePair<string, object> testCase)
        {
            AssertExecuteExpression(testCase.Key, testCase.Value);
        }

        [Test]
        [TestCase("default(bool)", default(bool))]
        [TestCase("default(byte)", default(byte))]
        [TestCase("default(sbyte)", default(sbyte))]
        [TestCase("default(char)", default(char))]
        [TestCase("default(double)", default(double))]
        [TestCase("default(float)", default(float))]
        [TestCase("default(int)", default(int))]
        [TestCase("default(uint)", default(uint))]
        [TestCase("default(long)", default(long))]
        [TestCase("default(ulong)", default(ulong))]
        [TestCase("default(object)", default(object))]
        [TestCase("default(short)", default(short))]
        [TestCase("default(ushort)", default(ushort))]
        [TestCase("default(string)", default(string))]
        [TestCase("default(System.Boolean)", default(bool))]
        [TestCase("default(System.Byte)", default(byte))]
        [TestCase("default(System.SByte)", default(sbyte))]
        [TestCase("default(System.Char)", default(char))]
        [TestCase("default(System.Double)", default(double))]
        [TestCase("default(System.Single)", default(float))]
        [TestCase("default(System.Int32)", default(int))]
        [TestCase("default(System.UInt32)", default(uint))]
        [TestCase("default(System.Int64)", default(long))]
        [TestCase("default(System.UInt64)", default(ulong))]
        [TestCase("default(System.Object)", default(object))]
        [TestCase("default(System.Int16)", default(short))]
        [TestCase("default(System.UInt16)", default(ushort))]
        [TestCase("default(System.String)", default(string))]
        public void DefaultExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("2 / 3", 2 / 3)]
        [TestCase("3 / 2", 3 / 2)]
        [TestCase("3.0 / 2.0", 3.0 / 2.0)]
        public void DivideExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("\"Abc\"[0]", 'A')]
        [TestCase("\"Abc\"[1]", 'b')]
        [TestCase("\"Abc\"[2]", 'c')]
        [TestCase("new[] { 1, 2, 3 }[0]", 1)]
        [TestCase("new[] { 1, 2, 3 }[1]", 2)]
        [TestCase("new[] { 1, 2, 3 }[2]", 3)]
        [TestCase("new[] { { 11, 12 }, { 21, 22 } }[0, 0]", 11)]
        [TestCase("new[,] { { 11, 12 }, { 21, 22 } }[0, 1]", 12)]
        [TestCase("new[,] { { 11, 12 }, { 21, 22 } }[1, 0]", 21)]
        [TestCase("new[,] { { 11, 12 }, { 21, 22 } }[1, 1]", 22)]
        public void ElementAccessExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCaseSource(nameof(EnumerableFunctionsCases))]
        public void EnumerableFunctions(KeyValuePair<string, object> testCase)
        {
            AssertExecuteExpression(testCase.Key, testCase.Value);
        }

        [Test]
        [TestCase("1 == 0", false)]
        [TestCase("1 == 1", true)]
        [TestCase("1.5 == 0.0", false)]
        [TestCase("1.5 == 1.5", true)]
        [TestCase("'A' == 'B'", false)]
        [TestCase("'A' == 'A'", true)]
        [TestCase("\"Abc\" == \"ABC\"", false)]
        [TestCase("\"Abc\" == \"Abc\"", true)]
        [TestCase("true == false", false)]
        [TestCase("true == true", true)]
        public void EqualsExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("0 ^ 0", 0 ^ 0)]
        [TestCase("0 ^ 1", 0 ^ 1)]
        [TestCase("0 ^ 2", 0 ^ 2)]
        [TestCase("0 ^ 3", 0 ^ 3)]
        [TestCase("1 ^ 1", 1 ^ 1)]
        [TestCase("1 ^ 2", 1 ^ 2)]
        [TestCase("1 ^ 3", 1 ^ 3)]
        [TestCase("2 ^ 2", 2 ^ 2)]
        [TestCase("2 ^ 3", 2 ^ 3)]
        [TestCase("3 ^ 3", 3 ^ 3)]
        [TestCase("false ^ false", false ^ false)]
        [TestCase("false ^ true", false ^ true)]
        [TestCase("true ^ false", true ^ false)]
        [TestCase("true ^ true", true ^ true)]
        public void ExclusiveOrExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("1 > 2", false)]
        [TestCase("1 > 1", false)]
        [TestCase("1 > 0", true)]
        [TestCase("1.5 > 2.5", false)]
        [TestCase("1.5 > 1.5", false)]
        [TestCase("1.5 > 0.0", true)]
        [TestCase("'B' > 'C'", false)]
        [TestCase("'B' > 'B'", false)]
        [TestCase("'B' > 'A'", true)]
        public void GreaterThanExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("1 >= 2", false)]
        [TestCase("1 >= 1", true)]
        [TestCase("1 >= 0", true)]
        [TestCase("1.5 >= 2.5", false)]
        [TestCase("1.5 >= 1.5", true)]
        [TestCase("1.5 >= 0.0", true)]
        [TestCase("'B' >= 'C'", false)]
        [TestCase("'B' >= 'B'", true)]
        [TestCase("'B' >= 'A'", true)]
        public void GreaterThanOrEqualExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("if (true) 1", 1)]
        [TestCase("if (false) 1", null)]
        [TestCase("if (true) 1 else 2", 1)]
        [TestCase("if (false) 1 else 2", 2)]
        [TestCase("if (true) 1 else if (true) 21 else 22", 1)]
        [TestCase("if (true) 1 else if (false) 21 else 22", 1)]
        [TestCase("if (false) 1 else if (true) 21 else 22", 21)]
        [TestCase("if (false) 1 else if (false) 21 else 22", 22)]
        public void IfStatement(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("0.ToString()", "0")]
        [TestCase("int.Parse(\"123\")", 123)]
        [TestCase("int.Parse(\"123\").ToString()", "123")]
        [TestCase("System.Int32.Parse(\"123\")", 123)]
        [TestCase("string.Format(\"{0}, {1}, {2}, {3}\", 1, 123, 'A', \"Abc\")", "1, 123, A, Abc")]
        [TestCase("System.String.Format(\"{0}, {1}, {2}, {3}\", 1, 123, 'A', \"Abc\")", "1, 123, A, Abc")]
        [TestCase("string.Empty.Trim()", "")]
        [TestCase("System.String.Empty.Trim()", "")]
        [TestCase("\"   Abc   \".Trim()", "Abc")]
        public void InvocationExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("123 is string", false)]
        [TestCase("123 is System.String", false)]
        [TestCase("123 is int", true)]
        [TestCase("123 is System.Int32", true)]
        [TestCase("123.456 is string", false)]
        [TestCase("123.456 is System.String", false)]
        [TestCase("123.456 is double", true)]
        [TestCase("123.456 is System.Double", true)]
        [TestCase("'A' is string", false)]
        [TestCase("'A' is System.String", false)]
        [TestCase("'A' is char", true)]
        [TestCase("'A' is System.Char", true)]
        [TestCase("\"Abc\" is int", false)]
        [TestCase("\"Abc\" is System.Int32", false)]
        [TestCase("\"Abc\" is string", true)]
        [TestCase("\"Abc\" is System.String", true)]
        public void IsExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("(() => 5).DynamicInvoke()", 5)]
        [TestCase("(i => i).DynamicInvoke(5)", 5)]
        [TestCase("((x, y) => x + y).DynamicInvoke(1, 2)", 3)]
        [TestCase("(delegate() { return 5; }).DynamicInvoke()", 5)]
        [TestCase("(delegate(dynamic i) { return i; }).DynamicInvoke(5)", 5)]
        [TestCase("(delegate(dynamic x, dynamic y) { return x + y; }).DynamicInvoke(1, 2)", 3)]
        public void LambdaExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("0 << 0", 0 << 0)]
        [TestCase("0 << 5", 0 << 5)]
        [TestCase("1 << 0", 1 << 0)]
        [TestCase("1 << 1", 1 << 1)]
        [TestCase("1 << 2", 1 << 2)]
        [TestCase("1 << 3", 1 << 3)]
        public void LeftShiftExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("1 < 2", true)]
        [TestCase("1 < 1", false)]
        [TestCase("1 < 0", false)]
        [TestCase("1.5 < 2.5", true)]
        [TestCase("1.5 < 1.5", false)]
        [TestCase("1.5 < 0.0", false)]
        [TestCase("'B' < 'C'", true)]
        [TestCase("'B' < 'B'", false)]
        [TestCase("'B' < 'A'", false)]
        public void LessThanExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("1 <= 2", true)]
        [TestCase("1 <= 1", true)]
        [TestCase("1 <= 0", false)]
        [TestCase("1.5 <= 2.5", true)]
        [TestCase("1.5 <= 1.5", true)]
        [TestCase("1.5 <= 0.0", false)]
        [TestCase("'B' <= 'C'", true)]
        [TestCase("'B' <= 'B'", true)]
        [TestCase("'B' <= 'A'", false)]
        public void LessThanOrEqualExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("false && false", false)]
        [TestCase("false && true", false)]
        [TestCase("true && false", false)]
        [TestCase("true && true", true)]
        public void LogicalAndExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("!false", !false)]
        [TestCase("!true", !true)]
        public void LogicalNotExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("false || false", false)]
        [TestCase("false || true", true)]
        [TestCase("true || false", true)]
        [TestCase("true || true", true)]
        public void LogicalOrExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("Math.E", Math.E)]
        [TestCase("Math.PI", Math.PI)]
        [TestCase("Math.Abs(1)", 1)]
        [TestCase("Math.Abs(1.5)", 1.5)]
        [TestCase("Math.Abs(-1)", 1)]
        [TestCase("Math.Abs(-1.5)", 1.5)]
        [TestCase("Math.Floor(1.7)", 1.0)]
        [TestCase("Math.Floor(-1.7)", -2.0)]
        [TestCase("Math.Ceiling(1.7)", 2.0)]
        [TestCase("Math.Ceiling(-1.7)", -1.0)]
        [TestCase("Math.Round(1.4567, 0)", 1.0)]
        [TestCase("Math.Round(1.4567, 1)", 1.5)]
        [TestCase("Math.Round(1.4567, 2)", 1.46)]
        [TestCase("Math.Round(1.4567, 3)", 1.457)]
        [TestCase("Math.Round(1.4567, 4)", 1.4567)]
        [TestCase("Math.Truncate(1.8)", 1.0)]
        [TestCase("Math.Truncate(-1.8)", -1.0)]
        [TestCase("Math.Min(1, 2)", 1)]
        [TestCase("Math.Min(1.5, 2.5)", 1.5)]
        [TestCase("Math.Min(1, 2.5)", 1.0)]
        [TestCase("Math.Min(1.5, 2)", 1.5)]
        [TestCase("Math.Max(1, 2)", 2)]
        [TestCase("Math.Max(1.5, 2.5)", 2.5)]
        [TestCase("Math.Max(1, 2.5)", 2.5)]
        [TestCase("Math.Max(1.5, 2)", 2.0)]
        [TestCase("Math.Pow(2, 3)", 8.0)]
        [TestCase("Math.Random() is int", true)]
        [TestCase("Math.Random(1, 5) <= 5", true)]
        public void MathFunctions(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("\"Abc\".Length", 3)]
        [TestCase("string.Empty", "")]
        [TestCase("System.String.Empty", "")]
        [TestCase("int.MaxValue", int.MaxValue)]
        [TestCase("System.Int32.MaxValue", int.MaxValue)]
        [TestCase("new { Property1 = 123 }.Property1", 123)]
        public void MemberAccessExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("2 % 3", 2 % 3)]
        [TestCase("3 % 2", 3 % 2)]
        [TestCase("3.5 % 2.0", 3.5 % 2.0)]
        public void ModuloExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("2 * 3", 2 * 3)]
        [TestCase("1.5 * 1.5", 1.5 * 1.5)]
        public void MultiplyExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("1 != 0", true)]
        [TestCase("1 != 1", false)]
        [TestCase("1.5 != 0.0", true)]
        [TestCase("1.5 != 1.5", false)]
        [TestCase("'A' != 'B'", true)]
        [TestCase("'A' != 'A'", false)]
        [TestCase("\"Abc\" != \"ABC\"", true)]
        [TestCase("\"Abc\" != \"Abc\"", false)]
        [TestCase("true != false", true)]
        [TestCase("true != true", false)]
        public void NotEqualsExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCaseSource(nameof(ObjectCreationExpressionCases))]
        public void ObjectCreationExpression(KeyValuePair<string, object> testCase)
        {
            AssertExecuteExpression(testCase.Key, testCase.Value);
        }

        [Test]
        [TestCase("0 >> 0", 0 >> 0)]
        [TestCase("0 >> 5", 0 >> 5)]
        [TestCase("8 >> 0", 8 >> 0)]
        [TestCase("8 >> 1", 8 >> 1)]
        [TestCase("8 >> 2", 8 >> 2)]
        [TestCase("8 >> 3", 8 >> 3)]
        [TestCase("8 >> 4", 8 >> 4)]
        [TestCase("8 >> 5", 8 >> 5)]
        public void RightShiftExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("sizeof(int)", sizeof(int))]
        [TestCase("sizeof(long)", sizeof(long))]
        public void SizeOfExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        public void Some()
        {
            AssertExecuteExpression("new[] { new[] { 11 }, new[] { 21, 22 } }", new[] { new[] { 11 }, new[] { 21, 22 } });
        }

        [Test]
        [TestCase("1 - 2", 1 - 2)]
        [TestCase("1.5 - 2.5", 1.5 - 2.5)]
        [TestCase("'B' - '\\u1'", 'B' - 1)]
        public void SubtractExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("switch (1) { case 1: 11; case 2: 22; case 3: 33; default: 44; }", 11)]
        [TestCase("switch (2) { case 1: 11; case 2: 22; case 3: 33; default: 44; }", 22)]
        [TestCase("switch (3) { case 1: 11; case 2: 22; case 3: 33; default: 44; }", 33)]
        [TestCase("switch (4) { case 1: 11; case 2: 22; case 3: 33; default: 44; }", 44)]
        public void SwitchStatement(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("Text.Empty", "")]
        [TestCase("Text.Length(null)", 0)]
        [TestCase("Text.Length(\"\")", 0)]
        [TestCase("Text.Length(\"Abc\")", 3)]
        [TestCase("Text.Equals(\"Abc\", \"Abc\")", true)]
        [TestCase("Text.Equals(\"Abc\", \"abc\")", false)]
        [TestCase("Text.Equals(\"Abc\", \"Abc\", false)", true)]
        [TestCase("Text.Equals(\"Abc\", \"Abc\", true)", true)]
        [TestCase("Text.Equals(\"Abc\", \"abc\", false)", false)]
        [TestCase("Text.Equals(\"Abc\", \"abc\", true)", true)]
        [TestCase("Text.IsNullOrEmpty(null)", true)]
        [TestCase("Text.IsNullOrEmpty(\"\")", true)]
        [TestCase("Text.IsNullOrEmpty(\" \")", false)]
        [TestCase("Text.IsNullOrEmpty(\"Abc\")", false)]
        [TestCase("Text.IsNullOrWhiteSpace(null)", true)]
        [TestCase("Text.IsNullOrWhiteSpace(\"\")", true)]
        [TestCase("Text.IsNullOrWhiteSpace(\" \")", true)]
        [TestCase("Text.IsNullOrWhiteSpace(\"Abc\")", false)]
        [TestCase("Text.Concat(1)", "1")]
        [TestCase("Text.Concat(1, 'A')", "1A")]
        [TestCase("Text.Concat(1)", "1")]
        [TestCase("Text.Concat(1, 'A', \"bc\")", "1Abc")]
        [TestCase("Text.Concat(new[] { 1, 2, 3 })", "123")]
        [TestCase("Text.Concat(new object[] { 1, 2, 3 })", "123")]
        [TestCase("Text.Format(\"{0}\", 1)", "1")]
        [TestCase("Text.Format(\"{0}, {1}\", 1, 'A')", "1, A")]
        [TestCase("Text.Format(\"{0}, {1}, {2}\", 1, 'A', \"bc\")", "1, A, bc")]
        [TestCase("Text.Format(\"{0}, {1}, {2}\", new[] { 1, 2, 3 })", "1, 2, 3")]
        [TestCase("Text.Format(\"{0}, {1}, {2}\", new object[] { 1, 2, 3 })", "1, 2, 3")]
        [TestCase("Text.Join(\", \", 1)", "1")]
        [TestCase("Text.Join(\"; \", 1, 2)", "1; 2")]
        [TestCase("Text.Join(\"; \", 1, 2, 3)", "1; 2; 3")]
        [TestCase("Text.Join(\"; \", 1, 'A', \"bc\")", "1; A; bc")]
        [TestCase("Text.Join(\"; \", new[] { 1, 2, 3 })", "1; 2; 3")]
        [TestCase("Text.Join(\"; \", new object[] { 1, 2, 3 })", "1; 2; 3")]
        [TestCase("Text.Split(\"1; 2; 3\", ';')", new[] { "1", " 2", " 3" })]
        [TestCase("Text.Split(\"1; 2, 3\", ';', ',')", new[] { "1", " 2", " 3" })]
        [TestCase("Text.Split(\"1; 2; 3\", \"; \")", new[] { "1", "2", "3" })]
        [TestCase("Text.Split(\"1; 2, 3\", \"; \", \", \")", new[] { "1", "2", "3" })]
        [TestCase("Text.Contains(null, null)", false)]
        [TestCase("Text.Contains(null, \"Ab\")", false)]
        [TestCase("Text.Contains(\" Abc \", null)", false)]
        [TestCase("Text.Contains(\" Abc \", \"Ab\")", true)]
        [TestCase("Text.Contains(\" Abc \", \"ab\")", false)]
        [TestCase("Text.Contains(\" Abc \", \"Ab\", false)", true)]
        [TestCase("Text.Contains(\" Abc \", \"Ab\", true)", true)]
        [TestCase("Text.Contains(\" Abc \", \"ab\")", false)]
        [TestCase("Text.Contains(\" Abc \", \"ab\", false)", false)]
        [TestCase("Text.Contains(\" Abc \", \"ab\", true)", true)]
        [TestCase("Text.StartsWith(null, null)", false)]
        [TestCase("Text.StartsWith(null, \"Ab\")", false)]
        [TestCase("Text.StartsWith(\"Abc \", null)", false)]
        [TestCase("Text.StartsWith(\"Abc \", \"Ab\")", true)]
        [TestCase("Text.StartsWith(\"Abc \", \"ab\")", false)]
        [TestCase("Text.StartsWith(\"Abc \", \"Ab\", false)", true)]
        [TestCase("Text.StartsWith(\"Abc \", \"Ab\", true)", true)]
        [TestCase("Text.StartsWith(\"Abc \", \"ab\")", false)]
        [TestCase("Text.StartsWith(\"Abc \", \"ab\", false)", false)]
        [TestCase("Text.StartsWith(\"Abc \", \"ab\", true)", true)]
        [TestCase("Text.EndsWith(null, null)", false)]
        [TestCase("Text.EndsWith(null, \"Abc\")", false)]
        [TestCase("Text.EndsWith(\" Abc\", null)", false)]
        [TestCase("Text.EndsWith(\" Abc\", \"Abc\")", true)]
        [TestCase("Text.EndsWith(\" Abc\", \"abc\")", false)]
        [TestCase("Text.EndsWith(\" Abc\", \"Abc\", false)", true)]
        [TestCase("Text.EndsWith(\" Abc\", \"Abc\", true)", true)]
        [TestCase("Text.EndsWith(\" Abc\", \"abc\")", false)]
        [TestCase("Text.EndsWith(\" Abc\", \"abc\", false)", false)]
        [TestCase("Text.EndsWith(\" Abc\", \"abc\", true)", true)]
        [TestCase("Text.IndexOf(null, 'A')", -1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", 'A')", 0)]
        [TestCase("Text.IndexOf(\"AbcAbc\", 'b')", 1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", 'c')", 2)]
        [TestCase("Text.IndexOf(null, 'A', 3)", -1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", 'A', 3)", 3)]
        [TestCase("Text.IndexOf(\"AbcAbc\", 'b', 3)", 4)]
        [TestCase("Text.IndexOf(\"AbcAbc\", 'c', 3)", 5)]
        [TestCase("Text.IndexOf(null, \"Ab\")", -1)]
        [TestCase("Text.IndexOf(null, \"Ab\", false)", -1)]
        [TestCase("Text.IndexOf(null, \"Ab\", true)", -1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"ab\")", -1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"ab\", false)", -1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"ab\", true)", 0)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"Ab\")", 0)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"Ab\", false)", 0)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"Ab\", true)", 0)]
        [TestCase("Text.IndexOf(null, \"Ab\", 2)", -1)]
        [TestCase("Text.IndexOf(null, \"Ab\", 2, false)", -1)]
        [TestCase("Text.IndexOf(null, \"Ab\", 2, true)", -1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"ab\", 2)", -1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"ab\", 2, false)", -1)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"ab\", 2, true)", 3)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"Ab\", 2)", 3)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"Ab\", 2, false)", 3)]
        [TestCase("Text.IndexOf(\"AbcAbc\", \"Ab\", 2, true)", 3)]
        [TestCase("Text.LastIndexOf(null, 'A')", -1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", 'A')", 3)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", 'b')", 4)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", 'c')", 5)]
        [TestCase("Text.LastIndexOf(null, 'A', 3)", -1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", 'A', 2)", 0)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", 'b', 2)", 1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", 'c', 2)", 2)]
        [TestCase("Text.LastIndexOf(null, \"Ab\")", -1)]
        [TestCase("Text.LastIndexOf(null, \"Ab\", false)", -1)]
        [TestCase("Text.LastIndexOf(null, \"Ab\", true)", -1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"ab\")", -1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"ab\", false)", -1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"ab\", true)", 3)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"Ab\")", 3)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"Ab\", false)", 3)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"Ab\", true)", 3)]
        [TestCase("Text.LastIndexOf(null, \"Ab\", 2)", -1)]
        [TestCase("Text.LastIndexOf(null, \"Ab\", 2, false)", -1)]
        [TestCase("Text.LastIndexOf(null, \"Ab\", 2, true)", -1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"ab\", 2)", -1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"ab\", 2, false)", -1)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"ab\", 2, true)", 0)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"Ab\", 2)", 0)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"Ab\", 2, false)", 0)]
        [TestCase("Text.LastIndexOf(\"AbcAbc\", \"Ab\", 2, true)", 0)]
        [TestCase("Text.Substring(null, 0)", null)]
        [TestCase("Text.Substring(null, 2)", null)]
        [TestCase("Text.Substring(\"AbcAbc\", 0)", "AbcAbc")]
        [TestCase("Text.Substring(\"AbcAbc\", 1)", "bcAbc")]
        [TestCase("Text.Substring(\"AbcAbc\", 2)", "cAbc")]
        [TestCase("Text.Substring(null, 0, 0)", null)]
        [TestCase("Text.Substring(null, 2, 2)", null)]
        [TestCase("Text.Substring(\"AbcAbc\", 0, 0)", "")]
        [TestCase("Text.Substring(\"AbcAbc\", 1, 2)", "bc")]
        [TestCase("Text.Substring(\"AbcAbc\", 2, 3)", "cAb")]
        [TestCase("Text.ToLower(null)", null)]
        [TestCase("Text.ToLower(\"AbcAbc\")", "abcabc")]
        [TestCase("Text.ToUpper(null)", null)]
        [TestCase("Text.ToUpper(\"AbcAbc\")", "ABCABC")]
        [TestCase("Text.Trim(null)", null)]
        [TestCase("Text.Trim(\" \")", "")]
        [TestCase("Text.Trim(\" Abc \")", "Abc")]
        [TestCase("Text.Trim(null, ' ', '.')", null)]
        [TestCase("Text.Trim(\" ... \", ' ', '.')", "")]
        [TestCase("Text.Trim(\" Abc... \", ' ', '.')", "Abc")]
        [TestCase("Text.TrimStart(null)", null)]
        [TestCase("Text.TrimStart(\" \")", "")]
        [TestCase("Text.TrimStart(\" Abc \")", "Abc ")]
        [TestCase("Text.TrimStart(null, ' ', '.')", null)]
        [TestCase("Text.TrimStart(\" ... \", ' ', '.')", "")]
        [TestCase("Text.TrimStart(\" ...Abc... \", ' ', '.')", "Abc... ")]
        [TestCase("Text.TrimEnd(null)", null)]
        [TestCase("Text.TrimEnd(\" \")", "")]
        [TestCase("Text.TrimEnd(\" Abc \")", " Abc")]
        [TestCase("Text.TrimEnd(null, ' ', '.')", null)]
        [TestCase("Text.TrimEnd(\" ... \", ' ', '.')", "")]
        [TestCase("Text.TrimEnd(\" ...Abc... \", ' ', '.')", " ...Abc")]
        [TestCase("Text.PadLeft(null, 0)", null)]
        [TestCase("Text.PadLeft(null, 0, 'X')", null)]
        [TestCase("Text.PadLeft(\"Abc\", 0)", "Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 1)", "Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 2)", "Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 3)", "Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 4)", " Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 5)", "  Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 6)", "   Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 0, 'X')", "Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 1, 'X')", "Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 2, 'X')", "Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 3, 'X')", "Abc")]
        [TestCase("Text.PadLeft(\"Abc\", 4, 'X')", "XAbc")]
        [TestCase("Text.PadLeft(\"Abc\", 5, 'X')", "XXAbc")]
        [TestCase("Text.PadLeft(\"Abc\", 6, 'X')", "XXXAbc")]
        [TestCase("Text.PadRight(null, 0)", null)]
        [TestCase("Text.PadRight(null, 0, 'X')", null)]
        [TestCase("Text.PadRight(\"Abc\", 0)", "Abc")]
        [TestCase("Text.PadRight(\"Abc\", 1)", "Abc")]
        [TestCase("Text.PadRight(\"Abc\", 2)", "Abc")]
        [TestCase("Text.PadRight(\"Abc\", 3)", "Abc")]
        [TestCase("Text.PadRight(\"Abc\", 4)", "Abc ")]
        [TestCase("Text.PadRight(\"Abc\", 5)", "Abc  ")]
        [TestCase("Text.PadRight(\"Abc\", 6)", "Abc   ")]
        [TestCase("Text.PadRight(\"Abc\", 0, 'X')", "Abc")]
        [TestCase("Text.PadRight(\"Abc\", 1, 'X')", "Abc")]
        [TestCase("Text.PadRight(\"Abc\", 2, 'X')", "Abc")]
        [TestCase("Text.PadRight(\"Abc\", 3, 'X')", "Abc")]
        [TestCase("Text.PadRight(\"Abc\", 4, 'X')", "AbcX")]
        [TestCase("Text.PadRight(\"Abc\", 5, 'X')", "AbcXX")]
        [TestCase("Text.PadRight(\"Abc\", 6, 'X')", "AbcXXX")]
        [TestCase("Text.Remove(null, 0)", null)]
        [TestCase("Text.Remove(\"Abc\", -1)", "")]
        [TestCase("Text.Remove(\"Abc\", 0)", "")]
        [TestCase("Text.Remove(\"Abc\", 1)", "A")]
        [TestCase("Text.Remove(\"Abc\", 2)", "Ab")]
        [TestCase("Text.Remove(\"Abc\", 3)", "Abc")]
        [TestCase("Text.Remove(\"Abc\", 4)", "Abc")]
        [TestCase("Text.Remove(null, 0, 2)", null)]
        [TestCase("Text.Remove(\"Abc\", -1, 2)", "c")]
        [TestCase("Text.Remove(\"Abc\", 0, 2)", "c")]
        [TestCase("Text.Remove(\"Abc\", 1, 2)", "A")]
        [TestCase("Text.Remove(\"Abc\", 2, 2)", "Ab")]
        [TestCase("Text.Remove(\"Abc\", 3, 2)", "Abc")]
        [TestCase("Text.Remove(\"Abc\", 4, 2)", "Abc")]
        [TestCase("Text.Replace(null, 'A', 'B')", null)]
        [TestCase("Text.Replace(\"Abc\", 'A', 'B')", "Bbc")]
        [TestCase("Text.Replace(\"Abc\", 'X', 'B')", "Abc")]
        [TestCase("Text.Replace(null, \"Ab\", \"bA\")", null)]
        [TestCase("Text.Replace(\"Abc\", null, null)", "Abc")]
        [TestCase("Text.Replace(\"Abc\", null, \"bA\")", "Abc")]
        [TestCase("Text.Replace(\"Abc\", \"Ab\", null)", "c")]
        [TestCase("Text.Replace(\"Abc\", \"Ab\", \"\")", "c")]
        [TestCase("Text.Replace(\"Abc\", \"Ab\", \"bA\")", "bAc")]
        [TestCase("Text.Replace(\"Abc\", \"Xy\", \"bA\")", "Abc")]
        [TestCase("Text.Insert(null, 0, \"bc\")", null)]
        [TestCase("Text.Insert(\"A\", 0, \"bc\")", "bcA")]
        [TestCase("Text.Insert(\"A\", 1, \"bc\")", "Abc")]
        [TestCase("Text.Insert(\"A\", 2, \"bc\")", "Abc")]
        public void TextFunctions(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("this", 123, 123)]
        [TestCase("this", 123.456, 123.456)]
        [TestCase("this", "Abc", "Abc")]
        public void ThisExpression(string expression, object expectedResult, object dataContext)
        {
            AssertExecuteExpression(expression, expectedResult, dataContext);
        }

        [Test]
        [TestCaseSource(nameof(TimeSpanFunctionsCases))]
        public void TimeSpanFunctions(KeyValuePair<string, object> testCase)
        {
            AssertExecuteExpression(testCase.Key, testCase.Value);
        }

        [Test]
        [TestCase("typeof(bool)", typeof(bool))]
        [TestCase("typeof(byte)", typeof(byte))]
        [TestCase("typeof(sbyte)", typeof(sbyte))]
        [TestCase("typeof(char)", typeof(char))]
        [TestCase("typeof(decimal)", typeof(decimal))]
        [TestCase("typeof(double)", typeof(double))]
        [TestCase("typeof(float)", typeof(float))]
        [TestCase("typeof(int)", typeof(int))]
        [TestCase("typeof(uint)", typeof(uint))]
        [TestCase("typeof(long)", typeof(long))]
        [TestCase("typeof(ulong)", typeof(ulong))]
        [TestCase("typeof(object)", typeof(object))]
        [TestCase("typeof(short)", typeof(short))]
        [TestCase("typeof(ushort)", typeof(ushort))]
        [TestCase("typeof(string)", typeof(string))]
        [TestCase("typeof(System.Boolean)", typeof(bool))]
        [TestCase("typeof(System.Byte)", typeof(byte))]
        [TestCase("typeof(System.SByte)", typeof(sbyte))]
        [TestCase("typeof(System.Char)", typeof(char))]
        [TestCase("typeof(System.Decimal)", typeof(decimal))]
        [TestCase("typeof(System.Double)", typeof(double))]
        [TestCase("typeof(System.Single)", typeof(float))]
        [TestCase("typeof(System.Int32)", typeof(int))]
        [TestCase("typeof(System.UInt32)", typeof(uint))]
        [TestCase("typeof(System.Int64)", typeof(long))]
        [TestCase("typeof(System.UInt64)", typeof(ulong))]
        [TestCase("typeof(System.Object)", typeof(object))]
        [TestCase("typeof(System.Int16)", typeof(short))]
        [TestCase("typeof(System.UInt16)", typeof(ushort))]
        [TestCase("typeof(System.String)", typeof(string))]
        public void TypeOfExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("-123", -123)]
        [TestCase("-123.456", -123.456)]
        public void UnaryMinusExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }

        [Test]
        [TestCase("+123", +123)]
        [TestCase("+123.456", +123.456)]
        public void UnaryPlusExpression(string expression, object expectedResult)
        {
            AssertExecuteExpression(expression, expectedResult);
        }


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AssemblyLoadContext.Default.Resolving += OnAssemblyResolving;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            AssemblyLoadContext.Default.Resolving -= OnAssemblyResolving;
        }

        private static Assembly OnAssemblyResolving(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            var assemblyFileName = assemblyName.Name + ".dll";
            var currentAssembly = new Uri(typeof(ExpressionExecutorTest).GetTypeInfo().Assembly.CodeBase).LocalPath;
            var currentDirectory = Path.GetDirectoryName(currentAssembly) ?? ".";
            var assemblyFilePath = Directory.EnumerateFiles(currentDirectory, assemblyFileName, SearchOption.AllDirectories).FirstOrDefault();
            return !string.IsNullOrEmpty(assemblyFilePath) ? AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFilePath) : null;
        }
    }
}