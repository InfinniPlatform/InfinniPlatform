using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues.Producers;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class TaskProducer : ITaskProducer
    {
        public TaskProducer(RabbitMqManager manager, IMessageSerializer messageSerializer)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
        }

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public void Publish<T>(T messageBody, string queueName = null)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(messageBody);

            if (queueName == null)
            {
                queueName = QueueNamingConventions.GetProducerQueueName(messageBody);
            }

            var channel = _manager.GetChannel();

            _manager.DeclareTaskQueue(queueName);

            channel.BasicPublish("", queueName, null, messageToBytes);
        }

        public void PublishDynamic(DynamicWrapper messageBody, string queueName)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(messageBody);

            var channel = _manager.GetChannel();

            _manager.DeclareTaskQueue(queueName);

            channel.BasicPublish("", queueName, null, messageToBytes);
        }

        public async Task PublishAsync<T>(T messageBody, string queueName = null)
        {
            await Task.Run(() =>
                           {
                               var messageToBytes = _messageSerializer.MessageToBytes(messageBody);

                               if (queueName == null)
                               {
                                   queueName = QueueNamingConventions.GetProducerQueueName(messageBody);
                               }

                               var channel = _manager.GetChannel();

                               _manager.DeclareTaskQueue(queueName);

                               channel.BasicPublish("", queueName, null, messageToBytes);
                           });
        }

        public async Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName)
        {
            await Task.Run(() =>
                           {
                               var messageToBytes = _messageSerializer.MessageToBytes(messageBody);

                               var channel = _manager.GetChannel();

                               _manager.DeclareTaskQueue(queueName);

                               channel.BasicPublish("", queueName, null, messageToBytes);
                           });
        }
    }
}