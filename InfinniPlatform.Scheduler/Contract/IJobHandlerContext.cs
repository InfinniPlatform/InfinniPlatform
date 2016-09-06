using System;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Scheduler.Contract
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
        /// Фактическое время срабатывания задания.
        /// </summary>
        DateTimeOffset FireTimeUtc { get; }

        /// <summary>
        /// Запланированное время срабатывания задания.
        /// </summary>
        DateTimeOffset ScheduledFireTimeUtc { get; }

        /// <summary>
        /// Предыдущее время срабатывания задания.
        /// </summary>
        DateTimeOffset? PreviousFireTimeUtc { get; }

        /// <summary>
        /// Следующее время срабатывания задания.
        /// </summary>
        DateTimeOffset? NextFireTimeUtc { get; }

        /// <summary>
        /// Данные для выполнения задания.
        /// </summary>
        DynamicWrapper Data { get; }
    }
}