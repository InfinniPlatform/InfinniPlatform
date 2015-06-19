using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Контракт для связывания метаданных конфигурации и документов конфигурации в глобальном контексте
    /// </summary>
    public interface IConfigurationMediatorComponent
    {
        /// <summary>
        ///     Конструктор объектов конфигураций для скриптового доступа
        /// </summary>
        IConfigurationObjectBuilder ConfigurationBuilder { get; }

        /// <summary>
        ///     Объект конфигурации метаданных для скриптового доступа
        /// </summary>
        IConfigurationObject GetConfiguration(string version, string configurationId);
    }
}