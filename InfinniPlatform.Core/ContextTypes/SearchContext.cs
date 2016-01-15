using System.Collections.Generic;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.ContextTypes
{
    /// <summary>
    /// Контекст поиска документов
    /// </summary>
    public sealed class SearchContext : CommonContext, ISearchContext
    {
        /// <summary>
        /// Наименование типа в индексе, соответствующем конфигурации
        /// </summary>
        public string IndexType { get; set; }

        /// <summary>
        /// Наименование индекса
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Фильтр данных
        /// </summary>
        public IList<FilterCriteria> Filter { get; set; }

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Размер страницы
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Результат поиска
        /// </summary>
        public IEnumerable<dynamic> SearchResult { get; set; }
    }
}