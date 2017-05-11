using System.Threading.Tasks;

namespace InfinniPlatform.Cache.Clusterization
{
    internal class TwoLayerCacheStateObserverStub : ITwoLayerCacheStateObserver
    {
        public Task OnResetKey(string key)
        {
            return Task.CompletedTask;
        }
    }
}