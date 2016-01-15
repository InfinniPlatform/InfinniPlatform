using System;
using System.Collections.Concurrent;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.SystemConfig.Metadata
{
    /// <summary>
    /// Прикладной объект конфигурации предметной области.
    /// </summary>
    public sealed class ConfigurationObject : IConfigurationObject
    {
        private static readonly ConcurrentDictionary<string, IVersionProvider> VersionProviderCache;

        static ConfigurationObject()
        {
            VersionProviderCache = new ConcurrentDictionary<string, IVersionProvider>(StringComparer.OrdinalIgnoreCase);
        }

        public ConfigurationObject(IMetadataConfiguration metadataConfiguration, IIndexFactory indexFactory)
        {
            MetadataConfiguration = metadataConfiguration;

            _indexFactory = indexFactory;
        }


        private readonly IIndexFactory _indexFactory;


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
    }
}