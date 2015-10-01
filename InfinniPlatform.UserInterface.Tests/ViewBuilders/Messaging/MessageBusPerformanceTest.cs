using System;
using System.Diagnostics;
using System.Threading.Tasks;
using InfinniPlatform.UserInterface.ViewBuilders.Messaging;
using NUnit.Framework;

namespace InfinniPlatform.UserInterface.Tests.ViewBuilders.Messaging
{
    [TestFixture]
    [Category(TestCategories.PerformanceTest)]
    public sealed class MessageBusPerformanceTest
    {
        private const string Exchange = "Exchange1";
        private const string MessageType = "MessageType1";


        [Test]
        [TestCase(1000, 1)]
        [TestCase(1000, 2)]
        [TestCase(1000, 3)]
        [TestCase(1000, 4)]
        [TestCase(1000, 5)]
        public async void AverageDeliveryTimeWhenFrequentMessages(int messageCount, int subscriberCount)
        {
            // Среднее время доставки при условии, что размер очереди превышает одно сообщение (частые сообщения)

            // Given

            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange);

            for (int i = 0; i < subscriberCount; ++i)
            {
                messageExchange.Subscribe(MessageType, m => { });
            }

            // When

            var stopwatch = new Stopwatch();
            var messageTasks = new Task[messageCount];

            await messageExchange.SendAsync(MessageType, message); // JIT

            for (int i = 0; i < messageCount; ++i)
            {
                messageTasks[i] = messageExchange.SendAsync(MessageType, message);
            }

            stopwatch.Start();
            Task.WaitAll(messageTasks);
            stopwatch.Stop();

            // Then
            Console.WriteLine("Message count:     {0}", messageCount);
            Console.WriteLine("Subscriber count:  {0}", subscriberCount);
            Console.WriteLine("Delivery time:     {0:N2} ms/message", stopwatch.Elapsed.TotalMilliseconds/messageCount);
            Console.WriteLine("Processing speed : {0:N2} message/sec", messageCount/stopwatch.Elapsed.TotalSeconds);
        }

        [Test]
        [TestCase(1000, 1)]
        [TestCase(1000, 2)]
        [TestCase(1000, 3)]
        [TestCase(1000, 4)]
        [TestCase(1000, 5)]
        public async void AverageDeliveryTimeWhenInfrequentMessages(int messageCount, int subscriberCount)
        {
            // Среднее время доставки при условии, что размер очереди не превышает одно сообщение (редкие сообщения)

            // Given

            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange);

            for (int i = 0; i < subscriberCount; ++i)
            {
                messageExchange.Subscribe(MessageType, m => { });
            }

            // When

            var stopwatch = new Stopwatch();

            await messageExchange.SendAsync(MessageType, message); // JIT

            for (int i = 0; i < messageCount; ++i)
            {
                stopwatch.Start();
                await messageExchange.SendAsync(MessageType, message);
                stopwatch.Stop();
            }

            // Then
            Console.WriteLine("Message count:     {0}", messageCount);
            Console.WriteLine("Subscriber count:  {0}", subscriberCount);
            Console.WriteLine("Delivery time:     {0:N2} ms/message", stopwatch.Elapsed.TotalMilliseconds/messageCount);
            Console.WriteLine("Processing speed : {0:N2} message/sec", messageCount/stopwatch.Elapsed.TotalSeconds);
        }
    }
}