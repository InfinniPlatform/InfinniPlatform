using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет методы для создания информации о задании.
    /// </summary>
    public interface IJobInfoBuilder
    {
        /// <summary>
        /// Устанавливает состояние выполнения задания.
        /// </summary>
        IJobInfoBuilder State(JobState state);

        /// <summary>
        /// Устанавливает описание назначения задания.
        /// </summary>
        IJobInfoBuilder Description(string description);

        /// <summary>
        /// Устанавливает CRON-выражение планирования задания.
        /// </summary>
        IJobInfoBuilder CronExpression(string cronExpression);

        /// <summary>
        /// Устанавливает время начала планирования задания.
        /// </summary>
        IJobInfoBuilder StartTimeUtc(DateTimeOffset? startTimeUtc);

        /// <summary>
        /// Устанавливает время окончания планирования задания.
        /// </summary>
        IJobInfoBuilder EndTimeUtc(DateTimeOffset? endTimeUtc);

        /// <summary>
        /// Устанавливает политику обработки пропущенных срабатываний задания.
        /// </summary>
        IJobInfoBuilder MisfirePolicy(JobMisfirePolicy? misfirePolicy);

        /// <summary>
        /// Устанавливает данные для выполнения задания.
        /// </summary>
        IJobInfoBuilder Data(object data);
    }
}