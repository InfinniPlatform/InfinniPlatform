using System;
using System.Collections.Concurrent;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.ElasticSearch.Factories;

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

        public ConfigurationObject(IConfigurationMetadata configurationMetadata, IIndexFactory indexFactory)
        {
            ConfigurationMetadata = configurationMetadata;

            _indexFactory = indexFactory;
        }


        private readonly IIndexFactory _indexFactory;


        public IConfigurationMetadata ConfigurationMetadata { get; }


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

            var documentIndexName = ConfigurationMetadata.Configuration;

            var versionProviderKey = $"{documentIndexName},{documentId}";

            if (!VersionProviderCache.TryGetValue(versionProviderKey, out versionProvider))
            {
                versionProvider = _indexFactory.BuildVersionProvider(documentIndexName, documentId);

                VersionProviderCache[versionProviderKey] = versionProvider;
            }

            return versionProvider;
        }
    }
}