using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.ElasticSearch.ElasticSearchModels;
using InfinniPlatform.ElasticSearch.Filters.NestFilters;
using InfinniPlatform.ElasticSearch.Filters.NestQueries;
using InfinniPlatform.ElasticSearch.IndexTypeSelectors;
using InfinniPlatform.ElasticSearch.IndexTypeVersions;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

using Nest;

namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    /// <summary>
    /// Исполнитель поисковых запросов к ElasticSearch
    /// </summary>
    public sealed class IndexQueryExecutor : IIndexQueryExecutor
    {
        public IndexQueryExecutor(ElasticConnection elasticConnection, ITenantProvider tenantProvider, IndexToTypeAccordanceSettings settings)
        {
            _elasticConnection = elasticConnection;
            _tenantProvider = tenantProvider;

            _indexNames = settings.Accordances.Select(x => x.Key.ToLower()).ToArray();
            _typeNames = settings.Accordances.ToArray();
            _searchInAllIndeces = settings.SearchInAllIndeces;
            _searchInAllTypes = settings.SearchInAllTypes;
        }

        private readonly ElasticConnection _elasticConnection;
        private readonly ITenantProvider _tenantProvider;
        private readonly IEnumerable<string> _indexNames;
        private readonly bool _searchInAllIndeces;
        private readonly bool _searchInAllTypes;
        private readonly KeyValuePair<string, IEnumerable<TypeMapping>>[] _typeNames;

        /// <summary>
        /// Найти список объектов в индексе по указанной модели поиска
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <returns>Модель результат поиска объектов</returns>
        public SearchViewModel Query(SearchModel searchModel)
        {
            return QueryOverObject(searchModel, (item, index, type) =>
                                                {
                                                    dynamic result = DynamicWrapperExtensions.ToDynamic(item.Values);
                                                    result.__ConfigId = index;
                                                    result.__DocumentId = type;
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
            var tenantId = _tenantProvider.GetTenantId();

            if (tenantId != AuthorizationStorageExtensions.AnonymousUser)
            {
                searchModel.AddFilter(new NestQuery(Query<dynamic>.Term(ElasticConstants.TenantIdField, tenantId)));
            }

            searchModel.AddFilter(new NestQuery(Query<dynamic>.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)));

            Func<CountDescriptor<dynamic>, CountDescriptor<dynamic>> desc =
                descriptor => new ElasticCountQueryBuilder(descriptor)
                    .BuildCountQueryDescriptor(searchModel)
                    .BuildSearchForType(_indexNames,
                        _typeNames == null || !_typeNames.Any()
                            ? null
                            : _typeNames.SelectMany(x => x.GetMappingsTypeNames()),
                        _searchInAllIndeces, _searchInAllTypes);

            var documentsResponse = _elasticConnection.Client.Count(desc);

            return documentsResponse?.Count ?? 0;
        }

        /// <summary>
        /// Выполнить запрос с получением объектов индекса без дополнительной обработки
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <param name="convert">Делегат для конвертирования результата</param>
        /// <returns>Результаты поиска</returns>
        public SearchViewModel QueryOverObject(SearchModel searchModel, Func<dynamic, string, string, object> convert)
        {
            var tenantId = _tenantProvider.GetTenantId();

            var tenantIdFilter = new NestFilter(Filter<dynamic>.Terms(ElasticConstants.TenantIdField, new[] { tenantId, AuthorizationStorageExtensions.AnonymousUser, AuthorizationStorageExtensions.SystemTenant }));

            searchModel.AddFilter(tenantIdFilter);
            searchModel.AddFilter(new NestFilter(Filter<dynamic>.Query(q => q.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid))));

            Func<SearchDescriptor<dynamic>, SearchDescriptor<dynamic>> desc = descriptor => new ElasticSearchQueryBuilder(descriptor)
                                                                                                .BuildSearchDescriptor(searchModel)
                                                                                                .BuildSearchForType(_indexNames,
                                                                                                                    _typeNames == null || !_typeNames.Any()
                                                                                                                        ? null
                                                                                                                        : _typeNames.SelectMany(x => x.GetMappingsTypeNames()),
                                                                                                                    _searchInAllIndeces,
                                                                                                                    _searchInAllTypes);

            var documentsResponse = _elasticConnection.Client.Search(desc);

            var hitsCount = documentsResponse?.Hits?.Count() ?? 0;

            var documentResponseCount = documentsResponse?.Hits?.Select(r => convert(r.Source, r.Index, ToDocumentId(r.Type))).ToList() ?? new List<dynamic>();

            return new SearchViewModel(searchModel.FromPage, searchModel.PageSize, hitsCount, documentResponseCount);
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