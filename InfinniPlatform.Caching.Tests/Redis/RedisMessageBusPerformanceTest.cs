using System;
using System.Diagnostics;
using System.Threading;

using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Sdk.Logging;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.Redis
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    [Ignore("Manual")]
    public sealed class RedisMessageBusPerformanceTest
    {
        private MessageBusImpl _messageBus;

        [SetUp]
        public void SetUp()
        {
            var cacheName = GetType().Name;

            var settings = new RedisConnectionSettings
            {
                Host = "localhost",
                Password = "TeamCity"
            };

            var log = new Mock<ILog>().Object;
            var performanceLog = new Mock<IPerformanceLog>().Object;

            var redisMessageBusManager = new RedisMessageBusManager(cacheName, new RedisConnectionFactory(settings), log, performanceLog);
            var redisMessageBusPublisher = new RedisMessageBusPublisher(cacheName, new RedisConnectionFactory(settings), log, performanceLog);
            var redisMessageBus = new MessageBusImpl(redisMessageBusManager, redisMessageBusPublisher);

            _messageBus = redisMessageBus;
        }

        [Test]
        [TestCase(1000, 100)]
        [TestCase(1000, 1000)]
        [TestCase(1000, 10000)]
        public void PubSubPerformanceTest(int publications, int subscriptions)
        {
            // Given

            const string key = "PubSubPerformanceTest_Key";
            const string value = "PubSubPerformanceTest_Value";

            var completeEvent = new CountdownEvent(publications * subscriptions);

            _messageBus.Publish(key, value); // JIT

            for (var i = 0; i < subscriptions; ++i)
            {
                _messageBus.Subscribe(key, (k, v) => completeEvent.Signal());
            }

            // When

            var totalTime = new Stopwatch();
            var publishTimer = new Stopwatch();

            totalTime.Start();

            for (var i = 0; i < publications; ++i)
            {
                publishTimer.Start();

                _messageBus.Publish(key, value);

                publishTimer.Stop();
            }

            var isComplete = completeEvent.Wait(120 * 1000);

            totalTime.Stop();

            // Then

            Assert.IsTrue(isComplete, "Too long...");

            var publishTime = publishTimer.Elapsed.TotalMilliseconds / publications;
            Console.WriteLine(@"RedisCacheMessageBusImpl: Publish");
            Console.WriteLine(@"  Iteration count: {0}", publications);
            Console.WriteLine(@"  Operation time : {0:N4} sec", publishTime);
            Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / publishTime);

            var deliveryTime = totalTime.Elapsed.TotalMilliseconds / (publications * subscriptions);
            Console.WriteLine(@"RedisCacheMessageBusImpl: Delivery");
            Console.WriteLine(@"  Iteration count: {0}", publications * subscriptions);
            Console.WriteLine(@"  Operation time : {0:N4} sec", deliveryTime);
            Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / deliveryTime);
        }
    }
}