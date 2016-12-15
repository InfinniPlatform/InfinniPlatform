using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Caching.Contract;
using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Consumers;

namespace InfinniPlatform.Caching.TwoLayer
{
    /// <summary>
    /// Источник потребителей сообщений сборки InfinniPlatform.Caching.
    /// </summary>
    public class CachingMessageConsumerSource : IMessageConsumerSource
    {
        public CachingMessageConsumerSource(CacheSettings cacheSettings,
                                            TwoLayerCacheConsumer twoLayerCacheConsumer)
        {
            _cacheSettings = cacheSettings;
            _twoLayerCacheConsumer = twoLayerCacheConsumer;
        }

        private readonly CacheSettings _cacheSettings;
        private readonly TwoLayerCacheConsumer _twoLayerCacheConsumer;

        public IEnumerable<IConsumer> GetConsumers()
        {
            return _cacheSettings.Type == CacheSettings.SharedCacheKey
                       ? new[] { _twoLayerCacheConsumer }
                       : Enumerable.Empty<IConsumer>();
        }
    }
}