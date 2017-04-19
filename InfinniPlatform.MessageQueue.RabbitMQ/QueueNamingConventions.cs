using System;
using System.Reflection;

using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.MessageQueue.RabbitMQ
{
    public static class QueueNamingConventions
    {
        public static string GetProducerQueueName(object message)
        {
            if (message == null)
            {
                return null;
            }

            return GetQueueName(message.GetType());
        }

        public static string GetConsumerQueueName(IConsumer consumer)
        {
            if (consumer == null)
            {
                return null;
            }

            return GetQueueName(consumer.MessageType, GetQueueName(consumer.GetType(), consumer.MessageType.FullName));
        }

        public static string GetQueueName(Type type, string defaultValue = null)
        {
            return type.GetTypeInfo().GetAttributeValue<QueueNameAttribute, string>(i => i.Name, defaultValue ?? type.FullName);
        }
    }
}