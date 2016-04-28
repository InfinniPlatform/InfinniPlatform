using InfinniPlatform.Sdk.Queues.Integration;

namespace InfinniPlatform.Core.Factories
{
    /// <summary>
    ///     Фабрика для создания хранилища подписок интеграционной шины.
    /// </summary>
    public interface IIntegrationBusStorageFactory
    {
        /// <summary>
        ///     Создать хранилище подписок интеграционной шины.
        /// </summary>
        IIntegrationBusSubscriptionStorage CreateSubscriptionStorage();

        /// <summary>
        ///     Создать менеджер для хранилища подписок интеграционной шины.
        /// </summary>
        IIntegrationBusSubscriptionStorageManager CreateSubscriptionStorageManager();
    }
}