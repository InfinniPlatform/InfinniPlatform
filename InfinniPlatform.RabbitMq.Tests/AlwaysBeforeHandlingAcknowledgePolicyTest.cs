using System;

using NUnit.Framework;

namespace InfinniPlatform.RabbitMq.Tests
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class AlwaysBeforeHandlingAcknowledgePolicyTest
	{
		[Test]
		public void ShouldProvideValidPolicy()
		{
			// Given
			var target = new AlwaysBeforeHandlingAcknowledgePolicy();

			// When
			var onBefore = target.OnBefore();
			var onSuccess = target.OnSuccess();
			var onFailure = target.OnFailure(new Exception());

			// Then
			Assert.IsTrue(onBefore);
			Assert.IsFalse(onSuccess);
			Assert.IsFalse(onFailure);
		}
	}
}