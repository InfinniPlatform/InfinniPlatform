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
                  // Срабатывает каждую секунду
                  {
                      b => { },
                      "* * * * * ?"
                  },
                  // Срабатывает в 12:00:00 каждый день
                  {
                      b => b.Hours(i => i.Each(12))
                            .Minutes(i => i.Each(0))
                            .Seconds(i => i.Each(0)),
                      "0 0 12 * * ?"
                  },
                  // Срабатывает в 10:15:00 каждый день
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0)),
                      "0 15 10 * * ?"
                  },
                  // Срабатывает каждую минуты с 14:00:00 по 14:59:00, каждый день
                  {
                      b => b.Hours(i => i.Each(14))
                            .Minutes(i => i.Every())
                            .Seconds(i => i.Each(0)),
                      "0 * 14 * * ?"
                  },
                  // Срабатывает каждые 5 минут с 14:00:00 по 14:55:00, каждый день
                  {
                      b => b.Hours(i => i.Each(14))
                            .Minutes(i => i.Each(0, 5))
                            .Seconds(i => i.Each(0)),
                      "0 0/5 14 * * ?"
                  },
                  // Срабатывает каждые 5 минут с 14:00:00 по 14:55:00 и с 18:00:00 по 18:55:00, каждый день
                  {
                      b => b.Hours(i => i.EachOfSet(14, 18))
                            .Minutes(i => i.Each(0, 5))
                            .Seconds(i => i.Each(0)),
                      "0 0/5 14,18 * * ?"
                  },
                  // Срабатывает каждую минуту с 14:00:00 по 14:05:00, каждый день
                  {
                      b => b.Hours(i => i.Each(14))
                            .Minutes(i => i.EachOfRange(0, 5))
                            .Seconds(i => i.Each(0)),
                      "0 0-5 14 * * ?"
                  },
                  // Срабатывает в 14:10:00 и 14:44:00, каждую среду марта
                  {
                      b => b.Hours(i => i.Each(14))
                            .Minutes(i => i.EachOfSet(10, 44))
                            .Seconds(i => i.Each(0))
                            .Month(i => i.Each(Month.March))
                            .DayOfWeek(i => i.Each(DayOfWeek.Wednesday)),
                      "0 10,44 14 * 3 4"
                  },
                  // Срабатывает в 10:15:00 каждый день с понедельника по пятницу
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0))
                            .DayOfWeek(i => i.EachOfRange(DayOfWeek.Monday, DayOfWeek.Friday)),
                      "0 15 10 * * 2-6"
                  },
                  // Срабатывает в 10:15:00 на 15 день каждого месяца
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0))
                            .DayOfMonth(i => i.Each(15)),
                      "0 15 10 15 * *"
                  },
                  // Срабатывает в 10:15:00 в последний день каждого месяца
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0))
                            .DayOfMonth(i => i.EachLast()),
                      "0 15 10 L * *"
                  },
                  // Срабатывает в 10:15:00 за два дня до последнего дня месяца
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0))
                            .DayOfMonth(i => i.EachLast(2)),
                      "0 15 10 L-2 * *"
                  },
                  // Срабатывает в 10:15:00 каждую последнюю пятницу каждого месяца
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0))
                            .DayOfWeek(i => i.EachLast(DayOfWeek.Friday)),
                      "0 15 10 * * 6L"
                  },
                  // Срабатывает в 10:15:00 каждую последнюю пятницу каждого месяца с 2016 по 2020 год
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0))
                            .DayOfWeek(i => i.EachLast(DayOfWeek.Friday))
                            .Year(i => i.EachOfRange(2016, 2020)),
                      "0 15 10 * * 6L 2016-2020"
                  },
                  // Срабатывает в 10:15:00 каждую третью пятницу каждого месяца
                  {
                      b => b.Hours(i => i.Each(10))
                            .Minutes(i => i.Each(15))
                            .Seconds(i => i.Each(0))
                            .DayOfWeek(i => i.EachNth(DayOfWeek.Friday, 3)),
                      "0 15 10 * * 6#3"
                  },
                  // Срабатывает в 12:00:00 каждые 5 дней каждого месяца, начиная с первого дня месяца
                  {
                      b => b.Hours(i => i.Each(12))
                            .Minutes(i => i.Each(0))
                            .Seconds(i => i.Each(0))
                            .DayOfMonth(i => i.Each(1, 5)),
                      "0 0 12 1/5 * *"
                  },
                  // Срабатывает в 11:11:00 11 ноября
                  {
                      b => b.Hours(i => i.Each(11))
                            .Minutes(i => i.Each(11))
                            .Seconds(i => i.Each(0))
                            .DayOfMonth(i => i.Each(11))
                            .Month(i => i.Each(Month.November)),
                      "0 11 11 11 11 *"
                  },
                  // Срабатывает в 10:15:00 ежедневно
                  {
                      b => b.AtHourAndMinuteDaily(10, 15),
                      "0 15 10 * * ?"
                  },
                  // Срабатывает в 10:15:00 каждый понедельник, среду и пятницу
                  {
                      b => b.AtHourAndMinuteOnGivenDaysOfWeek(10, 15, DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday),
                      "0 15 10 * * 2,4,6"
                  },
                  // Срабатывает в 10:15:00 каждого 1, 10 и 15 числа каждого месяца
                  {
                      b => b.AtHourAndMinuteMonthly(10, 15, 1, 10, 15),
                      "0 15 10 1,10,15 * *"
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