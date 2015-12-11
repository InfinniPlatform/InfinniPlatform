using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Реализует интерфейс шины сообщений на базе Redis.
    /// </summary>
    internal sealed class RedisCacheMessageBusImpl : ICacheMessageBus
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="keyspace">Пространство имен для ключей.</param>
        /// <param name="connectionFactory">Фабрика подключений к Redis.</param>
        public RedisCacheMessageBusImpl(string keyspace, RedisConnectionFactory connectionFactory)
        {
            _keyspace = keyspace;
            _connectionFactory = connectionFactory;
            _messageBusObserver = new Lazy<RedisMessageBusObserver>(CreateMessageBusObserver);
        }


        private readonly string _keyspace;
        private readonly RedisConnectionFactory _connectionFactory;
        private readonly Lazy<RedisMessageBusObserver> _messageBusObserver;


        private RedisMessageBusObserver CreateMessageBusObserver()
        {
            var wrappedKeyPattern = "*".WrapCacheKey(_keyspace);

            var keyspaceObservable = _connectionFactory.GetClient().PSubscribe(wrappedKeyPattern);

            var messageBusObserver = new RedisMessageBusObserver();

            keyspaceObservable.Subscribe(messageBusObserver);

            return messageBusObserver;
        }


        public Task Publish(string key, string value)
        {
            Logging.Logger.Log.Info($"REDIS: Publish({key}, {value})");

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var wrappedKey = key.WrapCacheKey(_keyspace);

            return Task.Run(() => _connectionFactory.GetClient().Publish(wrappedKey, value));
        }

        public IDisposable Subscribe(string key, Action<string, string> handler)
        {
            Logging.Logger.Log.Info($"REDIS: Subscribe({key})");

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var wrappedKey = key.WrapCacheKey(_keyspace);

            var subscription = _messageBusObserver.Value.Subscribe(wrappedKey, (wk, v) => TryHandle(wk, v, handler));

            return subscription;
        }


        private void TryHandle(string wrappedKey, string value, Action<string, string> handler)
        {
            Logging.Logger.Log.Info($"REDIS: TryHandle({wrappedKey}, {value})");

            var unwrappedKey = wrappedKey.UnwrapCacheKey(_keyspace);

            if (!string.IsNullOrEmpty(unwrappedKey))
            {
                try
                {
                    handler(unwrappedKey, value);
                }
                catch
                {
                    // TODO: Log
                }
            }
        }
    }
}