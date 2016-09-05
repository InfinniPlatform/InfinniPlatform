namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Настройки планировщика заданий.
    /// </summary>
    public class SchedulerSettings
    {
        public const string SectionName = "scheduler";

        public static SchedulerSettings Default = new SchedulerSettings();


        public SchedulerSettings()
        {
        }


        /// <summary>
        /// Время хранения информации о вызове обработчиков заданий (в секундах).
        /// </summary>
        public int? ExpireHistoryAfter { get; set; }
    }
}