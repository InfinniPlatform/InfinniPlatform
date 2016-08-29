namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Политика обработки пропущенных срабатываний задания.
    /// </summary>
    public enum JobMisfirePolicy
    {
        /// <summary>
        /// Игнорировать пропущенные срабатывания.
        /// </summary>
        DoNothing = 0,

        /// <summary>
        /// Выполнить одно срабатывание при старте и далее следовать плану.
        /// </summary>
        FireAndProceed = 1
    }
}