using System;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Методы расширения для <see cref="ICronExpressionBuilder" />.
    /// </summary>
    public static class CronExpressionBuilderExtensions
    {
        /// <summary>
        /// Устанавливает CRON-выражение планирования задания.
        /// </summary>
        /// <param name="target">Предоставляет методы для создания информации о задании.</param>
        /// <param name="сronExpression">Функция для определения времени свершения событий.</param>
        public static IJobInfoBuilder CronExpression(this IJobInfoBuilder target,
                                                     Action<ICronExpressionBuilder> сronExpression)
        {
            var cronBuilder = new CronExpressionBuilder();

            сronExpression?.Invoke(cronBuilder);

            return target.CronExpression(cronBuilder.Build());
        }

        /// <summary>
        /// Устанавливает CRON-выражение планирования задания, срабатывающего в определенный час и минуту ежедневно.
        /// </summary>
        /// <param name="target">Предоставляет набор методов для определения времени свершения событий.</param>
        /// <param name="hour">Час.</param>
        /// <param name="minute">Минута.</param>
        public static ICronExpressionBuilder AtHourAndMinuteDaily(this ICronExpressionBuilder target,
                                                                  int hour,
                                                                  int minute)
        {
            return target.Hours(h => h.Each(hour))
                         .Minutes(m => m.Each(minute))
                         .Seconds(s => s.Each(0))
                         .DayOfWeek(d => d.Every());
        }

        /// <summary>
        /// Устанавливает CRON-выражение планирования задания, срабатывающего в определенный час и минуту в указанные дни недели.
        /// </summary>
        /// <param name="target">Предоставляет набор методов для определения времени свершения событий.</param>
        /// <param name="hour">Час.</param>
        /// <param name="minute">Минута.</param>
        /// <param name="daysOfWeek">Список дней недели.</param>
        public static ICronExpressionBuilder AtHourAndMinuteOnGivenDaysOfWeek(this ICronExpressionBuilder target,
                                                                              int hour,
                                                                              int minute,
                                                                              params DayOfWeek[] daysOfWeek)
        {
            return target.Hours(h => h.Each(hour))
                         .Minutes(m => m.Each(minute))
                         .Seconds(s => s.Each(0))
                         .DayOfWeek(d => d.EachOfSet(daysOfWeek));
        }

        /// <summary>
        /// Устанавливает CRON-выражение планирования задания, срабатывающего в определенный час и минуту в указанные дни месяца.
        /// </summary>
        /// <param name="target">Предоставляет набор методов для определения времени свершения событий.</param>
        /// <param name="hour">Час.</param>
        /// <param name="minute">Минута.</param>
        /// <param name="daysOfMonth">Список дней месяца (каждый от 1 до 31).</param>
        public static ICronExpressionBuilder AtHourAndMinuteMonthly(this ICronExpressionBuilder target,
                                                                    int hour,
                                                                    int minute,
                                                                    params int[] daysOfMonth)
        {
            return target.Hours(h => h.Each(hour))
                         .Minutes(m => m.Each(minute))
                         .Seconds(s => s.Each(0))
                         .DayOfMonth(d => d.EachOfSet(daysOfMonth));
        }
    }
}