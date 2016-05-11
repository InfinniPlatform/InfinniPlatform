using InfinniPlatform.Sdk.Queues.Outdated.Integration;

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