using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.MessageQueue.Management;

namespace InfinniPlatform.MessageQueue
{
    internal class RabbitMqTaskProducer : ITaskProducer
    {
        public RabbitMqTaskProducer(RabbitMqManager manager,
                                    IRabbitMqMessageSerializer messageSerializer,
                                    IRabbitMqBasicPropertiesProvider basicPropertiesProvider)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
            _basicPropertiesProvider = basicPropertiesProvider;
        }


        private readonly RabbitMqManager _manager;
        private readonly IRabbitMqMessageSerializer _messageSerializer;
        private readonly IRabbitMqBasicPropertiesProvider _basicPropertiesProvider;


        public void Publish<T>(T messageBody, string queueName = null)
        {
            RabbitMqHelper.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);
        }

        public void PublishDynamic(DynamicWrapper messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);
        }

        public async Task PublishAsync<T>(T messageBody, string queueName = null)
        {
            RabbitMqHelper.CheckTypeRestrictions<T>();

            await Task.Run(() => { BasicPublish(messageBody, queueName); });
        }

        public async Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName)
        {
            await Task.Run(() => { BasicPublish(messageBody, queueName); });
        }

        private void BasicPublish<T>(T messageBody, string queueName)
        {
            var messageBodyToBytes = _messageSerializer.MessageToBytes(messageBody);
            var routingKey = queueName ?? RabbitMqHelper.GetProducerQueueName(messageBody);

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