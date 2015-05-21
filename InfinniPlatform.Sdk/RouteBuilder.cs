
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
        ///   Сформировать роутинг запроса для запросов на загрузку файла на сервер
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="application">Приложение</param>
        /// <returns>Роутинг для запросов на загрузку файлов</returns>
        public string BuildRestRoutingUploadFile(string version, string application)
        {
            return GetCompleteUrl(GetFileUploadPath()
                .ReplaceFormat("version", version)
                .ReplaceFormat("application", application));
        }

        public string BuildRestRoutingDownloadFile(string version, string application)
        {
            return GetCompleteUrl(GetFileDownloadPath()
                .ReplaceFormat("version", version)
                .ReplaceFormat("application", application));
        }


        private string GetFileUploadPath()
        {
            return GetBaseApplicationPath() + "/files/upload";
        }

        private string GetFileDownloadPath()
        {
            return GetBaseApplicationPath() + "/files/download";
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
            return GetCompleteUrl(GetRestTemplate()
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
            return GetCompleteUrl(GetRestTemplateDocument()
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
            return GetCompleteUrl(GetRestTemplateDocumentById()
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
            return GetCompleteUrl(GetRestTemplateVersionBase().ReplaceFormat("version", version));
        }


        /// <summary>
        ///  Сформировать роутинг запроса для получения документов клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="version">Версия приложения</param>
        /// <returns>Роутинг для запроса на создание клиентской сессии</returns>
        public string BuildRestRoutingUrlDefaultSessionById(string sessionId, string version)
        {
            return GetCompleteUrl(GetRestTemplateSessionById()
                .ReplaceFormat("version", version)
                .ReplaceFormat("sessionId", sessionId)
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
            return GetCompleteUrl(GetRestTemplateSessionById()
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
            return GetCompleteUrl(GetRestTemplateSessionDocument()
                .ReplaceFormat("version", version)
                .ReplaceFormat("sessionId", sessionId)
                .ReplaceFormat("attachmentId", attachmentId)
                );
        }

        /// <summary>
        /// Сформировать роутинг для входа пользователя в систему
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <returns>Роутинг для запроса на вход пользователя в систему</returns>
        public string BuildRestRoutingUrlSignIn(string version)
        {
            return GetCompleteUrl(GetRestTemplateSignIn().ReplaceFormat("version", version));
        }

        /// <summary>
        ///  Сформировать роутинг для выхода пользователя из системы
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <returns>Роутинг для запроса на выход пользователя из системы</returns>
        public string BuildRestRoutingUrlSignOut(string version)
        {
            return GetCompleteUrl(GetRestTemplateSignOut().ReplaceFormat("version", version));
        }

        /// <summary>
        ///  Сформировать роутинг для смены пароля пользователя
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <returns>Роутинг для запроса смены пароля пользователя</returns>
        public string BuildRestRoutingChangePassword(string version)
        {
            return GetCompleteUrl(GetRestTemplateChangePassword().ReplaceFormat("version", version));
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
        ///   Шаблон роутинга для указанной версии сервисов
        /// </summary>
        /// <returns>Шаблон роутинга</returns>
        private string GetRestTemplateVersionBase()
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
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateDocument()
        {
            return GetBaseApplicationPath() + "/{documentType}";
        }

        /// <summary>
        ///   Получить шаблон запроса для работы со стандартным сервисом документов
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateDocumentById()
        {
            return GetRestTemplateDocument() + "/{instanceId}";
        }

        /// <summary>
        ///   Получить шаблон роутинга для сервиса входа пользователя
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateSignIn()
        {
            return GetRestTemplateVersionBase() + "/signin";
        }

        /// <summary>
        ///   Получить шаблон роутинга для сервиса выхода пользователя
        /// </summary>
        /// <returns>Шаблон роутинга пользователя</returns>
        private string GetRestTemplateSignOut()
        {
            return GetRestTemplateVersionBase() + "/signout";
        }

        /// <summary>
        ///   Получить шаблон роутинга для смены пароля пользователя
        /// </summary>
        /// <returns>Шаблон роутинга пользователя</returns>
        private string GetRestTemplateChangePassword()
        {
            return GetRestTemplateVersionBase() + "/changepassword";
        }

        /// <summary>
        ///   Сформировать роутинг для кастомного запроса REST
        /// </summary>
        /// <param name="relativePath">Сформированный относительный роутинг запроса</param>
        /// <returns>Относительный роутинг запроса</returns>
        public string GetCompleteUrl(string relativePath)
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
