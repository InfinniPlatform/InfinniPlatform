using System;
using System.Collections.Generic;

using InfinniPlatform.Scheduler.Contract;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Tests.Contract
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class CronExpressionHourBuilderTest
    {
        private static readonly Dictionary<Action<ICronExpressionHourBuilder>, string> TestCases
            = new Dictionary<Action<ICronExpressionHourBuilder>, string>
              {
                  { i => { }, "*" },
                  { i => i.Every(), "*" },
                  { i => i.Each(5), "5" },
                  { i => i.Each(5, 6), "5/6" },
                  { i => i.EachOfSet(10, 11, 12), "10,11,12" },
                  { i => i.EachOfRange(10, 12), "10-12" }
              };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ShouldBuildExpression(KeyValuePair<Action<ICronExpressionHourBuilder>, string> testCase)
        {
            // Given
            var target = new CronExpressionHourBuilder();
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