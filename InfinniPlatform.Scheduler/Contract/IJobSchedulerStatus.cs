namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Состояние планировщика заданий.
    /// </summary>
    public interface IJobSchedulerStatus
    {
        /// <summary>
        /// Планировщик заданий запущен.
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Общее количество заданий.
        /// </summary>
        int Total { get; }

        /// <summary>
        /// Количество заданий в состоянии <see cref="JobState.Planned" />.
        /// </summary>
        int Planned { get; }

        /// <summary>
        /// Количество заданий в состоянии <see cref="JobState.Paused" />.
        /// </summary>
        int Paused { get; }
    }
}