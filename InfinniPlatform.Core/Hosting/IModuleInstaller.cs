using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    /// Модуль установки сервисов конфигурации
    /// </summary>
    public interface IModuleInstaller
    {
        /// <summary>
        /// Установить модуль приложения
        /// </summary>
        IModule InstallModule();
    }
}