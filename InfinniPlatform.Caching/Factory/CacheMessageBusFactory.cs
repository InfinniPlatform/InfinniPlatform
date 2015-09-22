using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;

namespace InfinniPlatform.Caching.Factory
{
	public sealed class CacheMessageBusFactory : ICacheMessageBusFactory
	{
		public static readonly CacheMessageBusFactory Instance = new CacheMessageBusFactory();


		public CacheMessageBusFactory()
		{
			_memoryCacheMessageBus = new Lazy<ICacheMessageBus>(CreateMemoryCacheMessageBus);
			_sharedCacheMessageBus = new Lazy<ICacheMessageBus>(CreateSharedCacheMessageBus);
		}


		private readonly Lazy<ICacheMessageBus> _memoryCacheMessageBus;
		private readonly Lazy<ICacheMessageBus> _sharedCacheMessageBus;


		public ICacheMessageBus GetMemoryCacheMessageBus()
		{
			return _memoryCacheMessageBus.Value;
		}

		public ICacheMessageBus GetSharedCacheMessageBus()
		{
			return _sharedCacheMessageBus.Value;
		}


		private static ICacheMessageBus CreateMemoryCacheMessageBus()
		{
			var memoryCacheMessageBus = new MemoryCacheMessageBusImpl();
			return memoryCacheMessageBus;
		}

		private static ICacheMessageBus CreateSharedCacheMessageBus()
		{
			var cacheName = CachingHelpers.GetConfigCacheName();
			var redisConnectionString = CachingHelpers.GetConfigRedisConnectionString();
			var sharedCacheMessageBus = new RedisCacheMessageBusImpl(cacheName, redisConnectionString);
			return sharedCacheMessageBus;
		}
	}
}