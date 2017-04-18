using System;
using System.Diagnostics;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.TwoLayer;
using InfinniPlatform.MessageQueue.Abstractions.Producers;
using InfinniPlatform.Core.Abstractions.Logging;
using InfinniPlatform.Core.Abstractions.Settings;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.TwoLayer
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Manual")]
    public sealed class TwoLayerCacheImplPerformanceTest
    {
        private TwoLayerCacheImpl _cache;

        [SetUp]
        public void SetUp()
        {
            var appOptions = new AppOptions { AppName = nameof(TwoLayerCacheImplPerformanceTest) };

            var settings = new RedisConnectionSettings { Host = "localhost", Password = "TeamCity" };

            var log = new Mock<ILog>().Object;
            var performanceLog = new Mock<IPerformanceLog>().Object;

            var memoryCache = new MemoryCacheImpl();
            var redisCache = new RedisCacheImpl(appOptions, new RedisConnectionFactory(settings), log, performanceLog);
            var twoLayerCache = new TwoLayerCacheImpl(memoryCache, redisCache, appOptions, new Mock<IBroadcastProducer>().Object, new Mock<ILog>().Object);

            _cache = twoLayerCache;
        }


        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void GetPerformance(int iterations)
        {
            // Given

            const string key = "GetPerformance_Key";
            const string value = "GetPerformance_Value";

            _cache.Set(key, value);
            _cache.Get(key);

            // When

            var stopwatch = new Stopwatch();

            for (var i = 0; i < iterations; ++i)
            {
                stopwatch.Start();

                _cache.Get(key);

                stopwatch.Stop();
            }

            // Then
            var avg = stopwatch.Elapsed.TotalMilliseconds / iterations;
            Console.WriteLine(@"TwoLayerCacheImpl.Get()");
            Console.WriteLine(@"  Iteration count: {0}", iterations);
            Console.WriteLine(@"  Operation time : {0:N4} ms", avg);
            Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / avg);
        }

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void SetPerformance(int iterations)
        {
            // Given

            const string key = "SetPerformance_Key";
            const string value = "SetPerformance_Value";

            _cache.Set(key, value);
            _cache.Get(key);

            // When

            var stopwatch = new Stopwatch();

            for (var i = 0; i < iterations; ++i)
            {
                var newValue = Guid.NewGuid().ToString("N");

                stopwatch.Start();

                _cache.Set(key, newValue);

                stopwatch.Stop();
            }

            // Then
            var avg = stopwatch.Elapsed.TotalMilliseconds / iterations;
            Console.WriteLine(@"TwoLayerCacheImpl.Set()");
            Console.WriteLine(@"  Iteration count: {0}", iterations);
            Console.WriteLine(@"  Operation time : {0:N4} ms", avg);
            Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / avg);
        }
    }
}