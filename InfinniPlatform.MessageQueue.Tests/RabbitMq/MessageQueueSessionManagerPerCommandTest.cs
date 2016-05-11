using InfinniPlatform.MessageQueue.RabbitMq.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageQueueSessionManagerPerCommandTest
	{
		[Test]
		public void ShouldReturnsNewSessionBeforeEachCommand()
		{
			// Given
			var sessionFactory = new Mock<IMessageQueueSessionFactory>();

			// When
			var target = new MessageQueueSessionManagerPerCommand(sessionFactory.Object);
			target.OpenSession();
			target.OpenSession();

			// Then
			sessionFactory.Verify(m => m.OpenSession(), Times.Exactly(2));
		}

		[Test]
		public void ShouldDisposeSessionAfterCommand()
		{
			// Given
			var session = new Mock<IMessageQueueSession>();
			session.SetupGet(m => m.IsOpen).Returns(true);
			var sessionFactory = new Mock<IMessageQueueSessionFactory>();
			sessionFactory.Setup(m => m.OpenSession()).Returns(session.Object);

			// When
			var target = new MessageQueueSessionManagerPerCommand(sessionFactory.Object);
			var actualSession = target.OpenSession();
			target.CloseSession(actualSession);

			// Then
			Assert.AreEqual(session.Object, actualSession);
			session.Verify(m => m.Dispose());
		}
	}
}