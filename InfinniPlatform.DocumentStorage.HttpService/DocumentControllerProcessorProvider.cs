using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Dynamic;
using InfinniPlatform.IoC;

namespace InfinniPlatform.DocumentStorage
{
    public class DocumentControllerProcessorProvider : IDocumentControllerProcessorProvider
    {
        public DocumentControllerProcessorProvider(IEnumerable<IDocumentHttpServiceHandlerBase> httpServiceHandlers,
                                                   IContainerResolver containerResolver)
        {
            _httpServiceHandlers = httpServiceHandlers;
            _containerResolver = containerResolver;
            _processorsCacheLazy = new Lazy<Dictionary<string, IDocumentControllerProcessor>>(CreateProcessorsCache);
        }

        private readonly IEnumerable<IDocumentHttpServiceHandlerBase> _httpServiceHandlers;
        private readonly IContainerResolver _containerResolver;
        private readonly Lazy<Dictionary<string, IDocumentControllerProcessor>> _processorsCacheLazy;

        public Dictionary<string, IDocumentControllerProcessor> ProcessorsCache => _processorsCacheLazy.Value;

        private Dictionary<string, IDocumentControllerProcessor> CreateProcessorsCache()
        {
            var dictionary = new Dictionary<string, IDocumentControllerProcessor>();

            foreach (var httpServiceHandler in _httpServiceHandlers)
            {
                // Создание типизированных сервисов

                var httpServiceHandlerType = httpServiceHandler.GetType();

                var clrDocumentTypes = httpServiceHandlerType
                    .GetTypeInfo()
                    .GetInterfaces()
                    .Where(i => IntrospectionExtensions.GetTypeInfo(i).IsGenericType && i.GetGenericTypeDefinition() == typeof(IDocumentHttpServiceHandler<>))
                    .Select(i => i.GetTypeInfo().GetGenericArguments()[0]);

                foreach (var clrDocumentType in clrDocumentTypes)
                {
                    var handlerType = typeof(IDocumentHttpServiceHandlerBase);
                    var serviceType = typeof(DocumentControllerProcessor<>).MakeGenericType(clrDocumentType);
                    var serviceFunc = typeof(Func<,>).MakeGenericType(handlerType, serviceType);

                    var serviceFactory = (Delegate)_containerResolver.Resolve(serviceFunc);

                    var processor = (IDocumentControllerProcessor)serviceFactory.FastDynamicInvoke(httpServiceHandler);

                    dictionary.Add(httpServiceHandler.DocumentType, processor);
                }

                // Создание не типизированных сервисов

                if (httpServiceHandler is IDocumentHttpServiceHandler)
                {
                    var serviceFactory = _containerResolver.Resolve<Func<IDocumentHttpServiceHandlerBase, DocumentControllerProcessor>>();

                    var processor = (IDocumentControllerProcessor)serviceFactory.Invoke(httpServiceHandler);

                    dictionary.Add(httpServiceHandler.DocumentType, processor);
                }
            }

            return dictionary;
        }
    }
}