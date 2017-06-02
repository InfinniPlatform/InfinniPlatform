using System;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Cache.Clusterization
{
    /// <summary>
    /// Handles the <see cref="TwoLayerCacheResetKeyEvent"/>.
    /// </summary>
    [QueueName(nameof(TwoLayerCache))]
    internal class TwoLayerCacheResetKeyConsumer : BroadcastConsumerBase<TwoLayerCacheResetKeyEvent>
    {
        public TwoLayerCacheResetKeyConsumer(AppOptions appOptions, IInMemoryCache inMemoryCache, ILogger<TwoLayerCacheResetKeyConsumer> logger)
        {
            _appOptions = appOptions;
            _inMemoryCache = inMemoryCache;
            _logger = logger;
        }


        private readonly AppOptions _appOptions;
        private readonly IInMemoryCache _inMemoryCache;
        private readonly ILogger _logger;


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
                _logger.LogError(exception);
            }

            return Task.CompletedTask;
        }

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }
}