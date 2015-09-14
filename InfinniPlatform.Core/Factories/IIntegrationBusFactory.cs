using InfinniPlatform.MessageQueue.Integration;

namespace InfinniPlatform.Factories
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