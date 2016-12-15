using System.Collections.Generic;

using InfinniPlatform.MessageQueue.Contract.Consumers;

namespace InfinniPlatform.MessageQueue.Contract
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