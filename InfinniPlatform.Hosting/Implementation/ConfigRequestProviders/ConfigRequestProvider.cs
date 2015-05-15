using System.Web.Http.Routing;

namespace InfinniPlatform.Hosting.WebApi.Implementation.ConfigRequestProviders
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
		///   Получить идентификатор метаданных обработчика запроса из роутинга
		/// </summary>
		/// <returns></returns>
		public string GetHandlerName()
		{
			return GetDataFromRoute("handler");
		}

        public IHttpRouteData RequestData { get; set; }
    }
}