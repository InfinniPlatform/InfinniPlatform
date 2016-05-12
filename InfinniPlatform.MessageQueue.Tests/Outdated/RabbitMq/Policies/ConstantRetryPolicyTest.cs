using System;

using InfinniPlatform.MessageQueue.RabbitMq.Outdated.Policies;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.Outdated.RabbitMq.Policies
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ConstantRetryPolicyTest
	{
		[Test]
		public void ShouldRetryOneTimes()
		{
			// Given
			var retryDecisionProvider = new RetryDecisionProviderStub(RetryDecision.Retry);
			var retryPolicy = new ConstantRetryPolicy(retryDecisionProvider, 1);
			var retryScope = retryPolicy.NewScope();

			// When

			var result = new[]
				             {
					             retryScope.OnError(new Exception()),
					             retryScope.OnError(new Exception())
				             };

			// Then

			CollectionAssert.AreEquivalent(new[] { RetryDecision.Retry, RetryDecision.Rethrow }, result);
		}

		[Test]
		public void ShouldRetryTwoTimes()
		{
			// Given
			var retryDecisionProvider = new RetryDecisionProviderStub(RetryDecision.Retry);
			var retryPolicy = new ConstantRetryPolicy(retryDecisionProvider, 2);
			var retryScope = retryPolicy.NewScope();

			// When

			var result = new[]
				             {
					             retryScope.OnError(new Exception()),
					             retryScope.OnError(new Exception()),
					             retryScope.OnError(new Exception())
				             };

			// Then

			CollectionAssert.AreEquivalent(new[] { RetryDecision.Retry, RetryDecision.Retry, RetryDecision.Rethrow }, result);
		}

		[Test]
		public void ShouldNotRetryWhenIgnore()
		{
			// Given
			var retryDecisionProvider = new RetryDecisionProviderStub(RetryDecision.Ignore);
			var retryPolicy = new ConstantRetryPolicy(retryDecisionProvider, 1);
			var retryScope = retryPolicy.NewScope();

			// When

			var result = new[]
				             {
					             retryScope.OnError(new Exception()),
					             retryScope.OnError(new Exception())
				             };

			// Then

			CollectionAssert.AreEquivalent(new[] { RetryDecision.Ignore, RetryDecision.Ignore }, result);
		}

		[Test]
		public void ShouldNotRetryWhenRethrow()
		{
			// Given
			var retryDecisionProvider = new RetryDecisionProviderStub(RetryDecision.Rethrow);
			var retryPolicy = new ConstantRetryPolicy(retryDecisionProvider, 1);
			var retryScope = retryPolicy.NewScope();

			// When

			var result = new[]
				             {
					             retryScope.OnError(new Exception()),
					             retryScope.OnError(new Exception())
				             };

			// Then

			CollectionAssert.AreEquivalent(new[] { RetryDecision.Rethrow, RetryDecision.Rethrow }, result);
		}


		class RetryDecisionProviderStub : IRetryDecisionProvider
		{
			private readonly RetryDecision _decision;

			public RetryDecisionProviderStub(RetryDecision decision)
			{
				_decision = decision;
			}

			public RetryDecision GetRetryDecision(Exception error)
			{
				return _decision;
			}
		}
	}
}