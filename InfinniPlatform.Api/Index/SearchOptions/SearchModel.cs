using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.SearchOptions;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Api.Index.SearchOptions
{

    /// <summary>
    ///  Модель данных для формирования поиска средствами
    ///  Elastic Search либо любого другого поставщика данных
    /// 
    ///  Для обеспечения инкапсуляции доступ к членам класса
    ///  осуществляется только через методы его публичного контракта
    /// </summary>
    public sealed class SearchModel
    {
        private readonly IList<SortOption> _criteriaOrder = new List<SortOption>();

        /// <summary>
        ///   Возвращать данные, начиная со страницы...
        /// </summary>
        public int FromPage { get; private set; }

        /// <summary>
        ///   Размер одной страницы данных
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Возвращать результаты, начиная с...
        /// </summary>
        public int Skip { get; private set; }

        /// <summary>
        ///   Список критериев для упорядочивания
        /// </summary>
        public IList<SortOption> SortOptions
        {
            get { return _criteriaOrder; }
        }

        /// <summary>
        ///   Список критериев для поиска
        /// </summary>
        public IFilter Filter { get; private set; }

        public void AddFilter(IFilter criteria)
        {
            Filter = Filter == null
                ? criteria
                : Filter.And(criteria);
        }

        public void AddSort(string propertyName, SortOrder sortOrder)
        {
            SortOptions.Add(new SortOption(propertyName, sortOrder));
        }

        public void SetFromPage(int fromPage)
        {
            FromPage = fromPage;
        }

        public void SetPageSize(int pageSize)
        {
            PageSize = pageSize;
        }

        public void SetSkip(int skip)
        {
            Skip = skip;
        }
    }

    public static class SearchModelExtensions
    {
        public static SearchModel ExtractSearchModel(this IEnumerable<dynamic> filterObject, IFilterBuilder filterFactory)
        {
            var searchModel = new SearchModel();

            // В случае, если ищем документы по фильтру, мы должны игнорировать документы со статусами Deleted и Invalid
            searchModel.AddFilter(filterFactory.Get("Status", "Deleted", CriteriaType.IsNotEquals));
            searchModel.AddFilter(filterFactory.Get("Status", "Invalid", CriteriaType.IsNotEquals));

            if (filterObject == null)
                return searchModel;

            var filters = filterObject
                .Select(x => filterFactory.Get((string) x.Property, (object) x.Value, (CriteriaType) x.CriteriaType))
                .ToList();

            foreach (var filter in filters)
            {
                searchModel.AddFilter(filter);
            }

            return searchModel;
        }
    }
}
