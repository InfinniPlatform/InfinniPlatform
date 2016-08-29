using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Контекст обработки задания.
    /// </summary>
    public interface IJobHandlerContext
    {
        /// <summary>
        /// Информация о задании.
        /// </summary>
        JobInfo JobInfo { get; }

        /// <summary>
        /// Фактическое время срабатывания задания.
        /// </summary>
        DateTimeOffset? FireTimeUtc { get; }

        /// <summary>
        /// Запланированное время срабатывания задания.
        /// </summary>
        DateTimeOffset? ScheduledFireTimeUtc { get; }

        /// <summary>
        /// Предыдущее время срабатывания задания.
        /// </summary>
        DateTimeOffset? PreviousFireTimeUtc { get; }

        /// <summary>
        /// Следующее время срабатывания задания.
        /// </summary>
        DateTimeOffset? NextFireTimeUtc { get; }
    }
}