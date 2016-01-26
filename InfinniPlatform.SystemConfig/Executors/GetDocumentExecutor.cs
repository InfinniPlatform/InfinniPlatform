using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.Factories;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.Executors
{
    internal sealed class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(IDocumentTransactionScopeProvider transactionScopeProvider,
                                   IConfigurationObjectBuilder configurationObjectBuilder,
                                   IMetadataApi metadataComponent,
                                   IReferenceResolver referenceResolver,
                                   IIndexFactory indexFactory)
        {
            _transactionScopeProvider = transactionScopeProvider;
            _configurationObjectBuilder = configurationObjectBuilder;
            _metadataComponent = metadataComponent;
            _referenceResolver = referenceResolver;
            _indexFactory = indexFactory;
        }


        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;
        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        private readonly IMetadataApi _metadataComponent;
        private readonly IReferenceResolver _referenceResolver;
        private readonly IIndexFactory _indexFactory;


        public dynamic GetDocumentById(string configuration, string documentType, string documentId)
        {
            var documentProvider = _indexFactory.BuildAllIndexesOperationProvider();

            var document = documentProvider.GetItem(documentId);

            if (document != null)
            {
                document = GetActualDocuments(configuration, documentType, new[] { document }).FirstOrDefault();
            }

            return document;
        }


        public int GetNumberOfDocuments(string configuration,
                                        string documentType,
                                        Action<FilterBuilder> filter)
        {
            return GetNumberOfDocuments(configuration, documentType, filter.ToFilterCriterias());
        }

        public int GetNumberOfDocuments(string configuration,
                                        string documentType,
                                        IEnumerable<FilterCriteria> filter)
        {
            var configurationObject = _configurationObjectBuilder.GetConfigurationObject(configuration);

            if (configurationObject == null)
            {
                throw new ArgumentException(configuration, nameof(configuration));
            }

            var documentProvider = configurationObject.GetDocumentProvider(documentType);

            if (documentProvider == null)
            {
                throw new ArgumentException(documentType, nameof(documentType));
            }

            var schema = _metadataComponent.GetDocumentSchema(configuration, documentType);

            var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, schema);

            var numberOfDocuments = documentProvider.GetNumberOfDocuments(queryAnalyzer.GetBeforeResolveCriteriaList(filter));

            return numberOfDocuments;
        }


        public IEnumerable<dynamic> GetDocument(string configuration,
                                                string documentType,
                                                Action<FilterBuilder> filter,
                                                int pageNumber,
                                                int pageSize,
                                                Action<SortingBuilder> sorting = null,
                                                IEnumerable<dynamic> ignoreResolve = null)
        {
            return GetDocument(configuration, documentType, filter.ToFilterCriterias(), pageNumber, pageSize, sorting.ToSortingCriterias(), ignoreResolve);
        }

        public IEnumerable<object> GetDocument(string configuration,
                                               string documentType,
                                               IEnumerable<FilterCriteria> filter,
                                               int pageNumber,
                                               int pageSize,
                                               IEnumerable<SortingCriteria> sorting = null,
                                               IEnumerable<dynamic> ignoreResolve = null)
        {
            var configurationObject = _configurationObjectBuilder.GetConfigurationObject(configuration);

            if (configurationObject == null)
            {
                throw new ArgumentException(configuration, nameof(configuration));
            }

            var documentProvider = configurationObject.GetDocumentProvider(documentType);

            var metadataConfiguration = _configurationObjectBuilder.GetConfigurationObject(configuration)
                                                                   .ConfigurationMetadata;

            if (metadataConfiguration == null)
            {
                return new List<dynamic>();
            }

            dynamic schema = metadataConfiguration.GetDocumentSchema(documentType);

            SortingCriteria[] sortingFields = null;

            if (schema != null)
            {
                // Извлекаем информацию о полях, по которым можно проводить сортировку из метаданных документа
                sortingFields = SortingPropertiesExtractor.ExtractSortingProperties(string.Empty, schema.Properties, _configurationObjectBuilder);
            }

            var sortingArray = (sorting != null) ? sorting.ToArray() : new SortingCriteria[] { };

            if (sortingArray.Any())
            {
                // Поля сортировки заданы в запросе. 
                // Берем только те поля, которые разрешены в соответствии с метаданными

                var filteredSortingFields = new List<SortingCriteria>();

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

            // делаем выборку документов для последующего Resolve и фильтрации по полям Resolved объектов
            var pageSizeUnresolvedDocuments = Math.Min(pageSize, 1000);

            IEnumerable<dynamic> result = documentProvider.GetDocument(filter, 0, Convert.ToInt32(pageSizeUnresolvedDocuments), sorting, pageNumber > 0
                ? pageNumber * pageSize
                : 0);

            _referenceResolver.ResolveReferences(configuration, documentType, result, ignoreResolve);

            var documents = result.Take(pageSize == 0 ? 10 : pageSize);

            return GetActualDocuments(configuration, documentType, documents);
        }


        private IEnumerable<object> GetActualDocuments(string configuration, string documentType, IEnumerable<object> documents)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            if (transactionScope != null)
            {
                return transactionScope.GetDocuments(configuration, documentType, documents);
            }

            return documents;
        }
    }
}