using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Caching.Memory
{
    /// <summary>
    /// Интерфейс публикации сообщений в шину на базе коллекции в памяти.
    /// </summary>
    internal sealed class MemoryMessageBusPublisher : IMessageBusPublisher
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="subscriptions">Список подписчиков шины сообщений.</param>
        public MemoryMessageBusPublisher(MessageBusSubscriptions subscriptions)
        {
            _subscriptions = subscriptions;
        }


        private readonly MessageBusSubscriptions _subscriptions;


        public Task Publish(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Task.Run(() =>
                            {
                                var subscribers = _subscriptions.GetSubscriptions(key);

                                if (subscribers != null)
                                {
                                    foreach (var subscriber in subscribers)
                                    {
                                        subscriber.Handle(key, value);
                                    }
                                }
                            });
        }
    }
}