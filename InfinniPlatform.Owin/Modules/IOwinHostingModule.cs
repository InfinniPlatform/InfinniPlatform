using InfinniPlatform.Sdk.Logging;

using Owin;

namespace InfinniPlatform.Owin.Modules
{
    /// <summary>
    /// Модуль хостинга на базе OWIN.
    /// </summary>
    public interface IOwinHostingModule
    {
        /// <summary>
        /// Возвращает тип модуля хостинга.
        /// </summary>
        OwinHostingModuleType ModuleType { get; }

        /// <summary>
        /// Настраивает модуль хостинга.
        /// </summary>
        /// <param name="builder">Объект для регистрации обработчиков запросов OWIN.</param>
        /// <param name="context">Контекст подсистемы хостинга на базе OWIN.</param>
        /// <param name="log">Лог.</param>
        void Configure(IAppBuilder builder, IOwinHostingContext context, ILog log);
    }
}