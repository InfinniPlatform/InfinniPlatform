using System;
using System.Diagnostics;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;

namespace InfinniPlatform.Caching.RabbitMQ
{
    public interface IRabbitBus : IBroadcastConsumer
    {
    }


    public class RabbitBus : IRabbitBus
    {
        public RabbitBus(IBroadcastProducer producer)
        {
            _producer = producer;
        }

        private readonly IBroadcastProducer _producer;

        public Type MessageType => typeof(CacheSubscriberMessage);

        public async Task Consume(IMessage message)
        {
            await Task.Run(() =>
                           {
                               var body = message.GetBody();
                               Trace.WriteLine(body);
                           });
        }

        public void Publish(string key, string value)
        {
            _producer.Publish(new CacheSubscriberMessage(key, value));
        }
    }


    public class CacheSubscriberMessage
    {
        public CacheSubscriberMessage(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}