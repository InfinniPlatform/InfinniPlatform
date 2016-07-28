using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Helpers;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Settings;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests
{
    public class RabbitMqTestBase
    {
        internal static RabbitMqManager RabbitMqManager { get; set; }

        internal static RabbitMqManagementHttpClient RabbitMqManagementHttpClient { get; set; }

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var appEnvironmentMock = new Mock<IAppEnvironment>();
            appEnvironmentMock.SetupGet(env => env.Name).Returns(TestConstants.ApplicationName);

            var logMock = new Mock<ILog>();
            logMock.Setup(log => log.Error(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<Exception>()))
                   .Callback((object message, Dictionary<string, object> context, Exception exception) => { Console.WriteLine(message); });

            RabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, appEnvironmentMock.Object);
            RabbitMqManagementHttpClient = new RabbitMqManagementHttpClient(RabbitMqConnectionSettings.Default);

            await RabbitMqManagementHttpClient.DeleteQueues(await RabbitMqManagementHttpClient.GetQueues());

            WindowsServices.StartService(TestConstants.ServiceName, TestConstants.WaitTimeout);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await RabbitMqManagementHttpClient.DeleteQueues(await RabbitMqManagementHttpClient.GetQueues());
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

            var messageConsumersManager = new MessageConsumersStartupInitializer(taskConsumers ?? Enumerable.Empty<ITaskConsumer>(), broadcastConsumers ?? Enumerable.Empty<IBroadcastConsumer>(), RabbitMqManager, new MessageSerializer(), logMock.Object, perfLogMock.Object);

            messageConsumersManager.OnAfterStart();
        }
    }
}