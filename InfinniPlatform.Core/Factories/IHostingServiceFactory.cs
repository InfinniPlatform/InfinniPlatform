using InfinniPlatform.Hosting;

namespace InfinniPlatform.Factories
{
    /// <summary>
    ///     Фабрика для создания сервиса хостинга.
    /// </summary>
    public interface IHostingServiceFactory
    {
        /// <summary>
        ///     Создать сервис хостинга.
        /// </summary>
        IHostingService CreateHostingService();
    }
}