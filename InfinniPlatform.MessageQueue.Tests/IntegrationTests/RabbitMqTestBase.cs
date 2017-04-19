﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;
using InfinniPlatform.Core.Serialization;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI;
using InfinniPlatform.MessageQueue.RabbitMQ;
using InfinniPlatform.MessageQueue.RabbitMQ.Hosting;
using InfinniPlatform.MessageQueue.RabbitMQ.Serialization;

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
            var appOptions = new AppOptions { AppName = TestConstants.AppName, AppInstance = TestConstants.AppInstanceId };

            var logMock = new Mock<ILog>();
            logMock.Setup(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));

            RabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, appOptions, logMock.Object);
            RabbitMqManagementHttpClient = new RabbitMqManagementHttpClient(RabbitMqConnectionSettings.Default, JsonObjectSerializer.Default);

            MessageSerializer = new MessageSerializer(new JsonObjectSerializer());

            var basicPropertiesProviderMock = new Mock<IBasicPropertiesProvider>();
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

        public void RegisterConsumers(IEnumerable<ITaskConsumer> taskConsumers, IEnumerable<IBroadcastConsumer> broadcastConsumers, RabbitMqConnectionSettings customSettings = null)
        {
            var logMock = new Mock<ILog>();
            logMock.Setup(log => log.Debug(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));
            logMock.Setup(log => log.Info(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));
            logMock.Setup(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<Func<Dictionary<string, object>>>()));

            var subscriptionManager = new MessageQueueConsumersManager(new Mock<IMessageConsumeEventHandler>().Object,
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

            ((IAppStartedHandler) messageConsumersManager).Handle();
        }
    }
}