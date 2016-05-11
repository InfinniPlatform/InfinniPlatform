using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using InfinniPlatform.Helpers;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests
{
	[TestFixture]
	[Category(TestCategories.PerformanceTest)]
	public sealed class MessageQueuePerformanceTest
	{
		private const int WaitTimeout = 30000;
		private const string ServiceName = "RabbitMQ";

		private const int IterationCount = 100;
		private const string ExchangeName = "PerformanceTestExchange";
		private const string QueueName = "PerformanceTestQueue";


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

			var stopwatch = new Stopwatch();
			var message = CreateMessage(messageSize);
			var publisher = GetMessageQueuePublisher();

			// When

			for (var i = 0; i < IterationCount; ++i)
			{
				stopwatch.Start();

				publisher.Publish(ExchangeName, null, null, message);

				stopwatch.Stop();
			}

			// Then

			const int totalMessages = IterationCount;
			var totalSeconds = stopwatch.Elapsed.TotalSeconds;

			Console.WriteLine(@"Message size: {0} Kb", messageSize / 1024);
			Console.WriteLine(@"Total time: {0:N4} sec", totalSeconds);
			Console.WriteLine(@"Publish time: {0:N4} ms/message", 1000 * totalSeconds / totalMessages);
			Console.WriteLine(@"Publish speed: {0:N4} message/sec", totalMessages / totalSeconds);
			Console.WriteLine(@"Publish bitrate: {0:N4} kbps", 8.0 * messageSize * totalMessages / (1024 * totalSeconds));
		}

		[Test]
		[TestCase(1024)]
		[TestCase(10240)]
		[TestCase(102400)]
		[TestCase(1024000)]
		public void PublishSpeedMultithreadTest(int messageSize)
		{
			// Given

			const int threadCount = 5;
			var result = new List<TimeSpan>();
			var message = CreateMessage(messageSize);
			var publisher = GetMessageQueuePublisher();

			var publishEvent = new ManualResetEvent(false);
			var startedEvent = new CountdownEvent(threadCount);
			var stoppedEvent = new CountdownEvent(threadCount);

			// When

			for (var t = 0; t < threadCount; ++t)
			{
				var index = t;

				result.Add(TimeSpan.Zero);

				ThreadPool.QueueUserWorkItem(state =>
												 {
													 startedEvent.Signal();
													 publishEvent.WaitOne();

													 try
													 {
														 var stopwatch = new Stopwatch();

														 for (var i = 0; i < IterationCount; ++i)
														 {
															 stopwatch.Start();

															 publisher.Publish(ExchangeName, null, null, message);

															 stopwatch.Stop();
														 }

														 result[index] = stopwatch.Elapsed;
													 }
													 finally
													 {
														 stoppedEvent.Signal();
													 }
												 });
			}

			startedEvent.Wait();
			publishEvent.Set();
			stoppedEvent.Wait();

			// Then

			const int totalMessages = threadCount * IterationCount;
			var totalSeconds = result.Sum(i => i.TotalSeconds);

			Console.WriteLine(@"Message size: {0} Kb", messageSize / 1024);
			Console.WriteLine(@"Total time: {0:N4} sec", totalSeconds);
			Console.WriteLine(@"Publish time: {0:N4} ms/message", 1000 * totalSeconds / totalMessages);
			Console.WriteLine(@"Publish speed: {0:N4} message/sec", totalMessages / totalSeconds);
			Console.WriteLine(@"Publish bitrate: {0:N4} kbps", 8.0 * messageSize * totalMessages / (1024 * totalSeconds));
		}


		[Test]
		[TestCase(1024)]
		[TestCase(10240)]
		[TestCase(102400)]
		[TestCase(1024000)]
		public void DeliverySpeedTest(int messageSize)
		{
			// Given

			var stopwatch = new Stopwatch();
			var message = CreateMessage(messageSize);

			var factory = new RabbitMqMessageQueueFactory(RabbitMqSettings.Default, new Mock<ILog>().Object);
			var publisher = factory.CreateMessageQueuePublisher();
			var listener = factory.CreateMessageQueueListener();
			var subscriptions = factory.CreateMessageQueueManager();

			var completedEvent = new CountdownEvent(IterationCount);
			var consumer = new CountdownConsumer(completedEvent);
			subscriptions.CreateExchangeFanout(ExchangeName).Subscribe(QueueName, () => consumer);
			listener.StartListenAll();

			// When

			stopwatch.Start();

			for (var i = 0; i < IterationCount; ++i)
			{
				publisher.Publish(ExchangeName, null, null, message);
			}

			completedEvent.Wait(WaitTimeout);

			stopwatch.Stop();

			listener.StopListenAll();

			// Then

			const int totalMessages = IterationCount;
			var totalSeconds = stopwatch.Elapsed.TotalSeconds;

			Console.WriteLine(@"Message size: {0} Kb", messageSize / 1024);
			Console.WriteLine(@"Total time: {0:N4} sec", totalSeconds);
			Console.WriteLine(@"Publish time: {0:N4} ms/message", 1000 * totalSeconds / totalMessages);
			Console.WriteLine(@"Publish speed: {0:N4} message/sec", totalMessages / totalSeconds);
			Console.WriteLine(@"Publish bitrate: {0:N4} kbps", 8.0 * messageSize * totalMessages / (1024 * totalSeconds));
		}

		[Test]
		[TestCase(1024)]
		[TestCase(10240)]
		[TestCase(102400)]
		[TestCase(1024000)]
		public void DeliverySpeedMultithreadTest(int messageSize)
		{
			// Given

			const int threadCount = 5;
			var stopwatch = new Stopwatch();
			var message = CreateMessage(messageSize);

			var factory = new RabbitMqMessageQueueFactory(RabbitMqSettings.Default, new Mock<ILog>().Object);
			var publisher = factory.CreateMessageQueuePublisher();
			var listener = factory.CreateMessageQueueListener();
			var subscriptions = factory.CreateMessageQueueManager();

			var publishEvent = new ManualResetEvent(false);
			var startedEvent = new CountdownEvent(threadCount);
			var completedEvent = new CountdownEvent(threadCount * IterationCount);
			var consumer = new CountdownConsumer(completedEvent);
			subscriptions.CreateExchangeFanout(ExchangeName).Subscribe(QueueName, () => consumer);
			listener.StartListenAll();

			// When

			for (var t = 0; t < threadCount; ++t)
			{
				ThreadPool.QueueUserWorkItem(state =>
												 {
													 startedEvent.Signal();
													 publishEvent.WaitOne();

													 for (var i = 0; i < IterationCount; ++i)
													 {
														 publisher.Publish(ExchangeName, null, null, message);
													 }
												 });
			}

			startedEvent.Wait();

			stopwatch.Start();
			publishEvent.Set();
			completedEvent.Wait(WaitTimeout);
			stopwatch.Stop();

			listener.StopListenAll();

			// Then

			const int totalMessages = threadCount * IterationCount;
			var totalSeconds = stopwatch.Elapsed.TotalSeconds;

			Console.WriteLine(@"Message size: {0} Kb", messageSize / 1024);
			Console.WriteLine(@"Total time: {0:N4} sec", totalSeconds);
			Console.WriteLine(@"Publish time: {0:N4} ms/message", 1000 * totalSeconds / totalMessages);
			Console.WriteLine(@"Publish speed: {0:N4} message/sec", totalMessages / totalSeconds);
			Console.WriteLine(@"Publish bitrate: {0:N4} kbps", 8.0 * messageSize * totalMessages / (1024 * totalSeconds));
		}


		private static byte[] CreateMessage(int messageSize)
		{
			var message = new byte[messageSize];
			new Random().NextBytes(message);
			return message;
		}

		private static IMessageQueuePublisher GetMessageQueuePublisher()
		{
			var factory = new RabbitMqMessageQueueFactory(RabbitMqSettings.Default, new Mock<ILog>().Object);
			var publisher = factory.CreateMessageQueuePublisher();
			var subscriptions = factory.CreateMessageQueueManager();
			subscriptions.CreateExchangeFanout(ExchangeName).Subscribe(QueueName, () => null);

			return publisher;
		}
	}
}