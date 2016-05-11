using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Outdated;

namespace InfinniPlatform.Core.Factories
{
    /// <summary>
    ///     Фабрика для создания инфраструктурных сервисов очереди сообщений.
    /// </summary>
    public interface IMessageQueueFactory
    {
        /// <summary>
        ///     Создать сервис для управления рабочими потоками очередей сообщений.
        /// </summary>
        IMessageQueueListener CreateMessageQueueListener();

        /// <summary>
        ///     Создать сервис для управления подписками на очереди сообщений.
        /// </summary>
        IMessageQueueManager CreateMessageQueueManager();

        /// <summary>
        ///     Создать сервис для публикации сообщений.
        /// </summary>
        IMessageQueuePublisher CreateMessageQueuePublisher();
    }
}