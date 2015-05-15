
namespace InfinniPlatform.Sdk
{
    /// <summary>
    ///   Конструктор роутинга запросов к сервисам infinniPlatform
    /// </summary>
    public sealed class RouteBuilder
    {
        private readonly string _serverName;
        private readonly string _serverPort;


        //Шаблон строки формирования адреса сервера
        private const string AppServerAddressFormat = "http://{0}:{1}/{2}";

        public RouteBuilder(string serverName, string serverPort)
        {
            _serverName = serverName;
            _serverPort = serverPort;
        }

        /// <summary>
        ///   Сформировать роутинг запроса для кастомных запросов к платформе
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="service">Действие</param>
        /// <returns>Роутинг для кастомных запросов к платформе</returns>
        public string BuildRestRoutingUrl(string version, string application, string documentType, string service)
        {
            return GetCustomRouting(GetRestTemplatePath()
                                        .ReplaceFormat("version", version)
                                        .ReplaceFormat("application", application)
                                        .ReplaceFormat("documentType", documentType)
                                        .ReplaceFormat("service", service));
        }

        /// <summary>
        ///   Сформировать роутинг запроса для стандартного запроса документа указанного типа
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <returns>Роутинг для стандартного запроса документа указанного типа</returns>
        public string BuildRestRoutingUrlDefault(string version, string application, string documentType)
        {
            return GetCustomRouting(GetRestTemplateDocumentPath()
                                        .ReplaceFormat("version", version)
                                        .ReplaceFormat("application", application)
                                        .ReplaceFormat("documentType", documentType));
        }


        /// <summary>
        ///   Сформировать роутинг запроса для синхронного Post документов указанного типа
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <returns>Роутинг запроса для синхронного Post документов указанного типа</returns>
        public string BuildRestRoutingUrlEncodedData(string version, string application, string documentType)
        {
            return BuildRestRoutingUrl(version, application, documentType, "UrlEncodedData");
        }

        /// <summary>
        ///   Сформировать роутинг запроса для синхронной загрузки/выгрузки бинарных данных
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <returns>Роутинг запроса для загрузки/выгрузки данных указанного типа</returns>
        public string BuildRestRoutingUrlUploadData(string version, string application, string documentType)
        {
            return BuildRestRoutingUrl(version, application, documentType, "Upload");
        }

        private string GetBaseApplicationPath()
        {
            return "{version}/{application}";
        }

        private string GetRestTemplatePath()
        {
            return GetBaseApplicationPath() + "/{documentType}/{service}";
        }

        private string GetRestTemplateDocumentPath()
        {
            return GetBaseApplicationPath() + "/{documentType}";
        }

        /// <summary>
        ///   Сформировать роутинг для кастомного запроса REST
        /// </summary>
        /// <param name="relativePath">Сформированный относительный роутинг запроса</param>
        /// <returns>Относительный роутинг запроса</returns>
        public string GetCustomRouting(string relativePath)
        {
            return string.Format(AppServerAddressFormat,
                                 _serverName,
                                 _serverPort,
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
