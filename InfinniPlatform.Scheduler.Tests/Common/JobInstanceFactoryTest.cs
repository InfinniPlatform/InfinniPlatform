using System;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Common
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class JobInstanceFactoryTest
    {
        [Test]
        public void ShouldCreateJobInstance()
        {
            // Given
            const string jobId = "Group1.Job1";
            var scheduledFireTimeUtc = DateTimeOffset.UtcNow;
            var target = new JobInstanceFactory();

            // When
            var result1 = target.CreateJobInstance(jobId, scheduledFireTimeUtc);
            var result2 = target.CreateJobInstance(jobId, scheduledFireTimeUtc);

            // Then
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(result1, result2);
        }
    }
}