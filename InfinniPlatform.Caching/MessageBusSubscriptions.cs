using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace InfinniPlatform.Caching
{
    /// <summary>
    /// Список подписчиков шины сообщений.
    /// </summary>
    /// <remarks>
    /// Все методы и члены класса являются потокобезопасными.
    /// </remarks>
    internal sealed class MessageBusSubscriptions
    {
        private readonly ReaderWriterLockSlim _syncKeySubscriptions
            = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<MessageBusSubscriber, MessageBusSubscriber>> _subscriptions
            = new ConcurrentDictionary<string, ConcurrentDictionary<MessageBusSubscriber, MessageBusSubscriber>>(StringComparer.Ordinal);


        /// <summary>
        /// Добавляет подписку.
        /// </summary>
        public MessageBusSubscriber AddSubscription(string key, Action<string, string> handler)
        {
            ConcurrentDictionary<MessageBusSubscriber, MessageBusSubscriber> keySubscriptions;

            if (!_subscriptions.TryGetValue(key, out keySubscriptions))
            {
                keySubscriptions = _subscriptions.GetOrAdd(key, new ConcurrentDictionary<MessageBusSubscriber, MessageBusSubscriber>());
            }

            MessageBusSubscriber subscriber = null;

            // ReSharper disable AccessToModifiedClosure

            subscriber = new MessageBusSubscriber(handler, () => RemoveSubscription(keySubscriptions, subscriber));

            // ReSharper restore AccessToModifiedClosure

            _syncKeySubscriptions.EnterWriteLock();

            try
            {
                keySubscriptions.TryAdd(subscriber, subscriber);
            }
            finally
            {
                _syncKeySubscriptions.ExitWriteLock();
            }

            return subscriber;
        }

        private void RemoveSubscription(ConcurrentDictionary<MessageBusSubscriber, MessageBusSubscriber> keySubscriptions, MessageBusSubscriber subscriber)
        {
            _syncKeySubscriptions.EnterWriteLock();

            try
            {
                keySubscriptions.TryRemove(subscriber, out subscriber);
            }
            finally
            {
                _syncKeySubscriptions.ExitWriteLock();
            }
        }


        /// <summary>
        /// Возвращает список всех подписок.
        /// </summary>
        public IEnumerable<MessageBusSubscriber> GetSubscriptions(string key)
        {
            ConcurrentDictionary<MessageBusSubscriber, MessageBusSubscriber> keySubscriptions;

            if (_subscriptions.TryGetValue(key, out keySubscriptions))
            {
                _syncKeySubscriptions.EnterReadLock();

                try
                {
                    return keySubscriptions.Values.ToArray();
                }
                finally
                {
                    _syncKeySubscriptions.ExitReadLock();
                }
            }

            return Enumerable.Empty<MessageBusSubscriber>();
        }
    }
}