using System;
using System.Collections.Concurrent;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.BlobStorage;

namespace InfinniPlatform.SystemConfig.Metadata
{
    /// <summary>
    /// Прикладной объект конфигурации предметной области.
    /// </summary>
    public sealed class ConfigurationObject : IConfigurationObject
    {
        static ConfigurationObject()
        {
            VersionProviderCache = new ConcurrentDictionary<string, IVersionProvider>(StringComparer.OrdinalIgnoreCase);
        }

        private static readonly ConcurrentDictionary<string, IVersionProvider> VersionProviderCache;


        public ConfigurationObject(IMetadataConfiguration metadataConfiguration, IIndexFactory indexFactory, IBlobStorage blobStorage)
        {
            MetadataConfiguration = metadataConfiguration;
            _indexFactory = indexFactory;
            _blobStorage = blobStorage;
        }

        private readonly IIndexFactory _indexFactory;
        private readonly IBlobStorage _blobStorage;


        public IMetadataConfiguration MetadataConfiguration { get; }

        /// <summary>
        /// Возвращает провайдер версий документов.
        /// </summary>
        /// <param name="documentId">Имя документа.</param>
        /// <remarks>
        /// Создает провайдер, возвращающий версии документов всех существующих в индексе типов.
        /// </remarks>
        public IVersionProvider GetDocumentProvider(string documentId)
        {
            IVersionProvider versionProvider;

            var documentIndexName = MetadataConfiguration.ConfigurationId;
            var documentTypeName = MetadataConfiguration.GetMetadataIndexType(documentId);

            var versionProviderKey = $"{documentIndexName},{documentTypeName}";

            if (!VersionProviderCache.TryGetValue(versionProviderKey, out versionProvider))
            {
                versionProvider = _indexFactory.BuildVersionProvider(documentIndexName, documentTypeName);

                VersionProviderCache[versionProviderKey] = versionProvider;
            }

            return versionProvider;
        }

        /// <summary>
        /// Возвращает конструктор версий индекса.
        /// </summary>
        /// <param name="documentId">Имя документа.</param>
        public IVersionBuilder GetVersionBuilder(string documentId)
        {
            return _indexFactory.BuildVersionBuilder(
                MetadataConfiguration.ConfigurationId,
                MetadataConfiguration.GetMetadataIndexType(documentId));
        }

        /// <summary>
        /// Возвращает хранилище бинарных данных.
        /// </summary>
        public IBlobStorage GetBlobStorage()
        {
            return _blobStorage;
        }

        /// <summary>
        /// Возвращает версию метаданных конфигурации.
        /// </summary>
        public string GetConfigurationVersion()
        {
            return MetadataConfiguration.Version;
        }

        /// <summary>
        /// Возвращает идентификатор конфигурации.
        /// </summary>
        public string GetConfigurationIdentifier()
        {
            return MetadataConfiguration.ConfigurationId;
        }
    }
}