using System;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Dynamic;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Tests.Common
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class JobInfoFactoryTest
    {
        [Test]
        public void ShouldCreateJobInfo()
        {
            // Given

            var handlerTypeSerializer = new Mock<IJobHandlerTypeSerializer>();
            handlerTypeSerializer.Setup(i => i.CanSerialize(typeof(MyJobHandler))).Returns(true);
            handlerTypeSerializer.Setup(i => i.Serialize(typeof(MyJobHandler))).Returns(nameof(MyJobHandler));

            var target = new JobInfoFactory(handlerTypeSerializer.Object);

            // When

            var expectedName = "Job1";
            var expectedGroup = "Group1";
            var expectedState = JobState.Planned;
            var expectedDescription = "Description1";
            var expectedCronExpression = "* * * * * ?";
            var expectedStartTimeUtc = DateTimeOffset.UtcNow;
            var expectedEndTimeUtc = DateTimeOffset.UtcNow;
            var expectedMisfirePolicy = JobMisfirePolicy.FireAndProceed;
            var expectedData = new DynamicWrapper();

            var jobInfo = target.CreateJobInfo<MyJobHandler>(expectedName, expectedGroup, b => b.State(expectedState)
                                                                                                .Description(expectedDescription)
                                                                                                .CronExpression(expectedCronExpression)
                                                                                                .StartTimeUtc(expectedStartTimeUtc)
                                                                                                .EndTimeUtc(expectedEndTimeUtc)
                                                                                                .MisfirePolicy(expectedMisfirePolicy)
                                                                                                .Data(expectedData));

            // Then

            Assert.IsNotNull(jobInfo);
            Assert.AreEqual($"{expectedGroup}.{expectedName}", jobInfo.Id);
            Assert.AreEqual(expectedName, jobInfo.Name);
            Assert.AreEqual(expectedGroup, jobInfo.Group);
            Assert.AreEqual(expectedState, jobInfo.State);
            Assert.AreEqual(expectedDescription, jobInfo.Description);
            Assert.AreEqual(nameof(MyJobHandler), jobInfo.HandlerType);
            Assert.AreEqual(expectedCronExpression, jobInfo.CronExpression);
            Assert.AreEqual(expectedStartTimeUtc, jobInfo.StartTimeUtc);
            Assert.AreEqual(expectedEndTimeUtc, jobInfo.EndTimeUtc);
            Assert.AreEqual(expectedMisfirePolicy, jobInfo.MisfirePolicy);
            Assert.AreEqual(expectedData, jobInfo.Data);
        }


        private class MyJobHandler : IJobHandler
        {
            public Task Handle(IJobInfo jobInfo, IJobHandlerContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}