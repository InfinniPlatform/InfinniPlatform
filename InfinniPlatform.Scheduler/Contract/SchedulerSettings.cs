namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Настройки планировщика заданий.
    /// </summary>
    public class SchedulerSettings
    {
        /// <summary>
        /// Имя секции в файле конфигурации.
        /// </summary>
        public const string SectionName = "scheduler";

        /// <summary>
        /// Настройка планировщика заданий по умолчанию.
        /// </summary>
        public static readonly SchedulerSettings Default = new SchedulerSettings();


        /// <summary>
        /// Конструктор.
        /// </summary>
        public SchedulerSettings()
        {
        }


        /// <summary>
        /// Время хранения информации о вызове обработчиков заданий (в секундах).
        /// </summary>
        public int? ExpireHistoryAfter { get; set; }
    }
}