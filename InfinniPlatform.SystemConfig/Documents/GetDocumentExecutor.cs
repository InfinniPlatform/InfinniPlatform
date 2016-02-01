using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Documents;
using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.Factories;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.Documents
{
    internal sealed class GetDocumentExecutor : IGetDocumentExecutor
    {
        public delegate IIndexQueryExecutor IndexQueryExecutorFactory(string configuration, string documentType);


        public GetDocumentExecutor(IDocumentTransactionScopeProvider transactionScopeProvider,
                                   IMetadataApi metadataApi,
                                   IReferenceResolver referenceResolver,
                                   IIndexFactory indexFactory,
                                   IndexQueryExecutorFactory indexQueryExecutorFactory)
        {
            _transactionScopeProvider = transactionScopeProvider;
            _metadataApi = metadataApi;
            _referenceResolver = referenceResolver;
            _indexFactory = indexFactory;
            _indexQueryExecutorFactory = indexQueryExecutorFactory;
        }

        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;
        private readonly IIndexFactory _indexFactory;
        private readonly IMetadataApi _metadataApi;
        private readonly IReferenceResolver _referenceResolver;
        private readonly IndexQueryExecutorFactory _indexQueryExecutorFactory;

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
            var schema = _metadataApi.GetDocumentSchema(configuration, documentType);

            var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataApi, schema);

            var queryFactory = QueryBuilderFactory.GetInstance();
            var filterCriteria = queryAnalyzer.GetBeforeResolveCriteriaList(filter);
            var searchModel = filterCriteria.ExtractSearchModel(queryFactory);

            var indexQueryExecutor = _indexQueryExecutorFactory(configuration, documentType);

            // вряд ли документов в одном индексе будет больше чем 2 147 483 647, конвертируем в int
            var numberOfDocuments = Convert.ToInt32(indexQueryExecutor.CalculateCountQuery(searchModel));

            return numberOfDocuments;
        }

        public IEnumerable<dynamic> GetDocument(string configurationName,
                                                string documentName,
                                                Action<FilterBuilder> filter,
                                                int pageNumber,
                                                int pageSize,
                                                Action<SortingBuilder> sorting = null,
                                                IEnumerable<dynamic> ignoreResolve = null)
        {
            return GetDocument(configurationName, documentName, filter.ToFilterCriterias(), pageNumber, pageSize, sorting.ToSortingCriterias(), ignoreResolve);
        }

        public IEnumerable<object> GetDocument(string configurationName,
                                               string documentName,
                                               IEnumerable<FilterCriteria> filter,
                                               int pageNumber,
                                               int pageSize,
                                               IEnumerable<SortingCriteria> sorting = null,
                                               IEnumerable<dynamic> ignoreResolve = null)
        {
            dynamic schema = _metadataApi.GetDocumentSchema(configurationName, documentName);

            SortingCriteria[] sortingFields = null;

            if (schema != null)
            {
                // Извлекаем информацию о полях, по которым можно проводить сортировку из метаданных документа
                sortingFields = SortingPropertiesExtractor.ExtractSortingProperties(string.Empty, schema.Properties, _metadataApi);
            }

            var sortingArray = sorting != null
                                   ? sorting.ToArray()
                                   : new SortingCriteria[] { };

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

            var filterFactory = FilterBuilderFactory.GetInstance();
            var searchModel = filter.ExtractSearchModel(filterFactory);
            searchModel.SetPageSize(Convert.ToInt32(pageSizeUnresolvedDocuments));
            searchModel.SetSkip(pageNumber > 0
                                    ? pageNumber * pageSize
                                    : 0);
            searchModel.SetFromPage(0);

            if (sorting != null)
            {
                foreach (var sorting1 in sorting)
                {
                    searchModel.AddSort(sorting1.PropertyName, sorting1.SortingOrder);
                }
            }

            var indexQueryExecutor = _indexQueryExecutorFactory(configurationName, documentName);

            IEnumerable<dynamic> result = indexQueryExecutor.Query(searchModel).Items.ToList();

            _referenceResolver.ResolveReferences(configurationName, documentName, result, ignoreResolve);

            var documents = result.Take(pageSize == 0
                                            ? 10
                                            : pageSize);

            return GetActualDocuments(configurationName, documentName, documents);
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