using System;
using System.Threading.Tasks;

using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Tests.Common
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class JobHandlerTypeSerializerTest
    {
        private const string HandlerType = "InfinniPlatform.Scheduler.Tests.Common.JobHandlerTypeSerializerTest+MyJobHandler,InfinniPlatform.Scheduler.Tests";


        [Test]
        public void ShouldCheckIfCanSerialize()
        {
            // Given
            var resolver = new Mock<IContainerResolver>();
            resolver.Setup(i => i.Services).Returns(new[] { typeof(MyJobHandler) });
            var target = new JobHandlerTypeSerializer(resolver.Object);

            // When
            var result1 = target.CanSerialize(typeof(MyJobHandler));
            var result2 = target.CanSerialize(typeof(JobHandlerTypeSerializerTest));

            // Then
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public void ShouldSerialize()
        {
            // Given
            var resolver = new Mock<IContainerResolver>();
            var target = new JobHandlerTypeSerializer(resolver.Object);

            // When
            var result = target.Serialize(typeof(MyJobHandler));

            // Then
            Assert.AreEqual(HandlerType, result);
        }

        [Test]
        public void ShouldDeserialize()
        {
            // Given
            var resolver = new Mock<IContainerResolver>();
            resolver.Setup(i => i.Services).Returns(new[] { typeof(MyJobHandler) });
            resolver.Setup(i => i.Resolve(typeof(MyJobHandler))).Returns(new MyJobHandler());
            var target = new JobHandlerTypeSerializer(resolver.Object);

            // When
            var result = target.Deserialize(HandlerType);

            // Then
            Assert.IsInstanceOf<MyJobHandler>(result);
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