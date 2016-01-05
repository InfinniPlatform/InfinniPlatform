using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Core.Modules
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