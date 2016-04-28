using System;

using InfinniPlatform.MessageQueue.RabbitMq.Policies;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq.Policies
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class RetryCommandExecutorTest
	{
		[Test]
		public void ShouldRetryCommand()
		{
			// Given
			var command = new CommandStub();
			var target = new RetryCommandExecutor(new AlwaysRetryPolicyStub(), new ZeroRetrySchedulePolicyStub());

			// When
			target.Execute(command.Execute);

			// Then
			Assert.AreEqual(2, command.Count);
		}


		class CommandStub
		{
			public int Count { get; private set; }

			public void Execute()
			{
				++Count;

				if (Count <= 1)
				{
					throw new Exception();
				}
			}
		}

		class AlwaysRetryPolicyStub : IRetryPolicy
		{
			public IRetryScope NewScope()
			{
				return new AlwaysRetryScopeStub();
			}

			class AlwaysRetryScopeStub : IRetryScope
			{
				public RetryDecision OnError(Exception error)
				{
					return RetryDecision.Retry;
				}
			}
		}

		class ZeroRetrySchedulePolicyStub : IRetrySchedulePolicy
		{
			public IRetrySchedule NewSchedule()
			{
				return new ZeroRetryScheduleStub();
			}

			class ZeroRetryScheduleStub : IRetrySchedule
			{
				public int NextDelayMs()
				{
					return 0;
				}
			}
		}
	}
}