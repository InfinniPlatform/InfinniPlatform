using System;
using System.Collections.Concurrent;

using InfinniPlatform.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Sdk.Environment.Binary;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.SystemConfig.Metadata.MetadataConfiguration
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


        public ConfigurationObject(IMetadataConfiguration metadataConfiguration, IIndexFactory indexFactory, IBlobStorageFactory blobStorageFactory)
        {
            MetadataConfiguration = metadataConfiguration;
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
            _elasticConnection = new ElasticConnection();
        }

        private readonly IIndexFactory _indexFactory;
        private readonly IBlobStorageFactory _blobStorageFactory;
        private readonly ElasticConnection _elasticConnection;


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
                // Проверка наличия индекса занимает очень много времени. На данный момент эту логику можно осуществлять
                // при старте системы. Не понятно, почему этого нельзя было сделать раньше или, во всяком случае, в другом
                // месте. Как выяснилось, этот код крайне отрицательно сказывается на производительности системы, поэтому
                // тут предпринята попытка простейшего кэширования результатов его работы.

                if (_elasticConnection.GetIndexStatus(documentIndexName, documentTypeName) == IndexStatus.NotExists)
                {
                    _elasticConnection.CreateType(documentIndexName, documentTypeName);
                }

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
            return _blobStorageFactory.CreateBlobStorage();
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