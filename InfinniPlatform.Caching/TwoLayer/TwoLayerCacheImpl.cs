using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Cache;

namespace InfinniPlatform.Caching.TwoLayer
{
    /// <summary>
    /// Реализует интерфейс для управления двухуровневым кэшем.
    /// </summary>
    public sealed class TwoLayerCacheImpl : ITwoLayerCache, IDisposable
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="memoryCache">Локальный кэш.</param>
        /// <param name="sharedCache">Распределенный кэш.</param>
        /// <param name="sharedMessageBus">Шина для синхронизации локальных кэшей.</param>
        public TwoLayerCacheImpl(IMemoryCache memoryCache, ISharedCache sharedCache, IMessageBus sharedMessageBus)
        {
            _memoryCache = memoryCache;
            _sharedCache = sharedCache;
            _sharedMessageBus = sharedMessageBus;

            _sharedCachePublisherId = Guid.NewGuid().ToString("N");
            _sharedCacheSubscriptions = new ConcurrentDictionary<string, IDisposable>();
        }


        private readonly IMemoryCache _memoryCache;
        private readonly ISharedCache _sharedCache;
        private readonly IMessageBus _sharedMessageBus;

        private readonly string _sharedCachePublisherId;
        private readonly ConcurrentDictionary<string, IDisposable> _sharedCacheSubscriptions;


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

                    SubscribeOnKeyChanged(key);
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
            SubscribeOnKeyChanged(key);
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
                    UnsubscribeOnKeyChanged(key);
                    NotifyOnKeyChanged(key);
                }
            }

            return deleted;
        }

        public void Clear()
        {
            try
            {
                try
                {
                    _memoryCache.Clear();
                }
                finally
                {
                    _sharedCache.Clear();
                }
            }
            finally
            {
                foreach (var subscription in _sharedCacheSubscriptions.ToArray())
                {
                    UnsubscribeOnKeyChanged(subscription.Key);
                    NotifyOnKeyChanged(subscription.Key);
                }
            }
        }

        public void Dispose()
        {
            foreach (var subscription in _sharedCacheSubscriptions.ToArray())
            {
                UnsubscribeOnKeyChanged(subscription.Key);
            }
        }


        private void NotifyOnKeyChanged(string key)
        {
            ExecuteAsync(() => _sharedMessageBus.Publish(key, _sharedCachePublisherId));
        }

        private void SubscribeOnKeyChanged(string key)
        {
            if (!_sharedCacheSubscriptions.ContainsKey(key))
            {
                ExecuteAsync(() =>
                {
                    var subscription = _sharedMessageBus.Subscribe(key, (k, v) =>
                    {
                        if (v != _sharedCachePublisherId)
                        {
                            try
                            {
                                _memoryCache.Remove(k);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    });

                    _sharedCacheSubscriptions.TryAdd(key, subscription);
                });
            }
        }

        private void UnsubscribeOnKeyChanged(string key)
        {
            IDisposable subscription;

            if (_sharedCacheSubscriptions.TryRemove(key, out subscription))
            {
                ExecuteAsync(subscription.Dispose);
            }
        }


        private static void ExecuteAsync(Action action)
        {
            Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch
                {
                    // ignored
                }
            });
        }
    }
}