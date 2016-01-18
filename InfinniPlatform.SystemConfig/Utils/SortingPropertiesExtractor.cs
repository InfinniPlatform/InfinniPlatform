using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.SystemConfig.Utils
{
    public static class SortingPropertiesExtractor
    {
        public static IEnumerable<SortingCriteria> ExtractSortingProperties(string rootName, dynamic properties, IConfigurationObjectBuilder configurationObjectBuilder)
        {
            var sortingProperties = new List<SortingCriteria>();

            if (properties != null)
            {
                foreach (var propertyMapping in properties)
                {
                    string formattedPropertyName = string.IsNullOrEmpty(rootName)
                                                       ? string.Format("{0}", propertyMapping.Key)
                                                       : string.Format("{0}.{1}", rootName, propertyMapping.Key);

                    if (propertyMapping.Value.Type.ToString() == DataType.Object.ToString())
                    {
                        var childProps = new SortingCriteria[] { };

                        var documentLink = propertyMapping.Value.TypeInfo?.DocumentLink;

                        if (documentLink?.Inline == true)
                        {
                            string configId = documentLink.ConfigId;
                            string documentType = documentLink.DocumentId;

                            // inline ссылка на документ: необходимо получить схему документа, на который сделана ссылка, чтобы получить сортировочные поля 
                            IConfigurationObject configurationObject = configurationObjectBuilder.GetConfigurationObject(configId);
                            IConfigurationMetadata inlineMetadataConfiguration =configurationObject.ConfigurationMetadata;

                            if (inlineMetadataConfiguration != null)
                            {
                                dynamic inlineDocumentSchema = inlineMetadataConfiguration.GetDocumentSchema(documentType);

                                if (inlineDocumentSchema != null)
                                {
                                    childProps = ExtractSortingProperties(formattedPropertyName,
                                                                          inlineDocumentSchema.Properties,
                                                                          configurationObjectBuilder);
                                }
                            }
                        }
                        else
                        {
                            childProps = ExtractSortingProperties(formattedPropertyName,
                                                                  propertyMapping.Value.Properties,
                                                                  configurationObjectBuilder);
                        }

                        sortingProperties.AddRange(childProps);
                    }
                    else if (propertyMapping.Value.Type.ToString() == DataType.Array.ToString())
                    {
                        if (propertyMapping.Value.Items != null)
                        {
                            sortingProperties.AddRange(
                                                       ExtractSortingProperties(formattedPropertyName,
                                                                                propertyMapping.Value.Items.Properties,
                                                                                configurationObjectBuilder));
                        }
                    }
                    else
                    {
                        var isSortingField = false;

                        if (propertyMapping.Value.Sortable != null)
                        {
                            isSortingField = propertyMapping.Value.Sortable;
                        }

                        if (isSortingField)
                        {
                            sortingProperties.Add(new SortingCriteria(formattedPropertyName, SortOrder.Ascending.ToString()));
                        }
                    }
                }
            }

            return sortingProperties.ToArray();
        }
    }
}