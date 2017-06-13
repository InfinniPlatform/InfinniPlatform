using System;
using System.Reflection;
using InfinniPlatform.Dynamic;

namespace InfinniPlatform.MessageQueue
{
    internal static class MessageQueueHelper
    {
        public static void CheckTypeRestrictions<T>()
        {
            if (typeof(T) == typeof(DynamicDocument))
            {
                throw new ArgumentException("Use PublishDynamic or PublishDynamicAsync methods to send DynamicDocument messages.");
            }
        }

        public static string GetProducerQueueName(object message)
        {
            return message != null ? GetQueueName(message.GetType()) : null;
        }

        public static string GetConsumerQueueName(IConsumer consumer)
        {
            return consumer != null ? GetQueueName(consumer.MessageType, GetQueueName(consumer.GetType(), consumer.MessageType.FullName)) : null;
        }

        public static string GetQueueName(Type type, string defaultValue = null)
        {
            return type.GetTypeInfo().GetAttributeValue<QueueNameAttribute, string>(i => i.Name, defaultValue ?? type.FullName);
        }
    }
}