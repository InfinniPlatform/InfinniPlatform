using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.Index
{
    /// <summary>
    ///     Модель данных для формирования поиска средствами
    ///     Elastic Search либо любого другого поставщика данных
    ///     Для обеспечения инкапсуляции доступ к членам класса
    ///     осуществляется только через методы его публичного контракта
    /// </summary>
    public sealed class SearchModel
    {
        /// <summary>
        ///     Возвращать данные, начиная со страницы...
        /// </summary>
        public int FromPage { get; private set; }

        /// <summary>
        ///     Размер одной страницы данных
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        ///     Возвращать результаты, начиная с...
        /// </summary>
        public int Skip { get; private set; }

        /// <summary>
        ///     Список критериев для упорядочивания
        /// </summary>
        public IList<SortOption> SortOptions { get; } = new List<SortOption>();

        /// <summary>
        ///     Список критериев для поиска
        /// </summary>
        public IFilter Filter { get; private set; }

        public void AddFilter(IFilter filterCriteria)
        {
            if (filterCriteria != null)
            {
                Filter = Filter == null
                             ? filterCriteria
                             : Filter.And(filterCriteria);
            }
        }

        public void AddFilters(IEnumerable<IFilter> filterCriterias)
        {
            foreach (var criteria in filterCriterias.Where(filter => filter != null))
            {
                Filter = Filter == null
                             ? criteria
                             : Filter.And(criteria);
            }
        }

        public void AddSort(string propertyName, SortOrder sortOrder)
        {
            SortOptions.Add(new SortOption(propertyName, sortOrder));
        }

        public void AddSort(string propertyName, string sortOrder)
        {
            SortOptions.Add(new SortOption(propertyName, sortOrder == "descending"
                                                             ? SortOrder.Descending
                                                             : SortOrder.Ascending));
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
}