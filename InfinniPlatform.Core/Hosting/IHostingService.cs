using System;

namespace InfinniPlatform.Core.Hosting
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

        /// <summary>
        /// Событие запуска хостинга приложения.
        /// </summary>
        event EventHandler OnAfterStart;

        /// <summary>
        /// Событие остановки хостинга приложения.
        /// </summary>
        event EventHandler OnBeforeStop;
    }
}