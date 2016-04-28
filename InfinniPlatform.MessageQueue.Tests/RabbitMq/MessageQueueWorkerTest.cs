using System;
using System.IO;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageQueueWorkerTest
	{
		[Test]
		public void ShouldStartAndStop()
		{
			// Given
			var queueConfig = GetQueueConfig();
			var target = new MessageQueueWorker(queueConfig, GetConsumer, GetCommandExecutor(), GetMessageQueue);

			// When
			target.Start();
			target.Stop();
			target.Start();
			target.Stop();

			// Then

		}


		private static QueueConfig GetQueueConfig()
		{
			var result = new QueueConfig("Exchange1", "Queue1");

			IQueueConfig config = result;

			config.ConsumerId("Consumer1")
				  .PrefetchCount(10)
				  .WorkerThreadCount(2)
				  .MinListenTime(60000);

			return result;
		}

		private static IQueueConsumer GetConsumer()
		{
			var result = new Mock<IQueueConsumer>();

			return result.Object;
		}

		private static IMessageQueueCommandExecutor GetCommandExecutor()
		{
			var session = new Mock<IMessageQueueSession>();
			var result = new Mock<IMessageQueueCommandExecutor>();
			result.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>())).Callback<Action<IMessageQueueSession>>(command => command(session.Object));

			return result.Object;
		}

		private static IMessageQueue GetMessageQueue(IMessageQueueSession session)
		{
			var result = new Mock<IMessageQueue>();

			var isClosed = false;

			result.Setup(m => m.Dispose()).Callback(() =>
														{
															isClosed = true;
														});

			result.Setup(m => m.Dequeue()).Callback(() =>
														{
															if (isClosed)
															{
																throw new IOException();
															}
														});

			return result.Object;
		}
	}
}