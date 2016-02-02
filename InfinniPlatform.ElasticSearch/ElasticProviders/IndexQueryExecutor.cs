using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.ElasticSearch.ElasticSearchModels;
using InfinniPlatform.ElasticSearch.Filters.NestFilters;
using InfinniPlatform.ElasticSearch.Filters.NestQueries;
using InfinniPlatform.ElasticSearch.IndexTypeSelectors;
using InfinniPlatform.ElasticSearch.IndexTypeVersions;
using InfinniPlatform.Sdk.Dynamic;

using Nest;

namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    /// <summary>
    /// Исполнитель поисковых запросов к ElasticSearch
    /// </summary>
    public sealed class IndexQueryExecutor : IIndexQueryExecutor
    {
        public IndexQueryExecutor(ElasticConnection elasticConnection,
                                  ElasticTypeManager elasticTypeManager,
                                  ITenantProvider tenantProvider,
                                  string documentType)
        {
            _elasticConnection = elasticConnection;
            _tenantProvider = tenantProvider;

            var indexTypeName = elasticTypeManager.GetActualTypeName(documentType);

            _typeNames = elasticTypeManager.GetTypeMappings(indexTypeName).GetMappingsTypeNames();
        }

        private readonly ElasticConnection _elasticConnection;
        private readonly ITenantProvider _tenantProvider;
        private readonly IEnumerable<string> _typeNames;

        /// <summary>
        /// Найти список объектов в индексе по указанной модели поиска
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <returns>Модель результат поиска объектов</returns>
        public SearchViewModel Query(SearchModel searchModel)
        {
            return QueryOverObject(searchModel, (item, index, type) =>
                                                {
                                                    // TODO: ЧТо это за извращение? Нельзя было нормально решить проблему?
                                                    dynamic result = DynamicWrapperExtensions.ToDynamic(item.Values);
                                                    result.TimeStamp = item.TimeStamp;
                                                    return result;
                                                });
        }

        /// <summary>
        /// Определить количество объектов в индексе по указанной модели поиска
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <returns>Количество объектов, удовлетворяющих условиям поиска</returns>
        public long CalculateCountQuery(SearchModel searchModel)
        {
            searchModel.AddFilter(new NestQuery(Query<dynamic>.Terms(ElasticConstants.TenantIdField, _tenantProvider.GetTenantId(), AuthorizationStorageExtensions.AnonymousUser)));
            searchModel.AddFilter(new NestQuery(Query<dynamic>.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)));

            Func<CountDescriptor<dynamic>, CountDescriptor<dynamic>> desc =
                descriptor => new ElasticCountQueryBuilder(descriptor).BuildCountQueryDescriptor(searchModel)
                                                                      .BuildSearchForType(_typeNames);

            var documentsResponse = _elasticConnection.Client.Count(desc);

            return (documentsResponse != null) ? documentsResponse.Count : 0;
        }

        /// <summary>
        /// Выполнить запрос с получением объектов индекса без дополнительной обработки
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <param name="convert">Делегат для конвертирования результата</param>
        /// <returns>Результаты поиска</returns>
        public SearchViewModel QueryOverObject(SearchModel searchModel, Func<dynamic, string, string, object> convert)
        {
            searchModel.AddFilter(new NestFilter(Filter<dynamic>.Terms(ElasticConstants.TenantIdField, new[] { _tenantProvider.GetTenantId(), AuthorizationStorageExtensions.AnonymousUser })));
            searchModel.AddFilter(new NestFilter(Filter<dynamic>.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)));

            Func<SearchDescriptor<dynamic>, SearchDescriptor<dynamic>> desc =
                descriptor => new ElasticSearchQueryBuilder(descriptor).BuildSearchDescriptor(searchModel)
                                                                       .BuildSearchForType(_typeNames);

            var documentsResponse = _elasticConnection.Search(desc);

            var hitsCount = (documentsResponse != null && documentsResponse.Hits != null) 
                ? documentsResponse.Hits.Count() 
                : 0;

            var hitItems = (hitsCount > 0) 
                ? documentsResponse.Hits.Select(r => convert(r.Source, r.Index, ToDocumentId(r.Type))).ToList()
                : new List<dynamic>();

            return new SearchViewModel(searchModel.FromPage, searchModel.PageSize, hitsCount, hitItems);
        }

        /// <summary>
        /// Метод удаляет окончание _typeschema_ из имени типа
        /// </summary>
        private static string ToDocumentId(string fullTypeName)
        {
            var documentId = fullTypeName;

            var posSchema = fullTypeName.IndexOf(IndexTypeMapper.MappingTypeVersionPattern, StringComparison.Ordinal);
            if (posSchema > -1)
            {
                documentId = fullTypeName.Substring(0, posSchema);
            }

            return documentId;
        }
    }
}