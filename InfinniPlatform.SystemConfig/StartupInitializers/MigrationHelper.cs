using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Отвечает за создание маппингов при старте системы.
    /// </summary>
    public static class MigrationHelper
    {
        public static string TryUpdateDocumentMappings(IMetadataConfiguration metadataConfiguration,
                                                       IConfigurationObjectBuilder configurationObjectBuilder,
                                                       IIndexFactory indexFactory,
                                                       string documentId)
        {
            var versionBuilder = indexFactory.BuildVersionBuilder(metadataConfiguration.ConfigurationId,
                metadataConfiguration.GetMetadataIndexType(documentId));

            dynamic schema = metadataConfiguration.GetSchemaVersion(documentId);

            var props = new List<PropertyMapping>();

            if (schema != null)
            {
                // convert document schema to index mapping
                props = DocumentSchemaHelper.ExtractProperties(schema.Properties, configurationObjectBuilder);
            }

            var indexTypeMapping = props.Count > 0
                ? props
                : null;

            string resultMessage = null;

            if (!versionBuilder.VersionExists(indexTypeMapping))
            {
                resultMessage = $"Created new version of {documentId} document.";

                versionBuilder.CreateVersion(false, indexTypeMapping);

                // Необходимо создать новые версии для контейнеров документов, имеющих inline ссылки на измененный документ
                resultMessage += UpdateContainersWithInlineLinks(configurationObjectBuilder, indexFactory, metadataConfiguration.ConfigurationId, documentId);
            }

            return resultMessage;
        }

        private static string UpdateContainersWithInlineLinks(IConfigurationObjectBuilder configurationObjectBuilder,
                                                              IIndexFactory indexFactory,
                                                              string configId,
                                                              string documentId)
        {
            var configList = configurationObjectBuilder.GetConfigurationList();

            foreach (var metadataConfiguration in configList)
            {
                var documents = metadataConfiguration.Documents;
                foreach (var containerId in documents)
                {
                    var versionBuilder = indexFactory.BuildVersionBuilder(metadataConfiguration.ConfigurationId,
                        metadataConfiguration.GetMetadataIndexType(containerId));

                    var schema = metadataConfiguration.GetSchemaVersion(containerId);

                    if (schema != null)
                    {
                        // Проверяем, имеется ли в схеме данных документа inline ссылка 
                        // на документ с documentId из конфигурации configId
                        if (DocumentSchemaHelper.CheckObjectForSpecifiedInline(schema, configId, documentId))
                        {
                            // convert document schema to index mapping
                            List<PropertyMapping> props = DocumentSchemaHelper.ExtractProperties(schema.Properties,
                                configurationObjectBuilder);

                            versionBuilder.CreateVersion(false, props.Count > 0
                                ? props
                                : null);

                            return string.Format("{3}Created new version of {0} document from configuration {1} due inline link on {2}.",
                                containerId,
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