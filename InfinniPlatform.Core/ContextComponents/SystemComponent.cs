using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Core.ContextComponents
{
    /// <summary>
    /// Компонент, используемый для работы с зависимостями системного уровня
    /// (Используется только в системных конфигурациях)
    /// </summary>
    public sealed class SystemComponent : ISystemComponent
    {
        /// <summary>
        /// Менеджер для чтения конфигураций JSON
        /// </summary>
        public IJsonConfigReader ConfigurationReader { get; set; }

        /// <summary>
        /// Менеджер для управления идентификаторами метаданных
        /// </summary>
        public IManagerIdentifiers ManagerIdentifiers { get; set; }
    }
}