using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;

using Moq;

using NUnit.Framework;

using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
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
            var appEnvironmentMock = new Mock<IAppEnvironment>();
            appEnvironmentMock.SetupGet(env => env.Name).Returns(TestConstants.ApplicationName);
            appEnvironmentMock.SetupGet(env => env.Id).Returns(TestConstants.ApplicationName);

            var logMock = new Mock<ILog>();
            logMock.Setup(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));

            RabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, appEnvironmentMock.Object, logMock.Object);
            RabbitMqManagementHttpClient = new RabbitMqManagementHttpClient(RabbitMqConnectionSettings.Default);

            MessageSerializer = new MessageSerializer(new JsonObjectSerializer());

            var basicPropertiesProviderMock = new Mock<IBasicPropertiesProvider>();
            basicPropertiesProviderMock.Setup(provider => provider.Create())
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

        public void RegisterConsumers(IEnumerable<ITaskConsumer> taskConsumers, IEnumerable<IBroadcastConsumer> broadcastConsumers, RabbitMqConnectionSettings customSettings = null)
        {
            var logMock = new Mock<ILog>();
            logMock.Setup(log => log.Debug(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));
            logMock.Setup(log => log.Info(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));
            logMock.Setup(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));

            var subscriptionManager = new MessageQueueSubscriptionManager(new Mock<IMessageConsumeEventHandler>().Object,
                                                                          new MessageQueueThreadPool(customSettings ?? RabbitMqConnectionSettings.Default),
                                                                          MessageSerializer,
                                                                          RabbitMqManager,
                                                                          logMock.Object, new Mock<IPerformanceLog>().Object);

            var list = new List<IConsumer>();
            list.AddRange(taskConsumers ?? Enumerable.Empty<ITaskConsumer>());
            list.AddRange(broadcastConsumers ?? Enumerable.Empty<IBroadcastConsumer>());

            var messageConsumerSourceMock = new Mock<IMessageConsumerSource>();
            messageConsumerSourceMock.Setup(source => source.GetConsumers()).Returns(list);

            var messageConsumersManager = new MessageQueueInitializer(subscriptionManager,
                                                                      new[] { messageConsumerSourceMock.Object },
                                                                      RabbitMqManager,
                                                                      logMock.Object);

            messageConsumersManager.OnAfterStart();
        }
    }
}