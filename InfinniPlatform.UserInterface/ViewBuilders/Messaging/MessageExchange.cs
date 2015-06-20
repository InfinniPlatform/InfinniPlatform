using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
    /// <summary>
    ///     Точка обмена сообщениями.
    /// </summary>
    public sealed class MessageExchange : IMessageExchange
    {
        private readonly string _exchangeName;
        private readonly MessageQueue<MessageRequest> _messageQueue;
        private readonly SubscriptionList _subscriptions;

        public MessageExchange(string exchangeName, MessageQueue<MessageRequest> messageQueue)
        {
            if (exchangeName == null)
            {
                throw new ArgumentNullException("exchangeName");
            }

            if (messageQueue == null)
            {
                throw new ArgumentNullException("messageQueue");
            }

            _exchangeName = exchangeName;
            _messageQueue = messageQueue;
            _subscriptions = new SubscriptionList();
        }

        /// <summary>
        ///     Отправляет сообщение асинхронно.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="messageBody">Тело сообщения.</param>
        public void Send(string messageType, dynamic messageBody)
        {
            Task.WaitAll(SendAsync(messageType, messageBody));
        }

        /// <summary>
        ///     Отправляет сообщение.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Задача обработки сообщения.</returns>
        public Task SendAsync(string messageType, dynamic messageBody)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                throw new ArgumentNullException("messageType");
            }

            var messageRequest = new MessageRequest(_exchangeName, messageType, messageBody);

            _messageQueue.Enqueue(messageRequest);

            return messageRequest.RequestTask;
        }

        /// <summary>
        ///     Подписывает на сообщения.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="messageHandler">Обработчик сообщения.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Интерфейс для отписки от сообщения.</returns>
        public IDisposable Subscribe(string messageType, Action<dynamic> messageHandler)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                throw new ArgumentNullException("messageType");
            }

            if (messageHandler == null)
            {
                throw new ArgumentNullException("messageHandler");
            }

            var subscription = _subscriptions.AddSubscription(messageType, messageHandler);

            return subscription;
        }

        /// <summary>
        ///     Обрабатывает сообщение.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="messageBody">Тело сообщения.</param>
        public async Task HandleAsync(string messageType, dynamic messageBody)
        {
            var errors = new List<Exception>();
            var subscriptions = _subscriptions.GetSubscriptions(messageType);

            foreach (var subscription in subscriptions)
            {
                try
                {
                    await subscription.HandleAsync(messageBody);
                }
                catch (Exception error)
                {
                    errors.Add(error);
                }
            }

            if (errors.Count > 0)
            {
                throw new AggregateException(errors);
            }
        }
    }
}