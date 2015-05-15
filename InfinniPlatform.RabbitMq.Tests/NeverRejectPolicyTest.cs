using NUnit.Framework;

namespace InfinniPlatform.RabbitMq.Tests
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class NeverRejectPolicyTest
	{
		[Test]
		public void MustRejectShouldReturnFalse()
		{
			// Given
			var target = new NeverRejectPolicy();

			// When
			var result = target.MustReject();

			// Then
			Assert.IsFalse(result);
		}
	}
}