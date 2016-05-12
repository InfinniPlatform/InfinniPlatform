using System;

using InfinniPlatform.MessageQueue.RabbitMq.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.Outdated.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageQueueCommandExecutorTest
	{
		[Test]
		public void ShouldExecuteCommandWithinSession()
		{
			// Given

			var commandExecutor = new Mock<ICommandExecutor>();
			commandExecutor.Setup(m => m.Execute(It.IsAny<Action>())).Callback<Action>(command => command());

			var session = new Mock<IMessageQueueSession>();
			var sessionManager = new Mock<IMessageQueueSessionManager>();
			sessionManager.Setup(m => m.OpenSession()).Returns(session.Object).Callback(() => session.SetupGet(m => m.IsOpen).Returns(true));
			sessionManager.Setup(m => m.CloseSession(session.Object)).Callback(() => session.SetupGet(m => m.IsOpen).Returns(false));

			var target = new MessageQueueCommandExecutor(commandExecutor.Object, sessionManager.Object);

			// When
			var isOpen = false;
			target.Execute(s => isOpen = s.IsOpen);

			// Then
			Assert.IsTrue(isOpen);
			Assert.IsFalse(session.Object.IsOpen);
		}
	}
}