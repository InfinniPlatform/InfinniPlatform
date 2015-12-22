using System;
using System.Collections.Generic;

using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.QueryLanguage.Implementation;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.RestfulApi.Executors
{
    public class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(DocumentExecutor documentExecutor,
                                   IIndexComponent indexComponent,
                                   IConfigurationMediatorComponent configurationMediatorComponent,
                                   IMetadataComponent metadataComponent,
                                   InprocessDocumentComponent documentComponent)
        {
            _documentExecutor = documentExecutor;
            _indexComponent = indexComponent;
            _configurationMediatorComponent = configurationMediatorComponent;
            _metadataComponent = metadataComponent;
            _documentComponent = documentComponent;
        }

        private readonly DocumentExecutor _documentExecutor;
        private readonly IIndexComponent _indexComponent;
        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly IMetadataComponent _metadataComponent;
        private readonly InprocessDocumentComponent _documentComponent;

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
            dynamic returnResult = new DynamicWrapper();
            returnResult.NumberOfDocuments = 0;
            returnResult.IsValid = true;

            var documentProvider = _documentComponent.GetDocumentProvider(configurationName, documentType);

            if (documentProvider == null)
            {
                return returnResult;
            }

            var metadataConfiguration = _configurationMediatorComponent.ConfigurationBuilder.GetConfigurationObject(configurationName).MetadataConfiguration;

            if (metadataConfiguration == null)
            {
                return returnResult;
            }

            var schema = metadataConfiguration.GetSchemaVersion(documentType);

            var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, schema);

            returnResult.NumberOfDocuments = documentProvider.GetNumberOfDocuments(queryAnalyzer.GetBeforeResolveCriteriaList(filter));

            return returnResult;
        }

        public int GetNumberOfDocuments(string configurationName, string documentType, Action<FilterBuilder> filter)
        {
            var filterBuilder = new FilterBuilder();

            filter?.Invoke(filterBuilder);

            return GetNumberOfDocuments(configurationName, documentType, filterBuilder.GetFilter());
        }

        public IEnumerable<object> GetDocument(string configurationName, string documentType, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
        {
            var result = _documentExecutor.GetCompleteDocuments(configurationName,
                                                                documentType,
                                                                pageNumber,
                                                                pageSize,
                                                                filter,
                                                                sorting,
                                                                ignoreResolve);

            return result;
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
    }
}