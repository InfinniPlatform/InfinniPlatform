namespace InfinniPlatform.Modules
{
    /// <summary>
    ///     Контракт регистрации шаблонов обработчиков
    /// </summary>
    public interface ITemplateInstaller
    {
        /// <summary>
        ///     Зарегистрировать шаблоны в конфигурации
        /// </summary>
        void RegisterTemplates();
    }
}