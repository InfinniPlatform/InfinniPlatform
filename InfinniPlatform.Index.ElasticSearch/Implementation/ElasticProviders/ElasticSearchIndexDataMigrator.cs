using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Facets;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.ElasticSearch.Implementation.Versioning.IndexStrategies;
using InfinniPlatform.SearchOptions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    public sealed class ElasticSearchIndexDataMigrator : IIndexDataMigrator
    {
        private readonly ElasticConnection _elasticConnection;
        private readonly IndexAliasesManager _indexAliasesManager;
        private readonly IndexStateProvider _indexStateProvider;
        private readonly IFilterBuilder _filterFactory;
        
        public ElasticSearchIndexDataMigrator()
        {
            _elasticConnection = new ElasticConnection();
            _indexAliasesManager = new IndexAliasesManager(_elasticConnection.SearchingClient);
            _indexStateProvider = new IndexStateProvider();
            _filterFactory = FilterBuilderFactory.GetInstance();
        }

        /// <summary>
        /// Копирует данные из sourceAliasedIndexName в два индекса, объединенных под именем targetAliasedIndexName.
        /// В один индекс копируются актуальные версии документов, во второй - устаревшие версии
        /// </summary>
        public void ConvertDataToTwoIndexHistoryDataProvider(string sourceAliasedIndexName, string targetAliasedIndexName, IIndexTypeMapping targetIndexMapping = null)
        {
            sourceAliasedIndexName = sourceAliasedIndexName.ToLowerInvariant();
            targetAliasedIndexName = targetAliasedIndexName.ToLowerInvariant();

            if (_indexStateProvider.GetIndexStatus(sourceAliasedIndexName, TODO) == IndexStatus.NotExists)
            {
                throw new Exception("Type " + sourceAliasedIndexName + "doesn't exist. Can't convert data.");
            }
            
            var targetHistoryIndexName = targetAliasedIndexName + ElasticConstants.HistoryIndexPostfix;
            
            _indexStateProvider.CreateIndexTypeVersion(targetAliasedIndexName, false, targetIndexMapping);
            _indexStateProvider.CreateIndexTypeVersion(targetHistoryIndexName, false, targetIndexMapping);
            
            // Начинаем перенос данных в новые индексы
            var facetExecutor = new ElasticSearchFacetExecutor(sourceAliasedIndexName, TODO, new FacetExecutorCustomizer(null, 10000));
            var queryExecutor = new IndexQueryExecutor(sourceAliasedIndexName, TODO);
            var handledIds = new List<string>();
            
            do
            {
                var facetResult = facetExecutor.ExecuteTermFacet(new[] { ElasticConstants.IndexObjectIdentifierField }, handledIds);
                if (facetResult.Total == 0)
                {
                    // Все записи были перемещены
                    break;
                }

                foreach (var documentId in facetResult.FacetItems.Keys)
                {
                    var searchModel = new SearchModel();
                    searchModel.AddFilter(_filterFactory.Get(ElasticConstants.IndexObjectIdentifierField, documentId, CriteriaType.IsEquals));
                    searchModel.AddSort(ElasticConstants.IndexObjectTimeStampField, SortOrder.Descending);
                    searchModel.SetPageSize(10000); // поддерживается не больше 10000 версий одного документа...
                    var docs = queryExecutor.Query(searchModel).Items.ToList();

                    var aliasedManipulationProvider = new DataManipulationProvider(targetAliasedIndexName);
                    var historyManipulationProvider = new DataManipulationProvider(targetHistoryIndexName);
                    
                    aliasedManipulationProvider.Set(docs.Take(1), new InsertItemStrategy());
                    historyManipulationProvider.Set(docs.Skip(1), new InsertItemStrategy());
                }

                handledIds.AddRange(facetResult.FacetItems.Keys);
                
            } while (true);
        }

        /// <summary>
        /// Создаёт алиасы для существующего индекса, после чего indexName может использоваться в качестве aliasedIndexName.
        /// Метод позволяет использовать существующие индексы с данными в рамках новой концепции работы с алиасами
        /// </summary>
        public void LeverageIndexToAliasedIndex(string indexName)
        {
            _indexAliasesManager.CreateAliasesForActualIndexVersion(indexName, string.Empty);
        }
    }
}