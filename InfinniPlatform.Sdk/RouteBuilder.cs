
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
            return GetCustomRouting(GetRestTemplate()
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
            return GetCustomRouting(GetRestTemplateDocument()
                                        .ReplaceFormat("version", version)
                                        .ReplaceFormat("application", application)
                                        .ReplaceFormat("documentType", documentType));
        }

        /// <summary>
        ///   Сформировать роутинг запроса для стандартного запроса документа указанного типа с указанным идентификатором
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <returns>Роутинг для стандартного запроса документа указанного типа</returns>
        public string BuildRestRoutingUrlDefaultById(string version, string application, string documentType, string instanceId)
        {
            return GetCustomRouting(GetRestTemplateDocumentById()
                                        .ReplaceFormat("version", version)
                                        .ReplaceFormat("application", application)
                                        .ReplaceFormat("documentType", documentType)
                                        .ReplaceFormat("instanceId", instanceId)
                                        );
        }


        /// <summary>
        ///  Сформировать роутинг запроса для создания новой клиентской сессии
        /// </summary>
        /// <param name="version">Версия платформы</param>
        /// <returns>Роутинг для запроса на создание клиентской сессии</returns>
        public string BuildRestRoutingUrlDefaultSession(string version)
        {
            return GetCustomRouting(GetRestTemplateSession().ReplaceFormat("version",version));
        }


        /// <summary>
        ///  Сформировать роутинг запроса для получения документов клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="version">Версия приложения</param>
        /// <returns>Роутинг для запроса на создание клиентской сессии</returns>
        public string BuildRestRoutingUrlDefaultSessionById(string sessionId, string version)
        {
            return GetCustomRouting(GetRestTemplateSessionById()
                .ReplaceFormat("version", version)
                .ReplaceFormat("sessionId",sessionId)
                );
        }


        /// <summary>
        ///  Сформировать роутинг запроса для присоединения документов к клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="version">Версия приложения</param>
        /// <returns>Роутинг для запроса на присоединение документа к клиентской сессии</returns>
        public string BuildRestRoutingUrlAttachDocument(string sessionId, string version)
        {
            return GetCustomRouting(GetRestTemplateSessionById()
                .ReplaceFormat("version", version)
                .ReplaceFormat("sessionId", sessionId)
                );
        }

        /// <summary>
        ///  Сформировать роутинг запроса для отсоединения документов от клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="version">Версия приложения</param>
        /// <param name="attachmentId">Идентификатор присоединенного документа</param>
        /// <returns>Роутинг для запроса на отсоединение документа к клиентской сессии</returns>
        public string BuildRestRoutingUrlDetachDocument(string sessionId, string version, string attachmentId)
        {
            return GetCustomRouting(GetRestTemplateSessionDocument()
                .ReplaceFormat("version", version)
                .ReplaceFormat("sessionId", sessionId)
                .ReplaceFormat("attachmentId", attachmentId)
                );
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

        /// <summary>
        ///   Шаблон базового роутинга для обращения к конкретному приложению
        /// </summary>
        /// <returns>Базовая часть шаблона строки роутинга</returns>
        private string GetBaseApplicationPath()
        {
            return "{version}/{application}";
        }

        /// <summary>
        ///   Шаблон роутинга для обращения к стандартному сервису платформы
        /// </summary>
        /// <returns>Шаблон роутинга</returns>
        private string GetRestTemplate()
        {
            return GetBaseApplicationPath() + "/{documentType}/{service}";
        }

        /// <summary>
        ///   Шаблон роутинга для клиентской сессии для указанной версии сервисов
        /// </summary>
        /// <returns>Шаблон роутинга</returns>
        private string GetRestTemplateSession()
        {
            return "{version}";
        }


        /// <summary>
        ///   Шаблон роутинга для получения клиентской сессии для указанной версии сервисов
        /// </summary>
        /// <returns>Шаблон роутинга</returns>
        private string GetRestTemplateSessionById()
        {
            return "{version}/{sessionId}";
        }

        /// <summary>
        ///   Шаблон роутинга для обращения к документу внутри клиентской сессии
        /// </summary>
        /// <returns>Шаблон роутинга</returns>
        private string GetRestTemplateSessionDocument()
        {
            return "{version}/{sessionId}/{attachmentId}";
        }

        /// <summary>
        ///   Получить шаблон запроса для работы со стандартным сервисом документов
        /// </summary>
        /// <returns></returns>
        private string GetRestTemplateDocument()
        {
            return GetBaseApplicationPath() + "/{documentType}";
        }

        /// <summary>
        ///   Получить шаблон запроса для работы со стандартным сервисом документов
        /// </summary>
        /// <returns></returns>
        private string GetRestTemplateDocumentById()
        {
            return GetRestTemplateDocument() + "/{instanceId}";
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

    /// <summary>
    ///   Методы расширения для работы с шаблонами запросов
    /// </summary>
    public static class RoutingExtension
    {
        /// <summary>
        ///   Заполнить шаблон роутинга запросов
        /// </summary>
        /// <param name="processingString">Обрабатываемый шаблон</param>
        /// <param name="oldString">Заменяемый объект</param>
        /// <param name="newString">Новый объект</param>
        /// <returns>Форматрированная строка</returns>
        public static string ReplaceFormat(this string processingString, string oldString, string newString)
        {
            return processingString.Replace(string.Format("{0}", "{" + oldString + "}"), newString);
        }
    }
}
