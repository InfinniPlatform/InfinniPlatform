using System;
using System.Diagnostics;
using System.Threading.Tasks;

using InfinniPlatform.Caching.RabbitMQ;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Core;
using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;

namespace InfinniPlatform.Caching.TwoLayer
{
    /// <summary>
    /// Реализует интерфейс для управления двухуровневым кэшем.
    /// </summary>
    public class TwoLayerCacheImpl : BroadcastConsumerBase<SharedCacheMessage>, ICache
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="memoryCache">Локальный кэш.</param>
        /// <param name="sharedCache">Распределенный кэш.</param>
        /// <param name="broadcastProducer">Шина для синхронизации локальных кэшей.</param>
        /// <param name="appIdentity"></param>
        /// <param name="log"></param>
        public TwoLayerCacheImpl(IMemoryCache memoryCache,
                                 ISharedCache sharedCache,
                                 IBroadcastProducer broadcastProducer,
                                 IAppIdentity appIdentity,
                                 ILog log)
        {
            _memoryCache = memoryCache;
            _sharedCache = sharedCache;
            _broadcastProducer = broadcastProducer;
            _appIdentity = appIdentity;
            _log = log;
        }

        private readonly IAppIdentity _appIdentity;
        private readonly ILog _log;
        private readonly IBroadcastProducer _broadcastProducer;
        private readonly IMemoryCache _memoryCache;
        private readonly ISharedCache _sharedCache;

        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _memoryCache.Contains(key) || _sharedCache.Contains(key);
        }

        public string Get(string key)
        {
            string value;

            TryGet(key, out value);

            return value;
        }

        public bool TryGet(string key, out string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var exists = false;

            if (!_memoryCache.TryGet(key, out value))
            {
                if (!_sharedCache.TryGet(key, out value))
                {
                    value = null;
                }
                else
                {
                    exists = true;

                    _memoryCache.Set(key, value);
                }
            }
            else
            {
                exists = true;
            }

            return exists;
        }

        public void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _memoryCache.Set(key, value);
            _sharedCache.Set(key, value);

            NotifyOnKeyChanged(key);
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var deleted = false;

            try
            {
                try
                {
                    deleted |= _memoryCache.Remove(key);
                }
                finally
                {
                    deleted |= _sharedCache.Remove(key);
                }
            }
            finally
            {
                if (deleted)
                {
                    NotifyOnKeyChanged(key);
                }
            }

            return deleted;
        }

        private void NotifyOnKeyChanged(string key)
        {
            if (!(_sharedCache is RedisCacheStubImpl))
            {
                _broadcastProducer.Publish(new SharedCacheMessage(key));
            }
        }

        protected override Task Consume(Message<SharedCacheMessage> message)
        {
            return Task.Run(() =>
                            {
                                if (_sharedCache is RedisCacheStubImpl)
                                {
                                    return;
                                }
                                try
                                {
                                    var sharedCacheMessage1 = (SharedCacheMessage)message.GetBody();

                                    if (message.PublisherId == _appIdentity.Id)
                                    {
                                        //ignore own message
                                    }
                                    else
                                    {
                                        var key1 = sharedCacheMessage1.Key;

                                        if (!string.IsNullOrEmpty(key1))
                                        {
                                            _memoryCache.Remove(key1);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    _log.Error(e);
                                }
                            });
        }
    }
}