using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Metadata.Handlers
{
    /// <summary>
    ///   Обработчик поиска объектов
    /// </summary>
    public interface ISearchHandler
    {
        /// <summary>
        ///   Найти список объектов, удовлетворяющих указанным критериям
        /// </summary>
        /// <param name="filterObject">Фильтр поиска объектов</param>
        /// <param name="pageNumber">Номер страницы результатов поиска</param>
        /// <param name="pageSize">Размер страницы результатов</param>
        /// <returns>Список результатов поиска</returns>
        object GetSearchResult(IEnumerable<object> filterObject, int pageNumber, int pageSize);
    }
}
