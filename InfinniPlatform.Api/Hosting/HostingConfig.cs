using System;

using InfinniPlatform.Api.Settings;

namespace InfinniPlatform.Api.Hosting
{
	/// <summary>
	/// Настройки подсистемы хостинга.
	/// </summary>
	public sealed class HostingConfig
	{
		public HostingConfig()
		{
			ServerScheme = DefaultServerScheme;
			ServerName = DefaultServerName;
			ServerPort = DefaultServerPort;
			ServerCertificate = DefaultServerCertificate;
			ServerProfileQuery = DefaultServerProfileQuery;
		}


		/// <summary>
		/// Настройки подсистемы хостинга по умолчанию.
		/// </summary>
		public static readonly HostingConfig Default = new HostingConfig();


		private const string AppServerSchemeConfig = "AppServerScheme";
		public readonly string DefaultServerScheme = AppSettings.GetValue(AppServerSchemeConfig, Uri.UriSchemeHttp);

		/// <summary>
		/// Имя схемы протокола сервера.
		/// </summary>
		/// <example>
		/// https
		/// </example>
		public string ServerScheme { get; set; }


		private const string AppServerNameConfig = "AppServerName";
		public readonly string DefaultServerName = AppSettings.GetValue(AppServerNameConfig, "localhost");

		/// <summary>
		/// Адрес или имя сервера.
		/// </summary>
		/// <example>
		/// localhost
		/// </example>
		public string ServerName { get; set; }


		private const string AppServerPortConfig = "AppServerPort";
		public readonly int DefaultServerPort = AppSettings.GetValue(AppServerPortConfig, 9900);

		/// <summary>
		/// Номер порта сервера.
		/// </summary>
		/// <example>
		/// 9900
		/// </example>
		public int ServerPort { get; set; }


		private const string AppServerCertificateConfig = "AppServerCertificate";
		public readonly string DefaultServerCertificate = AppSettings.GetValue(AppServerCertificateConfig);

		/// <summary>
		/// Отпечаток сертификата.
		/// </summary>
		/// <example>
		/// 49 09 66 d6 df 5b 95 b5 45 6e 70 79 a0 bf 96 9f 43 62 05 34
		/// </example>
		public string ServerCertificate { get; set; }


		private const string AppServerProfileQueryConfig = "AppServerProfileQuery";
		public readonly bool DefaultServerProfileQuery = AppSettings.GetValue(AppServerProfileQueryConfig, false);

		/// <summary>
		/// Включить профилирование запросов.
		/// </summary>
		public bool ServerProfileQuery { get; set; }


		public override string ToString()
		{
			return string.Format("{0}{1}{2}:{3}", ServerScheme, Uri.SchemeDelimiter, ServerName, ServerPort);
		}
	}
}