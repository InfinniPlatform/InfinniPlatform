using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.Utils;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.SystemConfig.Executors
{
    public class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(DocumentExecutor documentExecutor,
                                   IConfigurationObjectBuilder configurationObjectBuilder,
                                   IMetadataComponent metadataComponent,
                                   InprocessDocumentComponent documentComponent,
                                   IReferenceResolver referenceResolver)
        {
            _documentExecutor = documentExecutor;
            _configurationObjectBuilder = configurationObjectBuilder;
            _metadataComponent = metadataComponent;
            _documentComponent = documentComponent;
            _referenceResolver = referenceResolver;
        }

        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        private readonly InprocessDocumentComponent _documentComponent;
        private readonly IReferenceResolver _referenceResolver;
        private readonly DocumentExecutor _documentExecutor;
        private readonly IMetadataComponent _metadataComponent;

        public dynamic GetDocument(string id)
        {
            return _documentExecutor.GetBaseDocument(id);
        }

        public int GetNumberOfDocuments(string configurationName, string documentType, dynamic filter)
        {
            var numberOfDocuments = 0;

            var documentProvider = _documentComponent.GetDocumentProvider(configurationName, documentType);

            if (documentProvider == null)
            {
                return numberOfDocuments;
            }

            var metadataConfiguration = _configurationObjectBuilder.GetConfigurationObject(configurationName).MetadataConfiguration;

            if (metadataConfiguration == null)
            {
                return numberOfDocuments;
            }

            var schema = metadataConfiguration.GetSchemaVersion(documentType);

            var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, schema);

            numberOfDocuments = documentProvider.GetNumberOfDocuments(queryAnalyzer.GetBeforeResolveCriteriaList(filter));

            return numberOfDocuments;
        }

        public int GetNumberOfDocuments(string configurationName, string documentType, Action<FilterBuilder> filter)
        {
            var filterBuilder = new FilterBuilder();

            filter?.Invoke(filterBuilder);

            return GetNumberOfDocuments(configurationName, documentType, filterBuilder.GetFilter());
        }

        public IEnumerable<object> GetDocument(string configurationName, string documentType, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
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

                    foreach (var sortingProperty in sorting.ToEnumerable())
                    {
                        if (sortingFields.ToEnumerable().Any(validProperty => validProperty.PropertyName == sortingProperty.PropertyName))
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

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return GetDocument(configurationName, documentType, filter, pageNumber, pageSize, null, sorting);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve, Action<SortingBuilder> sorting = null)
        {
            var filterBuilder = new FilterBuilder();

            filter?.Invoke(filterBuilder);

            var sortingBuilder = new SortingBuilder();

            sorting?.Invoke(sortingBuilder);

            return GetDocument(configurationName, documentType, filterBuilder.GetFilter(), pageNumber, pageSize, ignoreResolve, sortingBuilder.GetSorting());
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
                            sortingProperties.Add(new
                            {
                                PropertyName = formattedPropertyName,
                                SortOrder = SortOrder.Ascending
                            }.ToDynamic());
                        }
                    }
                }
            }

            return sortingProperties.ToArray();
        }
    }
}