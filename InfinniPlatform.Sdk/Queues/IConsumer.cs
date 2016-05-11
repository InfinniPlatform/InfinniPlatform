using System;

namespace InfinniPlatform.Sdk.Queues
{
    public interface IConsumer
    {
        string QueueName { get; }

        Type MessageType { get; }

        /// <summary>
        /// Обработчик сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        void Consume(IMessage message);
    }
}