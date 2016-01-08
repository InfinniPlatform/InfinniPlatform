using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Sdk.Environment.Index
{
    public static class DocumentSchemaHelper
    {
        /// <summary>
        ///     Метод преобразует схему данных документа в схему, применимую к контейнеру документов
        /// </summary>
        public static IEnumerable<PropertyMapping> ExtractProperties(dynamic schemaProperties, IConfigurationObjectBuilder configurationObjectBuilder)
        {
            var properties = new List<PropertyMapping>();

            if (schemaProperties != null)
            {
                foreach (dynamic propertyModel in schemaProperties)
                {
                    bool sortable = false;

                    if (propertyModel.Value.Sortable != null)
                    {
                        sortable = propertyModel.Value.Sortable;
                    }

                    if (propertyModel.Value.Type.ToString() == "Float")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, PropertyDataType.Float, sortable));
                    }
                    else if (propertyModel.Value.Type.ToString() == "Integer")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, PropertyDataType.Integer, sortable));
                    }
                    else if (propertyModel.Value.Type.ToString() == "Bool")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, PropertyDataType.Boolean, sortable));
                    }
                    else if (propertyModel.Value.Type.ToString() == "String" ||
                             propertyModel.Value.Type.ToString() == "Uuid")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, PropertyDataType.String, sortable));
                    }
                    else if (propertyModel.Value.Type.ToString() == "DateTime")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, PropertyDataType.Date, sortable));
                    }
                    else if (propertyModel.Value.Type.ToString() == "Binary")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key, PropertyDataType.Binary, false));
                    }
                    else if (propertyModel.Value.Type.ToString() == "Object")
                    {
                        // Свойство типа 'объект' может являться inline ссылкой на документ
                        if (propertyModel.Value.TypeInfo != null &&
                            propertyModel.Value.TypeInfo.DocumentLink != null &&
                            propertyModel.Value.TypeInfo.DocumentLink.Inline != null &&
                            propertyModel.Value.TypeInfo.DocumentLink.Inline == true)
                        {
                            // inline ссылка на документ: необходимо получить схему документа, на который сделана ссылка,
                            // чтобы получить сортировочные поля 
                            dynamic builder =
                                configurationObjectBuilder.GetConfigurationObject(propertyModel.Value.TypeInfo
                                                                                               .DocumentLink.ConfigId);
                            //.Configurations.FirstOrDefault(
                            //c => c.ConfigurationId == propertyModel.Value.TypeInfo.DocumentLink.ConfigId);
                            dynamic inlineMetadataConfiguration = builder.MetadataConfiguration;

                            if (inlineMetadataConfiguration != null)
                            {
                                dynamic inlineDocumentSchema =
                                    inlineMetadataConfiguration.GetSchemaVersion(
                                        propertyModel.Value.TypeInfo.DocumentLink.DocumentId);

                                if (inlineDocumentSchema != null)
                                {
                                    properties.Add(new PropertyMapping(propertyModel.Key,
                                                                       ExtractProperties(inlineDocumentSchema.Properties,
                                                                                         configurationObjectBuilder)));
                                }
                            }
                        }
                        else
                        {
                            properties.Add(new PropertyMapping(propertyModel.Key,
                                                               ExtractProperties(propertyModel.Value.Properties,
                                                                                 configurationObjectBuilder)));
                        }
                    }
                    else if (propertyModel.Value.Type.ToString() == "Array")
                    {
                        properties.Add(new PropertyMapping(propertyModel.Key,
                                                           ExtractProperties(propertyModel.Value.Items.Properties,
                                                                             configurationObjectBuilder)));
                    }
                }
            }

            return properties;
        }

        /// <summary>
        ///     Проверяет наличие inline ссылки на определенный документ в схеме
        /// </summary>
        public static bool CheckObjectForSpecifiedInline(dynamic schema, string configId, string documentId)
        {
            if (schema.Properties is DynamicWrapper)
            {
                foreach (dynamic propertyModel in schema.Properties)
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