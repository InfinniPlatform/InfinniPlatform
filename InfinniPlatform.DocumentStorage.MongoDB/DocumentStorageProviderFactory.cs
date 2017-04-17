using System;
using System.Collections.Concurrent;

using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.DocumentStorage.Abstractions;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal class DocumentStorageProviderFactory : IDocumentStorageProviderFactory
    {
        public DocumentStorageProviderFactory(IContainerResolver containerResolver, Func<string, IDocumentStorageProvider> storageProviderFactory)
        {
            _containerResolver = containerResolver;
            _storageProviderFactory = storageProviderFactory;
        }


        private readonly IContainerResolver _containerResolver;
        private readonly Func<string, IDocumentStorageProvider> _storageProviderFactory;


        private readonly ConcurrentDictionary<string, IDocumentStorageProvider> _storageProviders
            = new ConcurrentDictionary<string, IDocumentStorageProvider>();

        public IDocumentStorageProvider GetStorageProvider(string documentType)
        {
            if (string.IsNullOrEmpty(documentType))
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            IDocumentStorageProvider storageProvider;

            // Кэширование провайдера для каждого типа документа, чтобы не создавать его каждый раз при запросе
            if (!_storageProviders.TryGetValue(documentType, out storageProvider))
            {
                storageProvider = _storageProviderFactory.Invoke(documentType);
                storageProvider = _storageProviders.GetOrAdd(documentType, storageProvider);
            }

            return storageProvider;
        }


        private readonly ConcurrentDictionary<string, object> _genericStorageProviders
            = new ConcurrentDictionary<string, object>();

        public IDocumentStorageProvider<TDocument> GetStorageProvider<TDocument>(string documentType = null)
        {
            object storageProvider;

            var storageProviderKey = $"{typeof(TDocument).FullName};{documentType}";

            // Кэширование провайдера для каждого типа документа, чтобы не создавать его каждый раз при запросе
            if (!_genericStorageProviders.TryGetValue(storageProviderKey, out storageProvider))
            {
                if (string.IsNullOrEmpty(documentType))
                {
                    storageProvider = _containerResolver.Resolve<IDocumentStorageProvider<TDocument>>();
                }
                else
                {
                    var storageProviderFactory = _containerResolver.Resolve<Func<string, IDocumentStorageProvider<TDocument>>>();
                    storageProvider = storageProviderFactory.Invoke(documentType);
                }

                storageProvider = _genericStorageProviders.GetOrAdd(storageProviderKey, storageProvider);
            }

            return (IDocumentStorageProvider<TDocument>)storageProvider;
        }
    }
}