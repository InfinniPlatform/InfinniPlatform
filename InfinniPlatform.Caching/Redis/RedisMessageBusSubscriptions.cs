using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Список подписчиков шины сообщений Redis.
    /// </summary>
    /// <remarks>
    /// Все методы и члены класса являются потокобезопасными.
    /// </remarks>
    internal sealed class RedisMessageBusSubscriptions
    {
        private readonly ReaderWriterLockSlim _syncKeySubscriptions
            = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<RedisMessageBusSubscriber, RedisMessageBusSubscriber>> _subscriptions
            = new ConcurrentDictionary<string, ConcurrentDictionary<RedisMessageBusSubscriber, RedisMessageBusSubscriber>>(StringComparer.Ordinal);


        /// <summary>
        /// Добавляет подписку.
        /// </summary>
        public RedisMessageBusSubscriber AddSubscription(string key, Action<string, string> handler)
        {
            ConcurrentDictionary<RedisMessageBusSubscriber, RedisMessageBusSubscriber> keySubscriptions;

            if (!_subscriptions.TryGetValue(key, out keySubscriptions))
            {
                keySubscriptions = _subscriptions.GetOrAdd(key, new ConcurrentDictionary<RedisMessageBusSubscriber, RedisMessageBusSubscriber>());
            }

            RedisMessageBusSubscriber subscriber = null;

            // ReSharper disable AccessToModifiedClosure

            subscriber = new RedisMessageBusSubscriber(handler, () => RemoveSubscription(keySubscriptions, subscriber));

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

        private void RemoveSubscription(ConcurrentDictionary<RedisMessageBusSubscriber, RedisMessageBusSubscriber> keySubscriptions, RedisMessageBusSubscriber subscriber)
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
        public IEnumerable<RedisMessageBusSubscriber> GetSubscriptions(string key)
        {
            ConcurrentDictionary<RedisMessageBusSubscriber, RedisMessageBusSubscriber> keySubscriptions;

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

            return Enumerable.Empty<RedisMessageBusSubscriber>();
        }
    }
}