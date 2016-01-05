using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Core.MessageQueue;
using InfinniPlatform.Helpers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class MessageQueueHeadersIntegrationTest
	{
		private const int WaitTimeout = 5000;
		private const string ServiceName = "RabbitMQ";

		private const string ExchangeName = "IntegrationTestHeadersExchange";
		private const string QueueName1 = "IntegrationTestHeadersQueue1";
		private const string QueueName2 = "IntegrationTestHeadersQueue2";

		private static readonly MessageHeaders QueueHeaders1
			= new MessageHeaders(new Dictionary<string, byte[]>
				                     {
					                     { "Header11", new byte[] { 11, 12, 13 } },
					                     { "Header12", new byte[] { 14, 15, 16 } }
				                     });
		private static readonly MessageHeaders QueueHeaders2
			= new MessageHeaders(new Dictionary<string, byte[]>
				                     {
					                     { "Header21", new byte[] { 21, 22, 23 } },
					                     { "Header22", new byte[] { 24, 25, 26 } }
				                     });

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

												 var properties1 = new MessageProperties { Headers = QueueHeaders1 };

												 foreach (var message in Messages1)
												 {
													 publisher.Publish(ExchangeName, null, properties1, message);
												 }

												 var properties2 = new MessageProperties { Headers = QueueHeaders2 };

												 foreach (var message in Messages2)
												 {
													 publisher.Publish(ExchangeName, null, properties2, message);
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

												 var exchange = subscribtions.CreateExchangeHeaders(ExchangeName);

												 var completeConsumer1 = new CountdownEvent(Messages1.Length);
												 var consumer1 = new TestConsumer(completeConsumer1);
												 exchange.Subscribe(QueueName1, () => consumer1, QueueHeaders1);

												 var completeConsumer2 = new CountdownEvent(Messages2.Length);
												 var consumer2 = new TestConsumer(completeConsumer2);
												 exchange.Subscribe(QueueName2, () => consumer2, QueueHeaders2);

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