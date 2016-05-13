﻿using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    [QueueName("String")]
    public class NamedQueueStringConsumer : ConsumerBase<string>
    {
        public NamedQueueStringConsumer(List<string> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        private readonly CountdownEvent _completeEvent;

        private readonly List<string> _messages;

        protected override void Consume(Message<string> message)
        {
            _messages.Add(message.Body);
            _completeEvent.Signal();
        }
    }
}