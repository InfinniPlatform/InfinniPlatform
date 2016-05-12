using InfinniPlatform.MessageQueue.RabbitMq.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.Outdated.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageQueueListenerTest
	{
		[Test]
		public void ShouldStartRegisteredWorkers()
		{
			// Given
			var worker1 = new Mock<IMessageQueueWorker>();
			var worker2 = new Mock<IMessageQueueWorker>();
			var target = new MessageQueueListener();

			// When
			target.RegisterWorker("queue1", worker1.Object);
			target.RegisterWorker("queue2", worker2.Object);
			target.StartListenAll();

			// Then
			worker1.Verify(m => m.Start(), Times.Once());
			worker1.Verify(m => m.Stop(), Times.Never());
			worker2.Verify(m => m.Start(), Times.Once());
			worker2.Verify(m => m.Stop(), Times.Never());
		}

		[Test]
		public void ShouldStartSpecifiedWorker()
		{
			// Given
			var worker1 = new Mock<IMessageQueueWorker>();
			var worker2 = new Mock<IMessageQueueWorker>();
			var target = new MessageQueueListener();

			// When
			target.RegisterWorker("queue1", worker1.Object);
			target.RegisterWorker("queue2", worker2.Object);
			target.StartListen("queue1");

			// Then
			worker1.Verify(m => m.Start(), Times.Once());
			worker1.Verify(m => m.Stop(), Times.Never());
			worker2.Verify(m => m.Start(), Times.Never());
			worker2.Verify(m => m.Stop(), Times.Never());
		}

		[Test]
		public void ShouldStopRegisteredWorkers()
		{
			// Given
			var worker1 = new Mock<IMessageQueueWorker>();
			var worker2 = new Mock<IMessageQueueWorker>();
			var target = new MessageQueueListener();

			// When
			target.RegisterWorker("queue1", worker1.Object);
			target.RegisterWorker("queue2", worker2.Object);
			target.StartListenAll();
			target.StopListenAll();

			// Then
			worker1.Verify(m => m.Start(), Times.Once());
			worker1.Verify(m => m.Stop(), Times.Once());
			worker2.Verify(m => m.Start(), Times.Once());
			worker2.Verify(m => m.Stop(), Times.Once());
		}

		[Test]
		public void ShouldStopSpecifiedWorkers()
		{
			// Given
			var worker1 = new Mock<IMessageQueueWorker>();
			var worker2 = new Mock<IMessageQueueWorker>();
			var target = new MessageQueueListener();

			// When
			target.RegisterWorker("queue1", worker1.Object);
			target.RegisterWorker("queue2", worker2.Object);
			target.StartListenAll();
			target.StopListen("queue1");

			// Then
			worker1.Verify(m => m.Start(), Times.Once());
			worker1.Verify(m => m.Stop(), Times.Once());
			worker2.Verify(m => m.Start(), Times.Once());
			worker2.Verify(m => m.Stop(), Times.Never());
		}

		[Test]
		public void UnregisterShouldStopWorker()
		{
			// Given
			var worker1 = new Mock<IMessageQueueWorker>();
			var worker2 = new Mock<IMessageQueueWorker>();
			var target = new MessageQueueListener();

			// When
			target.RegisterWorker("queue1", worker1.Object);
			target.RegisterWorker("queue2", worker2.Object);
			target.StartListenAll();
			target.UnregisterWorker("queue1");

			// Then
			worker1.Verify(m => m.Start(), Times.Once());
			worker1.Verify(m => m.Stop(), Times.Once());
			worker2.Verify(m => m.Start(), Times.Once());
			worker2.Verify(m => m.Stop(), Times.Never());
		}

		[Test]
		public void ShouldSuccessfullyUnregisterNotExistsWorker()
		{
			// Given
			var target = new MessageQueueListener();

			// When
			TestDelegate action = () => target.UnregisterWorker("queue1");

			// Then
			Assert.DoesNotThrow(action);
		}

		[Test]
		public void ShouldSuccessfullyStartNotExistsWorker()
		{
			// Given
			var target = new MessageQueueListener();

			// When
			TestDelegate action = () => target.StartListen("queue1");

			// Then
			Assert.DoesNotThrow(action);
		}

		[Test]
		public void ShouldSuccessfullyStartWhenNoWorkers()
		{
			// Given
			var target = new MessageQueueListener();

			// When
			TestDelegate action = target.StartListenAll;

			// Then
			Assert.DoesNotThrow(action);
		}

		[Test]
		public void ShouldSuccessfullyStopNotExistsWorker()
		{
			// Given
			var target = new MessageQueueListener();

			// When
			TestDelegate action = () => target.StopListen("queue1");

			// Then
			Assert.DoesNotThrow(action);
		}

		[Test]
		public void ShouldSuccessfullyStopWhenNoWorkers()
		{
			// Given
			var target = new MessageQueueListener();

			// When
			TestDelegate action = target.StopListenAll;

			// Then
			Assert.DoesNotThrow(action);
		}
	}
}