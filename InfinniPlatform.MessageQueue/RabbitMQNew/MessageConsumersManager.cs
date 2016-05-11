using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Queues;

using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class MessageConsumersManager : ApplicationEventHandler
    {
        public MessageConsumersManager(RabbitMqManager manager, IConsumer[] consumers)
        {
            _manager = manager;
            _consumers = consumers;
        }

        private readonly IConsumer[] _consumers;
        private readonly RabbitMqManager _manager;

        public override void OnAfterStart()
        {
            foreach (var consumer in _consumers)
            {
                var channel = _manager.GetChannel(consumer.QueueName);

                _manager.GetQueue(consumer.QueueName);

                var eventingConsumer = new EventingBasicConsumer(channel);

                eventingConsumer.Received += (o, e) => { consumer.Consume(e.Body); };

                channel.BasicConsume(consumer.QueueName, true, eventingConsumer);
            }
        }
    }
}