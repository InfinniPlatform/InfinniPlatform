﻿using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Caching.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.Caching.TwoLayer
{
    /// <summary>
    /// Источник потребителей сообщений сборки InfinniPlatform.Caching.
    /// </summary>
    public class CachingMessageConsumerSource : IMessageConsumerSource
    {
        public CachingMessageConsumerSource(TwoLayerCacheConsumer twoLayerCacheConsumer)
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