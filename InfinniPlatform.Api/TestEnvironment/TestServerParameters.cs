using System.Collections.Generic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Api;


namespace InfinniPlatform.Api.TestEnvironment
{
    /// <summary>
    ///     Параметры тестового сервера.
    /// </summary>
    public class TestServerParameters
    {
        /// <summary>
        ///     Перенаправить вывод консоли в файл.
        /// </summary>
        public bool RedirectConsoleToFileOutput;

        public TestServerParameters()
        {
            HostingConfig = new HostingConfig();
            Configurations = new List<ConfigurationInfo>();
            ConfigurationAssemblies = new List<string>();
            RedirectConsoleToFileOutput = false;
            RealConfigNeeds = true;
        }

        /// <summary>
        ///     Настройки подсистемы хостинга.
        /// </summary>
        public HostingConfig HostingConfig { get; set; }

        /// <summary>
        ///     Список конфигураций.
        /// </summary>
        public IList<ConfigurationInfo> Configurations { get; set; }

        /// <summary>
        ///     Список сборок конфигураций.
        /// </summary>
        public IList<string> ConfigurationAssemblies { get; set; }

        /// <summary>
        ///     Не пытаться подключить к серверу отладчик.
        /// </summary>
        public bool DoNotAttachDebug { get; set; }

        /// <summary>
        ///     Необходимо формирование реальной (а не заглушки для теста) конфигурации JSON в индексе, аналогично production.
        /// </summary>
        public bool RealConfigNeeds { get; set; }

        /// <summary>
        ///     Ожидать нажатия клавиши для аттача к серверу в режиме отладки.
        /// </summary>
        public bool WaitForDebugAttach { get; set; }

        public string GetServerBaseAddress()
        {
            var config = HostingConfig;

            return (config != null)
                ? string.Format("{0}://{1}:{2}", config.ServerScheme, config.ServerName, config.ServerPort)
                : null;
        }
    }
}