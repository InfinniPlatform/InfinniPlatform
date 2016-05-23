using InfinniPlatform.Helpers;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;

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
    }
}