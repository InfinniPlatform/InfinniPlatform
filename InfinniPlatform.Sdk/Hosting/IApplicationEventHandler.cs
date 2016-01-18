namespace InfinniPlatform.Sdk.Hosting
{
    /// <summary>
    /// Обработчик событий приложения.
    /// </summary>
    public interface IApplicationEventHandler
    {
        /// <summary>
        /// Обрабатывает событие запуска приложения.
        /// </summary>
        void OnStart();

        /// <summary>
        /// Обрабатывает событие остановки приложения.
        /// </summary>
        void OnStop();
    }
}