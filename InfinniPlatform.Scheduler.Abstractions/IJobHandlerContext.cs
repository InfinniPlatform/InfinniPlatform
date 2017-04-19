using System;

using InfinniPlatform.Core.Abstractions.Dynamic;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Контекст обработки задания.
    /// </summary>
    public interface IJobHandlerContext
    {
        /// <summary>
        /// Уникальный идентификатор экземпляра задания.
        /// </summary>
        string InstanceId { get; }

        /// <summary>
        /// Фактическое время выполнения задания.
        /// </summary>
        DateTimeOffset FireTimeUtc { get; }

        /// <summary>
        /// Запланированное время выполнения задания.
        /// </summary>
        DateTimeOffset ScheduledFireTimeUtc { get; }

        /// <summary>
        /// Время предыдущего запланированного выполнения задания.
        /// </summary>
        DateTimeOffset? PreviousFireTimeUtc { get; }

        /// <summary>
        /// Время следующего запланированного выполнения задания.
        /// </summary>
        DateTimeOffset? NextFireTimeUtc { get; }

        /// <summary>
        /// Данные для выполнения задания.
        /// </summary>
        DynamicWrapper Data { get; }
    }
}