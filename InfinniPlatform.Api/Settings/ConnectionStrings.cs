using System.Configuration;

namespace InfinniPlatform.Api.Settings
{
	/// <summary>
	/// Предоставляет доступ к конфигурационной секции со строками подключения.
	/// </summary>
	public static class ConnectionStrings
	{
		/// <summary>
		/// Получить строку подключения.
		/// </summary>
		/// <param name="name">Наименование строки подключения.</param>
		/// <returns>Строка подключения.</returns>
		public static string GetConnectionString(string name)
		{
			var value = ConfigurationManager.ConnectionStrings[name];

			return (value != null) ? value.ConnectionString : null;
		}
	}
}