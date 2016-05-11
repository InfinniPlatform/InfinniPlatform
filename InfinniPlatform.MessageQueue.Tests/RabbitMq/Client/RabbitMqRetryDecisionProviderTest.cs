using System;
using System.IO;

using InfinniPlatform.MessageQueue.RabbitMq.Outdated.Client;
using InfinniPlatform.MessageQueue.RabbitMq.Outdated.Policies;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq.Client
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class RabbitMqRetryDecisionProviderTest
	{
		[Test]
		public void ShouldIgnoreWhenNoError()
		{
			// Given
			var target = new RabbitMqRetryDecisionProvider();

			// When
			var result = target.GetRetryDecision(null);

			// Then
			Assert.AreEqual(RetryDecision.Ignore, result);
		}

		[Test]
		public void ShouldRetryWhenIOException()
		{
			// Given
			var target = new RabbitMqRetryDecisionProvider();

			// When
			var result = target.GetRetryDecision(new IOException());

			// Then
			Assert.AreEqual(RetryDecision.Retry, result);
		}

		[Test]
		public void ShouldRethrowWhenBusinessError()
		{
			// Given
			var target = new RabbitMqRetryDecisionProvider();

			// When
			var result = target.GetRetryDecision(new ArgumentException());

			// Then
			Assert.AreEqual(RetryDecision.Rethrow, result);
		}
	}
}