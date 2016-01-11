using System;
using System.Collections.Generic;

using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Index.SearchOptions;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.SearchOptions.Builders;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.Executors
{
    public class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(DocumentExecutor documentExecutor,
                                   IConfigurationObjectBuilder configurationObjectBuilder,
                                   IMetadataComponent metadataComponent,
                                   InprocessDocumentComponent documentComponent)
        {
            _documentExecutor = documentExecutor;
            _configurationObjectBuilder = configurationObjectBuilder;
            _metadataComponent = metadataComponent;
            _documentComponent = documentComponent;
        }

        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        private readonly InprocessDocumentComponent _documentComponent;
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