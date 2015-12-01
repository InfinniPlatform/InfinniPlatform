using System;
using System.Diagnostics;
using System.Threading;

using InfinniPlatform.Caching.Memory;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.Memory
{
	[TestFixture]
	[Category(TestCategories.PerformanceTest)]
	[Ignore("Should setup Redis on TeamCity")]
	public sealed class MemoryCacheMessageBusImplPerformanceTest
	{
		private ICacheMessageBus _cacheMessageBus;

		[SetUp]
		public void SetUp()
		{
			_cacheMessageBus = new MemoryCacheMessageBusImpl();
		}

		[TearDown]
		public void TearDown()
		{
			((IDisposable)_cacheMessageBus).Dispose();
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

			_cacheMessageBus.Publish(key, value); // JIT

			for (var i = 0; i < subscriptions; ++i)
			{
				_cacheMessageBus.Subscribe(key, (k, v) => completeEvent.Signal());
			}

			// When

			var totalTime = new Stopwatch();
			var publishTimer = new Stopwatch();

			totalTime.Start();

			for (var i = 0; i < publications; ++i)
			{
				publishTimer.Start();

				_cacheMessageBus.Publish(key, value);

				publishTimer.Stop();
			}

			var isComplete = completeEvent.Wait(120 * 1000);

			totalTime.Stop();

			// Then

			Assert.IsTrue(isComplete, "Too long...");

			var publishTime = publishTimer.Elapsed.TotalMilliseconds / publications;
			Console.WriteLine(@"MemoryCacheMessageBusImpl: Publish");
			Console.WriteLine(@"  Iteration count: {0}", publications);
			Console.WriteLine(@"  Operation time : {0:N4} sec", publishTime);
			Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / publishTime);

			var deliveryTime = totalTime.Elapsed.TotalMilliseconds / (publications * subscriptions);
			Console.WriteLine(@"MemoryCacheMessageBusImpl: Delivery");
			Console.WriteLine(@"  Iteration count: {0}", publications * subscriptions);
			Console.WriteLine(@"  Operation time : {0:N4} sec", deliveryTime);
			Console.WriteLine(@"  Operation/sec  : {0:N4}", 1000 / deliveryTime);
		}
	}
}