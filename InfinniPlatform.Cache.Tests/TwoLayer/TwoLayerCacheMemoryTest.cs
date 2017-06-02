using System;

using InfinniPlatform.Logging;
using InfinniPlatform.Tests;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Cache.TwoLayer
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Manual")]
    public sealed class TwoLayerCacheMemoryTest
    {
        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void MemoryTest(int iterations)
        {
            // Given

            var inMemoryCache = new InMemoryCache();
            var inMemoryCacheFactory = new Mock<IInMemoryCacheFactory>();
            inMemoryCacheFactory.Setup(i => i.Create()).Returns(inMemoryCache);

            var appOptions = new AppOptions { AppName = nameof(TwoLayerCacheMemoryTest) };
            var redisOptions = new RedisSharedCacheOptions { Host = "localhost", Password = "TeamCity" };
            var sharedCache = new RedisSharedCache(appOptions, new RedisConnectionFactory(redisOptions), new Mock<ILogger<RedisSharedCache>>().Object, new Mock<IPerformanceLogger<RedisSharedCache>>().Object);
            var sharedCacheFactory = new Mock<ISharedCacheFactory>();
            sharedCacheFactory.Setup(i => i.Create()).Returns(sharedCache);

            var twoLayerCache = new TwoLayerCache(inMemoryCacheFactory.Object, sharedCacheFactory.Object, new Mock<ITwoLayerCacheStateObserver>().Object);


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

            double stopSize = GC.GetTotalMemory(true);

            var memoryLeak = (stopSize - startSize);

            // Then
            Console.WriteLine(@"Iteration count: {0}", iterations);
            Console.WriteLine(@"Memory Leak : {0:N2} %", memoryLeak / startSize);
            Console.WriteLine(@"Memory Leak : {0:N2} Kb", memoryLeak / 1024);
        }
    }
}