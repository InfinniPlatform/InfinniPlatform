using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Helpers;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.Outdated
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class MessageQueueDirectIntegrationTest
	{
		private const int WaitTimeout = 5000;
		private const string ServiceName = "RabbitMQ";

		private const string ExchangeName = "IntegrationTestDirectExchange";
		private const string QueueName1 = "IntegrationTestDirectQueue1";
		private const string QueueName2 = "IntegrationTestDirectQueue2";

		private const string QueueKey1 = "RoutingKey1";
		private const string QueueKey2 = "RoutingKey2";

		private static readonly string[] Messages1
			= new[]
				  {
					  "Message10",
					  "Message11",
					  "Message12",
					  "Message13",
					  "Message14"
				  };

		private static readonly string[] Messages2
			= new[]
				  {
					  "Message20",
					  "Message21",
					  "Message22",
					  "Message23",
					  "Message24"
				  };


		[OneTimeSetUp]
		public void SetUp()
		{
			WindowsServices.StartService(ServiceName, WaitTimeout);
		}

		[Test]
		public void ShouldDeliverAllMessages()
		{
			// Given
			var result = new List<string[]>();

			// When

			var consumeCompleted = ListenerThread(result);

			PublisherThread();

			consumeCompleted.WaitOne(WaitTimeout);

			// Then
			CollectionAssert.AreEquivalent(Messages1, result[0]);
			CollectionAssert.AreEquivalent(Messages2, result[1]);
		}


		private static void PublisherThread()
		{
			var consumeCompleted = new ManualResetEvent(false);

			ThreadPool.QueueUserWorkItem(state =>
											 {
												 var factory = new RabbitMqMessageQueueFactory(RabbitMqSettings.Default, new Mock<ILog>().Object);
												 var publisher = factory.CreateMessageQueuePublisher();

												 // Публикация сообщений

												 foreach (var message in Messages1)
												 {
													 publisher.Publish(ExchangeName, QueueKey1, null, message);
												 }

												 foreach (var message in Messages2)
												 {
													 publisher.Publish(ExchangeName, QueueKey2, null, message);
												 }

												 consumeCompleted.Set();
											 });

			consumeCompleted.WaitOne(WaitTimeout);
		}

		private static EventWaitHandle ListenerThread(ICollection<string[]> result)
		{
			var subscribeCompleted = new ManualResetEvent(false);
			var consumeCompleted = new ManualResetEvent(false);

			ThreadPool.QueueUserWorkItem(state =>
											 {
												 var factory = new RabbitMqMessageQueueFactory(RabbitMqSettings.Default, new Mock<ILog>().Object);
												 var listener = factory.CreateMessageQueueListener();
												 var subscribtions = factory.CreateMessageQueueManager();

												 // Создание подписок

												 var exchange = subscribtions.CreateExchangeDirect(ExchangeName);

												 var completeConsumer1 = new CountdownEvent(Messages1.Length);
												 var consumer1 = new TestConsumer(completeConsumer1);
												 exchange.Subscribe(QueueName1, () => consumer1, QueueKey1);

												 var completeConsumer2 = new CountdownEvent(Messages2.Length);
												 var consumer2 = new TestConsumer(completeConsumer2);
												 exchange.Subscribe(QueueName2, () => consumer2, QueueKey2);

												 subscribeCompleted.Set();

												 // Запуск прослушивания

												 listener.StartListenAll();

												 // Ожидание окончания приема

												 WaitHandle.WaitAll(new[] { completeConsumer1.WaitHandle, completeConsumer2.WaitHandle }, WaitTimeout);
												 result.Add(consumer1.Messages);
												 result.Add(consumer2.Messages);

												 consumeCompleted.Set();

												 // Остановка прослушивания

												 listener.StopListenAll();
											 });

			subscribeCompleted.WaitOne(WaitTimeout);

			return consumeCompleted;
		}
	}
}