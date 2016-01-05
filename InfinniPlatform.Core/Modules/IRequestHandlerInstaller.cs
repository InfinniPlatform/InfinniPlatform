namespace InfinniPlatform.Core.Modules
{
    /// <summary>
    /// Контракт регистрации шаблонов обработчиков
    /// </summary>
    public interface IRequestHandlerInstaller
    {
        /// <summary>
        /// Зарегистрировать шаблоны в конфигурации
        /// </summary>
        void RegisterTemplates();
    }
}