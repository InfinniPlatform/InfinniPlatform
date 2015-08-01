
namespace InfinniPlatform.Sdk
{
    /// <summary>
    ///   Конструктор роутинга запросов к сервисам infinniPlatform
    /// </summary>
    public sealed class RouteBuilder
    {
        private readonly string _serverName;
        private readonly string _serverPort;
        private readonly string _route;


        //Шаблон строки формирования адреса сервера
        private const string AppServerAddressFormat = "http://{0}:{1}/{2}/{3}";

        public RouteBuilder(string serverName, string serverPort, string route)
        {
            _serverName = serverName;
            _serverPort = serverPort;
            _route = route;
        }

        /// <summary>
        ///   Сформировать роутинг запроса для запросов на загрузку файла на сервер
        /// </summary>
        /// <param name="application">Приложение</param>
        /// <returns>Роутинг для запросов на загрузку файлов</returns>
        public string BuildRestRoutingUploadFile(string application)
        {
            return GetCompleteUrl(GetFileUploadPath()
                .ReplaceFormat("application", application));
        }

        public string BuildRestRoutingDownloadFile(string application)
        {
            return GetCompleteUrl(GetFileDownloadPath()
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
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="service">Действие</param>
        /// <returns>Роутинг для кастомных запросов к платформе</returns>
        public string BuildRestRoutingUrl(string application, string documentType, string service)
        {
            return GetCompleteUrl(GetRestTemplate()
                                        .ReplaceFormat("application", application)
                                        .ReplaceFormat("documentType", documentType)
                                        .ReplaceFormat("service", service));
        }

        /// <summary>
        ///   Сформировать роутинг запроса для работы с метаданными решений
        /// </summary>
        /// <returns></returns>
        public string BuildRestRoutingUrlMetadataSolution()
        {
            return GetCompleteUrl(GetRestTemplateStandard()
                                      .ReplaceFormat("application", "metadata")
                                      .ReplaceFormat("documentType", "solution"));
        }

        /// <summary>
        ///  Сформировать роутинг запроса для работы с метаданными по идентификатору 
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="id">Идентификатор приложения</param>
        /// <returns></returns>
        public string BuildRestRoutingUrlMetadataSolutionById(string version, string id)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedVersionAndInstanceId()
                                      .ReplaceFormat("application", "metadata")
                                      .ReplaceFormat("documentType", "solution")
                                      .ReplaceFormat("version", version)
                                      .ReplaceFormat("instanceId",id)
                                      );            
        }

        /// <summary>
        ///   Сформировать роутинг запроса для работы с метаданными конфигурации
        /// </summary>
        /// <returns></returns>
        public string BuildRestRoutingUrlMetadataConfig()
        {
            return GetCompleteUrl(GetRestTemplateStandard()
                                      .ReplaceFormat("application", "metadata")
                                      .ReplaceFormat("documentType", "configuration"));
        }

