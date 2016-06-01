using System;

using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.MessageSerializationTests
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class MessageSerializerTest
    {
        [Test]
        public void IntMessageSerializeAndDeserializeWithoutErrors()
        {
            var messageSerializer = new MessageSerializer();
            const int message = 42;

            var bytes = messageSerializer.MessageToBytes(message);
            var actual = messageSerializer.BytesToMessage<int>(bytes);

            Assert.AreEqual(message, actual.GetBody());
        }

        [Test]
        public void TestMessageSerializeAndDeserializeWithoutErrors()
        {
            var messageSerializer = new MessageSerializer();
            var message = new TestMessage("1", 1, new DateTime(1, 1, 1));

            var bytes = messageSerializer.MessageToBytes(message);
            var actual = messageSerializer.BytesToMessage<TestMessage>(bytes);

            Assert.AreEqual(message, actual.GetBody());
        }
    }
}