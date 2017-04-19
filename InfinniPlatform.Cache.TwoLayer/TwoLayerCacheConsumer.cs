using System;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Обработчик сообщений синхронизации кэша.
    /// </summary>
    [QueueName(nameof(TwoLayerCache))]
    public class TwoLayerCacheConsumer : BroadcastConsumerBase<string>
    {
        public TwoLayerCacheConsumer(ITwoLayerCacheSynchronizer cache)
        {
            _cache = cache;
        }

        private readonly ITwoLayerCacheSynchronizer _cache;

        protected override async Task Consume(Message<string> message)
        {
            await _cache.ProcessMessage(message);
        }

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }
}