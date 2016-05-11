using InfinniPlatform.MessageQueue.RabbitMq.Outdated.Policies;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq.Policies
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ConstantRetrySchedulePolicyTest
	{
		[Test]
		public void ShouldProvideConstantSchedule([Range(1, 10)] int delay)
		{
			// Given
			var target = new ConstantRetrySchedulePolicy(delay);

			// When
			var schedule = target.NewSchedule();
			var nextDelay1 = schedule.NextDelayMs();
			var nextDelay2 = schedule.NextDelayMs();
			var nextDelay3 = schedule.NextDelayMs();

			// Then
			Assert.AreEqual(delay, nextDelay1);
			Assert.AreEqual(delay, nextDelay2);
			Assert.AreEqual(delay, nextDelay3);
		}
	}
}