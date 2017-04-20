using System;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Settings;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Cache.Redis
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Manual")]
    public sealed class RedisCacheImplMemoryTest
    {
        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void MemoryTest(int iterations)
        {
            // Given

            var appOptions = new AppOptions { AppName = nameof(RedisCacheImplMemoryTest) };

            var settings = new RedisSharedCacheOptions
            {
                Host = "localhost",
                Password = "TeamCity"
            };

            var log = new Mock<ILog>().Object;
            var performanceLog = new Mock<IPerformanceLog>().Object;

            var redisCache = new RedisSharedCache(appOptions, new RedisConnectionFactory(settings), log, performanceLog);

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