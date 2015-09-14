using System;
using InfinniPlatform.UserInterface.ViewBuilders.Messaging;
using NUnit.Framework;

namespace InfinniPlatform.UserInterface.Tests.ViewBuilders.Messaging
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class SubscriptionTest
    {
        [Test]
        public async void ShouldHandleAsync()
        {
            // Given

            object actualMessage = null;
            var expectedMessage = new object();

            Action unsubscribeAction = () => { };

            Action<dynamic> messageHandler = message => { actualMessage = message; };

            // When
            var subscription = new Subscription(unsubscribeAction, messageHandler);
            await subscription.HandleAsync(expectedMessage);

            // Then
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void ShouldUnsubscribeWhenDispose()
        {
            // Given

            bool unsubscribe = false;

            Action unsubscribeAction = () => { unsubscribe = true; };

            Action<dynamic> messageHandler = message => { };

            // When
            var subscription = new Subscription(unsubscribeAction, messageHandler);
            subscription.Dispose();

            // Then
            Assert.IsTrue(unsubscribe);
        }
    }
}