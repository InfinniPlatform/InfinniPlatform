using System;
using System.Collections.Generic;

using InfinniPlatform.Hosting;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.SystemConfig.Initializers
{
    /// <summary>
    /// Созадает типы индексов для документов.
    /// </summary>
    public sealed class DocumentIndexTypeInitializer : IStartupInitializer
    {
        public DocumentIndexTypeInitializer(IMetadataConfigurationProvider metadataProvider,
                                            IConfigurationObjectBuilder configurationBuilder)
        {
            _metadataProvider = metadataProvider;
            _configurationBuilder = configurationBuilder;
            _indexFactory = new Lazy<IIndexFactory>(() => new ElasticFactory());
        }

        private readonly IConfigurationObjectBuilder _configurationBuilder;
        private readonly Lazy<IIndexFactory> _indexFactory;
        private readonly IMetadataConfigurationProvider _metadataProvider;

        public void OnStart(HostingContextBuilder contextBuilder)
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
            var message = MigrationHelper.TryUpdateDocumentMappings(_metadataProvider.GetMetadataConfiguration(null, configId), _configurationBuilder, _indexFactory.Value, documentId);

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