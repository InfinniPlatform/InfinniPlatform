using InfinniPlatform.Sdk.Queues.Integration;

namespace InfinniPlatform.Core.Factories
{
    /// <summary>
    ///     Фабрика для создания интеграционной шины.
    /// </summary>
    public interface IIntegrationBusFactory
    {
        /// <summary>
        ///     Создать интеграционную шину.
        /// </summary>
        IIntegrationBus CreateIntegrationBus();
    }
}