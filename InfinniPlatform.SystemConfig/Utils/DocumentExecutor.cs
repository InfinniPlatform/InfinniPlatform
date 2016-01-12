using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Utils
{
    public sealed class DocumentExecutor
    {
        public DocumentExecutor(IConfigurationObjectBuilder configurationObjectBuilder,
                                InprocessDocumentComponent documentComponent,
                                IReferenceResolver referenceResolver)
        {
            _configurationObjectBuilder = configurationObjectBuilder;
            _documentComponent = documentComponent;
            _referenceResolver = referenceResolver;
        }

        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        private readonly InprocessDocumentComponent _documentComponent;
        private readonly IReferenceResolver _referenceResolver;

        public dynamic GetBaseDocument(string instanceId)
        {
            var documentProvider = _documentComponent.GetAllIndexesOperationProvider();
            return documentProvider.GetItem(instanceId);
        }

        public dynamic GetCompleteDocument(string configId, string documentId, string instanceId)
        {
            var docsToResolve = new[] { GetBaseDocument(instanceId) };
            _referenceResolver.ResolveReferences(configId, documentId, docsToResolve, null);
            return docsToResolve.FirstOrDefault();
        }

        public IEnumerable<dynamic> GetCompleteDocuments(string configurationName, string documentType,
                                                         int pageNumber, int pageSize,
                                                         dynamic filter, dynamic sorting,
                                                         IEnumerable<dynamic> ignoreResolve)
        {
            var documentProvider = _documentComponent.GetDocumentProvider(configurationName, documentType);

            if (documentProvider != null)
            {
                var metadataConfiguration = _configurationObjectBuilder.GetConfigurationObject(configurationName)
                                                                       .MetadataConfiguration;

                if (metadataConfiguration == null)
                {
                    return new List<dynamic>();
                }

                dynamic schema = metadataConfiguration.GetSchemaVersion(documentType);

                IEnumerable<dynamic> sortingFields = null;

                if (schema != null)
                {
                    // Извлекаем информацию о полях, по которым можно проводить сортировку из метаданных документа
                    sortingFields = ExtractSortingProperties(string.Empty, schema.Properties, _configurationObjectBuilder);
                }

                if (sorting != null && sorting.Count > 0)
                {
                    // Поля сортировки заданы в запросе. 
                    // Берем только те поля, которые разрешены в соответствии с метаданными

                    var filteredSortingFields = new List<object>();

                    foreach (var sortingProperty in sorting)
                    {
                        if (sortingFields.Any(validProperty => validProperty.PropertyName == sortingProperty.PropertyName))
                        {
                            filteredSortingFields.Add(sortingProperty);
                        }
                    }

                    sorting = filteredSortingFields;
                }
                else if (sortingFields != null && sortingFields.Any())
                {
                    sorting = sortingFields;
                }

                //делаем выборку документов для последующего Resolve и фильтрации по полям Resolved объектов
                var pageSizeUnresolvedDocuments = Math.Min(pageSize, 1000);

                var filterArray = filter.ToArray();

                IEnumerable<dynamic> result = documentProvider.GetDocument(filterArray, 0, Convert.ToInt32(pageSizeUnresolvedDocuments), sorting, pageNumber > 0
                                                                                                                                                      ? pageNumber * pageSize
                                                                                                                                                      : 0);

                _referenceResolver.ResolveReferences(configurationName, documentType, result, ignoreResolve);

                return result.Take(pageSize == 0
                                       ? 10
                                       : pageSize);
            }

            return new List<dynamic>();
        }

        private static IEnumerable<object> ExtractSortingProperties(string rootName, dynamic properties, IConfigurationObjectBuilder configurationObjectBuilder)
        {
            var sortingProperties = new List<object>();

            if (properties != null)
            {
                foreach (var propertyMapping in properties)
                {
                    string formattedPropertyName = string.IsNullOrEmpty(rootName)
                        ? string.Format("{0}", propertyMapping.Key)
                        : string.Format("{0}.{1}", rootName, propertyMapping.Key);

                    if (propertyMapping.Value.Type.ToString() == DataType.Object.ToString())
                    {
                        var childProps = new object[] { };

                        if (propertyMapping.Value.TypeInfo != null &&
                            propertyMapping.Value.TypeInfo.DocumentLink != null &&
                            propertyMapping.Value.TypeInfo.DocumentLink.Inline != null &&
                            propertyMapping.Value.TypeInfo.DocumentLink.Inline == true)
                        {
                            // inline ссылка на документ: необходимо получить схему документа, на который сделана ссылка,
                            // чтобы получить сортировочные поля 
                            dynamic inlineMetadataConfiguration =
                                configurationObjectBuilder.GetConfigurationObject(propertyMapping.Value.TypeInfo
                                                                                                 .DocumentLink.ConfigId)
                                                          .MetadataConfiguration;

                            if (inlineMetadataConfiguration != null)
                            {
                                dynamic inlineDocumentSchema =
                                    inlineMetadataConfiguration.GetSchemaVersion(
                                        propertyMapping.Value.TypeInfo.DocumentLink.DocumentId);

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
                            sortingProperties.Add(new CriteriaSorting(formattedPropertyName, SortOrder.Ascending.ToString()));
                        }
                    }
                }
            }

            return sortingProperties.ToArray();
        }
    }
}