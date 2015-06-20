using System.Collections.Generic;
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
        public void ShouldProvideUnsubscribeSubscription()
        {
            // Given
            const string messageType = "Type";
            var target = new SubscriptionList();
            Subscription subscription1 = target.AddSubscription(messageType, m => { });
            Subscription subscription2 = target.AddSubscription(messageType, m => { });

            // When

            Subscription[] subscriptions0 = target.GetSubscriptions(messageType).ToArray();

            subscription1.Dispose();
            Subscription[] subscriptions1 = target.GetSubscriptions(messageType).ToArray();

            subscription2.Dispose();
            Subscription[] subscriptions2 = target.GetSubscriptions(messageType).ToArray();

            // Then

            Assert.IsNotNull(subscriptions0);
            Assert.IsNotNull(subscriptions1);
            Assert.IsNotNull(subscriptions2);

            CollectionAssert.AreEquivalent(new[] {subscription1, subscription2}, subscriptions0);
            CollectionAssert.AreEquivalent(new[] {subscription2}, subscriptions1);
            CollectionAssert.AreEquivalent(new Subscription[] {}, subscriptions2);
        }

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

            Subscription subscription11 = target.AddSubscription(messageType1, m => { });
            Subscription subscription21 = target.AddSubscription(messageType2, m => { });
            Subscription subscription22 = target.AddSubscription(messageType2, m => { });
            Subscription subscription31 = target.AddSubscription(messageType3, m => { });
            Subscription subscription32 = target.AddSubscription(messageType3, m => { });
            Subscription subscription33 = target.AddSubscription(messageType3, m => { });

            IEnumerable<Subscription> subscriptions0 = target.GetSubscriptions(messageType0);
            IEnumerable<Subscription> subscriptions1 = target.GetSubscriptions(messageType1);
            IEnumerable<Subscription> subscriptions2 = target.GetSubscriptions(messageType2);
            IEnumerable<Subscription> subscriptions3 = target.GetSubscriptions(messageType3);

            // Then

            Assert.IsNotNull(subscriptions0);
            Assert.IsNotNull(subscriptions1);
            Assert.IsNotNull(subscriptions2);
            Assert.IsNotNull(subscriptions3);

            CollectionAssert.AreEquivalent(new Subscription[] {}, subscriptions0);
            CollectionAssert.AreEquivalent(new[] {subscription11}, subscriptions1);
            CollectionAssert.AreEquivalent(new[] {subscription21, subscription22}, subscriptions2);
            CollectionAssert.AreEquivalent(new[] {subscription31, subscription32, subscription33}, subscriptions3);
        }
    }
}