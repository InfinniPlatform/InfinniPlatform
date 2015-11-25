using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Owin.Modules
{
    /// <summary>
    /// Контекст подсистемы хостинга на базе OWIN.
    /// </summary>
    public interface IOwinHostingContext
    {
        /// <summary>
        /// Настройки подсистемы хостинга.
        /// </summary>
        HostingConfig Configuration { get; }

        /// <summary>
        /// Провайдер разрешения зависимостей приложения.
        /// </summary>
        IContainerResolver ContainerResolver { get; }

        /// <summary>
        /// Провайдер разрешения типа обработчика запросов OWIN.
        /// </summary>
        IOwinMiddlewareResolver OwinMiddlewareResolver { get; }
    }
}