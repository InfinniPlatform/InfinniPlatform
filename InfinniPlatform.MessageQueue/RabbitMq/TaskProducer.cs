using System.Threading.Tasks;

using InfinniPlatform.Core;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues.Producers;

using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class TaskProducer : ITaskProducer
    {
        public TaskProducer(RabbitMqManager manager,
                            IMessageSerializer messageSerializer,
                            IAppIdentity appIdentity)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
            _appIdentity = appIdentity;
        }

        private readonly IAppIdentity _appIdentity;

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
            var messageBodyToBytes = _messageSerializer.MessageToBytes(messageBody);
            var routingKey = queueName ?? QueueNamingConventions.GetProducerQueueName(messageBody);
            var basicProperties = new BasicProperties { AppId = _appIdentity.Id };

            using (var channel = _manager.GetChannel())
            {
                _manager.DeclareTaskQueue(queueName);

                channel.BasicPublish(string.Empty, routingKey, basicProperties, messageBodyToBytes);
            }
        }
    }
}