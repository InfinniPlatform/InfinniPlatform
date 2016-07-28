using System.Threading.Tasks;

using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.Caching.TwoLayer
{
    /// <summary>
    /// Обработчик сообщений синхронизации кэша.
    /// </summary>
    [QueueName(nameof(TwoLayerCacheImpl))]
    public class TwoLayerCacheConsumer : BroadcastConsumerBase<string>
    {
        public TwoLayerCacheConsumer(ICacheSynchronizer cache)
        {
            _cache = cache;
        }

        private readonly ICacheSynchronizer _cache;

        protected override Task Consume(Message<string> message)
        {
            _cache.ProcessMessage(message);
            return Task.FromResult<object>(null);
        }
    }
}