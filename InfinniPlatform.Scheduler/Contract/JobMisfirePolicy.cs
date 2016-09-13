namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Политика обработки пропущенных заданий.
    /// </summary>
    public enum JobMisfirePolicy
    {
        /// <summary>
        /// Игнорировать пропущенные задания.
        /// </summary>
        DoNothing = 0,

        /// <summary>
        /// Выполнить одно задание при старте и далее следовать расписанию.
        /// </summary>
        FireAndProceed = 1
    }
}