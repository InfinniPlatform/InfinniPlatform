using System;

using InfinniPlatform.Core.Dynamic;

namespace InfinniPlatform.MessageQueue.RabbitMQ
{
    public static class Helpers
    {
        public static void CheckTypeRestrictions<T>()
        {
            if (typeof(T) == typeof(DynamicWrapper))
            {
                throw new ArgumentException("Use PublishDynamic or PublishDynamicAsync methods to send DynamicWrapper messages.");
            }
        }
    }
}