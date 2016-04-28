using System;

using InfinniPlatform.Core.MessageQueue;
using InfinniPlatform.MessageQueue.RabbitMq;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageQueuePublisherTest
	{
		[Test]
		public void ShouldPublishSpecifiedMessage()
		{
			// Given

			var session = new Mock<IMessageQueueSession>();
			var commandExecutor = new Mock<IMessageQueueCommandExecutor>();
			commandExecutor.Setup(m => m.Execute(It.IsAny<Action<IMessageQueueSession>>())).Callback<Action<IMessageQueueSession>>(command => command(session.Object));

			const string exchange = "exchange1";
			const string routingKey = "routingKey1";
			var properties = new MessageProperties();
			var body = new byte[] { 1, 2, 3 };

			// When
			var target = new MessageQueuePublisher(commandExecutor.Object);
			target.Publish(exchange, routingKey, properties, body);

			// Then
			session.Verify(m => m.Publish(exchange, routingKey, properties, body));
		}
	}
}