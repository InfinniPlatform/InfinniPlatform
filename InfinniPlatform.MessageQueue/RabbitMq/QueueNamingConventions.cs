using System;

using InfinniPlatform.Sdk.Dynamic;
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
            return type.GetAttributeValue<QueueNameAttribute, string>(i => i.Name, defaultValue ?? type.FullName);
        }
    }
}