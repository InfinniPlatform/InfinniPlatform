﻿using System.Collections.Generic;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.MessageQueue.RabbitMq.Hosting
{
    /// <summary>
    /// Источник зарегистрированных потребителей сообщений.
    /// </summary>
    public class MessageConsumerSource : IMessageConsumerSource
    {
        public MessageConsumerSource(IEnumerable<IConsumer> consumers)
        {
            _consumers = consumers;
        }

        private readonly IEnumerable<IConsumer> _consumers;

        public IEnumerable<IConsumer> GetConsumers()
        {
            return _consumers;
        }
    }
}