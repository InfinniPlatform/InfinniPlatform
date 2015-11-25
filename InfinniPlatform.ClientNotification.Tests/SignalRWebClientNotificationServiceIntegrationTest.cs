using System;

using InfinniPlatform.NodeServiceHost;

using NUnit.Framework;

namespace InfinniPlatform.ClientNotification.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class SignalRWebClientNotificationServiceIntegrationTest
    {
        private IDisposable _server;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _server = InfinniPlatformInprocessHost.Start();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldNotifyClients()
        {
            // Given
            var notificationServiceFactory = new SignalRWebClientNotificationServiceFactory();
            var notificationService = notificationServiceFactory.CreateClientNotificationService();

            // When
            TestDelegate notifyClients = () => notificationService.Notify("someEvent1", "eventBody1");

            // Then
            Assert.DoesNotThrow(notifyClients);
        }
    }
}