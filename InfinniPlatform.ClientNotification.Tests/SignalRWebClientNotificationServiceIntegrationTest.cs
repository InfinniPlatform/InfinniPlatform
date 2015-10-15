using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.SignalR;

using NUnit.Framework;

namespace InfinniPlatform.ClientNotification.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class SignalRWebClientNotificationServiceIntegrationTest
	{
		[Test]
		public void ShouldNotifyClients()
		{
			// Given

			var hosting = new OwinHostingService(config => config.Configuration(HostingConfig.Default));
			hosting.RegisterModule(new SignalROwinHostingModule());

			var notificationServiceFactory = new SignalRWebClientNotificationServiceFactory();
			var notificationService = notificationServiceFactory.CreateClientNotificationService();

			// When

			TestDelegate notifyClients
				= () =>
					  {
						  hosting.Start();

						  notificationService.Notify("someEvent1", "eventBody1");

						  hosting.Stop();
					  };

			// Then

			Assert.DoesNotThrow(notifyClients);
		}
	}
}