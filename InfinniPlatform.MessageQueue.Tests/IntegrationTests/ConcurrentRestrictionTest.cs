using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [Category(TestCategories.IntegrationTest)]
    public class ConcurrentRestrictionTest : RabbitMqTestBase
    {
        [Test]
        public async Task Test()
        {
            var semaphoreProvider = new MessageQueueThreadPool(new RabbitMqConnectionSettings { MaxConcurrentThreads = 1 });

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
        public async Task ConcurrentTest()
        {
            var messages = new List<string>();
            const int length = 5;
            const int timeout = 1000;
            for (var i = 0; i < length; i++)
            {
                messages.Add(i.ToString());
            }

            var customSettings = RabbitMqConnectionSettings.Default;
            customSettings.MaxConcurrentThreads = 1;
            customSettings.PrefetchCount = 0;

            var countdownEvent = new CountdownEvent(length);
            var actualMessages = new ConcurrentBag<string>();
            RegisterConsumers(new[] { new Consumero(actualMessages, countdownEvent, timeout) }, null, customSettings);

            var producerBase = new TaskProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProviderMock);
            foreach (var message in messages)
            {
                await producerBase.PublishAsync(message, "ConcurrentRestrictionTest");
            }

            Assert.IsTrue(countdownEvent.Wait(5000));
            CollectionAssert.AreEquivalent(messages, actualMessages);
        }
    }


    [QueueName("ConcurrentRestrictionTest")]
    public class Consumero : TaskConsumerBase<string>
    {
        public Consumero(ConcurrentBag<string> messages,
                         CountdownEvent completeEvent,
                         int workTime)
        {
            _messages = messages;
            _completeEvent = completeEvent;
            _workTime = workTime;
        }

        private readonly CountdownEvent _completeEvent;
        private readonly ConcurrentBag<string> _messages;
        private readonly int _workTime;

        protected override async Task Consume(Message<string> message)
        {
            _messages.Add(message.Body);
            await Task.Delay(_workTime);
            _completeEvent.Signal();
        }
    }
}