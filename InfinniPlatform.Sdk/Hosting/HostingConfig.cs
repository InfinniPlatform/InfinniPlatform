using System;

namespace InfinniPlatform.Sdk.Hosting
{
    /// <summary>
    /// Настройки подсистемы хостинга.
    /// </summary>
    public sealed class HostingConfig
    {
        public const string SectionName = "host";


        /// <summary>
        /// Настройки подсистемы хостинга по умолчанию.
        /// </summary>
        public static readonly HostingConfig Default = new HostingConfig();


        public HostingConfig()
        {
            Scheme = "http";
            Name = "localhost";
            Port = 9900;
        }


        /// <summary>
        /// Имя схемы протокола сервера.
        /// </summary>
        /// <example>
        /// https
        /// </example>
        public string Scheme { get; set; }

        /// <summary>
        /// Адрес или имя сервера.
        /// </summary>
        /// <example>
        /// localhost
        /// </example>
        public string Name { get; set; }

        /// <summary>
        /// Номер порта сервера.
        /// </summary>
        /// <example>
        /// 9900
        /// </example>
        public int Port { get; set; }

        /// <summary>
        /// Отпечаток сертификата.
        /// </summary>
        /// <example>
        /// 49 09 66 d6 df 5b 95 b5 45 6e 70 79 a0 bf 96 9f 43 62 05 34
        /// </example>
        public string Certificate { get; set; }


        public override string ToString()
        {
            return $"{Scheme}{Uri.SchemeDelimiter}{Name}:{Port}";
        }
    }
}