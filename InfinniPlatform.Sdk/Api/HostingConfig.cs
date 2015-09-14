using System;
using System.Configuration;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    ///     Настройки подсистемы хостинга.
    /// </summary>
    public sealed class HostingConfig
    {
        private const string AppServerSchemeConfig = "AppServerScheme";
        private const string AppServerNameConfig = "AppServerName";
        private const string AppServerPortConfig = "AppServerPort";
        private const string AppServerCertificateConfig = "AppServerCertificate";
        private const string AppServerProfileQueryConfig = "AppServerProfileQuery";

        /// <summary>
        ///     Настройки подсистемы хостинга по умолчанию.
        /// </summary>
        public static readonly HostingConfig Default = new HostingConfig();

        public readonly string DefaultServerCertificate =  ConfigurationManager.AppSettings[AppServerCertificateConfig];

        public readonly string DefaultServerName = ConfigurationManager.AppSettings[AppServerNameConfig] ??
                                                   "localhost";

        public readonly int DefaultServerPort = Convert.ToInt32(ConfigurationManager.AppSettings[AppServerPortConfig]) != 0  ? Convert.ToInt32(ConfigurationManager.AppSettings[AppServerPortConfig]) : 9900;
        public readonly bool DefaultServerProfileQuery = Convert.ToBoolean(ConfigurationManager.AppSettings[AppServerProfileQueryConfig]);
        public readonly string DefaultServerScheme = ConfigurationManager.AppSettings[AppServerSchemeConfig] ?? Uri.UriSchemeHttp;

        public HostingConfig()
        {
            ServerScheme = DefaultServerScheme;
            ServerName = DefaultServerName;
            ServerPort = DefaultServerPort;
            ServerCertificate = DefaultServerCertificate;
            ServerProfileQuery = DefaultServerProfileQuery;
        }

        /// <summary>
        ///     Имя схемы протокола сервера.
        /// </summary>
        /// <example>
        ///     https
        /// </example>
        public string ServerScheme { get; set; }

        /// <summary>
        ///     Адрес или имя сервера.
        /// </summary>
        /// <example>
        ///     localhost
        /// </example>
        public string ServerName { get; set; }

        /// <summary>
        ///     Номер порта сервера.
        /// </summary>
        /// <example>
        ///     9900
        /// </example>
        public int ServerPort { get; set; }

        /// <summary>
        ///     Отпечаток сертификата.
        /// </summary>
        /// <example>
        ///     49 09 66 d6 df 5b 95 b5 45 6e 70 79 a0 bf 96 9f 43 62 05 34
        /// </example>
        public string ServerCertificate { get; set; }

        /// <summary>
        ///     Включить профилирование запросов.
        /// </summary>
        public bool ServerProfileQuery { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}:{3}", ServerScheme, Uri.SchemeDelimiter, ServerName, ServerPort);
        }
    }
}