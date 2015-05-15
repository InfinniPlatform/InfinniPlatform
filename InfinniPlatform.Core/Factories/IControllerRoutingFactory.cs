using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.Factories
{
	/// <summary>
	///   Фабрика роутинга контроллеров REST
	/// </summary>
	public interface IControllerRoutingFactory
	{
		/// <summary>
		///   Получить адрес выполняемого запроса по указанным метаданным роутинга
		/// </summary>
		/// <param name="configRequestProvider">Провайдер метаданных запроса</param>
		/// <returns>Строка запроса</returns>
		string BuildRestRoutingUrl(IConfigRequestProvider configRequestProvider);
	}
}
