using System;

using InfinniPlatform.IoC;

namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Two layer cache configuration options.
    /// </summary>
    public class TwoLayerCacheOptions : IOptions
    {
        /// <inheritdoc />
        public string SectionName => "twoLayerCache";

        /// <summary>
        /// Default instance of <see cref="TwoLayerCacheOptions" />.
        /// </summary>
        public static readonly TwoLayerCacheOptions Default = new TwoLayerCacheOptions();


        /// <summary>
        /// The factory method to get the <see cref="IInMemoryCacheFactory"/>.
        /// </summary>
        public Func<IContainerResolver, IInMemoryCacheFactory> InMemoryCacheFactory { get; set; }

        /// <summary>
        /// The factory method to get the <see cref="ISharedCacheFactory"/>.
        /// </summary>
        public Func<IContainerResolver, ISharedCacheFactory> SharedCacheFactory { get; set; }

        /// <summary>
        /// The factory method to get the <see cref="ITwoLayerCacheStateObserver"/>.
        /// </summary>
        public Func<IContainerResolver, ITwoLayerCacheStateObserver> TwoLayerCacheStateObserver { get; set; }
    }
}