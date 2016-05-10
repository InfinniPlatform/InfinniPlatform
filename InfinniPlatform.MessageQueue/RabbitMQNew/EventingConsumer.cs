using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Queues;

using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class EventingConsumer : ApplicationEventHandler
    {
        public EventingConsumer(RabbitMqManager manager, IEventingConsumer[] consumers)
        {
            _manager = manager;
            _consumers = consumers;
        }

        private readonly IEventingConsumer[] _consumers;
        private readonly RabbitMqManager _manager;

        public override void OnAfterStart()
        {
            var channel = _manager.GetChannel("test_queue");

            _manager.GetQueue("test_queue");

            var eventingConsumer = new EventingBasicConsumer(channel);

            foreach (var consumer in _consumers)
            {
                eventingConsumer.Received += (o, e) => { consumer.Consume(e.Body); };
            }

            channel.BasicConsume("test_queue", true, eventingConsumer);
        }
    }
}