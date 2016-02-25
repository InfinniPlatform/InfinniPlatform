namespace InfinniPlatform.Sdk.Hosting
{
    /// <summary>
    /// Обработчик событий приложения.
    /// </summary>
    public interface IApplicationEventHandler
    {
        /// <summary>
        /// Порядковый номер при выполнении.
        /// </summary>
        int Order { get; }

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