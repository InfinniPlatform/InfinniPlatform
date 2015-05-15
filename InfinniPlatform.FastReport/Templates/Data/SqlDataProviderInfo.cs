namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Описание поставщика данных в виде реляционной базы данных.
	/// </summary>
	public sealed class SqlDataProviderInfo : IDataProviderInfo
	{
		/// <summary>
		/// Тип SQL-сервера.
		/// </summary>
		public SqlServerType ServerType { get; set; }

		/// <summary>
		/// Время ожидания (в миллисекундах) перед завершением попытки выполнить команду и созданием ошибки.
		/// </summary>
		public int CommandTimeout { get; set; }

		/// <summary>
		/// Наименование в конфигурации строки подключения к базе данных или сама строка подключения.
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Команда выборки данных.
		/// </summary>
		public string SelectCommand { get; set; }
	}
}