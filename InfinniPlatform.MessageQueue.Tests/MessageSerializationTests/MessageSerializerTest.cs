using System;

using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;

using NUnit.Framework;

using RabbitMQ.Client.Events;

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

            var args = new BasicDeliverEventArgs
                       {
                           Body = messageSerializer.MessageToBytes(message)
                       };

            var actual = messageSerializer.BytesToMessage<int>(args);

            Assert.AreEqual(message, actual.GetBody());
        }

        [Test]
        public void TestMessageSerializeAndDeserializeWithoutErrors()
        {
            var messageSerializer = new MessageSerializer();
            var message = new TestMessage("1", 1, new DateTime(1, 1, 1));

            var args = new BasicDeliverEventArgs
                       {
                           Body = messageSerializer.MessageToBytes(message)
                       };
            var actual = messageSerializer.BytesToMessage(args, typeof(TestMessage));

            Assert.AreEqual(message, actual.GetBody());
        }

        [Test]
        public void TestMessageSerializeAndDeserializeWithoutErrorsWithGeneric()
        {
            var messageSerializer = new MessageSerializer();
            var message = new TestMessage("1", 1, new DateTime(1, 1, 1));

            var args = new BasicDeliverEventArgs
                       {
                           Body = messageSerializer.MessageToBytes(message)
                       };
            var actual = messageSerializer.BytesToMessage<TestMessage>(args);

            Assert.AreEqual(message, actual.GetBody());
        }
    }
}