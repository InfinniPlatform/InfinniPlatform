using System;
using System.Reflection;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    public static class QueueNamingConventions
    {
        public static string GetProducerQueueName(IMessage message)
        {
            var queueNameAttribute = message.GetBodyType().GetCustomAttribute<QueueNameAttribute>()?.Value;
            return queueNameAttribute ?? message.GetBodyType().ToString();
        }

        public static string GetConsumerQueueName(IConsumer consumer)
        {
            var queueNameAttribute = consumer.GetType().GetCustomAttribute<QueueNameAttribute>()?.Value;
            return queueNameAttribute ?? consumer.MessageType.ToString();
        }

        public static string GetConsumerQueueName(Type type)
        {
            var queueNameAttribute = type.GetCustomAttribute<QueueNameAttribute>()?.Value;
            return queueNameAttribute ?? type.ToString();
        }

        public static string GetBasicConsumerQueueName(Type type)
        {
            return type.ToString();
        }
    }
}