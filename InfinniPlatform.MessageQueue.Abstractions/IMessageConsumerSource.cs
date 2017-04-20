using System.Collections.Generic;

namespace InfinniPlatform.MessageQueue
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