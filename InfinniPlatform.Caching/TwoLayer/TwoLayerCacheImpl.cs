using System;
using System.Threading.Tasks;

using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Producers;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Caching.TwoLayer
{
    /// <summary>
    /// Реализует интерфейс для управления двухуровневым кэшем.
    /// </summary>
    public class TwoLayerCacheImpl : ICache, ICacheSynchronizer
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="memoryCache">Локальный кэш.</param>
        /// <param name="sharedCache">Распределенный кэш.</param>
        /// <param name="appEnvironment">Настройки приложения.</param>
        /// <param name="broadcastProducer">Шина для синхронизации кэша.</param>
        /// <param name="log">Лог.</param>
        public TwoLayerCacheImpl(IMemoryCache memoryCache,
                                 ISharedCache sharedCache,
                                 IAppEnvironment appEnvironment,
                                 IBroadcastProducer broadcastProducer,
                                 ILog log)
        {
            _memoryCache = memoryCache;
            _sharedCache = sharedCache;
            _appEnvironment = appEnvironment;
            _broadcastProducer = broadcastProducer;
            _log = log;

            _isSharedCacheEnabled = !(_sharedCache is NullSharedCacheImpl);
        }

        private readonly IAppEnvironment _appEnvironment;
        private readonly IBroadcastProducer _broadcastProducer;
        private readonly ILog _log;

        private readonly IMemoryCache _memoryCache;
        private readonly ISharedCache _sharedCache;
        private readonly bool _isSharedCacheEnabled;

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

            NotifyOnKeyChangedAsync(key);
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
                    NotifyOnKeyChangedAsync(key);
                }
            }

            return deleted;
        }


        //ICacheSynchronizer


        public Task ProcessMessage(Message<string> message)
        {
            return Task.Run(() =>
                            {
                                if (_isSharedCacheEnabled)
                                {
                                    try
                                    {
                                        if (message.AppId == _appEnvironment.InstanceId)
                                        {
                                            //ignore own message
                                        }
                                        else
                                        {
                                            var key = (string)message.GetBody();

                                            if (!string.IsNullOrEmpty(key))
                                            {
                                                _memoryCache.Remove(key);
                                            }
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        _log.Error(exception);
                                    }
                                }
                            });
        }

        private async void NotifyOnKeyChangedAsync(string key)
        {
            if (_isSharedCacheEnabled)
            {
                await _broadcastProducer.PublishAsync(key, nameof(TwoLayerCacheImpl));
            }
        }
    }
}