namespace InfinniPlatform.Core.Hosting
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