using System;

namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Потребитель сообщений.
    /// </summary>
    public interface IConsumer
    {
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