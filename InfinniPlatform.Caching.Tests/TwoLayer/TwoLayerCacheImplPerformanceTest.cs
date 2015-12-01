using System;
using System.Diagnostics;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.TwoLayer;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.TwoLayer
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Should setup Redis on TeamCity")]
    public sealed class TwoLayerCacheImplPerformanceTest
    {
        private ICache _cache;

        [SetUp]
        public void SetUp()
        {
            var cacheName = nameof(TwoLayerCacheImplPerformanceTest);
            var memoryCache = new MemoryCacheImpl(cacheName);
            var sharedCache = new RedisCacheImpl(cacheName, "localhost");
            var sharedCacheMessageBus = new RedisCacheMessageBusImpl(cacheName, "localhost");
            var twoLayerCache = new TwoLayerCacheImpl(memoryCache, sharedCache, sharedCacheMessageBus);

            _cache = twoLayerCache;
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_cache).Dispose();
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
            Console.WriteLine(@"  Operation time : {0:N4} sec", avg);
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
            Console.WriteLine(@"  Operation time : {0:N4} sec", avg);
            Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / avg);
        }
    }
}