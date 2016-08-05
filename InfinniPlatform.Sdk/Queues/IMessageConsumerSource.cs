using System.Collections.Generic;

using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Источник потребителей сообщений.
    /// </summary>
    public interface IMessageConsumerSource
    {
        /// <summary>
        /// Возвращает перечисление потребителей сообщений из очереди.
        /// </summary>
        IEnumerable<IConsumer> GetConsumers();
    }
}