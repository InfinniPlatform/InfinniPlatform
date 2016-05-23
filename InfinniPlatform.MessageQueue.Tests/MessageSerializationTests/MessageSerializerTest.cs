using System;

using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Queues;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.MessageSerializationTests
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class MessageSerializerTest
    {
        [Test]
        public void StringMessageSerializeAndDeserializeWithoutErrors()
        {
            var messageSerializer = new MessageSerializer();
            const string expected = "message";
            var message = new Message<string>(expected);

            var bytes = messageSerializer.MessageToBytes(message);

            var actual = messageSerializer.BytesToMessage(bytes, typeof(Message<string>)).GetBody();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMessageSerializeAndDeserializeWithoutErrors()
        {
            var messageSerializer = new MessageSerializer();
            var expected = new TestMessage("1", 1, new DateTime(1, 1, 1));
            var message = new Message<TestMessage>(expected);

            var bytes = messageSerializer.MessageToBytes(message);

            var actual = messageSerializer.BytesToMessage(bytes, typeof(Message<TestMessage>)).GetBody();

            Assert.AreEqual(expected, actual);
        }
    }
}