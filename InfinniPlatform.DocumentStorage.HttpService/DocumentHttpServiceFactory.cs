using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.IoC;

namespace InfinniPlatform.DocumentStorage.HttpService
{
    /// <summary>
    /// Предоставляет интерфейс для создания сервисов по работе с документами.
    /// </summary>
    public class DocumentHttpServiceFactory : IDocumentHttpServiceFactory
    {
        public DocumentHttpServiceFactory(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }


        private readonly IContainerResolver _containerResolver;


        /// <summary>
        /// Создает сервисы по работе с документами на основе указанного обработчика.
        /// </summary>
        /// <param name="httpServiceHandler">Обработчик для сервиса по работе с документами.</param>
        public IEnumerable<IHttpService> CreateServices(IDocumentHttpServiceHandlerBase httpServiceHandler)
        {
            // Обработчик может быть универсальным и реализовывать сразу несколько интерфейсов

            var httpServiceHandlerType = httpServiceHandler.GetType();

            // Создание типизированных сервисов

            var clrDocumentTypes = httpServiceHandlerType
                .GetTypeInfo()
                .GetInterfaces()
                .Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IDocumentHttpServiceHandler<>))
                .Select(i => i.GetTypeInfo().GetGenericArguments()[0]);

            foreach (var clrDocumentType in clrDocumentTypes)
            {
                var handlerType = typeof(IDocumentHttpServiceHandlerBase);
                var serviceType = typeof(DocumentHttpService<>).MakeGenericType(clrDocumentType);
                var serviceFunc = typeof(Func<,>).MakeGenericType(handlerType, serviceType);

                var serviceFactory = (Delegate)_containerResolver.Resolve(serviceFunc);

                var service = (IHttpService)serviceFactory.FastDynamicInvoke(httpServiceHandler);

                yield return service;
            }

            // Создание не типизированных сервисов

            if (httpServiceHandler is IDocumentHttpServiceHandler)
            {
                var serviceFactory = _containerResolver.Resolve<Func<IDocumentHttpServiceHandlerBase, DocumentHttpService>>();

                var service = (IHttpService)serviceFactory.Invoke(httpServiceHandler);

                yield return service;
            }
        }
    }
}