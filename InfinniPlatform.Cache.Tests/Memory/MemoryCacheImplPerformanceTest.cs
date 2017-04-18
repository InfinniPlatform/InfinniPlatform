using System;
using System.Diagnostics;
using InfinniPlatform.Cache.Memory;
using NUnit.Framework;

namespace InfinniPlatform.Cache.Tests.Memory
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Manual")]
    public sealed class MemoryCacheImplPerformanceTest
    {
        private MemoryCacheImpl _cache;

        [SetUp]
        public void SetUp()
        {
            _cache = new MemoryCacheImpl();
        }

        [TearDown]
        public void TearDown()
        {
            _cache.Dispose();
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
            Console.WriteLine(@"MemoryCacheImpl.Get()");
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
            Console.WriteLine(@"MemoryCacheImpl.Set()");
            Console.WriteLine(@"  Iteration count: {0}", iterations);
            Console.WriteLine(@"  Operation time : {0:N4} sec", avg);
            Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / avg);
        }
    }
}