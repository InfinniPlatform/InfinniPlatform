using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.Sdk.Logging;

using Nest;

using PropertyMapping = InfinniPlatform.ElasticSearch.IndexTypeVersions.PropertyMapping;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Отвечает за создание маппингов при старте системы.
    /// </summary>
    public class ElasticTypesMigrationHelper
    {
        public ElasticTypesMigrationHelper(IMetadataApi metadataApi,
                                           ElasticTypeManager elasticTypeManager,
                                           ILog log)
        {
            _metadataApi = metadataApi;
            _elasticTypeManager = elasticTypeManager;
            _log = log;
        }

        private readonly ElasticTypeManager _elasticTypeManager;
        private readonly ILog _log;
        private readonly IMetadataApi _metadataApi;

        public void CreateOrUpdateStorage(IEnumerable<string> documentTypes)
        {
            _log.Info("Creating indexes started.");

            foreach (var documentName in documentTypes)
            {
                dynamic schema = _metadataApi.GetDocumentSchema(documentName);

                var props = new List<PropertyMapping>();

                if (schema != null)
                {
                    // convert document schema to index mapping
                    props = DocumentSchemaHelper.ExtractProperties(schema.Properties, _metadataApi);
                }

                var indexTypeMapping = props.Count > 0
                                           ? props
                                           : null;

                if (!VersionExists(documentName, indexTypeMapping))
                {
                    _elasticTypeManager.CreateType(documentName, indexTypeMapping);
                    UpdateContainersWithInlineLinks(documentName);
                }

                _log.Info("Created type.", new Dictionary<string, object>
                                           {
                                               { "documentType", documentName }
                                           });
            }

            _log.Info("Creating indexes successfully completed.");
        }

        private void UpdateContainersWithInlineLinks(string documentId)
        {
            var documentNames = _metadataApi.GetDocumentNames();

            foreach (var documentName in documentNames)
            {
                var schema = _metadataApi.GetDocumentSchema(documentName);

                if (schema != null)
                {
                    // Проверяем, имеется ли в схеме данных документа inline ссылка на документ с documentId
                    if (DocumentSchemaHelper.CheckObjectForSpecifiedInline(schema, documentId))
                    {
                        // convert document schema to index mapping
                        List<PropertyMapping> props = DocumentSchemaHelper.ExtractProperties(schema.Properties, _metadataApi);
                        _elasticTypeManager.CreateType(documentName, props.Count > 0
                                                                         ? props
                                                                         : null);

                        _log.Info("Created inlined type.", new Dictionary<string, object>
                                                           {
                                                               { "documentType", documentName }
                                                           });
                    }
                }
            }
        }

        /// <summary>
        /// Проверяет, что версия индекса метаданных существует.
        /// Если в параметрах указан маппинг, то дополнительно будет проверено соответствие имеющегося маппинга с новым.
        /// Считается, что версия существует, если все свойства переданного маппинга соответствуют по типу всем имеющимся
        /// свойствам,
        /// в противном случае нужно создавать новую версию.
        /// </summary>
        private bool VersionExists(string typeName, IList<PropertyMapping> properties = null)
        {
            var isTypeExists = _elasticTypeManager.TypeExists(typeName);

            var isPropertiesMatch = true;

            if (properties != null && isTypeExists)
            {
                var currentProperties = _elasticTypeManager.GetPropertyMappings(typeName);

                foreach (var newMappingProperty in properties)
                {
                    var propertyToCheck = currentProperties.FirstOrDefault(p => p.Name == newMappingProperty.Name);
                    if (propertyToCheck != null)
                    {
                        if (!CheckPropertiesEquality(propertyToCheck, newMappingProperty))
                        {
                            isPropertiesMatch = false;
                            break;
                        }
                    }
                    else
                    {
                        // Это значит, что было добавлено новое свойство
                        isPropertiesMatch = false;
                        break;
                    }
                }
            }

            return isTypeExists && isPropertiesMatch;
        }

        /// <summary>
        /// Проверяет соответствие маппинга свойства
        /// </summary>
        private static bool CheckPropertiesEquality(PropertyMapping propertyToCheck, PropertyMapping newMappingProperty)
        {
            // Первая проверка на соответствие типа данных
            if (!(newMappingProperty.DataType == FieldType.Binary
                      ? propertyToCheck.DataType == FieldType.Object
                      : propertyToCheck.DataType == newMappingProperty.DataType))
            {
                return false;
            }

            // Проверяем возможность сортировки по полю
            if (propertyToCheck.AddSortField != newMappingProperty.AddSortField)
            {
                return false;
            }

            if (propertyToCheck.DataType == FieldType.Object)
            {
                foreach (var childProperty in newMappingProperty.ChildProperties)
                {
                    var childPropertyToCheck =
                        propertyToCheck.ChildProperties.FirstOrDefault(p => p.Name == childProperty.Name);

                    if (childPropertyToCheck != null)
                    {
                        if (!CheckPropertiesEquality(childPropertyToCheck, childProperty))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}