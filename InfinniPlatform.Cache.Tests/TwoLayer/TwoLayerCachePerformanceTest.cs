using System;
using System.Diagnostics;

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
    public sealed class TwoLayerCachePerformanceTest
    {
        private TwoLayerCache _cache;

        [SetUp]
        public void SetUp()
        {
            var inMemoryCache = new InMemoryCache();
            var inMemoryCacheFactory = new Mock<IInMemoryCacheFactory>();
            inMemoryCacheFactory.Setup(i => i.Create()).Returns(inMemoryCache);

            var appOptions = new AppOptions { AppName = nameof(TwoLayerCachePerformanceTest) };
            var redisOptions = new RedisSharedCacheOptions { Host = "localhost", Password = "TeamCity" };
            var sharedCache = new RedisSharedCache(appOptions, new RedisConnectionFactory(redisOptions, new Mock<ILogger<RedisConnectionFactory>>().Object), new Mock<ILogger<RedisSharedCache>>().Object, new Mock<IPerformanceLogger<RedisSharedCache>>().Object);
            var sharedCacheFactory = new Mock<ISharedCacheFactory>();
            sharedCacheFactory.Setup(i => i.Create()).Returns(sharedCache);

            _cache = new TwoLayerCache(inMemoryCacheFactory.Object, sharedCacheFactory.Object, new Mock<ITwoLayerCacheStateObserver>().Object);
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
            Console.WriteLine(@"TwoLayerCache.Get()");
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
            Console.WriteLine(@"TwoLayerCache.Set()");
            Console.WriteLine(@"  Iteration count: {0}", iterations);
            Console.WriteLine(@"  Operation time : {0:N4} ms", avg);
            Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / avg);
        }
    }
}