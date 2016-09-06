using System;
using System.Collections.Generic;

using InfinniPlatform.Scheduler.Contract;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Tests.Contract
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class CronExpressionDayOfWeekBuilderTest
    {
        private static readonly Dictionary<Action<ICronExpressionDayOfWeekBuilder>, string> TestCases
            = new Dictionary<Action<ICronExpressionDayOfWeekBuilder>, string>
              {
                  { i => { }, "*" },
                  { i => i.Every(), "*" },
                  { i => i.Each(DayOfWeek.Monday), "2" },
                  { i => i.Each(DayOfWeek.Monday, 2), "2/2" },
                  { i => i.EachOfSet(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday), "2,3,4" },
                  { i => i.EachOfRange(DayOfWeek.Monday, DayOfWeek.Wednesday), "2-4" },
                  { i => i.EachNth(DayOfWeek.Friday, 1), "6#1" }
              };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ShouldBuildExpression(KeyValuePair<Action<ICronExpressionDayOfWeekBuilder>, string> testCase)
        {
            // Given
            var target = new CronExpressionDayOfWeekBuilder();
            var action = testCase.Key;
            var expected = testCase.Value;

            // When
            action(target);
            var actual = target.Build();

            // Then
            Assert.AreEqual(expected, actual);
        }
    }
}