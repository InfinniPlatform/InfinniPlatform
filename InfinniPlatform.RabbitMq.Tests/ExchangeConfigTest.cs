using InfinniPlatform.MessageQueue;

using NUnit.Framework;

namespace InfinniPlatform.RabbitMq.Tests
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ExchangeConfigTest
	{
		private const string ExchangeName = "TestExchange";


		[Test]
		public void ShouldSetDurable()
		{
			// Given
			var target = new ExchangeConfig(ExchangeName);
			var config = (IExchangeConfig)target;

			// When
			config.Durable();

			// Then
			Assert.AreEqual(ExchangeName, target.ExchangeName);
			Assert.IsTrue(target.ExchangeDurable);
		}
	}
}