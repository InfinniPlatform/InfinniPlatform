using System;
using System.Threading;

using InfinniPlatform.Caching.Redis;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.Redis
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class RedisCacheMessageBusImplTest
	{
		[Test]
		public void GeneralPubSubTest()
		{
			// GIVEN

			var redisConnectionString = CachingHelpers.GetConfigRedisConnectionString();

			var messageBus = new RedisCacheMessageBusImpl("RedisCacheImplTest", redisConnectionString);

			// subscriber1 for Key1

			var subscriberCount1 = 0;
			var subscriberKey1 = string.Empty;
			var subscriberValue1 = string.Empty;
			var subscriberEvent1 = new AutoResetEvent(false);

			Action<string, string> subscriber1 = (k, v) =>
			{
				Interlocked.Increment(ref subscriberCount1);
				subscriberKey1 = k;
				subscriberValue1 = v;
				subscriberEvent1.Set();
			};

			messageBus.Subscribe("Key1", subscriber1);

			// subscriber2 for Key1

			var subscriberCount2 = 0;
			var subscriberKey2 = string.Empty;
			var subscriberValue2 = string.Empty;
			var subscriberEvent2 = new AutoResetEvent(false);

			Action<string, string> subscriber2 = (k, v) =>
			{
				Interlocked.Increment(ref subscriberCount2);
				subscriberKey2 = k;
				subscriberValue2 = v;
				subscriberEvent2.Set();
			};

			messageBus.Subscribe("Key1", subscriber2);

			// subscriber3 for Key2

			var subscriberCount3 = 0;
			var subscriberKey3 = string.Empty;
			var subscriberValue3 = string.Empty;
			var subscriberEvent3 = new AutoResetEvent(false);

			Action<string, string> subscriber3 = (k, v) =>
			{
				Interlocked.Increment(ref subscriberCount3);
				subscriberKey3 = k;
				subscriberValue3 = v;
				subscriberEvent3.Set();
			};

			messageBus.Subscribe("Key2", subscriber3);

			// subscriber4 for Key3

			var subscriberCount4 = 0;
			var subscriberKey4 = string.Empty;
			var subscriberValue4 = string.Empty;
			var subscriberEvent4 = new AutoResetEvent(false);

			Action<string, string> subscriber4 = (k, v) =>
			{
				Interlocked.Increment(ref subscriberCount4);
				subscriberKey4 = k;
				subscriberValue4 = v;
				subscriberEvent4.Set();
			};

			messageBus.Subscribe("Key3", subscriber4);

			// WHEN

			messageBus.Publish("Key1", "Value1");
			messageBus.Publish("Key2", "Value2");

			var subscriberComplete1 = subscriberEvent1.WaitOne(5000);
			var subscriberComplete2 = subscriberEvent2.WaitOne(5000);
			var subscriberComplete3 = subscriberEvent3.WaitOne(5000);
			var subscriberComplete4 = subscriberEvent4.WaitOne(5000);

			// THEN

			Assert.IsTrue(subscriberComplete1);
			Assert.AreEqual(1, subscriberCount1);
			Assert.AreEqual("Key1", subscriberKey1);
			Assert.AreEqual("Value1", subscriberValue1);

			Assert.IsTrue(subscriberComplete2);
			Assert.AreEqual(1, subscriberCount2);
			Assert.AreEqual("Key1", subscriberKey2);
			Assert.AreEqual("Value1", subscriberValue2);

			Assert.IsTrue(subscriberComplete3);
			Assert.AreEqual(1, subscriberCount3);
			Assert.AreEqual("Key2", subscriberKey3);
			Assert.AreEqual("Value2", subscriberValue3);

			Assert.IsFalse(subscriberComplete4);
			Assert.AreEqual(0, subscriberCount4);
			Assert.AreEqual(string.Empty, subscriberKey4);
			Assert.AreEqual(string.Empty, subscriberValue4);
		}
	}
}