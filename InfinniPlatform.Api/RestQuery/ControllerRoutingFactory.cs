using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Api;


namespace InfinniPlatform.Api.RestQuery
{
    /// <summary>
    ///     Фабрика роутинга контроллеров REST.
    /// </summary>
    public sealed class ControllerRoutingFactory
    {
        private const string AppServerAddressFormat = "{0}://{1}:{2}/{3}";
        private static ControllerRoutingFactory _instance;
        private static readonly object SyncObject = new object();
        private readonly HostingConfig _hostingConfig;

        public ControllerRoutingFactory()
            : this(null)
        {
        }

        public ControllerRoutingFactory(HostingConfig hostingConfig)
        {
            _hostingConfig = hostingConfig ?? HostingConfig.Default;
        }

        /// <summary>
        ///     Свойство добавлено для возможности явной инициализации в тестах.
        /// </summary>
        public static ControllerRoutingFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new ControllerRoutingFactory();
                        }
                    }
                }

                return _instance;
            }
            set { _instance = value; }
        }

        public string BuildRestRoutingUrlStandardApi(string configuration, string metadata,string action)
        {
            return BuildRestRoutingUrl(configuration, "StandardApi", metadata, action);
        }

        public string BuildRestRoutingUrlUpload(string configuration, string metadata, string action)
        {
            return BuildRestRoutingUrl(configuration, "Upload", metadata, action);
        }

        private string BuildRestRoutingUrl(string configuration, string controller, string metadata,string action)
        {
            return GetCustomRouting(GetRestTemplatePath()
                .ReplaceFormat("configuration", configuration)
                .ReplaceFormat("controller", controller)
                .ReplaceFormat("metadata", metadata)
                .ReplaceFormat("service", action));
        }

        public string BuildRestRoutingUrlUrlEncodedData(string configuration, string metadata,
            string action)
        {
            return BuildRestRoutingUrl(configuration, "UrlEncodedData", metadata, action);
        }

        public string GetRestTemplatePath()
        {
            return "{configuration}/{controller}/{metadata}/{service}";
        }

        public string GetCustomRouting(string relativePath)
        {
            return string.Format(AppServerAddressFormat,
                _hostingConfig.ServerScheme,
                _hostingConfig.ServerName,
                _hostingConfig.ServerPort,
                relativePath);
        }
    }


    public static class RoutingExtension
    {
        public static string ReplaceFormat(this string processingString, string oldString, string newString)
        {
            return processingString.Replace(string.Format("{0}", "{" + oldString + "}"), newString);
        }
    }
}