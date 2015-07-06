using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.WebApi.WebApi
{
	public interface IRestVerbsRegistrator
	{
		/// <summary>
		///  Добавить обработчик методов сервиса REST
		/// </summary>
		/// <returns>Контейнер обработчиков REST запросов</returns>
		IRestVerbsContainer AddVerb(IQueryHandler queryHandler);
	}
}
