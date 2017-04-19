using System.Collections.Generic;

using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Источник потребителей сообщений сборки InfinniPlatform.Caching.
    /// </summary>
    public class TwoLayerCacheConsumerSource : IMessageConsumerSource
    {
        public TwoLayerCacheConsumerSource(TwoLayerCacheConsumer twoLayerCacheConsumer)
        {
            _twoLayerCacheConsumer = twoLayerCacheConsumer;
        }

        private readonly TwoLayerCacheConsumer _twoLayerCacheConsumer;

        public IEnumerable<IConsumer> GetConsumers()
        {
            return new[] { _twoLayerCacheConsumer };
        }
    }
}