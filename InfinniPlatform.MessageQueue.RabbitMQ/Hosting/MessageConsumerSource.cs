using System.Collections.Generic;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.MessageQueue.RabbitMQ.Hosting
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