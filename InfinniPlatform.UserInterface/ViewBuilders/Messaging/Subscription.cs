using System;
using System.Threading.Tasks;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
    /// <summary>
    ///     Подписка на обработку сообщения.
    /// </summary>
    internal sealed class Subscription : IDisposable
    {
        private readonly Action<dynamic> _messageHandler;
        private readonly Action _unsubscribeAction;

        public Subscription(Action unsubscribeAction, Action<dynamic> messageHandler)
        {
            _messageHandler = messageHandler;
            _unsubscribeAction = unsubscribeAction;
        }

        public void Dispose()
        {
            _unsubscribeAction();
        }

        /// <summary>
        ///     Обрабатывает сообщение.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <returns>Задача обработки.</returns>
        public Task HandleAsync(dynamic messageBody)
        {
            return Task.Run((Action) (() => _messageHandler(messageBody)));
        }
    }
}