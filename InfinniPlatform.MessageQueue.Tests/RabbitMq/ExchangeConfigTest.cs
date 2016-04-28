using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
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