using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.TwoLayer;

namespace InfinniPlatform.Caching.Factory
{
	public sealed class CacheFactory : ICacheFactory
	{
		public static readonly CacheFactory Instance = new CacheFactory(CacheMessageBusFactory.Instance);


		public CacheFactory(ICacheMessageBusFactory cacheMessageBusFactory)
		{
			if (cacheMessageBusFactory == null)
			{
				throw new ArgumentNullException("cacheMessageBusFactory");
			}

			_cacheMessageBusFactory = cacheMessageBusFactory;
			_memoryCache = new Lazy<ICache>(CreateMemoryCache);
			_sharedCache = new Lazy<ICache>(CreateSharedCache);
			_twoLayerCache = new Lazy<ICache>(CreateTwoLayerCache);
		}


		private readonly ICacheMessageBusFactory _cacheMessageBusFactory;
		private readonly Lazy<ICache> _memoryCache;
		private readonly Lazy<ICache> _sharedCache;
		private readonly Lazy<ICache> _twoLayerCache;


		public ICache GetMemoryCache()
		{
			return _memoryCache.Value;
		}

		public ICache GetSharedCache()
		{
			return _sharedCache.Value;
		}

		public ICache GetTwoLayerCache()
		{
			return _twoLayerCache.Value;
		}


		private static ICache CreateMemoryCache()
		{
			var cacheName = CachingHelpers.GetConfigCacheName();
			var memoryCache = new MemoryCacheImpl(cacheName);
			return memoryCache;
		}

		private static ICache CreateSharedCache()
		{
			var cacheName = CachingHelpers.GetConfigCacheName();
			var redisConnectionString = CachingHelpers.GetConfigRedisConnectionString();
			var sharedCache = new RedisCacheImpl(cacheName, redisConnectionString);
			return sharedCache;
		}

		private ICache CreateTwoLayerCache()
		{
			var memoryCache = GetMemoryCache();
			var sharedCache = GetSharedCache();
			var sharedCacheMessageBus = _cacheMessageBusFactory.GetSharedCacheMessageBus();
			var twoLayerCache = new TwoLayerCacheImpl(memoryCache, sharedCache, sharedCacheMessageBus);
			return twoLayerCache;
		}
	}
}