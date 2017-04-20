using System;
using System.Collections.Generic;

using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Contract
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class CronExpressionMinuteBuilderTest
    {
        private static readonly Dictionary<Action<ICronExpressionMinuteBuilder>, string> TestCases
            = new Dictionary<Action<ICronExpressionMinuteBuilder>, string>
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
        public void ShouldBuildExpression(KeyValuePair<Action<ICronExpressionMinuteBuilder>, string> testCase)
        {
            // Given
            var target = new CronExpressionMinuteBuilder();
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