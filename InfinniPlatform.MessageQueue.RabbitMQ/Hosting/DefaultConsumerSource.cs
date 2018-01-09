using System.Collections.Generic;

namespace InfinniPlatform.MessageQueue.Hosting
{
    /// <summary>
    /// Source of message consumers instances.
    /// </summary>
    public class DefaultConsumerSource : IConsumerSource
    {
        private readonly IEnumerable<IConsumer> _consumers;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultConsumerSource" />.
        /// </summary>
        /// <param name="consumers">List of message consumers.</param>
        public DefaultConsumerSource(IEnumerable<IConsumer> consumers)
        {
            _consumers = consumers;
        }

        /// <inheritdoc />
        public IEnumerable<IConsumer> GetConsumers()
        {
            return _consumers;
        }
    }
}