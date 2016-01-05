using InfinniPlatform.Core.MessageQueue;

using NUnit.Framework;

namespace InfinniPlatform.RabbitMq.Tests
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MessageHeadersTest
	{
		[Test]
		public void ShouldSetAndGetData()
		{
			// Given
			const string key1 = "key1";
			const string key2 = "key2";
			var value1 = new byte[] { 1, 2, 3, 4, 5 };
			var value2 = new byte[] { 6, 7, 8, 9, 0 };
			var headers = new MessageHeaders();

			// When
			headers.Set(key1, value1);
			headers.Set(key2, value2);
			var actualValue1 = headers.Get(key1);
			var actualValue2 = headers.Get(key2);

			// Then
			Assert.AreEqual(value1, actualValue1);
			Assert.AreEqual(value2, actualValue2);
		}

		[Test]
		public void SetWithSameKeyShouldReplaceValue()
		{
			// Given
			const string key = "key";
			var value1 = new byte[] { 1, 2, 3, 4, 5 };
			var value2 = new byte[] { 6, 7, 8, 9, 0 };
			var headers = new MessageHeaders();

			// When
			headers.Set(key, value1);
			var actualValue1 = headers.Get(key);
			headers.Set(key, value2);
			var actualValue2 = headers.Get(key);

			// Then
			Assert.AreEqual(value1, actualValue1);
			Assert.AreEqual(value2, actualValue2);
		}
	}
}