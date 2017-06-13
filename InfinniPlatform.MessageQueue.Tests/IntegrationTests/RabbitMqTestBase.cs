using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfinniPlatform.Hosting;
using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue.Hosting;
using InfinniPlatform.MessageQueue.Management;
using InfinniPlatform.Serialization;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    public class RabbitMqTestBase
    {
        internal RabbitMqManager RabbitMqManager { get; set; }

        internal RabbitMqManagementHttpClient RabbitMqManagementHttpClient { get; set; }

        internal MessageSerializer MessageSerializer { get; set; }

        internal IBasicPropertiesProvider BasicPropertiesProvider { get; set; }

        [SetUp]
        public async Task SetUp()
        {
            var appOptions = new AppOptions {AppName = TestConstants.AppName, AppInstance = TestConstants.AppInstanceId};

            RabbitMqManager = new RabbitMqManager(RabbitMqMessageQueueOptions.Default, appOptions, new Mock<ILogger<RabbitMqManager>>().Object);
            RabbitMqManagementHttpClient = new RabbitMqManagementHttpClient(RabbitMqMessageQueueOptions.Default, JsonObjectSerializer.Default);

            MessageSerializer = new MessageSerializer(new JsonObjectSerializer());

            var basicPropertiesProviderMock = new Mock<IBasicPropertiesProvider>();
            basicPropertiesProviderMock.Setup(provider => provider.Get()).Returns(new BasicProperties());
            basicPropertiesProviderMock.Setup(provider => provider.GetPersistent()).Returns(new BasicProperties());
            BasicPropertiesProvider = basicPropertiesProviderMock.Object;

            var queues = (await RabbitMqManagementHttpClient.GetQueues()).ToArray();

            using (var channel = RabbitMqManager.GetChannel())
            {
                foreach (var queue in queues)
                {
                    channel.QueueDelete(queue.Name, false, false);
                }
            }
        }

        [TearDown]
        public async Task TearDown()
        {
            var queues = (await RabbitMqManagementHttpClient.GetQueues()).ToArray();

            using (var channel = RabbitMqManager.GetChannel())
            {
                foreach (var queue in queues)
                {
                    channel.QueueDelete(queue.Name, false, false);
                }
            }

            RabbitMqManager.Dispose();
        }

        public void RegisterConsumers(IEnumerable<ITaskConsumer> taskConsumers, IEnumerable<IBroadcastConsumer> broadcastConsumers, RabbitMqMessageQueueOptions customSettings = null)
        {
            var subscriptionManager = new MessageQueueConsumersManager(new MessageQueueThreadPool(customSettings ?? RabbitMqMessageQueueOptions.Default),
                MessageSerializer,
                RabbitMqManager,
                new Mock<ILogger<MessageQueueConsumersManager>>().Object,
                new Mock<IPerformanceLogger<MessageQueueConsumersManager>>().Object);

            var list = new List<IConsumer>();
            list.AddRange(taskConsumers ?? Enumerable.Empty<ITaskConsumer>());
            list.AddRange(broadcastConsumers ?? Enumerable.Empty<IBroadcastConsumer>());

            var messageConsumerSourceMock = new Mock<IConsumerSource>();
            messageConsumerSourceMock.Setup(source => source.GetConsumers()).Returns(list);

            var messageConsumersManager = new MessageQueueInitializer(subscriptionManager,
                new[] {messageConsumerSourceMock.Object},
                RabbitMqManager,
                new Mock<ILogger<MessageQueueInitializer>>().Object);

            ((IAppStartedHandler) messageConsumersManager).Handle();
        }
    }
}