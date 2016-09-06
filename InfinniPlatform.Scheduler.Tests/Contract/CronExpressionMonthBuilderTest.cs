using System;
using System.Collections.Generic;

using InfinniPlatform.Scheduler.Contract;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Tests.Contract
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class CronExpressionMonthBuilderTest
    {
        private static readonly Dictionary<Action<ICronExpressionMonthBuilder>, string> TestCases
            = new Dictionary<Action<ICronExpressionMonthBuilder>, string>
              {
                  { i => { }, "*" },
                  { i => i.Every(), "*" },
                  { i => i.Each(Month.January), "1" },
                  { i => i.Each(Month.January, 3), "1/3" },
                  { i => i.EachOfSet(Month.January, Month.February, Month.March), "1,2,3" },
                  { i => i.EachOfRange(Month.January, Month.March), "1-3" }
              };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ShouldBuildExpression(KeyValuePair<Action<ICronExpressionMonthBuilder>, string> testCase)
        {
            // Given
            var target = new CronExpressionMonthBuilder();
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