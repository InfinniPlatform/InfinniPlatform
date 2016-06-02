using System;
using System.Reflection;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    public static class QueueNamingConventions
    {
        public static string GetProducerQueueName(object message)
        {
            if (message == null)
            {
                return null;
            }

            var messageType = message.GetType();
            var queueNameAttribute = messageType.GetCustomAttribute<QueueNameAttribute>()?.Value;
            return queueNameAttribute ?? messageType.FullName;
        }

        public static string GetConsumerQueueName(IConsumer consumer)
        {
            if (consumer == null)
            {
                return null;
            }

            var queueNameAttribute = consumer.GetType().GetCustomAttribute<QueueNameAttribute>()?.Value;
            return queueNameAttribute ?? consumer.MessageType.FullName;
        }

        public static string GetConsumerQueueName(Type type)
        {
            if (type == null)
            {
                return null;
            }

            var queueNameAttribute = type.GetCustomAttribute<QueueNameAttribute>()?.Value;
            return queueNameAttribute ?? type.FullName;
        }

        public static string GetBasicConsumerQueueName(Type type)
        {
            return type?.FullName;
        }
    }
}