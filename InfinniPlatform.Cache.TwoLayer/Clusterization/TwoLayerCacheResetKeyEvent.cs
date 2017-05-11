namespace InfinniPlatform.Cache.Clusterization
{
    /// <summary>
    /// Represents an event about necessity of reset a cache key in the <see cref="IInMemoryCache"/>.
    /// </summary>
    internal class TwoLayerCacheResetKeyEvent
    {
        /// <summary>
        /// The key to reset.
        /// </summary>
        public string CacheKey { get; set; }
    }
}