using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.MessageQueue.RabbitMq.Outdated.Client;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Outdated;

using Moq;

using NUnit.Framework;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq.Client
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class RabbitMqMessageConverterTest
	{
		private const string AppId = "Application1";
		private const string UserId = "UserIdOrName";
		private const string TypeName = "SomeMessageType";
		private const string MessageId = "A41CA9AC-8E13-4198-BB26-929074FAB615";
		private const string ContentType = "application/json";
		private const string ContentEncoding = "utf-8";

		private static readonly Dictionary<string, byte[]> Headers
			= new Dictionary<string, byte[]>
				  {
					  { "key1", new byte[] { 1, 2, 3 } },
					  { "key2", new byte[] { 4, 5, 6 } },
					  { "key3", new byte[] { 7, 8, 9 } }
				  };


		[Test]
		public void ShouldConvertFrom()
		{
			// Given
			var properties = CreateRabbitMessageProperties();
			properties.AppId = AppId;
			properties.UserId = UserId;
			properties.Type = TypeName;
			properties.MessageId = MessageId;
			properties.ContentType = ContentType;
			properties.ContentEncoding = ContentEncoding;
			properties.Headers = Headers.ToDictionary(i => i.Key, i => (object)i.Value);

			// When
			var target = new RabbitMqMessageConverter(CreateRabbitMessageProperties);
			var result = target.ConvertFrom(properties);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(AppId, result.AppId);
			Assert.AreEqual(UserId, result.UserId);
			Assert.AreEqual(TypeName, result.TypeName);
			Assert.AreEqual(MessageId, result.MessageId);
			Assert.AreEqual(ContentType, result.ContentType);
			Assert.AreEqual(ContentEncoding, result.ContentEncoding);
			Assert.IsNotNull(result.Headers);
			CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, result.Headers.Get("key1"));
			CollectionAssert.AreEqual(new byte[] { 4, 5, 6 }, result.Headers.Get("key2"));
			CollectionAssert.AreEqual(new byte[] { 7, 8, 9 }, result.Headers.Get("key3"));
		}

		[Test]
		public void ShouldConvertTo()
		{
			// Given
			var properties = CreateMessageProperties();
			properties.AppId = AppId;
			properties.UserId = UserId;
			properties.TypeName = TypeName;
			properties.MessageId = MessageId;
			properties.ContentType = ContentType;
			properties.ContentEncoding = ContentEncoding;
			properties.Headers = new MessageHeaders(Headers);

			// When
			var target = new RabbitMqMessageConverter(CreateRabbitMessageProperties);
			var result = target.ConvertTo(properties);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(AppId, result.AppId);
			Assert.AreEqual(UserId, result.UserId);
			Assert.AreEqual(TypeName, result.Type);
			Assert.AreEqual(MessageId, result.MessageId);
			Assert.AreEqual(ContentType, result.ContentType);
			Assert.AreEqual(ContentEncoding, result.ContentEncoding);
			Assert.AreEqual(2, result.DeliveryMode);
			Assert.IsNotNull(result.Headers);
			CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, (byte[])result.Headers["key1"]);
			CollectionAssert.AreEqual(new byte[] { 4, 5, 6 }, (byte[])result.Headers["key2"]);
			CollectionAssert.AreEqual(new byte[] { 7, 8, 9 }, (byte[])result.Headers["key3"]);
		}



		private static MessageProperties CreateMessageProperties()
		{
			return new MessageProperties();
		}

		private static IBasicProperties CreateRabbitMessageProperties()
		{
			var propertiesFrom = new Mock<IBasicProperties>();
			propertiesFrom.SetupProperty(m => m.AppId);
			propertiesFrom.SetupProperty(m => m.UserId);
			propertiesFrom.SetupProperty(m => m.Type);
			propertiesFrom.SetupProperty(m => m.MessageId);
			propertiesFrom.SetupProperty(m => m.ContentType);
			propertiesFrom.SetupProperty(m => m.ContentEncoding);
			propertiesFrom.SetupProperty(m => m.DeliveryMode);
			propertiesFrom.SetupProperty(m => m.Headers);
			return propertiesFrom.Object;
		}
	}
}