using System;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.Abstractions
{
    /// <summary>
    /// Обработчик событий процесса обработки сообщений.
    /// </summary>
    public interface IMessageConsumeEventHandler
    {
        /// <summary>
        /// Вызывается перед обработкой сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        Task OnBefore(IMessage message);

        /// <summary>
        /// Вызывается перед обработкой сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        Task OnAfter(IMessage message);

        /// <summary>
        /// Обрабатывает исключение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="error">Исключение.</param>
        Task<bool> OnError(IMessage message, Exception error);
    }
}