using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects
{
	/// <summary>
	///   Контракт преобразования объектов в список событий
	/// </summary>
	public interface IObjectToEventSerializer
	{
		/// <summary>
		///   Получить список событий по созданию указанного в аргументах объекта
		/// </summary>
		/// <returns>Список событий изменения/создания объекта</returns>
		IEnumerable<EventDefinition> GetEvents();
	}
}
