using System;
using System.Collections.Concurrent;

using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal sealed class DocumentStorageFactory : IDocumentStorageFactory
    {
        public DocumentStorageFactory(IContainerResolver containerResolver, Func<string, IDocumentStorage> storageFactory)
        {
            _containerResolver = containerResolver;
            _storageFactory = storageFactory;
        }


        private readonly IContainerResolver _containerResolver;
        private readonly Func<string, IDocumentStorage> _storageFactory;


        private readonly ConcurrentDictionary<string, IDocumentStorage> _storages
            = new ConcurrentDictionary<string, IDocumentStorage>();

        public IDocumentStorage GetStorage(string documentTypeName)
        {
            if (string.IsNullOrEmpty(documentTypeName))
            {
                throw new ArgumentNullException(nameof(documentTypeName));
            }

            IDocumentStorage storage;

            // Кэширование хранилища для каждого типа документа, чтобы не создавать его каждый раз при запросе
            if (!_storages.TryGetValue(documentTypeName, out storage))
            {
                storage = _storageFactory.Invoke(documentTypeName);
                storage = _storages.GetOrAdd(documentTypeName, storage);
            }

            return storage;
        }


        private readonly ConcurrentDictionary<string, object> _genericStorages
            = new ConcurrentDictionary<string, object>();

        public object GetStorage(Type documentType, string documentTypeName = null)
        {
            object storage;

            var storageProviderKey = $"{documentType.FullName};{documentTypeName}";

            // Кэширование хранилища для каждого типа документа, чтобы не создавать его каждый раз при запросе
            if (!_genericStorages.TryGetValue(storageProviderKey, out storage))
            {
                if (string.IsNullOrEmpty(documentTypeName))
                {
                    var storageType = typeof(IDocumentStorage<>).MakeGenericType(documentType);
                    storage = _containerResolver.Resolve(storageType);
                }
                else
                {
                    var storageFactoryType = typeof(Func<,>).MakeGenericType(typeof(string), typeof(IDocumentStorage<>).MakeGenericType(documentType));
                    var storageFactory = (Delegate)_containerResolver.Resolve(storageFactoryType);
                    storage = storageFactory.DynamicInvoke(documentTypeName);
                }

                storage = _genericStorages.GetOrAdd(storageProviderKey, storage);
            }

            return storage;
        }

        public IDocumentStorage<TDocument> GetStorage<TDocument>(string documentTypeName = null) where TDocument : Document
        {
            object storage;

            var storageProviderKey = $"{typeof(TDocument).FullName};{documentTypeName}";


            // Кэширование хранилища для каждого типа документа, чтобы не создавать его каждый раз при запросе
            if (!_genericStorages.TryGetValue(storageProviderKey, out storage))
            {
                if (string.IsNullOrEmpty(documentTypeName))
                {
                    storage = _containerResolver.Resolve<IDocumentStorage<TDocument>>();
                }
                else
                {
                    var storageProviderFactory = _containerResolver.Resolve<Func<string, IDocumentStorage<TDocument>>>();
                    storage = storageProviderFactory.Invoke(documentTypeName);
                }

                storage = _genericStorages.GetOrAdd(storageProviderKey, storage);
            }

            return (IDocumentStorage<TDocument>)storage;
        }
    }
}