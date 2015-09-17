using System;

using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Sdk.Api
{
	/// <summary>
	/// Настройки подсистемы хостинга.
	/// </summary>
	public sealed class HostingConfig
	{
		/// <summary>
		/// Настройки подсистемы хостинга по умолчанию.
		/// </summary>
		public static readonly HostingConfig Default = new HostingConfig();


		private const string AppServerSchemeConfig = "AppServerScheme";
		private const string AppServerNameConfig = "AppServerName";
		private const string AppServerPortConfig = "AppServerPort";
		private const string AppServerCertificateConfig = "AppServerCertificate";
		private const string AppServerProfileQueryConfig = "AppServerProfileQuery";

		public readonly string DefaultServerScheme = AppSettings.GetValue(AppServerSchemeConfig, Uri.UriSchemeHttp);
		public readonly string DefaultServerName = AppSettings.GetValue(AppServerNameConfig, "localhost");
		public readonly int DefaultServerPort = AppSettings.GetValue(AppServerPortConfig, 9900);
		public readonly string DefaultServerCertificate = AppSettings.GetValue(AppServerCertificateConfig);
		public readonly bool DefaultServerProfileQuery = AppSettings.GetValue(AppServerProfileQueryConfig, false);


		public HostingConfig()
		{
			ServerScheme = DefaultServerScheme;
			ServerName = DefaultServerName;
			ServerPort = DefaultServerPort;
			ServerCertificate = DefaultServerCertificate;
			ServerProfileQuery = DefaultServerProfileQuery;
		}


		/// <summary>
		/// Имя схемы протокола сервера.
		/// </summary>
		/// <example>
		/// https
		/// </example>
		public string ServerScheme { get; set; }

		/// <summary>
		/// Адрес или имя сервера.
		/// </summary>
		/// <example>
		/// localhost
		/// </example>
		public string ServerName { get; set; }

		/// <summary>
		/// Номер порта сервера.
		/// </summary>
		/// <example>
		/// 9900
		/// </example>
		public int ServerPort { get; set; }

		/// <summary>
		/// Отпечаток сертификата.
		/// </summary>
		/// <example>
		/// 49 09 66 d6 df 5b 95 b5 45 6e 70 79 a0 bf 96 9f 43 62 05 34
		/// </example>
		public string ServerCertificate { get; set; }

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