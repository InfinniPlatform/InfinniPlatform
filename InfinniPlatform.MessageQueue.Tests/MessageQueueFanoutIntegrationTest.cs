using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Core.MessageQueue;
using InfinniPlatform.Helpers;
using InfinniPlatform.Sdk.Logging;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class MessageQueueFanoutIntegrationTest
	{
		private const int WaitTimeout = 5000;
		private const string ServiceName = "RabbitMQ";

		private const string ExchangeName = "IntegrationTestFanoutExchange";
		private const string QueueName1 = "IntegrationTestFanoutQueue1";
		private const string QueueName2 = "IntegrationTestFanoutQueue2";

		private static readonly string[] Messages
			= new[]
				  {
					  "Message0",
					  "Message1",
					  "Message2",
					  "Message3",
					  "Message4",
					  "Message5",
					  "Message6",
					  "Message7",
					  "Message8",
					  "Message9"
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
			CollectionAssert.AreEquivalent(Messages, result[0]);
			CollectionAssert.AreEquivalent(Messages, result[1]);
		}


		private static void PublisherThread()
		{
			var consumeCompleted = new ManualResetEvent(false);

			ThreadPool.QueueUserWorkItem(state =>
											 {
												 var factory = new RabbitMqMessageQueueFactory(RabbitMqSettings.Default, new Mock<ILog>().Object);
												 var publisher = factory.CreateMessageQueuePublisher();

												 // Публикация сообщений

												 foreach (var message in Messages)
												 {
													 publisher.Publish(ExchangeName, null, null, message);
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

												 var exchange = subscribtions.CreateExchangeFanout(ExchangeName);

												 var completeConsumer1 = new CountdownEvent(Messages.Length);
												 var consumer1 = new TestConsumer(completeConsumer1);
												 exchange.Subscribe(QueueName1, () => consumer1);

												 var completeConsumer2 = new CountdownEvent(Messages.Length);
												 var consumer2 = new TestConsumer(completeConsumer2);
												 exchange.Subscribe(QueueName2, () => consumer2);

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