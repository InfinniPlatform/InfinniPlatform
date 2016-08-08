using System;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues.Producers;

using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal class BroadcastProducer : IBroadcastProducer
    {
        public BroadcastProducer(RabbitMqManager manager,
                                 IMessageSerializer messageSerializer,
                                 ILog log)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
            _log = log;
        }

        private readonly ILog _log;

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public void Publish<T>(T messageBody, string queueName = null)
        {
            Helpers.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);
        }

        public void PublishDynamic(DynamicWrapper messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);
        }

        public async Task PublishAsync<T>(T messageBody, string queueName = null)
        {
            Helpers.CheckTypeRestrictions<T>();

            await Task.Run(() => { BasicPublish(messageBody, queueName); });
        }

        public async Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName)
        {
            await Task.Run(() => { BasicPublish(messageBody, queueName); });
        }

        private void BasicPublish<T>(T messageBody, string queueName)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(messageBody);

            using (var channel = _manager.GetChannel())
            {
                var routingKey = queueName ?? QueueNamingConventions.GetProducerQueueName(messageBody);
                var basicProperties = new BasicProperties { AppId = _manager.AppId };

                try
                {
                    channel.BasicPublish(_manager.BroadcastExchangeName, routingKey, basicProperties, messageToBytes);
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }
    }
}