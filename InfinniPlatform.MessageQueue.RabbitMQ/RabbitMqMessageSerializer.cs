using System;

using InfinniPlatform.Serialization;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue
{
    internal class RabbitMqMessageSerializer : IRabbitMqMessageSerializer
    {
        public RabbitMqMessageSerializer(IJsonObjectSerializer serializer)
        {
            _serializer = serializer;
        }

        private readonly IJsonObjectSerializer _serializer;

        public byte[] MessageToBytes(object message)
        {
            return _serializer.Serialize(message);
        }

        public IMessage BytesToMessage(BasicDeliverEventArgs args, Type type)
        {
            var body = _serializer.Deserialize(args.Body, type);
            var genericType = typeof(Message<>).MakeGenericType(type);
            var message = Activator.CreateInstance(genericType, body, args.BasicProperties.AppId, args.BasicProperties.Headers);

            return (IMessage)message;
        }

        public IMessage BytesToMessage<T>(BasicDeliverEventArgs args)
        {
            var body = _serializer.Deserialize<T>(args.Body);
            var message = new Message<T>(body, args.BasicProperties.AppId, args.BasicProperties.Headers);

            return message;
        }

        public IMessage BytesToMessage<T>(BasicGetResult args)
        {
            var body = _serializer.Deserialize<T>(args.Body);
            var message = new Message<T>(body, args.BasicProperties.AppId, args.BasicProperties.Headers);

            return message;
        }
    }
}