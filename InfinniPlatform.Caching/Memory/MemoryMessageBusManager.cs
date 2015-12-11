using System;

namespace InfinniPlatform.Caching.Memory
{
    /// <summary>
    /// Реализует интерфейс управления подписками шины сообщений на базе коллекции в памяти.
    /// </summary>
    internal class MemoryMessageBusManager : IMessageBusManager
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="subscriptions">Список подписчиков шины сообщений.</param>
        public MemoryMessageBusManager(MessageBusSubscriptions subscriptions)
        {
            _subscriptions = subscriptions;
        }


        private readonly MessageBusSubscriptions _subscriptions;


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

            return _subscriptions.AddSubscription(key, handler);
        }
    }
}