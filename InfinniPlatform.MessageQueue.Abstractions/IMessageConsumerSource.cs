using System.Collections.Generic;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.MessageQueue.Abstractions
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