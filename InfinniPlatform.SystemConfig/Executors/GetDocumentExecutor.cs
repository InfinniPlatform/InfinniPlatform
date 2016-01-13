using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.Executors
{
    public class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(IConfigurationObjectBuilder configurationObjectBuilder,
                                   IMetadataComponent metadataComponent,
                                   IReferenceResolver referenceResolver,
                                   IIndexFactory indexFactory)
        {
            _configurationObjectBuilder = configurationObjectBuilder;
            _metadataComponent = metadataComponent;
            _referenceResolver = referenceResolver;
            _indexFactory = indexFactory;
        }

        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IReferenceResolver _referenceResolver;
        private readonly IIndexFactory _indexFactory;

        public dynamic GetDocument(string id)
        {
            var documentProvider = _indexFactory.BuildAllIndexesOperationProvider(); ;
            return documentProvider.GetItem(id);
        }

        public int GetNumberOfDocuments(string configurationName, string documentType, IEnumerable<CriteriaFilter> filter)
        {
            var numberOfDocuments = 0;

            var documentProvider = _configurationObjectBuilder.GetConfigurationObject(configurationName)?.GetDocumentProvider(documentType);

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

            return GetNumberOfDocuments(configurationName, documentType, filterBuilder.CriteriaList);
        }

        public IEnumerable<object> GetDocument(string configurationName, string documentType, IEnumerable<CriteriaFilter> filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, IEnumerable<CriteriaSorting> sorting = null)
        {
            var documentProvider = _configurationObjectBuilder.GetConfigurationObject(configurationName)?.GetDocumentProvider(documentType);

            if (documentProvider != null)
            {
                var metadataConfiguration = _configurationObjectBuilder.GetConfigurationObject(configurationName)
                                                                       .MetadataConfiguration;

                if (metadataConfiguration == null)
                {
                    return new List<dynamic>();
                }

                dynamic schema = metadataConfiguration.GetSchemaVersion(documentType);

                CriteriaSorting[] sortingFields = null;

                if (schema != null)
                {
                    // Извлекаем информацию о полях, по которым можно проводить сортировку из метаданных документа
                    sortingFields = SortingPropertiesExtractor.ExtractSortingProperties(string.Empty, schema.Properties, _configurationObjectBuilder);
                }

                var sortingArray = sorting.ToArray();

                if (sortingArray.Any())
                {
                    // Поля сортировки заданы в запросе. 
                    // Берем только те поля, которые разрешены в соответствии с метаданными

                    var filteredSortingFields = new List<CriteriaSorting>();

                    foreach (var sortingProperty in sortingArray)
                    {
                        if (sortingFields != null && sortingFields.Any(validProperty => validProperty.PropertyName == sortingProperty.PropertyName))
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

            return GetDocument(configurationName, documentType, filterBuilder.CriteriaList, pageNumber, pageSize, ignoreResolve, sortingBuilder.SortingList);
        }
    }
}