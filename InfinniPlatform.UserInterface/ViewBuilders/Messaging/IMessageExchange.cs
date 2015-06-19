using System;
using System.Threading.Tasks;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
    /// <summary>
    ///     Точка обмена сообщениями.
    /// </summary>
    public interface IMessageExchange
    {
        /// <summary>
        ///     Отправляет сообщение асинхронно.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="messageBody">Тело сообщения.</param>
        void Send(string messageType, dynamic messageBody);

        /// <summary>
        ///     Отправляет сообщение асинхронно.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <returns>Задача обработки сообщения.</returns>
        Task SendAsync(string messageType, dynamic messageBody);

        /// <summary>
        ///     Подписывает на сообщения.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="messageHandler">Обработчик сообщения.</param>
        /// <returns>Интерфейс для отписки от сообщения.</returns>
        IDisposable Subscribe(string messageType, Action<dynamic> messageHandler);
    }
}