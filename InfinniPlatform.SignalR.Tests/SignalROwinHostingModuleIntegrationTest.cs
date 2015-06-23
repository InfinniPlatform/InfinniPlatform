using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Sdk.Api;
using Microsoft.AspNet.SignalR.Client;

using NUnit.Framework;

namespace InfinniPlatform.SignalR.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class SignalROwinHostingModuleIntegrationTest
	{
		private static readonly HostingConfig HostingConfig = TestSettings.DefaultHostingConfig;


		[Test]
		public void ServerProxyShouldSendNotificationToAllClients()
		{
			// Given

			var hosting = new OwinHostingService(config => config.Configuration(HostingConfig));
			hosting.RegisterModule(new SignalROwinHostingModule());

			var serverProxy = new WebClientNotificationProxy();
			var receiveEvent = new CountdownEvent(3);

			// When

			hosting.Start();

			var webClient1 = new WebClientNotification(receiveEvent, "someEvent1");
			var webClient2 = new WebClientNotification(receiveEvent, "someEvent1");
			var webClient3 = new WebClientNotification(receiveEvent, "someEvent1");

			serverProxy.Notify("someEvent1", "eventBody1");

			receiveEvent.Wait(5000);

			hosting.Stop();

			// Then

			CollectionAssert.AreEquivalent(new[] { "eventBody1" }, webClient1.ReceiveMessages);
			CollectionAssert.AreEquivalent(new[] { "eventBody1" }, webClient2.ReceiveMessages);
			CollectionAssert.AreEquivalent(new[] { "eventBody1" }, webClient3.ReceiveMessages);
		}

		[Test]
		[Ignore("Manual")] // Почему-то SignalR в текущей версии не хочет "останавливаться"
		public void ServerProxyShouldSendNotificationToSpecifiedClients()
		{
			// Given

			var hosting = new OwinHostingService(config => config.Configuration(HostingConfig));
			hosting.RegisterModule(new SignalROwinHostingModule());

			var serverProxy = new WebClientNotificationProxy();
			var receiveEventWebClient1 = new CountdownEvent(1);
			var receiveEventWebClient2 = new CountdownEvent(1);
			var receiveEventWebClient3 = new CountdownEvent(1);

			// When

			hosting.Start();

			var webClient1 = new WebClientNotification(receiveEventWebClient1, "someEvent1");
			var webClient2 = new WebClientNotification(receiveEventWebClient2, "someEvent2");
			var webClient3 = new WebClientNotification(receiveEventWebClient3, "someEvent3");

			serverProxy.Notify("someEvent1", "eventBody1");
			serverProxy.Notify("someEvent2", "eventBody2");
			serverProxy.Notify("someEvent3", "eventBody3");

			receiveEventWebClient1.Wait(5000);
			receiveEventWebClient2.Wait(5000);
			receiveEventWebClient3.Wait(5000);

			hosting.Stop();

			// Then

			CollectionAssert.AreEquivalent(new[] { "eventBody1" }, webClient1.ReceiveMessages);
			CollectionAssert.AreEquivalent(new[] { "eventBody2" }, webClient2.ReceiveMessages);
			CollectionAssert.AreEquivalent(new[] { "eventBody3" }, webClient3.ReceiveMessages);
		}

		[Test]
		[Ignore("Manual")] // Почему-то SignalR в текущей версии не хочет "останавливаться"
		public void ClientsShouldToExchangeTogetherNotifications()
		{
			// Given

			var hosting = new OwinHostingService(config => config.Configuration(HostingConfig));
			hosting.RegisterModule(new SignalROwinHostingModule());

			var receiveEventWebClient1 = new CountdownEvent(1);
			var receiveEventWebClient2 = new CountdownEvent(1);

			// When

			hosting.Start();

			var webClient1 = new WebClientNotification(receiveEventWebClient1, "someEvent1");
			var webClient2 = new WebClientNotification(receiveEventWebClient2, "someEvent2");

			webClient1.Notify("someEvent2", "1to2");
			webClient2.Notify("someEvent1", "2to1");

			receiveEventWebClient1.Wait(5000);
			receiveEventWebClient2.Wait(5000);

			hosting.Stop();

			// Then

			CollectionAssert.AreEquivalent(new[] { "1to2" }, webClient2.ReceiveMessages);
			CollectionAssert.AreEquivalent(new[] { "2to1" }, webClient1.ReceiveMessages);
		}


		class WebClientNotification
		{
			public WebClientNotification(CountdownEvent receiveEvent, string routingKey)
			{
				var hubConnection = new HubConnection(string.Format("{0}://{1}:{2}/", HostingConfig.ServerScheme, HostingConfig.ServerName, HostingConfig.ServerPort));
				var hubProxy = hubConnection.CreateHubProxy("WebClientNotificationHub");
				hubProxy.On<object>(routingKey, OnReceive);
				hubConnection.Start().Wait();

				_receiveEvent = receiveEvent;
				_hubProxy = hubProxy;
			}


			private readonly CountdownEvent _receiveEvent;
			private readonly IHubProxy _hubProxy;

			public readonly List<object> ReceiveMessages = new List<object>();


			private void OnReceive(object message)
			{
				ReceiveMessages.Add(message);

				_receiveEvent.Signal();
			}


			public void Notify(string routingKey, object message)
			{
				_hubProxy.Invoke("Notify", routingKey, message);
			}
		}
	}
}