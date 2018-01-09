using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Dynamic;
using InfinniPlatform.IoC;

namespace InfinniPlatform.DocumentStorage
{
    /// <inheritdoc />
    public class DocumentRequestExecutorProvider : IDocumentRequestExecutorProvider
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentRequestExecutorProvider" />.
        /// </summary>
        /// <param name="httpServiceHandlers">Set of document HTTP service handlers.</param>
        /// <param name="containerResolver">Dependencies resolver from IoC-container.</param>
        public DocumentRequestExecutorProvider(IEnumerable<IDocumentHttpServiceHandlerBase> httpServiceHandlers,
                                               IContainerResolver containerResolver)
        {
            _httpServiceHandlers = httpServiceHandlers;
            _containerResolver = containerResolver;
            _executorsCacheLazy = new Lazy<Dictionary<string, IDocumentRequestExecutor>>(CreateProcessorsCache);
        }

        private readonly IEnumerable<IDocumentHttpServiceHandlerBase> _httpServiceHandlers;
        private readonly IContainerResolver _containerResolver;
        private readonly Lazy<Dictionary<string, IDocumentRequestExecutor>> _executorsCacheLazy;

        /// <inheritdoc />
        public IDocumentRequestExecutor Get(string name)
        {
            return _executorsCacheLazy.Value[name];
        }

        private Dictionary<string, IDocumentRequestExecutor> CreateProcessorsCache()
        {
            var dictionary = new Dictionary<string, IDocumentRequestExecutor>();

            foreach (var httpServiceHandler in _httpServiceHandlers)
            {
                // Creating generic executors

                var httpServiceHandlerType = httpServiceHandler.GetType();

                var clrDocumentTypes = httpServiceHandlerType
                                       .GetTypeInfo()
                                       .GetInterfaces()
                                       .Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IDocumentHttpServiceHandler<>))
                                       .Select(i => i.GetTypeInfo().GetGenericArguments()[0]);

                foreach (var clrDocumentType in clrDocumentTypes)
                {
                    var handlerType = typeof(IDocumentHttpServiceHandlerBase);
                    var serviceType = typeof(DocumentRequestExecutor<>).MakeGenericType(clrDocumentType);
                    var serviceFunc = typeof(Func<,>).MakeGenericType(handlerType, serviceType);

                    var serviceFactory = (Delegate)_containerResolver.Resolve(serviceFunc);

                    var processor = (IDocumentRequestExecutor)serviceFactory.FastDynamicInvoke(httpServiceHandler);

                    dictionary.Add(httpServiceHandler.DocumentType, processor);
                }

                // Creating non-generic executors

                if (httpServiceHandler is IDocumentHttpServiceHandler)
                {
                    var serviceFactory = _containerResolver.Resolve<Func<IDocumentHttpServiceHandlerBase, DocumentRequestExecutor>>();

                    var processor = (IDocumentRequestExecutor)serviceFactory.Invoke(httpServiceHandler);

                    dictionary.Add(httpServiceHandler.DocumentType, processor);
                }
            }

            return dictionary;
        }
    }
}