using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Cache.Clusterization
{
    /// <summary>
    /// Handles the <see cref="TwoLayerCacheResetKeyEvent"/>.
    /// </summary>
    [QueueName(nameof(TwoLayerCache))]
    internal class TwoLayerCacheResetKeyConsumer : BroadcastConsumerBase<TwoLayerCacheResetKeyEvent>
    {
        public TwoLayerCacheResetKeyConsumer(AppOptions appOptions, IInMemoryCache inMemoryCache, ILog log)
        {
            _appOptions = appOptions;
            _inMemoryCache = inMemoryCache;
            _log = log;
        }


        private readonly AppOptions _appOptions;
        private readonly IInMemoryCache _inMemoryCache;
        private readonly ILog _log;


        protected override Task Consume(Message<TwoLayerCacheResetKeyEvent> message)
        {
            if (message.AppId == _appOptions.AppInstance)
            {
                // Ignore own message
            }

            try
            {
                var cacheEvent = message.Body;

                if (!string.IsNullOrEmpty(cacheEvent?.CacheKey))
                {
                    // Resets of the key
                    _inMemoryCache.Remove(cacheEvent.CacheKey);
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }

            return Task.CompletedTask;
        }

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }
}