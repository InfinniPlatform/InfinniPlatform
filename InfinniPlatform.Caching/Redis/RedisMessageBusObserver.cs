using System;

using Sider;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Обозреватель шины сообщений Redis.
    /// </summary>
    internal sealed class RedisMessageBusObserver : IObserver<Message<string>>
    {
        public RedisMessageBusObserver()
        {
            _subscriptions = new RedisMessageBusSubscriptions();
        }


        private readonly RedisMessageBusSubscriptions _subscriptions;


        void IObserver<Message<string>>.OnNext(Message<string> message)
        {
            if (message.Type == MessageType.PMessage || message.Type == MessageType.Message)
            {
                var key = message.SourceChannel;

                if (!string.IsNullOrEmpty(key))
                {
                    TryHandle(key, message.Body);
                }
            }
        }

        void IObserver<Message<string>>.OnError(Exception error)
        {
        }

        void IObserver<Message<string>>.OnCompleted()
        {
        }


        /// <summary>
        /// Добавляет подписку.
        /// </summary>
        public IDisposable Subscribe(string key, Action<string, string> handler)
        {
            return _subscriptions.AddSubscription(key, handler);
        }


        private void TryHandle(string key, string value)
        {
            var subscribers = _subscriptions.GetSubscriptions(key);

            if (subscribers != null)
            {
                foreach (var subscriber in subscribers)
                {
                    try
                    {
                        subscriber.Handle(key, value);
                    }
                    catch
                    {
                        // TODO: Log
                    }
                }
            }
        }
    }
}