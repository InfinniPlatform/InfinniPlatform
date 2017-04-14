using System;
using InfinniPlatform.MessageQueue.RabbitMQ.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Serialization;

using NUnit.Framework;

using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq.Serialization
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class MessageSerializerTest
    {
        [Test]
        public void IntMessageSerializeAndDeserializeWithoutErrors()
        {
            var messageSerializer = new MessageSerializer(new JsonObjectSerializer());
            const int message = 42;

            var args = new BasicDeliverEventArgs
                       {
                           Body = messageSerializer.MessageToBytes(message),
                           BasicProperties = new BasicProperties { AppId = Guid.NewGuid().ToString() }
                       };

            var actual = messageSerializer.BytesToMessage<int>(args);

            Assert.AreEqual(message, actual.GetBody());
        }

        [Test]
        public void TestMessageSerializeAndDeserializeWithoutErrors()
        {
            var messageSerializer = new MessageSerializer(new JsonObjectSerializer());
            var message = new TestMessage("1", 1, new DateTime(1, 1, 1));

            var args = new BasicDeliverEventArgs
                       {
                           Body = messageSerializer.MessageToBytes(message),
                           BasicProperties = new BasicProperties { AppId = Guid.NewGuid().ToString() }
                       };
            var actual = messageSerializer.BytesToMessage(args, typeof(TestMessage));

            Assert.AreEqual(message, actual.GetBody());
        }

        [Test]
        public void TestMessageSerializeAndDeserializeWithoutErrorsWithGeneric()
        {
            var messageSerializer = new MessageSerializer(new JsonObjectSerializer());
            var message = new TestMessage("1", 1, new DateTime(1, 1, 1));

            var args = new BasicDeliverEventArgs
                       {
                           Body = messageSerializer.MessageToBytes(message),
                           BasicProperties = new BasicProperties { AppId = Guid.NewGuid().ToString() }
                       };
            var actual = messageSerializer.BytesToMessage<TestMessage>(args);

            Assert.AreEqual(message, actual.GetBody());
        }
    }
}