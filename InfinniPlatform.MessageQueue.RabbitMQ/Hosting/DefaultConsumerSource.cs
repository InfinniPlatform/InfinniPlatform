using System.Collections.Generic;

namespace InfinniPlatform.MessageQueue.Hosting
{
    /// <summary>
    /// Источник зарегистрированных потребителей сообщений.
    /// </summary>
    public class DefaultConsumerSource : IConsumerSource
    {
        private readonly IEnumerable<IConsumer> _consumers;

        public DefaultConsumerSource(IEnumerable<IConsumer> consumers)
        {
            _consumers = consumers;
        }

        public IEnumerable<IConsumer> GetConsumers()
        {
            return _consumers;
        }
    }
}