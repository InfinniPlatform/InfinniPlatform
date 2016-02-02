using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;

using Nest;

using PropertyMapping = InfinniPlatform.ElasticSearch.IndexTypeVersions.PropertyMapping;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    public static class DocumentSchemaHelper
    {
        /// <summary>
        /// Метод преобразует схему данных документа в схему, применимую к контейнеру документов
        /// </summary>
        public static IEnumerable<PropertyMapping> ExtractProperties(dynamic schemaProperties, IMetadataApi metadataApi)
        {
            var properties = new List<PropertyMapping>();

            if (schemaProperties == null)
            {
                return properties;
            }

            foreach (var propertyModel in schemaProperties)
            {
                bool sortable = propertyModel.Value.Sortable ?? false;
                string typeName = propertyModel.Value.Type.ToString();

                var simpleMappings = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase)
                                     {
                                         { "float", new PropertyMapping(propertyModel.Key, FieldType.Float, sortable) },
                                         { "integer", new PropertyMapping(propertyModel.Key, FieldType.Integer, sortable) },
                                         { "bool", new PropertyMapping(propertyModel.Key, FieldType.Boolean, sortable) },
                                         { "string", new PropertyMapping(propertyModel.Key, FieldType.String, sortable) },
                                         { "uuid", new PropertyMapping(propertyModel.Key, FieldType.String, sortable) },
                                         { "dateTime", new PropertyMapping(propertyModel.Key, FieldType.Date, sortable) },
                                         { "binary", new PropertyMapping(propertyModel.Key, FieldType.Binary, false) }
                                     };

                PropertyMapping propertyMapping;

                if (simpleMappings.TryGetValue(typeName, out propertyMapping))
                {
                    properties.Add(propertyMapping);
                }

                if (typeName == "Object")
                {
                    // Свойство типа 'объект' может являться inline ссылкой на документ
                    var documentLink = (propertyModel.Value.TypeInfo != null) ? propertyModel.Value.TypeInfo.DocumentLink : null;

                    if (documentLink != null && documentLink.Inline != null && documentLink.Inline == true)
                    {
                        // inline ссылка на документ:
                        // необходимо получить схему документа, на который сделана ссылка, чтобы получить сортировочные поля 
                        string documentName = documentLink.DocumentId.ToString();
                        dynamic inlineDocumentSchema = metadataApi.GetDocumentSchema(documentName);

                        if (inlineDocumentSchema != null)
                        {
                            properties.Add(new PropertyMapping(propertyModel.Key, ExtractProperties(inlineDocumentSchema.Properties, metadataApi)));
                        }
                    }
                    else
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, ExtractProperties(propertyModel.Value.Properties, metadataApi)));
                    }
                }
                else
                {
                    if (typeName == "Array")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, ExtractProperties(propertyModel.Value.Items.Properties, metadataApi)));
                    }
                }
            }

            return properties;
        }

        /// <summary>
        /// Проверяет наличие inline ссылки на определенный документ в схеме
        /// </summary>
        public static bool CheckObjectForSpecifiedInline(dynamic schema, string documentId)
        {
            if (schema.Properties is DynamicWrapper)
            {
                foreach (var propertyModel in schema.Properties)
                {
                    dynamic linkInfoToCheck = null;

                    if (propertyModel.Value.Type.ToString() == "Object")
                    {
                        linkInfoToCheck = propertyModel.Value;
                    }
                    else if (propertyModel.Value.Type.ToString() == "Array")
                    {
                        linkInfoToCheck = propertyModel.Value.Items;
                    }

                    var documentLink = (linkInfoToCheck != null && linkInfoToCheck.TypeInfo != null) ? linkInfoToCheck.TypeInfo.DocumentLink : null;

                    if (documentLink != null)
                    {
                        if (documentLink.Inline == true && documentLink.DocumentId == documentId)
                        {
                            return true;
                        }

                        if (CheckObjectForSpecifiedInline(linkInfoToCheck, documentId))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}