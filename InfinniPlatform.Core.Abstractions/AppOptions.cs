using System;
using System.Collections.Generic;

namespace InfinniPlatform
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    public class AppOptions
    {
        public const string SectionName = "app";

        public const string DefaultServerScheme = "http";

        public const string DefaultServerName = "localhost";

        public const int DefaultServerPort = 9900;


        /// <summary>
        /// Настройки приложения по умолчанию.
        /// </summary>
        public static readonly AppOptions Default = new AppOptions();


        public AppOptions()
        {
            AppName = "InfinniPlatform";
            AppInstance = Guid.NewGuid().ToString("N");

            ServerScheme = DefaultServerScheme;
            ServerName = DefaultServerName;
            ServerPort = DefaultServerPort;

            StaticFilesMapping = new Dictionary<string, string>();
            EmbeddedResourceMapping = new Dictionary<string, string>();
        }


        /// <summary>
        /// Имя приложения.
        /// </summary>
        /// <remarks>
        /// Используется для изоляции данных между приложениями.
        /// </remarks>
        /// <example>
        /// App1
        /// </example>
        public string AppName { get; set; }

        /// <summary>
        /// Имя текущего экземпляра приложения.
        /// </summary>
        /// <remarks>
        /// Используется для идентификации экземпляра приложения в кластере.
        /// </remarks>
        /// <example>
        /// App1_Instance1
        /// </example>
        public string AppInstance { get; set; }


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
        /// Соответствие виртуальных и физических путей до статических файлов.
        /// </summary>
        public Dictionary<string, string> StaticFilesMapping { get; set; }

        /// <summary>
        /// Соответствие виртуальных путей и сборок с файлами ресурсов.
        /// </summary>
        public Dictionary<string, string> EmbeddedResourceMapping { get; set; }


        public override string ToString()
        {
            return $"{ServerScheme}://{ServerName}:{ServerPort}";
        }
    }
}