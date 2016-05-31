using System;
using System.Threading.Tasks;

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
        Task Consume(IMessage message);
    }
}