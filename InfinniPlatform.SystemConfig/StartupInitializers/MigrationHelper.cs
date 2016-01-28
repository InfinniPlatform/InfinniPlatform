using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.ElasticSearch.Factories;
using InfinniPlatform.ElasticSearch.IndexTypeVersions;
using InfinniPlatform.ElasticSearch.Versioning;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Отвечает за создание маппингов при старте системы.
    /// </summary>
    public static class MigrationHelper
    {
        public static string TryUpdateDocumentMappings(IMetadataApi metadataApi,
                                                       IIndexFactory indexFactory,
                                                       string configurationName,
                                                       string documentName)
        {
            IVersionBuilder versionBuilder = indexFactory.BuildVersionBuilder(configurationName, documentName);

            dynamic schema = metadataApi.GetDocumentSchema(configurationName, documentName);

            var props = new List<PropertyMapping>();

            if (schema != null)
            {
                // convert document schema to index mapping
                props = DocumentSchemaHelper.ExtractProperties(schema.Properties, metadataApi);
            }

            var indexTypeMapping = props.Count > 0
                ? props
                : null;

            string resultMessage = null;

            if (!versionBuilder.VersionExists(indexTypeMapping))
            {
                resultMessage = $"Created new version of {documentName} document.";

                versionBuilder.CreateVersion(false, indexTypeMapping);

                // Необходимо создать новые версии для контейнеров документов, имеющих inline ссылки на измененный документ
                resultMessage += UpdateContainersWithInlineLinks(metadataApi, indexFactory, configurationName, documentName);
            }

            return resultMessage;
        }

        private static string UpdateContainersWithInlineLinks(IMetadataApi metadataApi,
                                                              IIndexFactory indexFactory,
                                                              string configId,
                                                              string documentId)
        {
            //MetadataApi
            var configurationNames = metadataApi.GetConfigurationNames();

            foreach (var configurationName in configurationNames)
            {
                var documentNames = metadataApi.GetDocumentNames(configurationName);

                foreach (var documentName in documentNames)
                {
                    var versionBuilder = indexFactory.BuildVersionBuilder(configurationName, documentName);

                    var schema = metadataApi.GetDocumentSchema(configurationName, documentName);

                    if (schema != null)
                    {
                        // Проверяем, имеется ли в схеме данных документа inline ссылка 
                        // на документ с documentId из конфигурации configId
                        if (DocumentSchemaHelper.CheckObjectForSpecifiedInline(schema, configId, documentId))
                        {
                            // convert document schema to index mapping
                            List<PropertyMapping> props = DocumentSchemaHelper.ExtractProperties(schema.Properties, metadataApi);

                            versionBuilder.CreateVersion(false, props.Count > 0
                                ? props
                                : null);

                            return string.Format("{3}Created new version of {0} document from configuration {1} due inline link on {2}.",
                                documentName,
                                configId,
                                documentId,
                                Environment.NewLine);
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}