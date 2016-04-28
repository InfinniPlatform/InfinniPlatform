using InfinniPlatform.MessageQueue.RabbitMq.Policies;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq.Policies
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ExponentialRetrySchedulePolicyTest
	{
		[Test]
		public void ShouldProvideExponentialSchedule()
		{
			// Given
			var target = new ExponentialRetrySchedulePolicy(2000, 300000);

			// When
			var schedule = target.NewSchedule();
			var nextDelay0 = schedule.NextDelayMs();
			var nextDelay1 = schedule.NextDelayMs();
			var nextDelay2 = schedule.NextDelayMs();
			var nextDelay3 = schedule.NextDelayMs();
			var nextDelay4 = schedule.NextDelayMs();
			var nextDelay5 = schedule.NextDelayMs();
			var nextDelay6 = schedule.NextDelayMs();
			var nextDelay7 = schedule.NextDelayMs();
			var nextDelay8 = schedule.NextDelayMs();
			var nextDelay9 = schedule.NextDelayMs();

			// Then
			Assert.AreEqual(2000, nextDelay0);
			Assert.AreEqual(4000, nextDelay1);
			Assert.AreEqual(8000, nextDelay2);
			Assert.AreEqual(16000, nextDelay3);
			Assert.AreEqual(32000, nextDelay4);
			Assert.AreEqual(64000, nextDelay5);
			Assert.AreEqual(128000, nextDelay6);
			Assert.AreEqual(256000, nextDelay7);
			Assert.AreEqual(300000, nextDelay8);
			Assert.AreEqual(300000, nextDelay9);
		}
	}
}