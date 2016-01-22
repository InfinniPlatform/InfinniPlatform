using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Caching.Properties;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Реализует интерфейс публикации сообщений в шину на базе Redis.
    /// </summary>
    internal sealed class RedisMessageBusPublisher : IMessageBusPublisher
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="keyspace">Пространство имен для ключей.</param>
        /// <param name="connectionFactory">Фабрика подключений к Redis.</param>
        /// <param name="log">Сервис регистрации событий.</param>
        /// <param name="performanceLog">Сервис регистрации длительности выполнения методов.</param>
        public RedisMessageBusPublisher(string keyspace, RedisConnectionFactory connectionFactory, ILog log, IPerformanceLog performanceLog)
        {
            _keyspace = keyspace;
            _connectionFactory = connectionFactory;

            _log = log;
            _performanceLog = performanceLog;
        }


        private readonly string _keyspace;
        private readonly RedisConnectionFactory _connectionFactory;

        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;


        public Task Publish(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Нет смысла в логировании данной операции, так как она выполняется асинхронно
            return Task.Run(() => TryPublish(key, value));
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

                _performanceLog.Log(CachingHelpers.PerformanceLogRedisComponent, CachingHelpers.PerformanceLogRedisPublishMethod, startTime, exception.GetFullMessage());

                throw;
            }
        }
    }
}