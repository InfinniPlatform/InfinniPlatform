using System.Threading.Tasks;

using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Cache.Clusterization
{
    internal class TwoLayerCacheStateObserver : ITwoLayerCacheStateObserver
    {
        public TwoLayerCacheStateObserver(IBroadcastProducer broadcastProducer)
        {
            _broadcastProducer = broadcastProducer;
        }


        private readonly IBroadcastProducer _broadcastProducer;


        public async Task OnResetKey(string key)
        {
            await _broadcastProducer.PublishAsync(new TwoLayerCacheResetKeyEvent { CacheKey = key });
        }


        public static bool CanBeCreated(IContainerResolver resolver)
        {
            return resolver.IsRegistered<IBroadcastProducer>();
        }
    }
}