        /// <summary>
        ///  Сформировать роутинг запроса для работы с метаданными по идентификатору 
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="name">Наименование приложения</param>
        /// <returns></returns>
        public string BuildRestRoutingUrlMetadataConfigById(string version, string name)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedVersionAndInstanceId()
                                      .ReplaceFormat("application", "metadata")
                                      .ReplaceFormat("documentType", "configuration")
                                      .ReplaceFormat("version", version)
                                      .ReplaceFormat("instanceId", name)
                                      );
        }

        /// <summary>
        ///   Сформировать роутинг запроса для работы с метаданными меню
        /// </summary>
        /// <returns></returns>
        public string BuildRestRoutingUrlMetadataElement(string version, string configuration, string metadataType)
        {
            return GetCompleteUrl(GetRestTemplateConfigMetadataElement()
                                      .ReplaceFormat("application", "metadata")
                                      .ReplaceFormat("version", version)
                                      .ReplaceFormat("configuration", configuration)
                                      .ReplaceFormat("metadataType", metadataType)                                      
                                      );        
        }

        /// <summary>
        ///  Сформировать роутинг запроса для работы с метаданными по идентификатору 
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="document">Документ</param>
        /// <param name="id">Идентификатор приложения</param>
        /// <returns></returns>
        public string BuildRestRoutingUrlDocumentMetadataElementById(string version, string configuration, string document, string metadataType, string id)
        {
            return GetCompleteUrl(GetRestTemplateDocumentMetadataElementSpecifiedVersionAndInstanceId()
                                      .ReplaceFormat("application", "metadata")
                                      .ReplaceFormat("version", version)
                                      .ReplaceFormat("configuration", configuration)      
                                      .ReplaceFormat("document",document)
                                      .ReplaceFormat("metadataType", metadataType)
                                      .ReplaceFormat("instanceId", id)
                                      );
        }

        /// <summary>
        ///   Сформировать роутинг запроса для работы с метаданными меню
        /// </summary>
        /// <returns></returns>
        public string BuildRestRoutingUrlDocumentMetadataElement(string version, string configuration, string document, string metadataType)
        {
            return GetCompleteUrl(GetRestTemplateDocumentMetadataElement()
                                      .ReplaceFormat("application", "metadata")
                                      .ReplaceFormat("version", version)
                                      .ReplaceFormat("configuration", configuration)
                                      .ReplaceFormat("document", document)
                                      .ReplaceFormat("metadataType", metadataType)
                                      );
        }

        /// <summary>
        ///  Сформировать роутинг запроса для работы с метаданными по идентификатору 
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="id">Идентификатор приложения</param>
        /// <returns></returns>
        public string BuildRestRoutingUrlMetadataElementById(string version, string configuration, string metadataType, string id)
        {
            return GetCompleteUrl(GetRestTemplateConfigMetadataElementSpecifiedVersionAndInstanceId()
                                      .ReplaceFormat("application", "metadata")
                                      .ReplaceFormat("version", version)
                                      .ReplaceFormat("configuration", configuration)
                                      .ReplaceFormat("metadataType", metadataType)
                                      .ReplaceFormat("instanceId", id)
                                      );
        }

        /// <summary>
        ///   Сформировать роутинг запроса для стандартного запроса документа указанного типа
        /// </summary>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <returns>Роутинг для стандартного запроса документа указанного типа</returns>
        public string BuildRestRoutingUrlDefault(string application, string documentType)
        {
            return GetCompleteUrl(GetRestTemplateStandard()
                                        .ReplaceFormat("application", application)
                                        .ReplaceFormat("documentType", documentType));
        }

        /// <summary>
        ///   Сформировать роутинг запроса для стандартного запроса документа указанного типа с указанным идентификатором
        /// </summary>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <returns>Роутинг для стандартного запроса документа указанного типа</returns>
        public string BuildRestRoutingUrlDefaultById(string application, string documentType, string instanceId)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedInstanceId()
                                        .ReplaceFormat("application", application)
                                        .ReplaceFormat("documentType", documentType)
                                        .ReplaceFormat("instanceId", instanceId)
                                        );
        }


        /// <summary>
        ///  Сформировать роутинг запроса для создания новой клиентской сессии
        /// </summary>
        /// <returns>Роутинг для запроса на создание клиентской сессии</returns>
        public string BuildRestRoutingUrlDefaultSession()
        {
            return GetCompleteUrl(GetRestTemplateVersionBase());
        }


        /// <summary>
        ///  Сформировать роутинг запроса для получения документов клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <returns>Роутинг для запроса на создание клиентской сессии</returns>
        public string BuildRestRoutingUrlDefaultSessionById(string sessionId)
        {
            return GetCompleteUrl(GetRestTemplateSessionById()
                .ReplaceFormat("sessionId", sessionId)
                );
        }


        /// <summary>
        ///  Сформировать роутинг запроса для присоединения документов к клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <returns>Роутинг для запроса на присоединение документа к клиентской сессии</returns>
        public string BuildRestRoutingUrlAttachDocument(string sessionId)
        {
            return GetCompleteUrl(GetRestTemplateSessionById()
                .ReplaceFormat("sessionId", sessionId)
                );
        }

        /// <summary>
        ///  Сформировать роутинг запроса для отсоединения документов от клиентской сессии
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="attachmentId">Идентификатор присоединенного документа</param>
        /// <returns>Роутинг для запроса на отсоединение документа к клиентской сессии</returns>
        public string BuildRestRoutingUrlDetachDocument(string sessionId, string attachmentId)
        {
            return GetCompleteUrl(GetRestTemplateSessionDocument()
                .ReplaceFormat("sessionId", sessionId)
                .ReplaceFormat("attachmentId", attachmentId)
                );
        }

        /// <summary>
        /// Сформировать роутинг для входа пользователя в систему
        /// </summary>
        /// <returns>Роутинг для запроса на вход пользователя в систему</returns>
        public string BuildRestRoutingUrlSignIn()
        {
            return GetCompleteUrl(GetRestTemplateSignIn());
        }

        /// <summary>
        ///  Сформировать роутинг для выхода пользователя из системы
        /// </summary>
        /// <returns>Роутинг для запроса на выход пользователя из системы</returns>
        public string BuildRestRoutingUrlSignOut()
        {
            return GetCompleteUrl(GetRestTemplateSignOut());
        }

        /// <summary>
        ///  Сформировать роутинг для смены пароля пользователя
        /// </summary>
        /// <returns>Роутинг для запроса смены пароля пользователя</returns>
        public string BuildRestRoutingChangePassword()
        {
            return GetCompleteUrl(GetRestTemplateChangePassword());
        }


        /// <summary>
        ///   Сформировать роутинг для предоставления прав пользователю
        /// </summary>
        /// <returns>Роутинг запроса</returns>
        public string BuildRestRoutingUrlGrantAccess()
        {
            return GetCompleteUrl(GetRestTemplate()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "User")
                .ReplaceFormat("service", "GrantAccess"));
        }

        /// <summary>
        ///   Сформировать роутинг для установки запрета прав
        /// </summary>
        /// <returns>Роутинг запроса</returns>
        public string BuildRestRoutingUrlDenyAccess()
        {
            return GetCompleteUrl(GetRestTemplate()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "User")
                .ReplaceFormat("service", "DenyAccess"));
        }

        /// <summary>
        ///  Сформировать роутинг для добавления пользователя
        /// </summary>
        /// <returns>Строка роутинга запросоа для добавления нового пользователя</returns>
        public string BuildRestRoutingUrlAddUser()
        {
            return GetCompleteUrl(GetRestTemplateStandard()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "User"));
        }

        /// <summary>
        ///   Сформировать роутинг для обращения к указанному пользователю
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Строка роутинга запроса для получения существующего пользователя</returns>
        public string BuildRestRoutingToSpecifiedUserRoles(string userName)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedUserName()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "User")
                .ReplaceFormat("userName", userName)
                );
        }

        /// <summary>
        ///   Сформировать роутинг для обращения к указанному пользователю
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="roleName">Роль пользователя</param>
        /// <returns>Строка роутинга запроса для получения существующего пользователя</returns>
        public string BuildRestRoutingToSpecifiedUserRole(string userName, string roleName)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedUserRole()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "User")
                .ReplaceFormat("userName", userName)
                .ReplaceFormat("roleName", roleName)
                );
        }

        /// <summary>
        ///  Сформировать роутинг для обращения (GET,PUT, DELETE)  к указанному утверждению (Claim)
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип утверждения</param>
        /// <returns>Роутинг запроса</returns>
        public string BuildRestRoutingToSpecifiedUserClaim(string userName, string claimType)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedUserClaim()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "User")
                .ReplaceFormat("userName", userName)
                .ReplaceFormat("claimType", claimType));
        }

        /// <summary>
        ///   Сформировать роутинг для обращения к указанному пользователю
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Строка роутинга запроса для получения существующего пользователя</returns>
        public string BuildRestRoutingToSpecifiedUser(string userName)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedUserName()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "User")
                .ReplaceFormat("userName", userName)
                );
        }

        /// <summary>
        ///   Сформировать роутинг для удаления роли
        /// </summary>
        /// <param name="roleName">Ключ удаляемой роли</param>
        /// <returns>Роутинг запроса</returns>
        public string BuildRestRoutingToSpecifiedRole(string roleName)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedRoleName()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "Role")
                .ReplaceFormat("roleName", roleName)
                );
        }

        /// <summary>
        ///   Сформировать роутинг для работы с версиями конфигураций
        /// </summary>
        /// <returns>Роутинг запроса</returns>
        public string BuildRestRoutingUrlVersion(string userName)
        {
            return GetCompleteUrl(GetRestTemplateStandardSpecifiedUserName()
                .ReplaceFormat("application", "Administration")
                .ReplaceFormat("documentType", "Version")
                .ReplaceFormat("userName", userName)
                );
        }

        /// <summary>
        ///   Шаблон базового роутинга для обращения к конкретному приложению
        /// </summary>
        /// <returns>Базовая часть шаблона строки роутинга</returns>
        private string GetBaseApplicationPath()
        {
            return "{application}";
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
            return "";
        }


        /// <summary>
        ///   Шаблон роутинга для получения клиентской сессии для указанной версии сервисов
        /// </summary>
        /// <returns>Шаблон роутинга</returns>
        private string GetRestTemplateSessionById()
        {
            return "{sessionId}";
        }

        /// <summary>
        ///   Шаблон роутинга для обращения к документу внутри клиентской сессии
        /// </summary>
        /// <returns>Шаблон роутинга</returns>
        private string GetRestTemplateSessionDocument()
        {
            return "{sessionId}/{attachmentId}";
        }

        /// <summary>
        ///   Получить шаблон стандартного запроса с действием по умолчанию
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateStandard()
        {
            return GetBaseApplicationPath() + "/{documentType}";
        }

        /// <summary>
        ///   Получить шаблон стандартного запроса с действием по умолчанию с указанием имени пользователя
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateStandardSpecifiedUserName()
        {
            return GetRestTemplateStandard() + "/{userName}";
        }

        /// <summary>
        ///Получить шаблон стандартного запроса с действием по умолчанию для указанного идентификатора сущности
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateStandardSpecifiedInstanceId()
        {
            return GetRestTemplateStandard() + "/{instanceId}";
        }

        /// <summary>
        ///Получить шаблон стандартного запроса с действием по умолчанию для указанного идентификатора сущности
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateStandardSpecifiedVersionAndInstanceId()
        {
            return GetRestTemplateStandard() + "/{version}/{instanceId}";
        }

        /// <summary>
        ///Получить шаблон роутинга для доступа к метаданным элементов конфигурации указанного типа
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateConfigMetadataElement()
        {
            return GetBaseApplicationPath() + "/{version}/{configuration}/{metadataType}";
        }

        /// <summary>
        ///Получить шаблон роутинга для доступа к метаданным указанного типа для документа
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateDocumentMetadataElement()
        {
            return GetBaseApplicationPath() + "/{version}/{configuration}/{document}/{metadataType}";
        }

        /// <summary>
        ///Получить шаблон роутинга для доступа к метаданным элементов конфигурации с указанием версии и идентификатора
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateConfigMetadataElementSpecifiedVersionAndInstanceId()
        {
            return GetBaseApplicationPath() + "/{version}/{configuration}/{metadataType}/{instanceId}";
        }

        /// <summary>
        ///Получить шаблон роутинга для доступа к метаданным документа с указанием версии и идентификатора
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateDocumentMetadataElementSpecifiedVersionAndInstanceId()
        {
            return GetBaseApplicationPath() + "/{version}/{configuration}/{document}/{metadataType}/{instanceId}";
        }

        /// <summary>
        ///   Получить шаблона стандартного запроса с действием по умолчанию с указанием роли пользователя
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateStandardSpecifiedRoleName()
        {
            return GetRestTemplateStandard() + "/{roleName}";
        }

        /// <summary>
        ///  Получить шаблон стандартного запроса с действием по умолчанию с указанием имени пользователя и наименования роли
        /// </summary>
        /// <returns>Шаблон роутинга роли</returns>
        private string GetRestTemplateStandardSpecifiedUserRole()
        {
            return GetRestTemplateStandardSpecifiedUserName() + "/Roles/{roleName}";
        }

        /// <summary>
        ///   Получить шаблон стандартного запроса с действием по умолчанию с указанием типа утверждения
        /// </summary>
        /// <returns>Шаблон роутинга запроса</returns>
        private string GetRestTemplateStandardSpecifiedUserClaim()
        {
            return GetRestTemplateStandardSpecifiedUserName() + "/Claims/{claimType}";
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
                                 _route,
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
