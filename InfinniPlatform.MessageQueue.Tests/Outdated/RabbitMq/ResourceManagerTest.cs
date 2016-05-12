using System;

using InfinniPlatform.MessageQueue.RabbitMq.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.Outdated.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ResourceManagerTest
	{
		[Test]
		public void ShouldDisposeAllRegisteredObjects()
		{
			// Given
			var resource1 = new Mock<IDisposable>();
			var resource2 = new Mock<IDisposable>();
			var resource3 = new Mock<IDisposable>();

			// When
			var target = new ResourceManager();
			target.RegisterObject(resource1.Object);
			target.RegisterObject(resource2.Object);
			target.RegisterObject(resource3.Object);
			target.Dispose();

			// Then
			resource1.Verify(m => m.Dispose());
			resource2.Verify(m => m.Dispose());
			resource3.Verify(m => m.Dispose());
		}

		[Test]
		public void RegisterObjectShouldBeDisposedIfManagerDisposed()
		{
			// Given
			var resource1 = new Mock<IDisposable>();
			var resource2 = new Mock<IDisposable>();
			var resource3 = new Mock<IDisposable>();
			var target = new ResourceManager();

			// When
			target.Dispose();
			target.RegisterObject(resource1.Object);
			target.RegisterObject(resource2.Object);
			target.RegisterObject(resource3.Object);

			// Then
			resource1.Verify(m => m.Dispose());
			resource2.Verify(m => m.Dispose());
			resource3.Verify(m => m.Dispose());
		}
	}
}