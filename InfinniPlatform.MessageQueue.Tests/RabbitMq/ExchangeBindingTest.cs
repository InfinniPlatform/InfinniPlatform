using System;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ExchangeBindingTest
	{
		private const string ExchangeName = "TestExchange";
		private const string QueueName = "TestQueue";


		[Test]
		public void SubscribeShouldCreateQueueFanoutAndRegisterWorker()
		{
			// Given

			var queueSession = new Mock<IMessageQueueSession>();
			var queueCommandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var queueWorkerContainer = new Mock<IMessageQueueWorkerContainer>();

			queueCommandExecutor
				.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>()))
				.Callback<Action<IMessageQueueSession>>(command => command(queueSession.Object));

			// When

			IExchangeFanoutBinding target = new ExchangeBinding(ExchangeName, queueCommandExecutor.Object, queueWorkerContainer.Object);
			target.Subscribe(QueueName, () => null);

			// Then

			queueSession.Verify(m => m.CreateQueueFanout(It.Is<QueueConfig>(q => q.QueueName == QueueName)));
			queueWorkerContainer.Verify(m => m.RegisterWorker(QueueName, It.IsAny<IMessageQueueWorker>()));
		}

		[Test]
		public void UnsubscribeFanoutShouldUnregisterWorkerAndDeleteQueue()
		{
			// Given

			var queueSession = new Mock<IMessageQueueSession>();
			var queueCommandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var queueWorkerContainer = new Mock<IMessageQueueWorkerContainer>();

			queueCommandExecutor
				.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>()))
				.Callback<Action<IMessageQueueSession>>(command => command(queueSession.Object));

			// When

			IExchangeFanoutBinding target = new ExchangeBinding(ExchangeName, queueCommandExecutor.Object, queueWorkerContainer.Object);
			target.Unsubscribe(QueueName);

			// Then

			queueWorkerContainer.Verify(m => m.UnregisterWorker(QueueName));
			queueSession.Verify(m => m.DeleteQueue(QueueName));
		}

		[Test]
		public void SubscribeShouldCreateQueueDirectAndRegisterWorker()
		{
			// Given

			const string routingKey = "key1";
			var queueSession = new Mock<IMessageQueueSession>();
			var queueCommandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var queueWorkerContainer = new Mock<IMessageQueueWorkerContainer>();

			queueCommandExecutor
				.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>()))
				.Callback<Action<IMessageQueueSession>>(command => command(queueSession.Object));

			// When

			IExchangeDirectBinding target = new ExchangeBinding(ExchangeName, queueCommandExecutor.Object, queueWorkerContainer.Object);
			target.Subscribe(QueueName, () => null, routingKey);

			// Then

			queueSession.Verify(m => m.CreateQueueDirect(It.Is<QueueConfig>(q => q.QueueName == QueueName), routingKey));
			queueWorkerContainer.Verify(m => m.RegisterWorker(QueueName, It.IsAny<IMessageQueueWorker>()));
		}

		[Test]
		public void UnsubscribeDirectShouldUnregisterWorkerAndDeleteQueue()
		{
			// Given

			var queueSession = new Mock<IMessageQueueSession>();
			var queueCommandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var queueWorkerContainer = new Mock<IMessageQueueWorkerContainer>();

			queueCommandExecutor
				.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>()))
				.Callback<Action<IMessageQueueSession>>(command => command(queueSession.Object));

			// When

			IExchangeDirectBinding target = new ExchangeBinding(ExchangeName, queueCommandExecutor.Object, queueWorkerContainer.Object);
			target.Unsubscribe(QueueName);

			// Then

			queueWorkerContainer.Verify(m => m.UnregisterWorker(QueueName));
			queueSession.Verify(m => m.DeleteQueue(QueueName));
		}

		[Test]
		public void SubscribeShouldCreateQueueTopicAndRegisterWorker()
		{
			// Given

			const string routingPattern = "key*";
			var queueSession = new Mock<IMessageQueueSession>();
			var queueCommandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var queueWorkerContainer = new Mock<IMessageQueueWorkerContainer>();

			queueCommandExecutor
				.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>()))
				.Callback<Action<IMessageQueueSession>>(command => command(queueSession.Object));

			// When

			IExchangeTopicBinding target = new ExchangeBinding(ExchangeName, queueCommandExecutor.Object, queueWorkerContainer.Object);
			target.Subscribe(QueueName, () => null, routingPattern);

			// Then

			queueSession.Verify(m => m.CreateQueueTopic(It.Is<QueueConfig>(q => q.QueueName == QueueName), routingPattern));
			queueWorkerContainer.Verify(m => m.RegisterWorker(QueueName, It.IsAny<IMessageQueueWorker>()));
		}

		[Test]
		public void UnsubscribeTopicShouldUnregisterWorkerAndDeleteQueue()
		{
			// Given

			var queueSession = new Mock<IMessageQueueSession>();
			var queueCommandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var queueWorkerContainer = new Mock<IMessageQueueWorkerContainer>();

			queueCommandExecutor
				.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>()))
				.Callback<Action<IMessageQueueSession>>(command => command(queueSession.Object));

			// When

			IExchangeTopicBinding target = new ExchangeBinding(ExchangeName, queueCommandExecutor.Object, queueWorkerContainer.Object);
			target.Unsubscribe(QueueName);

			// Then

			queueWorkerContainer.Verify(m => m.UnregisterWorker(QueueName));
			queueSession.Verify(m => m.DeleteQueue(QueueName));
		}

		[Test]
		public void SubscribeShouldCreateQueueHeadersAndRegisterWorker()
		{
			// Given

			var routingHeader = new MessageHeaders();
			var queueSession = new Mock<IMessageQueueSession>();
			var queueCommandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var queueWorkerContainer = new Mock<IMessageQueueWorkerContainer>();

			queueCommandExecutor
				.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>()))
				.Callback<Action<IMessageQueueSession>>(command => command(queueSession.Object));

			// When

			IExchangeHeadersBinding target = new ExchangeBinding(ExchangeName, queueCommandExecutor.Object, queueWorkerContainer.Object);
			target.Subscribe(QueueName, () => null, routingHeader);

			// Then

			queueSession.Verify(m => m.CreateQueueHeaders(It.Is<QueueConfig>(q => q.QueueName == QueueName), routingHeader));
			queueWorkerContainer.Verify(m => m.RegisterWorker(QueueName, It.IsAny<IMessageQueueWorker>()));
		}

		[Test]
		public void UnsubscribeHeadersShouldUnregisterWorkerAndDeleteQueue()
		{
			// Given

			var queueSession = new Mock<IMessageQueueSession>();
			var queueCommandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var queueWorkerContainer = new Mock<IMessageQueueWorkerContainer>();

			queueCommandExecutor
				.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>()))
				.Callback<Action<IMessageQueueSession>>(command => command(queueSession.Object));

			// When

			IExchangeHeadersBinding target = new ExchangeBinding(ExchangeName, queueCommandExecutor.Object, queueWorkerContainer.Object);
			target.Unsubscribe(QueueName);

			// Then

			queueWorkerContainer.Verify(m => m.UnregisterWorker(QueueName));
			queueSession.Verify(m => m.DeleteQueue(QueueName));
		}
	}
}