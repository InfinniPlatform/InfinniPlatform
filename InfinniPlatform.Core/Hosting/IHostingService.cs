using System;

namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    /// Сервис хостинга приложения.
    /// </summary>
    public interface IHostingService
    {
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
        event EventHandler OnStart;

        /// <summary>
        /// Событие остановки хостинга приложения.
        /// </summary>
        event EventHandler OnStop;
    }
}