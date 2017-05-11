using System.Threading.Tasks;

namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Provides the service to get notifications about changing the state of the <see cref="ITwoLayerCache"/>.
    /// </summary>
    public interface ITwoLayerCacheStateObserver
    {
        /// <summary>
        /// Invoked when there is a necessity to reset a cache key.
        /// </summary>
        /// <param name="key">The key to reset.</param>
        Task OnResetKey(string key);
    }
}