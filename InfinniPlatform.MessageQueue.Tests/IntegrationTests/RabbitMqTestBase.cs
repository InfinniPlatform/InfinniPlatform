using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Hosting;
using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue.Hosting;
using InfinniPlatform.MessageQueue.Management;
using InfinniPlatform.Serialization;

using Moq;

using NUnit.Framework;

using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    public class RabbitMqTestBase
    {
        internal RabbitMqManager RabbitMqManager { get; set; }

        internal RabbitMqManagementHttpClient RabbitMqManagementHttpClient { get; set; }

        internal RabbitMqMessageSerializer RabbitMqMessageSerializer { get; set; }

        internal IRabbitMqBasicPropertiesProvider BasicPropertiesProvider { get; set; }

        [SetUp]
        public async Task SetUp()
        {
            var appOptions = new AppOptions { AppName = TestConstants.AppName, AppInstance = TestConstants.AppInstanceId };

            var logMock = new Mock<ILog>();
            logMock.Setup(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));

            RabbitMqManager = new RabbitMqManager(RabbitMqMessageQueueOptions.Default, appOptions, logMock.Object);
            RabbitMqManagementHttpClient = new RabbitMqManagementHttpClient(RabbitMqMessageQueueOptions.Default, JsonObjectSerializer.Default);

            RabbitMqMessageSerializer = new RabbitMqMessageSerializer(new JsonObjectSerializer());

            var basicPropertiesProviderMock = new Mock<IRabbitMqBasicPropertiesProvider>();
            basicPropertiesProviderMock.Setup(provider => provider.Get())
                                       .Returns(new BasicProperties());
            basicPropertiesProviderMock.Setup(provider => provider.GetPersistent())
                                       .Returns(new BasicProperties());
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
            var logMock = new Mock<ILog>();
            logMock.Setup(log => log.Debug(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));
            logMock.Setup(log => log.Info(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));
            logMock.Setup(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));

            var subscriptionManager = new RabbitMqMessageQueueConsumersManager(new MessageQueueThreadPool(customSettings ?? RabbitMqMessageQueueOptions.Default),
                                                                               RabbitMqMessageSerializer,
                                                                               RabbitMqManager,
                                                                               logMock.Object, new Mock<IPerformanceLog>().Object);

            var list = new List<IConsumer>();
            list.AddRange(taskConsumers ?? Enumerable.Empty<ITaskConsumer>());
            list.AddRange(broadcastConsumers ?? Enumerable.Empty<IBroadcastConsumer>());

            var messageConsumerSourceMock = new Mock<IMessageConsumerSource>();
            messageConsumerSourceMock.Setup(source => source.GetConsumers()).Returns(list);

            var messageConsumersManager = new RabbitMqMessageQueueInitializer(subscriptionManager,
                                                                      new[] { messageConsumerSourceMock.Object },
                                                                      RabbitMqManager,
                                                                      logMock.Object);

            ((IAppStartedHandler) messageConsumersManager).Handle();
        }
    }
}