using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Core.MessageQueue;
using InfinniPlatform.Helpers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class MessageQueueTopicIntegrationTest
	{
		private const int WaitTimeout = 5000;
		private const string ServiceName = "RabbitMQ";

		private const string ExchangeName = "IntegrationTestTopicExchange";
		private const string QueueName1 = "IntegrationTestTopicQueue1";
		private const string QueueName2 = "IntegrationTestTopicQueue2";

		private const string QueueKey1 = "warning.*";
		private const string QueueKey2 = "error.*";

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


		[TestFixtureSetUp]
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
												 var factory = new RabbitMqMessageQueueFactory(RabbitMqSettings.Default);
												 var publisher = factory.CreateMessageQueuePublisher();

												 // Публикация сообщений

												 foreach (var message in Messages1)
												 {
													 publisher.Publish(ExchangeName, "warning.something", null, message);
												 }

												 foreach (var message in Messages2)
												 {
													 publisher.Publish(ExchangeName, "error.something", null, message);
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
												 var factory = new RabbitMqMessageQueueFactory(RabbitMqSettings.Default);
												 var listener = factory.CreateMessageQueueListener();
												 var subscribtions = factory.CreateMessageQueueManager();

												 // Создание подписок

												 var exchange = subscribtions.CreateExchangeTopic(ExchangeName);

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