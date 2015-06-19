using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент, используемый для работы с зависимостями системного уровня
    ///     (Используется только в системных конфигурациях)
    /// </summary>
    public sealed class SystemComponent : ISystemComponent
    {
        /// <summary>
        ///     Менеджер для чтения конфигураций JSON
        /// </summary>
        public IJsonConfigReader ConfigurationReader { get; set; }

        /// <summary>
        ///     Менеджер для управления идентификаторами метаданных
        /// </summary>
        public IManagerIdentifiers ManagerIdentifiers { get; set; }
    }
}