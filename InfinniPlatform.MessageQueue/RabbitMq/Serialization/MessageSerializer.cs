using System;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Serialization;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMq.Serialization
{
    public class MessageSerializer : IMessageSerializer
    {
        private readonly JsonObjectSerializer _serializer = new JsonObjectSerializer();

        public byte[] MessageToBytes(object message)
        {
            return _serializer.Serialize(message);
        }

        public IMessage BytesToMessage(BasicDeliverEventArgs args, Type type)
        {
            var body = _serializer.Deserialize(args.Body, type);
            var genericType = typeof(Message<>).MakeGenericType(type);
            var message = Activator.CreateInstance(genericType, body, args.BasicProperties.AppId);

            return (IMessage)message;
        }

        public IMessage BytesToMessage<T>(BasicDeliverEventArgs args)
        {
            var body = _serializer.Deserialize<T>(args.Body);
            var message = new Message<T>(body, args.BasicProperties.AppId);

            return message;
        }

        public IMessage BytesToMessage<T>(BasicGetResult args)
        {
            var body = _serializer.Deserialize<T>(args.Body);
            var message = new Message<T>(body, args.BasicProperties.AppId);

            return message;
        }
    }
}