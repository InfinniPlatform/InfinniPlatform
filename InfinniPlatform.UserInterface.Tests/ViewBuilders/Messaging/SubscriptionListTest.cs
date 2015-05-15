using System.Linq;

using InfinniPlatform.UserInterface.ViewBuilders.Messaging;

using NUnit.Framework;

namespace InfinniPlatform.UserInterface.Tests.ViewBuilders.Messaging
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class SubscriptionListTest
	{
		[Test]
		public void ShouldSubscribe()
		{
			// Given
			const string messageType0 = "Type0";
			const string messageType1 = "Type1";
			const string messageType2 = "Type2";
			const string messageType3 = "Type3";
			var target = new SubscriptionList();

			// When

			var subscription11 = target.AddSubscription(messageType1, m => { });
			var subscription21 = target.AddSubscription(messageType2, m => { });
			var subscription22 = target.AddSubscription(messageType2, m => { });
			var subscription31 = target.AddSubscription(messageType3, m => { });
			var subscription32 = target.AddSubscription(messageType3, m => { });
			var subscription33 = target.AddSubscription(messageType3, m => { });

			var subscriptions0 = target.GetSubscriptions(messageType0);
			var subscriptions1 = target.GetSubscriptions(messageType1);
			var subscriptions2 = target.GetSubscriptions(messageType2);
			var subscriptions3 = target.GetSubscriptions(messageType3);

			// Then

			Assert.IsNotNull(subscriptions0);
			Assert.IsNotNull(subscriptions1);
			Assert.IsNotNull(subscriptions2);
			Assert.IsNotNull(subscriptions3);

			CollectionAssert.AreEquivalent(new Subscription[] { }, subscriptions0);
			CollectionAssert.AreEquivalent(new[] { subscription11 }, subscriptions1);
			CollectionAssert.AreEquivalent(new[] { subscription21, subscription22 }, subscriptions2);
			CollectionAssert.AreEquivalent(new[] { subscription31, subscription32, subscription33 }, subscriptions3);
		}

		[Test]
		public void ShouldProvideUnsubscribeSubscription()
		{
			// Given
			const string messageType = "Type";
			var target = new SubscriptionList();
			var subscription1 = target.AddSubscription(messageType, m => { });
			var subscription2 = target.AddSubscription(messageType, m => { });

			// When

			var subscriptions0 = target.GetSubscriptions(messageType).ToArray();

			subscription1.Dispose();
			var subscriptions1 = target.GetSubscriptions(messageType).ToArray();

			subscription2.Dispose();
			var subscriptions2 = target.GetSubscriptions(messageType).ToArray();

			// Then

			Assert.IsNotNull(subscriptions0);
			Assert.IsNotNull(subscriptions1);
			Assert.IsNotNull(subscriptions2);

			CollectionAssert.AreEquivalent(new[] { subscription1, subscription2 }, subscriptions0);
			CollectionAssert.AreEquivalent(new[] { subscription2 }, subscriptions1);
			CollectionAssert.AreEquivalent(new Subscription[] { }, subscriptions2);
		}
	}
}