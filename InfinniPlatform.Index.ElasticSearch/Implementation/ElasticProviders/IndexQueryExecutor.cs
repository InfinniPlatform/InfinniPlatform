using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestQueries;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeSelectors;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    ///   Исполнитель поисковых запросов к ElasticSearch
    /// </summary>
    public sealed class IndexQueryExecutor : IIndexQueryExecutor
    {
        private readonly string _tenantId;
        private readonly IEnumerable<IndexToTypeAccordance> _typeNames;
        private readonly ElasticConnection _elasticConnection;

        // Имена индексов, использующиеся при поисковых запросах по нескольким индексам
        private readonly IEnumerable<string> _indexNames;

        private readonly bool _searchInAllIndeces;
        private readonly bool _searchInAllTypes;


        public IndexQueryExecutor(string indexName, string typeName, string tenantId)
        {
            _tenantId = string.IsNullOrEmpty(tenantId) ? "" : tenantId.ToLowerInvariant();
            _elasticConnection = new ElasticConnection();

            _typeNames = _elasticConnection.GetAllTypes(new[] {indexName}, new[] {typeName});
            _indexNames = new[] {indexName.ToLowerInvariant()};

            _elasticConnection.ConnectIndex();

            _searchInAllIndeces = false;
            _searchInAllTypes = false;
        }

        public IndexQueryExecutor(IEnumerable<string> indexNames, IEnumerable<string> typeNames, string tenantId)
        {
            _tenantId = tenantId;
            _elasticConnection = new ElasticConnection();

            if (indexNames != null && indexNames.Any())
            {
                _indexNames = indexNames.Select(s => s.ToLowerInvariant());
                _searchInAllIndeces = false;

                if (typeNames != null && typeNames.Any())
                {
                    _typeNames = _elasticConnection.GetAllTypes(_indexNames, typeNames);

                    // Указанных типов не обнаружено, выполнять поисковые запросы нельзя
                    if (!_typeNames.Any())
                    {
                        _searchInAllIndeces = false;
                        _searchInAllTypes = false;
                    }
                }
                else
                {
                    _searchInAllTypes = true;
                }
            }
            else
            {
                _searchInAllIndeces = true;
                _searchInAllTypes = true;
            }



            _elasticConnection.ConnectIndex();
        }

        /// <summary>
        ///   Найти список объектов в индексе по указанной модели поиска
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
        ///   Определить количество объектов в индексе по указанной модели поиска
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <returns>Количество объектов, удовлетворяющих условиям поиска</returns>
        public long CalculateCountQuery(SearchModel searchModel)
        {
            searchModel.AddFilter(new NestQuery(Query<dynamic>.Term(ElasticConstants.TenantIdField, _tenantId)));
            searchModel.AddFilter(new NestQuery(Query<dynamic>.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)));

            Func<CountDescriptor<dynamic>, CountDescriptor<dynamic>> desc =
                descriptor => new ElasticCountQueryBuilder(descriptor)
                    .BuildCountQueryDescriptor(searchModel)
                    .BuildSearchForType(_indexNames,
                        (_typeNames == null || !_typeNames.Any()) ? null : _typeNames.SelectMany(d => d.TypeNames),
                         _searchInAllIndeces, _searchInAllTypes);


            var documentsResponse = _elasticConnection.Client.Count(desc);

            return documentsResponse != null ? documentsResponse.Count : 0;
        }

        /// <summary>
        ///   Выполнить запрос с получением объектов индекса без дополнительной обработки
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <param name="convert">Делегат для конвертирования результата</param>
        /// <returns>Результаты поиска</returns>
        public SearchViewModel QueryOverObject(SearchModel searchModel, Func<dynamic, string, string, object> convert)
        {
            searchModel.AddFilter(new NestFilter(Filter<dynamic>.Query(q => q.Term(ElasticConstants.TenantIdField, _tenantId))));
            searchModel.AddFilter(new NestFilter(Filter<dynamic>.Query(q => q.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid))));

            Func<SearchDescriptor<dynamic>, SearchDescriptor<dynamic>> desc =
                descriptor => new ElasticSearchQueryBuilder(descriptor)
                    .BuildSearchDescriptor(searchModel)
                    .BuildSearchForType(_indexNames,
                        (_typeNames == null || !_typeNames.Any()) ? null : _typeNames.SelectMany(d => d.TypeNames), _searchInAllIndeces, _searchInAllTypes);


            var documentsResponse = _elasticConnection.Client.Search(desc);

            var hitsCount = documentsResponse != null && documentsResponse.Hits != null
                ? documentsResponse.Hits.Count()
                : 0;

            var documentResponseCount = documentsResponse != null &&
                                        documentsResponse.Hits != null
                ? documentsResponse.Hits.Select(r => convert(r.Source, r.Index, ToDocumentId(r.Type))).ToList()
                : new List<dynamic>();

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