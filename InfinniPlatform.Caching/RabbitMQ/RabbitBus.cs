using System;
using System.Diagnostics;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;

namespace InfinniPlatform.Caching.RabbitMQ
{
    public class RabbitBus : IRabbitBus, IBroadcastConsumer
    {
        public RabbitBus(IBroadcastProducer producer)
        {
            _producer = producer;
            _currentAppId = Guid.NewGuid();
        }

        private readonly Guid _currentAppId;
        private readonly IBroadcastProducer _producer;

        public Type MessageType => typeof(SharedCacheMessage);

        public async Task Consume(IMessage message)
        {
            await Task.Run(() =>
                           {
                               try
                               {
                                   var body = message.GetBody();
                                   var subMsg = (SharedCacheMessage)body;
                                   Trace.WriteLine(subMsg.PublisherId == _currentAppId
                                                       ? "Got your own message. Skip."
                                                       : $"{subMsg.Key} - {subMsg.Value}.");

                                   if (subMsg.PublisherId == _currentAppId)
                                   {
                                       Trace.WriteLine("Got your own message. Skip.");
                                   }
                                   else
                                   {
                                       Trace.WriteLine($"{subMsg.Key} - {subMsg.Value}.");
                                       OnMessageRecieve?.Invoke(subMsg);
                                   }
                               }
                               catch (Exception e)
                               {
                                   Trace.WriteLine(e);
                               }
                           });
        }

        public Action<SharedCacheMessage> OnMessageRecieve { get; set; }

        public void Publish(string key, string value)
        {
            _producer.Publish(new SharedCacheMessage(key, value, _currentAppId));
        }
    }
}