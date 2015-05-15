namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Описание поставщика данных в виде REST-сервиса.
	/// </summary>
	public sealed class RestDataProviderInfo : IDataProviderInfo
	{
		/// <summary>
		/// Время ожидания (в миллисекундах) перед завершением попытки выполнить запрос и созданием ошибки.
		/// </summary>
		public int RequestTimeout { get; set; }

		/// <summary>
		/// Наименование в конфигурации шаблона запрашиваемого URI или сам URI.
		/// </summary>
		public string RequestUri { get; set; }

		/// <summary>
		/// Метод запроса данных (GET, POST, PUT, DELETE).
		/// </summary>
		public string Method { get; set; }

		/// <summary>
		/// Тип данных тела запроса в формате MIME.
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Тип ожидаемых данных в формате MIME.
		/// </summary>
		/// <example>application/json</example>
		/// <example>application/xml</example>
		public string AcceptType { get; set; }

		/// <summary>
		/// Тело запроса.
		/// </summary>
		public string Body { get; set; }
	}
}