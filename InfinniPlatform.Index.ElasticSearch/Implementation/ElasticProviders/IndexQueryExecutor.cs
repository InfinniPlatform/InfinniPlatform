using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeSelectors;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    ///   Исполнитель поисковых запросов к ElasticSearch
    /// </summary>
    public sealed class IndexQueryExecutor : IIndexQueryExecutor
    {
	    private readonly string _routing;
	    private readonly IEnumerable<IndexToTypeAccordance> _typeNames;
        private readonly ElasticConnection _elasticConnection;
        
        // Имена индексов, использующиеся при поисковых запросах по нескольким индексам
        private readonly IEnumerable<string> _indexNames;

        private bool searchInAllIndeces;
        private bool searchInAllTypes;


        public IndexQueryExecutor(string indexName, string typeName, string routing)
        {
	        _routing = routing;
	        _elasticConnection = new ElasticConnection();
            
            _typeNames = _elasticConnection.GetAllTypes(new[] {indexName}, new[] {typeName});
            _indexNames = new[]{ indexName.ToLowerInvariant()};
            
            _elasticConnection.ConnectIndex();

            searchInAllIndeces = false;
            searchInAllTypes = false;
        }

        public IndexQueryExecutor(IEnumerable<string> indexNames, IEnumerable<string> typeNames, string routing)
        {
            _routing = routing;
            _elasticConnection = new ElasticConnection();

            if (indexNames != null && indexNames.Any())
            {
                _indexNames = indexNames.Select(s => s.ToLowerInvariant());
                searchInAllIndeces = false;

                if (typeNames != null && typeNames.Any())
                {
                    _typeNames = _elasticConnection.GetAllTypes(_indexNames, typeNames);

                    // Указанных типов не обнаружено, выполнять поисковые запросы нельзя
                    if (!_typeNames.Any())
                    {
                        searchInAllIndeces = false;
                        searchInAllTypes = false;
                    }
                }
                else
                {
                    searchInAllTypes = true;
                }
            }
            else
            {
                searchInAllIndeces = true;
                searchInAllTypes = true;
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
        ///   Выполнить запрос с получением объектов индекса без дополнительной обработки
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <param name="convert">Делегат для конвертирования результата</param>
        /// <returns>Результаты поиска</returns>
        public SearchViewModel QueryOverObject(SearchModel searchModel, Func<dynamic, string, string, object> convert)
        {
            Func<SearchDescriptor<dynamic>, SearchDescriptor<dynamic>> desc =
                descriptor => new ElasticSearchQueryBuilder(descriptor)					
                    .BuildSearchDescriptor(searchModel)
                    .BuildSearchForType(_indexNames, (_typeNames == null || !_typeNames.Any()) ? null : _typeNames.SelectMany(d => d.TypeNames), _routing, searchInAllIndeces, searchInAllTypes)
					;
            

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