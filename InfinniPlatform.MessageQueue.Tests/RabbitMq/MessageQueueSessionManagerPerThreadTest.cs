using InfinniPlatform.MessageQueue.RabbitMq.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageQueueSessionManagerPerThreadTest
	{
		[Test]
		public void ShouldReturnsSameSessionBeforeEachCommand()
		{
			// Given
			var session = new Mock<IMessageQueueSession>();
			session.SetupGet(m => m.IsOpen).Returns(true);
			var sessionFactory = new Mock<IMessageQueueSessionFactory>();
			sessionFactory.Setup(m => m.OpenSession()).Returns(session.Object);

			// When
			var target = new MessageQueueSessionManagerPerThread(sessionFactory.Object);
			var actualSession1 = target.OpenSession();
			var actualSession2 = target.OpenSession();

			// Then
			Assert.IsNotNull(actualSession1);
			Assert.AreEqual(actualSession1, actualSession2);
		}

		[Test]
		public void ShouldNotDisposeSessionAfterCommand()
		{
			// Given
			var session = new Mock<IMessageQueueSession>();
			session.SetupGet(m => m.IsOpen).Returns(true);
			var sessionFactory = new Mock<IMessageQueueSessionFactory>();
			sessionFactory.Setup(m => m.OpenSession()).Returns(session.Object);

			// When
			var target = new MessageQueueSessionManagerPerThread(sessionFactory.Object);
			var actualSession = target.OpenSession();
			target.CloseSession(actualSession);

			// Then
			session.Verify(m => m.Dispose(), Times.Never());
		}
	}
}