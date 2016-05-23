using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using EasyNetQ.Management.Client.Model;

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

            var bindings = RabbitMqManager.GetBindings();
            Trace.WriteLine(bindings.Count());

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