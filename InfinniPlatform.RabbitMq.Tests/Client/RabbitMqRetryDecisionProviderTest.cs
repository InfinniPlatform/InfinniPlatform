using System;
using System.IO;

using InfinniPlatform.RabbitMq.Client;
using InfinniPlatform.RabbitMq.Policies;

using NUnit.Framework;

namespace InfinniPlatform.RabbitMq.Tests.Client
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