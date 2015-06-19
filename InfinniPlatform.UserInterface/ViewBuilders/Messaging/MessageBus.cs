using System;
using System.Collections.Concurrent;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
    /// <summary>
    ///     Шина сообщений.
    /// </summary>
    public sealed class MessageBus : IMessageBus
    {
        private readonly ConcurrentDictionary<string, MessageExchange> _messageExchanges;
        private readonly MessageQueue<MessageRequest> _messageQueue;

        public MessageBus()
        {
            _messageQueue = new MessageQueue<MessageRequest>(MessageHandleAsync);
            _messageExchanges = new ConcurrentDictionary<string, MessageExchange>();
        }

        /// <summary>
        ///     Возвращает точку обмена сообщениями.
        /// </summary>
        /// <param name="exchangeName">Наименование точки обмена сообщениями.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public IMessageExchange GetExchange(string exchangeName)
        {
            return GetInternalExchange(exchangeName);
        }

        public void Dispose()
        {
            _messageQueue.Dispose();
        }

        private async void MessageHandleAsync(MessageRequest request)
        {
            try
            {
                var exchange = GetInternalExchange(request.ExchangeName);
                await exchange.HandleAsync(request.MessageType, request.MessageBody);

                request.OnSuccessComplete();
            }
            catch (Exception error)
            {
                request.OnErrorComplete(error);
            }
        }

        private MessageExchange GetInternalExchange(string exchangeName)
        {
            if (exchangeName == null)
            {
                throw new ArgumentNullException("exchangeName");
            }

            return _messageExchanges.GetOrAdd(exchangeName, key => new MessageExchange(key, _messageQueue));
        }
    }
}