using System;
using System.Threading;

using InfinniPlatform.Helpers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests
{
	[TestFixture]
	[Category(TestCategories.MemoryLeakTest)]
	public sealed class MessageQueueMemoryTest
	{
		private const int WaitTimeout = 30000;
		private const string ServiceName = "RabbitMQ";

		private const int IterationCount = 100;
		private const string ExchangeName = "MemoryTestExchange";
		private const string QueueName = "MemoryTestQueue";


		[SetUp]
		public void SetUp()
		{
			WindowsServices.RestartService(ServiceName, WaitTimeout);
		}

		[Test]
		[TestCase(1024)]
		[TestCase(10240)]
		[TestCase(102400)]
		[TestCase(1024000)]
		public void PublishSpeedTest(int messageSize)
		{
			// Given

			var factory = new RabbitMqMessageQueueFactory();
			var publisher = factory.CreateMessageQueuePublisher();
			var listener = factory.CreateMessageQueueListener();
			var subscriptions = factory.CreateMessageQueueManager();

			var completedEvent = new CountdownEvent(IterationCount);
			var consumer = new CountdownConsumer(completedEvent);
			subscriptions.CreateExchangeFanout(ExchangeName).Subscribe(QueueName, () => consumer);

			// When

			var memoryBefore = GC.GetTotalMemory(true);

			listener.StartListenAll();

			for (var i = 0; i < IterationCount; ++i)
			{
				var message = CreateMessage(messageSize);
				publisher.Publish(ExchangeName, null, null, message);
			}

			listener.StopListenAll();

            GC.Collect();
            GC.WaitForPendingFinalizers();
			var memoryAfter = GC.GetTotalMemory(true);

			// Then

			var transferSize = IterationCount * messageSize / 1024.0;
			var memoryLeak = (memoryAfter - memoryBefore) / 1024.0;
			var memeryLeakPercent = 100 * memoryLeak / transferSize;

			Console.WriteLine("Message size: {0} Kb", messageSize / 1024);
			Console.WriteLine("Transfer messages: {0} messages ({1:N0} Kb)", IterationCount, transferSize);
			Console.WriteLine("Memory leak: {0:N0} Kb ({1:N0}%)", memoryLeak, memeryLeakPercent);

			Assert.LessOrEqual(memeryLeakPercent, 10);
		}


		private static readonly Random Random = new Random();

		private static byte[] CreateMessage(int messageSize)
		{
			var message = new byte[messageSize];
			Random.NextBytes(message);
			return message;
		}
	}
}