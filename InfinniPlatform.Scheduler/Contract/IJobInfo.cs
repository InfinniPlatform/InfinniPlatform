using System;

using InfinniPlatform.Core.Abstractions.Dynamic;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Информация о задании.
    /// </summary>
    public interface IJobInfo
    {
        /// <summary>
        /// ID задания.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Имя задания.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Группа задания.
        /// </summary>
        string Group { get; }

        /// <summary>
        /// Состояние выполнения задания.
        /// </summary>
        JobState State { get; }

        /// <summary>
        /// Описание назначения задания.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Тип обработчика заданий.
        /// </summary>
        string HandlerType { get; }

        /// <summary>
        /// CRON-выражение планирования задания.
        /// </summary>
        string CronExpression { get; }

        /// <summary>
        /// Время начала планирования задания.
        /// </summary>
        DateTimeOffset? StartTimeUtc { get; }

        /// <summary>
        /// Время окончания планирования задания.
        /// </summary>
        DateTimeOffset? EndTimeUtc { get; }

        /// <summary>
        /// Политика обработки пропущенных заданий.
        /// </summary>
        JobMisfirePolicy MisfirePolicy { get; }

        /// <summary>
        /// Данные для выполнения задания.
        /// </summary>
        DynamicWrapper Data { get; }
    }
}