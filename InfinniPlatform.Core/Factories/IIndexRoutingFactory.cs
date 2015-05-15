using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Factories
{
	/// <summary>
	///   Фабрика роутинга запросов к индексам
	/// </summary>
	public interface IIndexRoutingFactory
	{
		/// <summary>
		///   Получить роутинг запросов к индексу для указанных типов
		/// </summary>
		/// <param name="userRouting">Роутинг, предоставленный пользователем</param>
		/// <param name="indexName">Индекс для формирования роутинга</param>
		/// <param name="indexType">Тип в индексе для формирования роутинга</param>
		/// <returns>Строка роутинга для запросов к индексу</returns>
		string GetRouting(string userRouting, string indexName, string indexType);
	}
}
