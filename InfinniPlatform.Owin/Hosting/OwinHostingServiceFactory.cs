using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Factories;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Hosting;

namespace InfinniPlatform.Owin.Hosting
{
    /// <summary>
    /// Фабрика для создания сервиса хостинга приложения на базе OWIN.
    /// </summary>
    public sealed class OwinHostingServiceFactory : IHostingServiceFactory
    {
        public OwinHostingServiceFactory(IOwinHostingContext hostingContext)
        {
            // Создание сервиса хостинга приложения на базе OWIN
            var hostingService = new OwinHostingService(hostingContext);

            // Получение списка обработчиков событий приложения
            var appEventHandlers = hostingContext.ContainerResolver.Resolve<IEnumerable<IAppEventHandler>>()?.OrderBy(i => i.Order).ToList();

            if (appEventHandlers != null)
            {
                hostingService.OnBeforeStart += (s, e) => InvokeAppHandlers(appEventHandlers, h => h.OnBeforeStart());
                hostingService.OnAfterStart += (s, e) => InvokeAppHandlers(appEventHandlers, h => h.OnAfterStart());
                hostingService.OnBeforeStop += (s, e) => InvokeAppHandlers(appEventHandlers, h => h.OnBeforeStop());
                hostingService.OnAfterStop += (s, e) => InvokeAppHandlers(appEventHandlers, h => h.OnAfterStop());
            }

            _hostingService = hostingService;
        }


        private readonly OwinHostingService _hostingService;


        /// <summary>
        /// Создать сервис хостинга.
        /// </summary>
        public IHostingService CreateHostingService()
        {
            return _hostingService;
        }


        private static void InvokeAppHandlers(IEnumerable<IAppEventHandler> handlers, Action<IAppEventHandler> handle)
        {
            foreach (var handler in handlers.OrderBy(i => i.Order))
            {
                handle(handler);
            }
        }
    }
}