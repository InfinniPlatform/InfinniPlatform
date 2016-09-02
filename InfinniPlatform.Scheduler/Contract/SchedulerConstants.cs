namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Константы для <see cref="InfinniPlatform.Scheduler"/>.
    /// </summary>
    public static class SchedulerConstants
    {
        /// <summary>
        /// Имя компонента.
        /// </summary>
        public const string ComponentName = "Scheduler";

        /// <summary>
        /// Префикс для наименований объектов.
        /// </summary>
        /// <remarks>
        /// Используется в наименовании типов документов хранилища данных
        /// и в наименовании очередей распределенной шины сообщений.
        /// </remarks>
        public const string ObjectNamePrefix = ComponentName + ".";
    }
}