using System;

using InfinniPlatform.MessageQueue.RabbitMq.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageQueueSubscriptionManagerTest
	{
		private const string ExchangeName = "TestEchange";


		[Test]
		public void ShouldThrowExceptionWhenExchangeNotExists()
		{
			// Given
			var commandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var workerContainer = new Mock<IMessageQueueWorkerContainer>();

			// When
			var target = new MessageQueueManager(commandExecutor.Object, workerContainer.Object);
			TestDelegate exchangeFanoutNotExists = () => target.GetExchangeFanout(ExchangeName);
			TestDelegate exchangeDirectNotExists = () => target.GetExchangeDirect(ExchangeName);
			TestDelegate exchangeTopicNotExists = () => target.GetExchangeTopic(ExchangeName);
			TestDelegate exchangeHeadersNotExists = () => target.GetExchangeHeaders(ExchangeName);

			// Then
			Assert.Throws<InvalidOperationException>(exchangeFanoutNotExists);
			Assert.Throws<InvalidOperationException>(exchangeDirectNotExists);
			Assert.Throws<InvalidOperationException>(exchangeTopicNotExists);
			Assert.Throws<InvalidOperationException>(exchangeHeadersNotExists);
		}

		[Test]
		public void ShouldCreateExchangeFanout()
		{
			// Given
			var session = new Mock<IMessageQueueSession>();
			var commandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var workerContainer = new Mock<IMessageQueueWorkerContainer>();
			commandExecutor.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>())).Callback<Action<IMessageQueueSession>>(command => command(session.Object));

			// When
			var target = new MessageQueueManager(commandExecutor.Object, workerContainer.Object);
			var result = target.CreateExchangeFanout(ExchangeName);

			// Then
			Assert.IsNotNull(result);
			session.Verify(m => m.CreateExchangeFanout(It.Is<ExchangeConfig>(p => p.ExchangeName == ExchangeName)));
		}

		[Test]
		public void ShouldCreateExchangeDirect()
		{
			// Given
			var session = new Mock<IMessageQueueSession>();
			var commandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var workerContainer = new Mock<IMessageQueueWorkerContainer>();
			commandExecutor.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>())).Callback<Action<IMessageQueueSession>>(command => command(session.Object));

			// When
			var target = new MessageQueueManager(commandExecutor.Object, workerContainer.Object);
			var result = target.CreateExchangeDirect(ExchangeName);

			// Then
			Assert.IsNotNull(result);
			session.Verify(m => m.CreateExchangeDirect(It.Is<ExchangeConfig>(p => p.ExchangeName == ExchangeName)));
		}

		[Test]
		public void ShouldCreateExchangeTopic()
		{
			// Given
			var session = new Mock<IMessageQueueSession>();
			var commandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var workerContainer = new Mock<IMessageQueueWorkerContainer>();
			commandExecutor.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>())).Callback<Action<IMessageQueueSession>>(command => command(session.Object));

			// When
			var target = new MessageQueueManager(commandExecutor.Object, workerContainer.Object);
			var result = target.CreateExchangeTopic(ExchangeName);

			// Then
			Assert.IsNotNull(result);
			session.Verify(m => m.CreateExchangeTopic(It.Is<ExchangeConfig>(p => p.ExchangeName == ExchangeName)));
		}

		[Test]
		public void ShouldCreateExchangeHeaders()
		{
			// Given
			var session = new Mock<IMessageQueueSession>();
			var commandExecutor = new Mock<IMessageQueueCommandExecutor>();
			var workerContainer = new Mock<IMessageQueueWorkerContainer>();
			commandExecutor.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>())).Callback<Action<IMessageQueueSession>>(command => command(session.Object));

			// When
			var target = new MessageQueueManager(commandExecutor.Object, workerContainer.Object);
			var result = target.CreateExchangeHeaders(ExchangeName);

			// Then
			Assert.IsNotNull(result);
			session.Verify(m => m.CreateExchangeHeaders(It.Is<ExchangeConfig>(p => p.ExchangeName == ExchangeName)));
		}
	}
}