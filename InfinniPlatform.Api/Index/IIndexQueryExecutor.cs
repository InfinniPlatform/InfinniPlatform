using System;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Index.SearchOptions;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Index
{
	/// <summary>
	///   Исполнитель запросов к ES
	///   Предоставляет вариант получения данных
	///   из индексов по модели поиска
	/// </summary>
	public interface IIndexQueryExecutor
	{
        /// <summary>
        ///   Найти список объектов в индексе по указанной модели поиска
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <returns>Модель результат поиска объектов</returns>
		SearchViewModel Query(SearchModel searchModel);

        /// <summary>
        ///   Выполнить запрос с получением объектов индекса без дополнительной обработки
        /// </summary>
        /// <param name="searchModel">Модель поиска</param>
        /// <param name="convert">Делегат для конвертирования результата</param>
        /// <returns>Результаты поиска</returns>
	    SearchViewModel QueryOverObject(SearchModel searchModel, Func<dynamic, string, string, object> convert);
	}

	public static class IndexQueryExtensions
	{
		public static SearchViewModel QueryAsJObject(this IIndexQueryExecutor indexQueryExecutor, SearchModel searchModel)
		{
			return indexQueryExecutor.QueryOverObject(searchModel, (item, index, type) =>
			{
			    var objectToReturn = item.Values;

			    if (objectToReturn != null)
			    {
			        objectToReturn.__ConfigId = index;
			        objectToReturn.__DocumentId = type;
			    }

			    return objectToReturn;
			});
		}
	}
}
