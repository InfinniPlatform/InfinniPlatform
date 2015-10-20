using System;
using System.Collections.Concurrent;

using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.Environment.Binary;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
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
            _metadataConfiguration = metadataConfiguration;
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
            _indexStateProvider = _indexFactory.BuildIndexStateProvider();
        }


        private readonly IMetadataConfiguration _metadataConfiguration;
        private readonly IIndexFactory _indexFactory;
        private readonly IBlobStorageFactory _blobStorageFactory;
        private readonly IIndexStateProvider _indexStateProvider;


        public IMetadataConfiguration MetadataConfiguration
        {
            get { return _metadataConfiguration; }
        }

        /// <summary>
        /// Возвращает провайдер версий документов.
        /// </summary>
        /// <param name="documentId">Имя документа.</param>
        /// <param name="version">Версия документа.</param>
        /// <param name="tenantId">Идентификатор организации.</param>
        /// <remarks>
        /// Создает провайдер, возвращающий версии документов всех существующих в индексе типов.
        /// </remarks>
        public IVersionProvider GetDocumentProvider(string documentId, string version, string tenantId)
        {
            IVersionProvider versionProvider;

            var documentIndexName = MetadataConfiguration.ConfigurationId;
            var documentTypeName = MetadataConfiguration.GetMetadataIndexType(documentId);

            var versionProviderKey = string.Format("{0},{1}", documentIndexName, documentTypeName);

            if (!VersionProviderCache.TryGetValue(versionProviderKey, out versionProvider))
            {
                // Проверка наличия индекса занимает очень много времени. На данный момент эту логику можно осуществлять
                // при старте системы. Не понятно, почему этого нельзя было сделать раньше или, во всяком случае, в другом
                // месте. Как выяснилось, этот код крайне отрицательно сказывается на производительности системы, поэтому
                // тут предпринята попытка простейшего кэширования результатов его работы.

                if (_indexStateProvider.GetIndexStatus(documentIndexName, documentTypeName) == IndexStatus.NotExists)
                {
                    _indexStateProvider.CreateIndexType(documentIndexName, documentTypeName);
                }

                versionProvider = _indexFactory.BuildVersionProvider(documentIndexName, documentTypeName, tenantId, version);

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
                MetadataConfiguration.GetMetadataIndexType(documentId),
                MetadataConfiguration.GetSearchAbilityType(documentId));
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