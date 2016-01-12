using InfinniPlatform.Core.Hosting;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    ///     Интерфейс регистрации модуля конфигурации
    /// </summary>
    public interface IModule
    {
        /// <summary>
        ///     Контейнер регистрации сервисов конфигурации
        /// </summary>
        IServiceRegistrationContainer ServiceRegistrationContainer { get; }

        /// <summary>
        ///     Контейнер шаблонов обработчиков для модуля
        /// </summary>
        IServiceTemplateConfiguration ServiceTemplateConfiguration { get; }

        /// <summary>
        ///     Версия конфигурации метаданных
        /// </summary>
        string Version { get; set; }
    }
}