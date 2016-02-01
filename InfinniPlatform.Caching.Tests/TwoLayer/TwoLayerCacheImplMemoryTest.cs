using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.TwoLayer;
using InfinniPlatform.Sdk.Logging;

using Moq;

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

            var log = new Mock<ILog>().Object;
            var performanceLog = new Mock<IPerformanceLog>().Object;

            var memoryCache = new MemoryCacheImpl(cacheName);
            var redisCache = new RedisCacheImpl(cacheName, new RedisConnectionFactory(settings), log, performanceLog);
            var redisMessageBusManager = new RedisMessageBusManager(cacheName, new RedisConnectionFactory(settings), log, performanceLog);
            var redisMessageBusPublisher = new RedisMessageBusPublisher(cacheName, new RedisConnectionFactory(settings), log, performanceLog);
            var redisMessageBus = new MessageBusImpl(redisMessageBusManager, redisMessageBusPublisher);
            var twoLayerCache = new TwoLayerCacheImpl(memoryCache, redisCache, redisMessageBus);

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

            cache.Dispose();

            double stopSize = GC.GetTotalMemory(true);

            var memoryLeak = (stopSize - startSize);

            // Then
            Console.WriteLine(@"Iteration count: {0}", iterations);
            Console.WriteLine(@"Memory Leak : {0:N2} %", memoryLeak / startSize);
            Console.WriteLine(@"Memory Leak : {0:N2} Kb", memoryLeak / 1024);
        }
    }
}