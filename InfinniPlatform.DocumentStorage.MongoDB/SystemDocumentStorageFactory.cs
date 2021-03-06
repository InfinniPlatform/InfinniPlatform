﻿using System;
using System.Collections.Concurrent;

using InfinniPlatform.IoC;

namespace InfinniPlatform.DocumentStorage
{
    /// <inheritdoc />
    public class SystemDocumentStorageFactory : ISystemDocumentStorageFactory
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SystemDocumentStorageFactory" />.
        /// </summary>
        /// <param name="containerResolver">Dependency resolver from container.</param>
        /// <param name="storageFactory">Factory for creating <see cref="ISystemDocumentStorage"/></param> instances.
        public SystemDocumentStorageFactory(IContainerResolver containerResolver, Func<string, ISystemDocumentStorage> storageFactory)
        {
            _containerResolver = containerResolver;
            _storageFactory = storageFactory;
        }

        private readonly IContainerResolver _containerResolver;
        private readonly Func<string, ISystemDocumentStorage> _storageFactory;

        private readonly ConcurrentDictionary<string, object> _genericStorages = new ConcurrentDictionary<string, object>();
        private readonly ConcurrentDictionary<string, ISystemDocumentStorage> _storages = new ConcurrentDictionary<string, ISystemDocumentStorage>();

        /// <inheritdoc />
        public ISystemDocumentStorage GetStorage(string documentTypeName)
        {
            if (string.IsNullOrEmpty(documentTypeName))
            {
                throw new ArgumentNullException(nameof(documentTypeName));
            }

            ISystemDocumentStorage storage;

            // Кэширование хранилища для каждого типа документа, чтобы не создавать его каждый раз при запросе
            if (!_storages.TryGetValue(documentTypeName, out storage))
            {
                storage = _storageFactory.Invoke(documentTypeName);
                storage = _storages.GetOrAdd(documentTypeName, storage);
            }

            return storage;
        }

        /// <inheritdoc />
        public object GetStorage(Type documentType, string documentTypeName = null)
        {
            var storageProviderKey = $"{documentType.FullName};{documentTypeName}";

            // Кэширование хранилища для каждого типа документа, чтобы не создавать его каждый раз при запросе
            if (!_genericStorages.TryGetValue(storageProviderKey, out var storage))
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

        /// <inheritdoc />
        public ISystemDocumentStorage<TDocument> GetStorage<TDocument>(string documentTypeName = null) where TDocument : Document
        {
            var storageProviderKey = $"{typeof(TDocument).FullName};{documentTypeName}";

            // Кэширование хранилища для каждого типа документа, чтобы не создавать его каждый раз при запросе
            if (!_genericStorages.TryGetValue(storageProviderKey, out var storage))
            {
                if (string.IsNullOrEmpty(documentTypeName))
                {
                    storage = _containerResolver.Resolve<ISystemDocumentStorage<TDocument>>();
                }
                else
                {
                    var storageProviderFactory = _containerResolver.Resolve<Func<string, ISystemDocumentStorage<TDocument>>>();
                    storage = storageProviderFactory.Invoke(documentTypeName);
                }

                storage = _genericStorages.GetOrAdd(storageProviderKey, storage);
            }

            return (ISystemDocumentStorage<TDocument>)storage;
        }
    }
}