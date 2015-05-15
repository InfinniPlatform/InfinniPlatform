using System.Collections.Generic;

namespace InfinniPlatform.Api.Index
{
    public interface IDocumentProvider
    {
	    /// <summary>
	    ///   Получить актуальные версии объектов, отсортированные по дате вставки в индекс по убыванию  
	    /// </summary>
	    /// <param name="filterObject">Фильтр объектов</param>
	    /// <param name="pageNumber">Номер страницы данных</param>
	    /// <param name="pageSize">Размер страницы данных</param>
	    /// <param name="sortingDescription">Описание правил сортировки</param>
	    /// <param name="skipSize">Количество записей с начала, которые необходимо пропустить</param>
	    /// <returns>Список актуальных версий</returns>
	    dynamic GetDocument(IEnumerable<object> filterObject, int pageNumber, int pageSize, IEnumerable<object> sortingDescription = null, int skipSize = 0);
    }
}