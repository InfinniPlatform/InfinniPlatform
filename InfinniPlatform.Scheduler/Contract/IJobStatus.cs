namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Информация о состоянии задания.
    /// </summary>
    public interface IJobStatus
    {
        /// <summary>
        /// Информация о задании.
        /// </summary>
        IJobInfo Info { get; }

        /// <summary>
        /// Состояние выполнения задания.
        /// </summary>
        JobState State { get; }
    }
}