using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Caching
{
    /// <summary>
    /// Реализует интерфейс шины сообщений.
    /// </summary>
    internal sealed class MessageBusImpl : IMessageBus
    {
        public MessageBusImpl(IMessageBusManager messageBusManager, IMessageBusPublisher messageBusPublisher)
        {
            _messageBusManager = messageBusManager;
            _messageBusPublisher = messageBusPublisher;
        }


        private readonly IMessageBusManager _messageBusManager;
        private readonly IMessageBusPublisher _messageBusPublisher;


        public IDisposable Subscribe(string key, Action<string, string> handler)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return _messageBusManager.Subscribe(key, handler);
        }

        public Task Publish(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _messageBusPublisher.Publish(key, value);
        }
    }
}