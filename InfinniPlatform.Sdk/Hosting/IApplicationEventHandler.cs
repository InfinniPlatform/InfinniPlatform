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
        /// Вызывается перед запуском приложения.
        /// </summary>
        void OnBeforeStart();

        /// <summary>
        /// Вызывается после запуска приложения.
        /// </summary>
        void OnAfterStart();

        /// <summary>
        /// Вызывается перед остановкой приложения.
        /// </summary>
        void OnBeforeStop();

        /// <summary>
        /// Вызывается после остановки приложения.
        /// </summary>
        void OnAfterStop();
    }
}