using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.SystemConfig.Initializers
{
    /// <summary>
    /// Создает типы индексов ElasticSearch для документов прикладных конфигураций.
    /// </summary>
    internal sealed class DocumentIndexTypeInitializer : IStartupInitializer
    {
        public DocumentIndexTypeInitializer(Lazy<IIndexFactory> indexFactory, IMetadataConfigurationProvider metadataProvider, IConfigurationObjectBuilder configurationBuilder)
        {
            _metadataProvider = metadataProvider;
            _configurationBuilder = configurationBuilder;
            _indexFactory = indexFactory;
        }


        private readonly IMetadataConfigurationProvider _metadataProvider;
        private readonly IConfigurationObjectBuilder _configurationBuilder;
        private readonly Lazy<IIndexFactory> _indexFactory;


        public int Order => 1;


        public void OnStart()
        {
            Logger.Log.Info("Creating indexes started.");

            var configurations = _metadataProvider.Configurations;

            if (configurations != null)
            {
                foreach (var configuration in configurations)
                {
                    var documents = configuration.Documents;

                    if (documents != null)
                    {
                        var configId = configuration.ConfigurationId;
                        var isSystemConfig = configuration.IsEmbeddedConfiguration;

                        foreach (var documentId in documents)
                        {
                            if (isSystemConfig)
                            {
                                var documentIndexType = configuration.GetMetadataIndexType(documentId);

                                CreateSystemStore(configId, documentId, documentIndexType);
                            }
                            else
                            {
                                CreateStorage(configId, documentId);
                            }
                        }
                    }
                }
            }

            Logger.Log.Info("Creating indexes successfully completed.");
        }


        private void CreateStorage(string configId, string documentId)
        {
            var message = MigrationHelper.TryUpdateDocumentMappings(_metadataProvider.GetMetadataConfiguration(configId), _configurationBuilder, _indexFactory.Value, documentId);

            if (message != null)
            {
                Logger.Log.Info("Creating index.", new Dictionary<string, object>
                                                   {
                                                       { "configurationId", configId },
                                                       { "documentId", documentId }
                                                   });
            }
        }

        private void CreateSystemStore(string configId, string documentId, string documentIndexType)
        {
            var versionBuilder = _indexFactory.Value.BuildVersionBuilder(configId, documentIndexType);

            // Для системных конфигураций использована упрощенная схема
            // создания хранилищ - без учета схем данных документов.
            // На момент написания кода ни для одного документа системной
            // конфигурации мапинг задан не был. Если в дальнейшем
            // по каким-то причинам будет необходимо учитывать схему
            // данных системных документов, необходимо будет использовать механизм
            // UpdateStoreMigration

            if (!versionBuilder.VersionExists())
            {
                Logger.Log.Info("Creating index.", new Dictionary<string, object>
                                                   {
                                                       { "configurationId", configId },
                                                       { "documentId", documentId }
                                                   });

                versionBuilder.CreateVersion();
            }
        }
    }
}