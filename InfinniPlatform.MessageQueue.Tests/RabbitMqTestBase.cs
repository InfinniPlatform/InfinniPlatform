﻿using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Helpers;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues.Consumers;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests
{
    public class RabbitMqTestBase
    {
        internal static RabbitMqManager RabbitMqManager { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            RabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, TestConstants.ApplicationName);

            RabbitMqManager.DeleteQueues(RabbitMqManager.GetQueues());

            WindowsServices.StartService(TestConstants.ServiceName, TestConstants.WaitTimeout);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            RabbitMqManager.DeleteQueues(RabbitMqManager.GetQueues());
        }

        public static void RegisterConsumers(IEnumerable<ITaskConsumer> taskConsumers, IEnumerable<IBroadcastConsumer> broadcastConsumers)
        {
            var logMock = new Mock<ILog>();
            logMock.Setup(log => log.Debug(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<Exception>()))
                   .Callback((object message, Dictionary<string, object> context, Exception exception) => { Console.WriteLine(message); });

            logMock.Setup(log => log.Info(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<Exception>()))
                   .Callback((object message, Dictionary<string, object> context, Exception exception) => { Console.WriteLine(message); });

            logMock.Setup(log => log.Error(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<Exception>()))
                   .Callback((object message, Dictionary<string, object> context, Exception exception) => { Console.WriteLine(message); });

            var perfLogMock = new Mock<IPerformanceLog>();
            perfLogMock.Setup(log => log.Log(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                       .Callback((string method, DateTime start, string outcome) => { Console.WriteLine(method); });

            var messageConsumersManager = new MessageConsumersStartupInitializer(taskConsumers ?? Enumerable.Empty<ITaskConsumer>(), broadcastConsumers ?? Enumerable.Empty<IBroadcastConsumer>(), RabbitMqManager, new MessageSerializer(), logMock.Object, perfLogMock.Object);

            messageConsumersManager.OnAfterStart();
        }
    }
}