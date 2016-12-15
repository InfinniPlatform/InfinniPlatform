using System;

using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Consumers;
using InfinniPlatform.Sdk.Dynamic;

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