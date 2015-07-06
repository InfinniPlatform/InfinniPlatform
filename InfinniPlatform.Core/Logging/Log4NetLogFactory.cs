using System;
using System.IO;
using System.Web.Hosting;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Sdk.Environment;
using log4net;
using log4net.Config;
using ILog = InfinniPlatform.Sdk.Environment.Log.ILog;

namespace InfinniPlatform.Logging
{
    /// <summary>
    ///     Фабрика для создания <see cref="ILog" /> на базе log4net.
    /// </summary>
    public sealed class Log4NetLogFactory : ILogFactory
    {
        /// <summary>
        ///     Создает <see cref="ILog" />.
        /// </summary>
        public ILog CreateLog()
        {
            var logConfiguration = GetLogConfigFile();

            if (logConfiguration.Exists)
            {
                XmlConfigurator.Configure(logConfiguration);
            }
            else
            {
                XmlConfigurator.Configure();
            }

            return new Log4NetLog(LogManager.GetLogger("Log4Net"));
        }

        /// <summary>
        ///     Получить конфигурационный файл для настройки сервиса журналирования.
        /// </summary>
        private static FileInfo GetLogConfigFile()
        {
            var isNonWebContext = string.IsNullOrEmpty(HostingEnvironment.MapPath("~"));
            var defaultLogConfigFilePath = isNonWebContext ? "Log.config" : "~/Log.config";
            var specifiedLogConfigFilePath = AppSettings.GetValue("LogConfigFile", defaultLogConfigFilePath);

            if (Path.IsPathRooted(specifiedLogConfigFilePath) == false)
            {
                specifiedLogConfigFilePath = isNonWebContext
                    ? GetFullPathForApp(specifiedLogConfigFilePath)
                    : GetFullPathForWeb(specifiedLogConfigFilePath);
            }

            return new FileInfo(specifiedLogConfigFilePath);
        }

        /// <summary>
        ///     Получить абсолютный путь к файлу для Web-приложения.
        /// </summary>
        private static string GetFullPathForWeb(string relativePath)
        {
            return HostingEnvironment.MapPath(relativePath);
        }

        /// <summary>
        ///     Получить абсолютный путь к файлу не для Web-приложения.
        /// </summary>
        private static string GetFullPathForApp(string relativePath)
        {
            string absolutePath;

            var currentDirectory = Directory.GetCurrentDirectory();
            var domainBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (string.Equals(currentDirectory, domainBaseDirectory, StringComparison.InvariantCultureIgnoreCase))
            {
                absolutePath = Path.GetFullPath(relativePath);
            }
            else
            {
                try
                {
                    Directory.SetCurrentDirectory(domainBaseDirectory);

                    absolutePath = Path.GetFullPath(relativePath);
                }
                finally
                {
                    Directory.SetCurrentDirectory(currentDirectory);
                }
            }

            return absolutePath;
        }
    }
}