using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Index.SearchOptions;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.ElasticSearch.QueryLanguage;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.SystemConfig.Utils;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.SystemConfig.Executors
{
    public class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(DocumentExecutor documentExecutor,
                                   IIndexComponent indexComponent,
                                   IConfigurationMediatorComponent configurationMediatorComponent,
                                   IMetadataComponent metadataComponent,
                                   InprocessDocumentComponent documentComponent,
                                   IReferenceResolver referenceResolver)
        {
            _documentExecutor = documentExecutor;
            _indexComponent = indexComponent;
            _configurationMediatorComponent = configurationMediatorComponent;
            _metadataComponent = metadataComponent;
            _documentComponent = documentComponent;
            _referenceResolver = referenceResolver;
        }

        private readonly DocumentExecutor _documentExecutor;
        private readonly IIndexComponent _indexComponent;
        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly IMetadataComponent _metadataComponent;
        private readonly InprocessDocumentComponent _documentComponent;
        private readonly IReferenceResolver _referenceResolver;

        public IEnumerable<dynamic> GetDocumentByQuery(string queryText, bool denormalizeResult = false)
        {
            dynamic query = queryText.ToDynamic();

            if (query.Select == null)
            {
                query.Select = new List<dynamic>();
            }

            if (query.Where == null)
            {
                query.Where = new List<dynamic>();
            }

            var jsonQueryExecutor = new JsonQueryExecutor(_indexComponent.IndexFactory, FilterBuilderFactory.GetInstance());
            JArray queryResult = jsonQueryExecutor.ExecuteQuery(JObject.FromObject(query));

            var result = denormalizeResult
                             ? new JsonDenormalizer().ProcessIqlResult(queryResult).ToDynamic()
                             : queryResult.ToDynamic();

            return result;
        }

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

            var metadataConfiguration = _configurationMediatorComponent.ConfigurationBuilder.GetConfigurationObject(configurationName).MetadataConfiguration;

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
                var metadataConfiguration = _configurationMediatorComponent.ConfigurationBuilder
                                                                           .GetConfigurationObject(configurationName)
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
                    sortingFields = ExtractSortingProperties(string.Empty, schema.Properties, _configurationMediatorComponent.ConfigurationBuilder);
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