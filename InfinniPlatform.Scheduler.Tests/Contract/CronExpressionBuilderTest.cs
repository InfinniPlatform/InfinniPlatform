using System;
using System.Collections.Generic;

using InfinniPlatform.Scheduler.Contract;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Tests.Contract
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class CronExpressionBuilderTest
    {
        private static readonly Dictionary<Action<ICronExpressionBuilder>, string> TestCases
            = new Dictionary<Action<ICronExpressionBuilder>, string>
              {
                  // Каждую секунду
                  {
                      b => { },
                      "* * * * * ?"
                  },
                  // Ежедневно в 12:00
                  {
                      b => b.Hours(i => i.Each(12))
                            .Minutes(i => i.Each(0))
                            .Seconds(i => i.Each(0)),
                      "0 0 12 * * ?"
                  },
                  // Ежедневно в 10:15
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0)),
                      "0 15 10 * * ?"
                  },
                  // Ежедневно каждую минуту с 14:00 по 14:59
                  {
                      b => b.Hours(i => i.Each(14))
                            .Minutes(i => i.Every())
                            .Seconds(i => i.Each(0)),
                      "0 * 14 * * ?"
                  },
                  // Ежедневно каждые 5 минут с 14:00 по 14:55
                  {
                      b => b.Hours(i => i.Each(14))
                            .Minutes(i => i.Each(0, 5))
                            .Seconds(i => i.Each(0)),
                      "0 0/5 14 * * ?"
                  },
                  // Ежедневно каждые 5 минут с 14:00 по 14:55 и с 18:00 по 18:55
                  {
                      b => b.Hours(i => i.EachOfSet(14, 18))
                            .Minutes(i => i.Each(0, 5))
                            .Seconds(i => i.Each(0)),
                      "0 0/5 14,18 * * ?"
                  },
                  // Ежедневно каждую минуту с 14:00 по 14:05
                  {
                      b => b.Hours(i => i.Each(14))
                            .Minutes(i => i.EachOfRange(0, 5))
                            .Seconds(i => i.Each(0)),
                      "0 0-5 14 * * ?"
                  },
                  // Каждую среду марта в 14:10 и 14:44
                  {
                      b => b.Hours(i => i.Each(14))
                            .Minutes(i => i.EachOfSet(10, 44))
                            .Seconds(i => i.Each(0))
                            .Month(i => i.Each(Month.March))
                            .DayOfWeek(i => i.Each(DayOfWeek.Wednesday)),
                      "0 10,44 14 ? 3 4"
                  },
                  // Каждый день с понедельника по пятницу в 10:15
                  {
                      b => b.AtHourAndMinuteDaily(10, 15)
                            .DayOfWeek(i => i.EachOfRange(DayOfWeek.Monday, DayOfWeek.Friday)),
                      "0 15 10 ? * 2-6"
                  },
                  // 15 числа каждого месяца в 10:15
                  {
                      b => b.AtHourAndMinuteDaily(10, 15)
                            .DayOfMonth(i => i.Each(15)),
                      "0 15 10 15 * ?"
                  },
                  // В последний день каждого месяца в 10:15
                  {
                      b => b.AtHourAndMinuteDaily(10, 15)
                            .DayOfMonth(i => i.EachLast()),
                      "0 15 10 L * ?"
                  },
                  // За 2 дня до последнего дня месяца в 10:15
                  {
                      b => b.AtHourAndMinuteDaily(10, 15)
                            .DayOfMonth(i => i.EachLast(2)),
                      "0 15 10 L-2 * ?"
                  },
                  // Каждую последнюю пятницу месяца в 10:15
                  {
                      b => b.AtHourAndMinuteDaily(10, 15)
                            .DayOfWeek(i => i.EachLast(DayOfWeek.Friday)),
                      "0 15 10 ? * 6L"
                  },
                  // Каждую последнюю пятницу месяца в 10:15 с 2016 по 2020 год
                  {
                      b => b.AtHourAndMinuteDaily(10, 15)
                            .DayOfWeek(i => i.EachLast(DayOfWeek.Friday))
                            .Year(i => i.EachOfRange(2016, 2020)),
                      "0 15 10 ? * 6L 2016-2020"
                  },
                  // Каждую третью пятницу месяца в 10:15
                  {
                      b => b.AtHourAndMinuteDaily(10, 15)
                            .DayOfWeek(i => i.EachNth(DayOfWeek.Friday, 3)),
                      "0 15 10 ? * 6#3"
                  },
                  // Через каждых 5 дней с 1 дня месяца в 12:00
                  {
                      b => b.AtHourAndMinuteDaily(12, 0)
                            .DayOfMonth(i => i.Each(1, 5)),
                      "0 0 12 1/5 * ?"
                  },
                  // 11 ноября в 11:11
                  {
                      b => b.AtHourAndMinuteDaily(11, 11)
                            .DayOfMonth(i => i.Each(11))
                            .Month(i => i.Each(Month.November)),
                      "0 11 11 11 11 ?"
                  },
                  // Ежедневно в 10:15
                  {
                      b => b.AtHourAndMinuteDaily(10, 15),
                      "0 15 10 * * ?"
                  },
                  // Каждый понедельник, среду и пятницу в 10:15
                  {
                      b => b.AtHourAndMinuteOnGivenDaysOfWeek(10, 15, DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday),
                      "0 15 10 ? * 2,4,6"
                  },
                  // 1, 10 и 15 числа в 10:15
                  {
                      b => b.AtHourAndMinuteMonthly(10, 15, 1, 10, 15),
                      "0 15 10 1,10,15 * ?"
                  },
                  // Каждый понедельник в 00:00
                  {
                      b => b.AtHourAndMinuteDaily(0, 0).DayOfWeek(d => d.Each(DayOfWeek.Monday)),
                      "0 0 0 ? * 2"
                  }
              };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ShouldBuildExpression(KeyValuePair<Action<ICronExpressionBuilder>, string> testCase)
        {
            // Given
            var target = new CronExpressionBuilder();
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