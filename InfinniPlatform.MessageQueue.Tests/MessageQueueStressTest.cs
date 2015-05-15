using System.Collections.Generic;
using System.Linq;
using System.Threading;

using InfinniPlatform.Helpers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests
{
	[TestFixture]
	[Category(TestCategories.StressTest)]
	public sealed class MessageQueueStressTest
	{
		private const int WaitTimeout = 30000;
		private const string ServiceName = "RabbitMQ";

		private const string ExchangeName = "StressTestFanoutExchange";
		private const string QueueName1 = "StressTestFanoutQueue1";
		private const string QueueName2 = "StressTestFanoutQueue2";

		private static readonly string[] BeforeRestartMessages
			= new[]
				  {
					  "Message0",
					  "Message1",
					  "Message2",
					  "Message3",
					  "Message4"
				  };

		private static readonly string[] AfterRestartMessages
			= new[]
				  {
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
			CollectionAssert.AreEquivalent(BeforeRestartMessages.Concat(AfterRestartMessages), result[0]);
			CollectionAssert.AreEquivalent(BeforeRestartMessages.Concat(AfterRestartMessages), result[1]);
		}


		private static void PublisherThread()
		{
			var consumeCompleted = new ManualResetEvent(false);

			ThreadPool.QueueUserWorkItem(state =>
											 {
												 var factory = new RabbitMqMessageQueueFactory();
												 var publisher = factory.CreateMessageQueuePublisher();

												 // Начало публикации сообщений

												 foreach (var message in BeforeRestartMessages)
												 {
													 publisher.Publish(ExchangeName, null, null, message);
												 }

												 // Перезапуск службы очереди сообщений
												 RestartMessageQueueService();

												 // Продолжение публикации сообщений

												 foreach (var message in AfterRestartMessages)
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
												 var factory = new RabbitMqMessageQueueFactory();
												 var listener = factory.CreateMessageQueueListener();
												 var subscribtions = factory.CreateMessageQueueManager();

												 // Создание подписок

												 var exchange = subscribtions.CreateExchangeFanout(ExchangeName, config => config.Durable());

												 var completeConsumer1 = new CountdownEvent(BeforeRestartMessages.Length + AfterRestartMessages.Length);
												 var consumer1 = new TestConsumer(completeConsumer1);
												 exchange.Subscribe(QueueName1, () => consumer1, config => config.Durable());

												 var completeConsumer2 = new CountdownEvent(BeforeRestartMessages.Length + AfterRestartMessages.Length);
												 var consumer2 = new TestConsumer(completeConsumer2);
												 exchange.Subscribe(QueueName2, () => consumer2, config => config.Durable());

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

		private static void RestartMessageQueueService()
		{
			WindowsServices.RestartService(ServiceName, WaitTimeout);
		}
	}
}