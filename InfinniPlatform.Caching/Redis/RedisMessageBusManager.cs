using System;
using System.Collections.Generic;

using InfinniPlatform.Caching.Properties;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Реализует интерфейс управления подписками шины сообщений на базе Redis.
    /// </summary>
    [LoggerName("Redis")]
    internal sealed class RedisMessageBusManager : IMessageBusManager
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="appEnvironment">Пространство имен для ключей.</param>
        /// <param name="connectionFactory">Фабрика подключений к Redis.</param>
        /// <param name="log">Сервис регистрации событий.</param>
        /// <param name="performanceLog">Сервис регистрации длительности выполнения методов.</param>
        public RedisMessageBusManager(IAppEnvironment appEnvironment, RedisConnectionFactory connectionFactory, ILog log, IPerformanceLog performanceLog)
        {
            _keyspace = appEnvironment.Name;
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

                _performanceLog.Log(CachingHelpers.PerformanceLogRedisSubscribeMethod, startTime);

                return messageBusObserver;
            }
            catch (Exception exception)
            {
                var errorContext = new Dictionary<string, object>
                                   {
                                       { "method", CachingHelpers.PerformanceLogRedisSubscribeMethod }
                                   };

                _log.Error(Resources.RedisCommandCompletedWithError, errorContext, exception);

                _performanceLog.Log(CachingHelpers.PerformanceLogRedisSubscribeMethod, startTime, exception);

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

                    _performanceLog.Log(CachingHelpers.PerformanceLogRedisHandleMethod, startTime);
                }
                catch (Exception exception)
                {
                    var errorContext = new Dictionary<string, object>
                                   {
                                       { "method", CachingHelpers.PerformanceLogRedisHandleMethod },
                                       { "key", unwrappedKey }
                                   };

                    _log.Error(Resources.RedisCommandCompletedWithError, errorContext, exception);

                    _performanceLog.Log(CachingHelpers.PerformanceLogRedisHandleMethod, startTime, exception);

                    // Не пробрасываем исключение, так как ошибка в одном обработчике не должна влиять на другой
                }
            }
        }
    }
}