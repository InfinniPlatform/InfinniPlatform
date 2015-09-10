using System;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Caching.Memory;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.Memory
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MemoryCacheMessageBusImplTest
	{
		[Test]
		public void GeneralPubSubTest()
		{
			// GIVEN

			var messageBus = new MemoryCacheMessageBusImpl();

			// subscriber1 for Key1

			var subscriberCount1 = 0;
			var subscriberKey1 = string.Empty;
			var subscriberValue1 = string.Empty;

			Action<string, string> subscriber1 = (k, v) =>
			{
				Interlocked.Increment(ref subscriberCount1);
				subscriberKey1 = k;
				subscriberValue1 = v;
			};

			messageBus.Subscribe("Key1", subscriber1);

			// subscriber2 for Key1

			var subscriberCount2 = 0;
			var subscriberKey2 = string.Empty;
			var subscriberValue2 = string.Empty;

			Action<string, string> subscriber2 = (k, v) =>
			{
				Interlocked.Increment(ref subscriberCount2);
				subscriberKey2 = k;
				subscriberValue2 = v;
			};

			messageBus.Subscribe("Key1", subscriber2);

			// subscriber3 for Key2

			var subscriberCount3 = 0;
			var subscriberKey3 = string.Empty;
			var subscriberValue3 = string.Empty;

			Action<string, string> subscriber3 = (k, v) =>
			{
				Interlocked.Increment(ref subscriberCount3);
				subscriberKey3 = k;
				subscriberValue3 = v;
			};

			messageBus.Subscribe("Key2", subscriber3);

			// subscriber4 for Key3

			var subscriberCount4 = 0;
			var subscriberKey4 = string.Empty;
			var subscriberValue4 = string.Empty;

			Action<string, string> subscriber4 = (k, v) =>
			{
				Interlocked.Increment(ref subscriberCount4);
				subscriberKey4 = k;
				subscriberValue4 = v;
			};

			messageBus.Subscribe("Key3", subscriber4);

			// WHEN

			var publishKey1 = messageBus.Publish("Key1", "Value1");
			var publishKey2 = messageBus.Publish("Key2", "Value2");
			Task.WaitAll(new[] { publishKey1, publishKey2 }, 5000);

			// THEN

			Assert.AreEqual(1, subscriberCount1);
			Assert.AreEqual("Key1", subscriberKey1);
			Assert.AreEqual("Value1", subscriberValue1);

			Assert.AreEqual(1, subscriberCount2);
			Assert.AreEqual("Key1", subscriberKey2);
			Assert.AreEqual("Value1", subscriberValue2);

			Assert.AreEqual(1, subscriberCount3);
			Assert.AreEqual("Key2", subscriberKey3);
			Assert.AreEqual("Value2", subscriberValue3);

			Assert.AreEqual(0, subscriberCount4);
			Assert.AreEqual(string.Empty, subscriberKey4);
			Assert.AreEqual(string.Empty, subscriberValue4);
		}
	}
}