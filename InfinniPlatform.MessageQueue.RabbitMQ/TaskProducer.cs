using System.Threading.Tasks;
using InfinniPlatform.Dynamic;
using InfinniPlatform.MessageQueue.Management;

namespace InfinniPlatform.MessageQueue
{
    internal class TaskProducer : ITaskProducer
    {
        private readonly IBasicPropertiesProvider _basicPropertiesProvider;


        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public TaskProducer(RabbitMqManager manager,
                            IMessageSerializer messageSerializer,
                            IBasicPropertiesProvider basicPropertiesProvider)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
            _basicPropertiesProvider = basicPropertiesProvider;
        }


        public void Publish<T>(T messageBody, string queueName = null)
        {
            MessageQueueHelper.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);
        }

        public void PublishDynamic(DynamicDocument messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);
        }

        public Task PublishAsync<T>(T messageBody, string queueName = null)
        {
            MessageQueueHelper.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);

            return Task.CompletedTask;
        }

        public Task PublishDynamicAsync(DynamicDocument messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);

            return Task.CompletedTask;
        }

        private void BasicPublish<T>(T messageBody, string queueName)
        {
            var messageBodyToBytes = _messageSerializer.MessageToBytes(messageBody);
            var routingKey = queueName ?? MessageQueueHelper.GetProducerQueueName(messageBody);

            _manager.DeclareTaskQueue(routingKey);

            using (var channel = _manager.GetChannel())
            {
                var taskKey = _manager.GetTaskKey(routingKey);
                var basicProperties = _basicPropertiesProvider.GetPersistent();
                basicProperties.Persistent = true;

                channel?.BasicPublish(string.Empty, taskKey, true, basicProperties, messageBodyToBytes);
            }
        }
    }
}