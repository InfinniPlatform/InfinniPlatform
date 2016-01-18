using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.ElasticSearch.Factories;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Создает типы индексов ElasticSearch для документов прикладных конфигураций.
    /// </summary>
    internal sealed class DocumentIndexTypeInitializer : IStartupInitializer
    {
        public DocumentIndexTypeInitializer(Lazy<IIndexFactory> indexFactory, IConfigurationMetadataProvider configurationMetadataProvider, IConfigurationObjectBuilder configurationBuilder)
        {
            _configurationMetadataProvider = configurationMetadataProvider;
            _configurationBuilder = configurationBuilder;
            _indexFactory = indexFactory;
        }


        private readonly IConfigurationMetadataProvider _configurationMetadataProvider;
        private readonly IConfigurationObjectBuilder _configurationBuilder;
        private readonly Lazy<IIndexFactory> _indexFactory;


        public int Order => 1;


        public void OnStart()
        {
            Logger.Log.Info("Creating indexes started.");

            var configurationNames = _configurationMetadataProvider.GetConfigurationNames();

            foreach (var configurationName in configurationNames)
            {
                var configuration = _configurationMetadataProvider.GetConfiguration(configurationName);

                if (configuration != null)
                {
                    var documentNames = configuration.GetDocumentNames();

                    foreach (var documentName in documentNames)
                    {
                        CreateStorage(configurationName, documentName);
                    }
                }
            }

            Logger.Log.Info("Creating indexes successfully completed.");
        }


        private void CreateStorage(string configId, string documentId)
        {
            var configurationMetadata = _configurationMetadataProvider.GetConfiguration(configId);

            var message = MigrationHelper.TryUpdateDocumentMappings(configurationMetadata, _configurationBuilder, _indexFactory.Value, documentId);

            if (message != null)
            {
                Logger.Log.Info("Creating index.", new Dictionary<string, object>
                                                   {
                                                       { "configurationId", configId },
                                                       { "documentId", documentId }
                                                   });
            }
        }
    }
}