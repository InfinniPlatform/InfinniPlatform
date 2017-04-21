using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Hosting;
using InfinniPlatform.MessageQueue.TestConsumers;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    [Category(TestCategories.IntegrationTest)]
    public class MessageQueueThreadPoolTest : RabbitMqTestBase
    {
        [Test]
        [Category(TestCategories.UnitTest)]
        public async Task ThreadPoolMustLimitNumberOfConcurrentTasks()
        {
            var semaphoreProvider = new MessageQueueThreadPool(new RabbitMqMessageQueueOptions { MaxConcurrentThreads = 1 });

            var result = "";

            const int taskDelay = 1000;

            var task1 = Task.Run(async () =>
                                 {
                                     await semaphoreProvider.Enqueue(async () =>
                                                                     {
                                                                         result += "A";
                                                                         await Task.Delay(taskDelay);
                                                                         result += "A";
                                                                     });
                                 });

            var task2 = Task.Run(async () =>
                                 {
                                     await semaphoreProvider.Enqueue(async () =>
                                                                     {
                                                                         result += "B";
                                                                         await Task.Delay(taskDelay);
                                                                         result += "B";
                                                                     });
                                 });

            await Task.WhenAll(task1, task2);

            Assert.IsTrue((result == "AABB") || (result == "BBAA"), $"Result was: {result}.");
        }

        [Test]
        [Ignore("Inconsistent")]
        [Category(TestCategories.IntegrationTest)]
        public async Task IntegrationTest()
        {
            var messages = new List<string>();
            const int length = 5;
            const int timeout = 1000;
            for (var i = 0; i < length; i++)
                messages.Add(i.ToString());

            var customSettings = RabbitMqMessageQueueOptions.Default;
            customSettings.MaxConcurrentThreads = 1;
            customSettings.PrefetchCount = 0;

            var countdownEvent = new CountdownEvent(length);
            var actualMessages = new ConcurrentBag<string>();
            RegisterConsumers(new[] { new MessageQueueThreadPoolConsumer(actualMessages, countdownEvent, timeout) }, null, customSettings);

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
            foreach (var message in messages)
                await producerBase.PublishAsync(message, "MessageQueueThreadPoolTest");

            Assert.IsTrue(countdownEvent.Wait(5000));
            CollectionAssert.AreEquivalent(messages, actualMessages);
        }
    }
}