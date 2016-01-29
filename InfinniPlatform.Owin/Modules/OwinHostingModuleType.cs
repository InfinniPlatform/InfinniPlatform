namespace InfinniPlatform.Owin.Modules
{
    /// <summary>
    /// Логические типы модулей хостинга на базе OWIN.
    /// </summary>
    /// <remarks>От типа зависит в каком порядке модуль будет зарегистрирован в конвейере обработки запросов.</remarks>
    public enum OwinHostingModuleType
    {
        /// <summary>
        /// Уровень разрешения зависимостей.
        /// </summary>
        IoC = 0,

        /// <summary>
        /// Уровень обработки ошибок выполнения запросов.
        /// </summary>
        ErrorHandling = 1,

        /// <summary>
        /// Уровень обработки Cross-origin resource sharing.
        /// </summary>
        Cors = 2,

        /// <summary>
        /// Уровень обработки аутентификации через Cookie.
        /// </summary>
        CookieAuth = 4,

        /// <summary>
        /// Уровень обработки аутентификации через внешний провайдер.
        /// </summary>
        ExternalAuth = 8,

        /// <summary>
        /// Уровень обработки аутентификации через внутренний провайдер.
        /// </summary>
        InternalAuth = 16,

        /// <summary>
        /// Уровень обработки запросов ASP.NET SignalR.
        /// </summary>
        AspNetSignalR = 32,

        /// <summary>
        /// Уровень обработки прикладных запросов.
        /// </summary>
        Application = 64
    }
}