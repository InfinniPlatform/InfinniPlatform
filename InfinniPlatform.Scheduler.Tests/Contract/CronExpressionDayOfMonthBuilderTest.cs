using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Contract
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class CronExpressionDayOfMonthBuilderTest
    {
        private static readonly Dictionary<Action<ICronExpressionDayOfMonthBuilder>, string> TestCases
            = new Dictionary<Action<ICronExpressionDayOfMonthBuilder>, string>
              {
                  { i => { }, "*" },
                  { i => i.Every(), "*" },
                  { i => i.Each(5), "5" },
                  { i => i.Each(5, 6), "5/6" },
                  { i => i.EachOfSet(10, 11, 12), "10,11,12" },
                  { i => i.EachOfRange(10, 12), "10-12" },
                  { i => i.EachLast(), "L" },
                  { i => i.EachLast(5), "L-5" },
                  { i => i.EachLastWeekday(), "LW" },
                  { i => i.EachNearestWeekday(15), "15W" }
              };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ShouldBuildExpression(KeyValuePair<Action<ICronExpressionDayOfMonthBuilder>, string> testCase)
        {
            // Given
            var target = new CronExpressionDayOfMonthBuilder();
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