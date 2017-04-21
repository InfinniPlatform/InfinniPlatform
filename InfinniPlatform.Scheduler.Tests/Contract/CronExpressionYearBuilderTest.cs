using System;
using System.Collections.Generic;

using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Contract
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class CronExpressionYearBuilderTest
    {
        private static readonly Dictionary<Action<ICronExpressionYearBuilder>, string> TestCases
            = new Dictionary<Action<ICronExpressionYearBuilder>, string>
              {
                  { i => { }, "*" },
                  { i => i.Every(), "*" },
                  { i => i.Each(2016), "2016" },
                  { i => i.Each(2016, 10), "2016/10" },
                  { i => i.EachOfSet(2016, 2017, 2018), "2016,2017,2018" },
                  { i => i.EachOfRange(2016, 2018), "2016-2018" }
              };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ShouldBuildExpression(KeyValuePair<Action<ICronExpressionYearBuilder>, string> testCase)
        {
            // Given
            var target = new CronExpressionYearBuilder();
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