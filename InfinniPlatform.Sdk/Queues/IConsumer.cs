using System;

namespace InfinniPlatform.Sdk.Queues
{
    public interface IConsumer
    {
        /// <summary>
        /// Имя очереди.
        /// </summary>
        string QueueName { get; }

        /// <summary>
        /// Тип тела сообщения.
        /// </summary>
        Type MessageType { get; }

        /// <summary>
        /// Обработчик сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        void Consume(IMessage message);
    }
}