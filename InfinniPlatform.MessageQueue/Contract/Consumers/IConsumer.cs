using System;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.Contract.Consumers
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

        /// <summary>
        /// Обработчик ошибок.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        Task<bool> OnError(Exception exception);
    }
}