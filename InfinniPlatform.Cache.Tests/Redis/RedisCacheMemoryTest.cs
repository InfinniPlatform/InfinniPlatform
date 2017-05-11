using System;

using InfinniPlatform.Logging;
using InfinniPlatform.Tests;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Cache.Redis
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Manual")]
    public sealed class RedisCacheMemoryTest
    {
        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void MemoryTest(int iterations)
        {
            // Given

            var appOptions = new AppOptions { AppName = nameof(RedisCacheMemoryTest) };
            var redisOptions = new RedisSharedCacheOptions { Host = "localhost", Password = "TeamCity" };
            var redisCache = new RedisSharedCache(appOptions, new RedisConnectionFactory(redisOptions), new Mock<ILog>().Object, new Mock<IPerformanceLog>().Object);

            const string key = "GetMemoryTest_Key";

            double startSize = GC.GetTotalMemory(true);

            // When

            var cache = redisCache;

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