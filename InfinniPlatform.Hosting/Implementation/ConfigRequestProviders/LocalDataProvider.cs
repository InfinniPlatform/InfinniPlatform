namespace InfinniPlatform.Hosting.WebApi.Implementation.ConfigRequestProviders
{
	/// <summary>
	///   Провайдер локального (без сервисов) роутинга системы
	/// </summary>
	public class LocalDataProvider : IConfigRequestProvider
	{
		private readonly string _configName;
		private readonly string _metadata;
		private readonly string _action;

		public LocalDataProvider(string configName, string metadata, string action)
		{
			_configName = configName;
			_metadata = metadata;
			_action = action;
		}

		/// <summary>
		///  Получить идентификатор метаданных конфигурации из роутинга
		/// </summary>
		/// <returns></returns>
		public string GetConfiguration()
		{
			return _configName;
		}

		/// <summary>
		///   Получить идентификатор метаданных объекта метаданных из роутинга
		/// </summary>
		/// <returns></returns>
		public string GetMetadataIdentifier()
		{
			return _metadata;
		}

		/// <summary>
		///   Получить идентификатор метаданных действия из роутинга
		/// </summary>
		/// <returns></returns>
		public string GetServiceName()
		{
			return _action;
		}
	}
}