namespace InfinniPlatform.Core.Http.Hosting
{
    /// <summary>
    /// Сервис хостинга приложения.
    /// </summary>
    public interface IHostingService
    {
        /// <summary>
        /// Запускает инициализацию приложения.
        /// </summary>
        void Init();

        /// <summary>
        /// Запускает хостинг приложения.
        /// </summary>
        void Start();

        /// <summary>
        /// Останавливает хостинг приложения.
        /// </summary>
        void Stop();
    }
}