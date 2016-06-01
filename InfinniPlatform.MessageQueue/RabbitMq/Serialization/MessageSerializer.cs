using System;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.MessageQueue.RabbitMq.Serialization
{
    public class MessageSerializer : IMessageSerializer
    {
        private readonly JsonObjectSerializer _serializer = new JsonObjectSerializer();

        public byte[] MessageToBytes(object message)
        {
            return _serializer.Serialize(message);
        }

        public IMessage BytesToMessage(byte[] bytes, Type type)
        {
            var body = _serializer.Deserialize(bytes, type);
            var genericType = typeof(Message<>).MakeGenericType(type);
            var message = Activator.CreateInstance(genericType, body);

            return (IMessage)message;
        }

        public IMessage BytesToMessage<T>(byte[] bytes)
        {
            var body = _serializer.Deserialize<T>(bytes);
            var message = new Message<T>(body);

            return message;
        }
    }
}