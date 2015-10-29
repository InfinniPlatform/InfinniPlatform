using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Hosting;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.SystemConfig.Initializers
{
    /// <summary>
    /// Созадает типы индексов для документов.
    /// </summary>
    public sealed class DocumentIndexTypeInitializer : IStartupInitializer
    {
        public DocumentIndexTypeInitializer(IMetadataConfigurationProvider metadataProvider)
        {
            _metadataProvider = metadataProvider;
            _indexFactory = new Lazy<IIndexFactory>(() => new ElasticFactory());
        }


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
                    var documents = configuration.Containers;

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


        private static void CreateStorage(string configId, string documentId)
        {
            var indexApi = new IndexApi();

            if (!indexApi.IndexExists(configId, documentId))
            {
                Logger.Log.Info("Creating index.", new Dictionary<string, object>
                                                   {
                                                       { "configurationId", configId },
                                                       { "documentId", documentId },
                                                   });

                indexApi.RebuildIndex(configId, documentId);
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
                                                       { "documentId", documentId },
                                                   });

                versionBuilder.CreateVersion();
            }
        }
    }
}