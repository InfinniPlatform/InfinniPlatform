using System.Web.Http.Routing;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.WebApi.ConfigRequestProviders
{
	/// <summary>
	///  Провайдер информации о текущем роутинге системы
	/// </summary>
    public class ConfigRequestProvider : IConfigRequestProvider
    {

		/// <summary>
		///  Получить идентификатор метаданных конфигурации из роутинга
		/// </summary>
		/// <returns></returns>
		public string GetConfiguration() {
            return GetDataFromRoute("configuration");
        }

        private string GetDataFromRoute(string paramName) {
            object config;
            RequestData.Values.TryGetValue(paramName, out config);
            return config as string;
        }

		/// <summary>
		///   Получить идентификатор метаданных объекта метаданных из роутинга
		/// </summary>
		/// <returns></returns>
        public string GetMetadataIdentifier() {
            return GetDataFromRoute("metadata");
        }

		/// <summary>
		///   Получить идентификатор метаданных контейнера обработчиков запроса из роутинга
		/// </summary>
		/// <returns></returns>
		public string GetServiceName()
		{
			return GetDataFromRoute("service");
		}

		/// <summary>
		///   Получить идентификатор авторизованного в системе пользователя
		/// </summary>
		/// <returns></returns>
		public string GetUserName()
		{
			return UserName;
		}


	    /// <summary>
	    ///  Получить идентификатор работающей версии сервисов
	    /// </summary>
	    /// <returns></returns>
	    public string GetVersion()
	    {
	        var version = GetDataFromRoute("version");
	        return version == "0" ? null : version;
	    }

	    public IHttpRouteData RequestData { get; set; }

		public string UserName { get; set; }
    }
}