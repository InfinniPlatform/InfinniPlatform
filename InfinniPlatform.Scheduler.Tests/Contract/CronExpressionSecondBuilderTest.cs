using System;
using System.Collections.Generic;

using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Contract
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class CronExpressionSecondBuilderTest
    {
        private static readonly Dictionary<Action<ICronExpressionSecondBuilder>, string> TestCases
            = new Dictionary<Action<ICronExpressionSecondBuilder>, string>
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
        public void ShouldBuildExpression(KeyValuePair<Action<ICronExpressionSecondBuilder>, string> testCase)
        {
            // Given
            var target = new CronExpressionSecondBuilder();
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