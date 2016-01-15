using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    public static class DocumentSchemaHelper
    {
        /// <summary>
        /// Метод преобразует схему данных документа в схему, применимую к контейнеру документов
        /// </summary>
        public static IEnumerable<PropertyMapping> ExtractProperties(dynamic schemaProperties, IConfigurationObjectBuilder configurationObjectBuilder)
        {
            var properties = new List<PropertyMapping>();

            if (schemaProperties == null)
            {
                return properties;
            }

            foreach (var propertyModel in schemaProperties)
            {
                var sortable = false;

                if (propertyModel.Value.Sortable != null)
                {
                    sortable = propertyModel.Value.Sortable;
                }

                string typeName = propertyModel.Value.Type.ToString();

                var simpleMappings = new Dictionary<string, PropertyMapping>
                                       {
                                           { "Float", new PropertyMapping(propertyModel.Key, PropertyDataType.Float, sortable) },
                                           { "Integer", new PropertyMapping(propertyModel.Key, PropertyDataType.Integer, sortable) },
                                           { "Bool", new PropertyMapping(propertyModel.Key, PropertyDataType.Boolean, sortable) },
                                           { "String", new PropertyMapping(propertyModel.Key, PropertyDataType.String, sortable) },
                                           { "Uuid", new PropertyMapping(propertyModel.Key, PropertyDataType.String, sortable) },
                                           { "DateTime", new PropertyMapping(propertyModel.Key, PropertyDataType.Date, sortable) },
                                           { "Binary", new PropertyMapping(propertyModel.Key, PropertyDataType.Binary, false) }
                                       };

                PropertyMapping propertyMapping;

                if (simpleMappings.TryGetValue(typeName, out propertyMapping))
                {
                    properties.Add(propertyMapping);
                }

                if (typeName == "Object")
                {
                    // Свойство типа 'объект' может являться inline ссылкой на документ
                    if (propertyModel.Value.TypeInfo != null &&
                        propertyModel.Value.TypeInfo.DocumentLink != null &&
                        propertyModel.Value.TypeInfo.DocumentLink.Inline != null &&
                        propertyModel.Value.TypeInfo.DocumentLink.Inline == true)
                    {
                        // inline ссылка на документ: необходимо получить схему документа, на который сделана ссылка,
                        // чтобы получить сортировочные поля 
                        string metadataIdentifier = propertyModel.Value.TypeInfo.DocumentLink.ConfigId.ToString();
                        var builder = configurationObjectBuilder.GetConfigurationObject(metadataIdentifier);
                        //.Configurations.FirstOrDefault(
                        //c => c.ConfigurationId == propertyModel.Value.TypeInfo.DocumentLink.ConfigId);
                        var inlineMetadataConfiguration = builder.MetadataConfiguration;

                        if (inlineMetadataConfiguration != null)
                        {
                            string documentId = propertyModel.Value.TypeInfo.DocumentLink.DocumentId.ToString();
                            dynamic inlineDocumentSchema = inlineMetadataConfiguration.GetSchemaVersion(documentId);

                            if (inlineDocumentSchema != null)
                            {
                                properties.Add(new PropertyMapping(propertyModel.Key, ExtractProperties(inlineDocumentSchema.Properties, configurationObjectBuilder)));
                            }
                        }
                    }
                    else
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, ExtractProperties(propertyModel.Value.Properties, configurationObjectBuilder)));
                    }
                }
                else
                {
                    if (typeName == "Array")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, ExtractProperties(propertyModel.Value.Items.Properties, configurationObjectBuilder)));
                    }
                }
            }

            return properties;
        }

        /// <summary>
        /// Проверяет наличие inline ссылки на определенный документ в схеме
        /// </summary>
        public static bool CheckObjectForSpecifiedInline(dynamic schema, string configId, string documentId)
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

                    if (linkInfoToCheck != null &&
                        linkInfoToCheck.TypeInfo != null &&
                        linkInfoToCheck.TypeInfo.DocumentLink != null &&
                        linkInfoToCheck.TypeInfo.DocumentLink.Inline != null)
                    {
                        if (linkInfoToCheck.TypeInfo.DocumentLink.Inline == true &&
                            linkInfoToCheck.TypeInfo.DocumentLink.ConfigId == configId &&
                            linkInfoToCheck.TypeInfo.DocumentLink.DocumentId == documentId)
                        {
                            return true;
                        }

                        if (CheckObjectForSpecifiedInline(linkInfoToCheck, configId, documentId))
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