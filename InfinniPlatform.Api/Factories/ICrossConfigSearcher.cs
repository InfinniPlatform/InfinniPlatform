using System.Collections.Generic;

namespace InfinniPlatform.Api.Factories
{
    /// <summary>
    ///     Исполнитель поисковых запросов по имеющимся конфигурациям и документам
    /// </summary>
    public interface ICrossConfigSearcher
    {
        /// <summary>
        ///     Выполнить запрос для поиска документа по определенным критериям
        ///     по всем имеющимся конфигурациям
        /// </summary>
        /// <param name="filterObject">Фильтр</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="routing"></param>
        /// <param name="sorting">Сортировка</param>
        /// <param name="configs">Наименования конфигураций, по которым будет производиться поиск</param>
        /// <param name="documents">Типы документов, по которым будет производиться поиск</param>
        /// <returns>Найденные документы</returns>
        dynamic GetDocuments(IEnumerable<object> filterObject, int pageNumber, int pageSize, string routing,
            IEnumerable<object> sorting, IEnumerable<string> configs = null, IEnumerable<string> documents = null);
    }
}