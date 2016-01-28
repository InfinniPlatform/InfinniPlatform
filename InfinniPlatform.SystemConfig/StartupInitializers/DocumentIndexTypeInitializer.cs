using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.ElasticSearch.Factories;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Создает типы индексов ElasticSearch для документов прикладных конфигураций.
    /// </summary>
    internal sealed class DocumentIndexTypeInitializer : IStartupInitializer
    {
        public DocumentIndexTypeInitializer(Lazy<IIndexFactory> indexFactory, IMetadataApi metadataApi, ILog log)
        {
            _indexFactory = indexFactory;
            _metadataApi = metadataApi;
            _log = log;
        }

        private readonly Lazy<IIndexFactory> _indexFactory;
        private readonly IMetadataApi _metadataApi;
        private readonly ILog _log;

        public int Order => 1;

        public void OnStart()
        {
            _log.Info("Creating indexes started.");

            var configurationNames = _metadataApi.GetConfigurationNames();

            foreach (var configurationName in configurationNames)
            {
                var documentNames = _metadataApi.GetDocumentNames(configurationName);

                foreach (var documentName in documentNames)
                {
                    CreateStorage(configurationName, documentName);
                }
            }

            _log.Info("Creating indexes successfully completed.");
        }

        private void CreateStorage(string configId, string documentId)
        {
            var message = MigrationHelper.TryUpdateDocumentMappings(_metadataApi, _indexFactory.Value, configId, documentId);

            if (message != null)
            {
                _log.Info("Creating index.", new Dictionary<string, object>
                                             {
                                                 { "configurationId", configId },
                                                 { "documentId", documentId }
                                             });
            }
        }
    }
}