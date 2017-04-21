namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Настройки планировщика заданий Quartz.
    /// </summary>
    public class QuartzSchedulerOptions
    {
        /// <summary>
        /// Имя секции в файле конфигурации.
        /// </summary>
        public const string SectionName = "quartzScheduler";

        /// <summary>
        /// Настройка планировщика заданий по умолчанию.
        /// </summary>
        public static readonly QuartzSchedulerOptions Default = new QuartzSchedulerOptions();


        /// <summary>
        /// Конструктор.
        /// </summary>
        public QuartzSchedulerOptions()
        {
        }


        /// <summary>
        /// Время хранения информации о вызове обработчиков заданий (в секундах).
        /// </summary>
        public int? ExpireHistoryAfter { get; set; }
    }
}