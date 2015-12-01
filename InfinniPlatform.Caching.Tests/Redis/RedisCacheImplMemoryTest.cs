using System;

using InfinniPlatform.Caching.Factory;
using InfinniPlatform.Sdk.Environment.Settings;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.Redis
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Should setup Redis on TeamCity")]
    public sealed class RedisCacheImplMemoryTest
    {
        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void MemoryTest(int iterations)
        {
            // Given

            var appConfigMock = new Mock<IAppConfiguration>();
            appConfigMock.Setup(m => m.GetSection<CacheSettings>(CacheSettings.SectionName)).Returns(new CacheSettings());
            appConfigMock.Setup(m => m.GetSection<RedisSettings>(RedisSettings.SectionName)).Returns(new RedisSettings());

            const string key = "GetMemoryTest_Key";

            double startSize = GC.GetTotalMemory(true);

            // When

            var cache = new CacheFactory(appConfigMock.Object, new CacheMessageBusFactory(appConfigMock.Object)).GetSharedCache();

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