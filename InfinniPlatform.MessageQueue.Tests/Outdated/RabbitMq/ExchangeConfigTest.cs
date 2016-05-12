using InfinniPlatform.MessageQueue.RabbitMq.Outdated;
using InfinniPlatform.Sdk.Queues.Outdated;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.Outdated.RabbitMq
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