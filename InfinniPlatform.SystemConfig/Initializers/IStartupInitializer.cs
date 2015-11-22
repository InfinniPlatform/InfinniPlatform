namespace InfinniPlatform.SystemConfig.Initializers
{
    /// <summary>
    /// Сервис инициализации для обработки события старта приложения.
    /// </summary>
    public interface IStartupInitializer
    {
        /// <summary>
        /// Обрабатывает событие старта приложения.
        /// </summary>
        void OnStart();
    }
}