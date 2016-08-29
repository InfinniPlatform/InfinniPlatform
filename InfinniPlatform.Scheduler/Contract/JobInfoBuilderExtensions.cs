using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Методы расширения для <see cref="IJobInfoBuilder"/>.
    /// </summary>
    public static class JobInfoBuilderExtensions
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
        /// <param name="target">Предоставляет методы для создания информации о задании.</param>
        /// <param name="hour">Час.</param>
        /// <param name="minute">Минута.</param>
        public static IJobInfoBuilder AtHourAndMinuteDaily(this IJobInfoBuilder target,
                                                           int hour,
                                                           int minute)
        {
            return CronExpression(target, c => c
                .Hours(h => h.Each(hour))
                .Minutes(m => m.Each(minute))
                .DayOfWeek(d => d.Every()));
        }

        /// <summary>
        /// Устанавливает CRON-выражение планирования задания, срабатывающего в определенный час и минуту в указанные дни недели.
        /// </summary>
        /// <param name="target">Предоставляет методы для создания информации о задании.</param>
        /// <param name="hour">Час.</param>
        /// <param name="minute">Минута.</param>
        /// <param name="daysOfWeek">Список дней недели.</param>
        public static IJobInfoBuilder AtHourAndMinuteOnGivenDaysOfWeek(this IJobInfoBuilder target,
                                                                       int hour,
                                                                       int minute,
                                                                       params DayOfWeek[] daysOfWeek)
        {
            return CronExpression(target, c => c
                .Hours(h => h.Each(hour))
                .Minutes(m => m.Each(minute))
                .DayOfWeek(d => d.EachOfSet(daysOfWeek)));
        }

        /// <summary>
        /// Устанавливает CRON-выражение планирования задания, срабатывающего в определенный час и минуту в указанные дни месяца.
        /// </summary>
        /// <param name="target">Предоставляет методы для создания информации о задании.</param>
        /// <param name="hour">Час.</param>
        /// <param name="minute">Минута.</param>
        /// <param name="daysOfMonth">Список дней месяца (каждый от 1 до 31).</param>
        public static IJobInfoBuilder AtHourAndMinuteMonthly(this IJobInfoBuilder target,
                                                             int hour,
                                                             int minute,
                                                             params int[] daysOfMonth)
        {
            return CronExpression(target, c => c
                .Hours(h => h.Each(hour))
                .Minutes(m => m.Each(minute))
                .DayOfMonth(d => d.EachOfSet(daysOfMonth)));
        }
    }
}