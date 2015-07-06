using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    ///   Провайдер документов
    /// </summary>
    public sealed class DocumentProvider : IDocumentProvider
    {
        private readonly IIndexQueryExecutor _indexQueryExecutor;

        public DocumentProvider(IIndexQueryExecutor indexQueryExecutor)
        {
            _indexQueryExecutor = indexQueryExecutor;
        }

	    /// <summary>
	    ///   Получить актуальные версии объектов, отсортированные по дате вставки в индекс по убыванию  
	    /// </summary>
	    /// <param name="filterObject">Фильтр объектов</param>
	    /// <param name="pageNumber">Номер страницы данных</param>
	    /// <param name="pageSize">Размер страницы данных</param>
	    /// <param name="sortingDescription">Описание правил сортировки</param>
	    /// <param name="skipSize"></param>
	    /// <returns>Список актуальных версий</returns>
	    public dynamic GetDocument(IEnumerable<object> filterObject, int pageNumber, int pageSize, IEnumerable<dynamic> sortingDescription = null, int skipSize = 0)
        {
            var filterFactory = FilterBuilderFactory.GetInstance();
            var searchModel = filterObject.ExtractSearchModel(filterFactory);
            searchModel.SetPageSize(pageSize);
			searchModel.SetSkip(skipSize);
            searchModel.SetFromPage(pageNumber);

            if (sortingDescription != null)
            {
                foreach (var sorting in sortingDescription)
                {
                    searchModel.AddSort(sorting.PropertyName, (SortOrder)sorting.SortOrder);
                }
            }

            return _indexQueryExecutor.Query(searchModel).Items.ToList();
        }
    }
}