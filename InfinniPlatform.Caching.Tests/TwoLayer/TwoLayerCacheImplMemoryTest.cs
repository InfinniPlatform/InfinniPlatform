using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.TwoLayer;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.TwoLayer
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Manual")]
    public sealed class TwoLayerCacheImplMemoryTest
    {
        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void MemoryTest(int iterations)
        {
            // Given

            var cacheName = GetType().Name;

            var settings = new RedisConnectionSettings
            {
                Host = "localhost",
                Password = "TeamCity"
            };

            var memoryCache = new MemoryCacheImpl(cacheName);
            var redisConnectionFactory = new RedisConnectionFactory(settings);
            var redisCache = new RedisCacheImpl(cacheName, redisConnectionFactory);
            var redisCacheMessageBus = new RedisCacheMessageBusImpl(cacheName, redisConnectionFactory);
            var twoLayerCache = new TwoLayerCacheImpl(memoryCache, redisCache, redisCacheMessageBus);

            const string key = "GetMemoryTest_Key";

            double startSize = GC.GetTotalMemory(true);

            // When

            var cache = twoLayerCache;

            for (var i = 0; i < iterations; ++i)
            {
                var value = Guid.NewGuid().ToString("N");
                cache.Set(key, value);
                cache.Get(key);
            }

            ((IDisposable)cache).Dispose();

            double stopSize = GC.GetTotalMemory(true);

            var memoryLeak = (stopSize - startSize);

            // Then
            Console.WriteLine(@"Iteration count: {0}", iterations);
            Console.WriteLine(@"Memory Leak : {0:N2} %", memoryLeak / startSize);
            Console.WriteLine(@"Memory Leak : {0:N2} Kb", memoryLeak / 1024);
        }
    }
}