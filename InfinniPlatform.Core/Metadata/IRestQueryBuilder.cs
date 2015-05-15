using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Hosting;
using InfinniPlatform.SearchOptions;

namespace InfinniPlatform.Metadata
{
	/// <summary>
	///   Конструктор запросов к REST сервисам платформы
	/// </summary>
	public interface IRestQueryBuilder
	{
		/// <summary>
		///   Сформировать и выполнить запрос на применение изменений 
		/// </summary>
		/// <param name="id">Идентификатор объекта, который необходимо изменить</param>
		/// <param name="changesObject">Объект, из которого будет сформирован список изменений</param>
		/// <returns>Ответ на вызов сервиса</returns>
		RestQueryResponse QueryApplyChanges(string id, object changesObject);

		/// <summary>
		///   Сформировать и выполнить запрос на поиск данных
		/// </summary>
		/// <param name="filterObject">Фильтр по данным</param>
		/// <param name="pageNumber">Номер страницы данных</param>
		/// <param name="pageSize">Размер страницы</param>
		/// <param name="searchType">Тип поиска записей</param>
		/// <param name="version">Версия конфигурации, для которой осуществляется поиск объектов (если null - поиск по всем версиям объектов)</param>
		/// <returns>Ответ на вызов сервиса</returns>
		RestQueryResponse QuerySearch(IEnumerable<object> filterObject, int pageNumber, int pageSize,
		                                              SearchType searchType = SearchType.All, string version = null);
	}
}
