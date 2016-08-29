namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Состояние выполнения задания.
    /// </summary>
    public enum JobState
    {
        /// <summary>
        /// Запланировано.
        /// </summary>
        Planned = 0,

        /// <summary>
        /// Преостановлено.
        /// </summary>
        Paused = 1
    }
}