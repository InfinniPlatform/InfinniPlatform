using System;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Outdated;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageQueueExtensionsTest
	{
		[Test]
		public void ShouldSetAndGetObjectInMessageHeaders()
		{
			// Given
			var key = Guid.NewGuid().ToString();
			var value = new SomeEntity { Id = Guid.NewGuid() };
			var headers = new MessageHeaders();

			// When
			headers.Set(key, value);
			var actualValue = headers.Get<SomeEntity>(key);

			// Then
			Assert.IsNotNull(actualValue);
			Assert.AreEqual(value.Id, actualValue.Id);
		}

		[Test]
		public void ShouldSetAndGetObjectInMessage()
		{
			// Given
			var message = new Message();
			var value = new SomeEntity { Id = Guid.NewGuid() };

			// When
			message.SetBodyObject(value);
			var actualValue = message.GetBodyObject<SomeEntity>();

			// Then
			Assert.IsNotNull(actualValue);
			Assert.AreEqual(value.Id, actualValue.Id);
		}


		public class SomeEntity
		{
			public Guid Id { get; set; }
		}
	}
}