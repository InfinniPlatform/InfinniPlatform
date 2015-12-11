using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Caching.Properties;
using InfinniPlatform.Sdk.Environment.Log;

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
        /// <param name="log">Сервис регистрации событий.</param>
        /// <param name="performanceLog">Сервис регистрации длительности выполнения методов.</param>
        public RedisCacheMessageBusImpl(string keyspace, RedisConnectionFactory connectionFactory, ILog log, IPerformanceLog performanceLog)
        {
            _keyspace = keyspace;
            _connectionFactory = connectionFactory;
            _messageBusObserver = new Lazy<RedisMessageBusObserver>(CreateMessageBusObserver);

            _log = log;
            _performanceLog = performanceLog;
        }


        private readonly string _keyspace;
        private readonly RedisConnectionFactory _connectionFactory;
        private readonly Lazy<RedisMessageBusObserver> _messageBusObserver;

        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;


        private RedisMessageBusObserver CreateMessageBusObserver()
        {
            var startTime = DateTime.Now;

            var wrappedKeyPattern = CachingHelpers.RedisStarWildcards.WrapCacheKey(_keyspace);

            try
            {
                // Подписка на все ключи текущего приложения
                var keyspaceObservable = _connectionFactory.GetClient().PSubscribe(wrappedKeyPattern);
                var messageBusObserver = new RedisMessageBusObserver();
                keyspaceObservable.Subscribe(messageBusObserver);

                _performanceLog.Log(CachingHelpers.PerformanceLogRedisComponent, CachingHelpers.PerformanceLogRedisSubscribeMethod, startTime, null);

                return messageBusObserver;
            }
            catch (Exception exception)
            {
                var errorContext = new Dictionary<string, object>
                                   {
                                       { "method", CachingHelpers.PerformanceLogRedisSubscribeMethod }
                                   };

                _log.Error(Resources.RedisCommandCompletedWithError, errorContext, exception);

                _performanceLog.Log(CachingHelpers.PerformanceLogRedisComponent, CachingHelpers.PerformanceLogRedisSubscribeMethod, startTime, exception.GetMessage());

                throw;
            }
        }

        private void TryPublish(string unwrappedKey, string value)
        {
            var startTime = DateTime.Now;

            var wrappedKey = unwrappedKey.WrapCacheKey(_keyspace);

            try
            {
                _connectionFactory.GetClient().Publish(wrappedKey, value);

                _performanceLog.Log(CachingHelpers.PerformanceLogRedisComponent, CachingHelpers.PerformanceLogRedisPublishMethod, startTime, null);
            }
            catch (Exception exception)
            {
                var errorContext = new Dictionary<string, object>
                                   {
                                       { "method", CachingHelpers.PerformanceLogRedisPublishMethod },
                                       { "key", unwrappedKey }
                                   };

                _log.Error(Resources.RedisCommandCompletedWithError, errorContext, exception);

                _performanceLog.Log(CachingHelpers.PerformanceLogRedisComponent, CachingHelpers.PerformanceLogRedisPublishMethod, startTime, exception.GetMessage());

                throw;
            }
        }

        private void TryHandle(string wrappedKey, string value, Action<string, string> handler)
        {
            var startTime = DateTime.Now;

            var unwrappedKey = wrappedKey.UnwrapCacheKey(_keyspace);

            if (!string.IsNullOrEmpty(unwrappedKey))
            {
                try
                {
                    handler(unwrappedKey, value);

                    _performanceLog.Log(CachingHelpers.PerformanceLogRedisComponent, CachingHelpers.PerformanceLogRedisHandleMethod, startTime, null);
                }
                catch (Exception exception)
                {
                    var errorContext = new Dictionary<string, object>
                                   {
                                       { "method", CachingHelpers.PerformanceLogRedisHandleMethod },
                                       { "key", unwrappedKey }
                                   };

                    _log.Error(Resources.RedisCommandCompletedWithError, errorContext, exception);

                    _performanceLog.Log(CachingHelpers.PerformanceLogRedisComponent, CachingHelpers.PerformanceLogRedisHandleMethod, startTime, exception.GetMessage());

                    // Не пробрасываем исключение, так как ошибка в одном обработчике не должна влиять на другой
                }
            }
        }


        public Task Publish(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Нет смысла в логировании данной операции, так как она выполняется асинхронно
            return Task.Run(() => TryPublish(key, value));
        }

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

            var wrappedKey = key.WrapCacheKey(_keyspace);

            // Нет смысла в логировании данной операции, так как добавление подписчика идет без обращения к Redis
            var subscription = _messageBusObserver.Value.Subscribe(wrappedKey, (wk, v) => TryHandle(wk, v, handler));

            return subscription;
        }
    }
}