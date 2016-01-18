using System.Collections.Generic;

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
            var appEventHandlers = hostingContext.ContainerResolver.Resolve<IEnumerable<IApplicationEventHandler>>();

            if (appEventHandlers != null)
            {
                hostingService.OnStart += (s, e) =>
                                          {
                                              foreach (var handler in appEventHandlers)
                                              {
                                                  handler.OnStart();
                                              }
                                          };

                hostingService.OnStart += (s, e) =>
                                          {
                                              foreach (var handler in appEventHandlers)
                                              {
                                                  handler.OnStop();
                                              }
                                          };
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
    }
}