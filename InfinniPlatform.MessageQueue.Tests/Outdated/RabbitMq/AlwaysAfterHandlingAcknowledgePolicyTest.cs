using System;

using InfinniPlatform.MessageQueue.RabbitMq.Outdated;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.Outdated.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class AlwaysAfterHandlingAcknowledgePolicyTest
	{
		[Test]
		public void ShouldProvideValidPolicy()
		{
			// Given
			var target = new AlwaysAfterHandlingAcknowledgePolicy();

			// When
			var onBefore = target.OnBefore();
			var onSuccess = target.OnSuccess();
			var onFailure = target.OnFailure(new Exception());

			// Then
			Assert.IsFalse(onBefore);
			Assert.IsTrue(onSuccess);
			Assert.IsTrue(onFailure);
		}
	}
}