using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.UserInterface.ViewBuilders.Messaging;
using NUnit.Framework;

namespace InfinniPlatform.UserInterface.Tests.ViewBuilders.Messaging
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class MessageBusTest
    {
        private const int MaxTimeout = 10000;
        private const string Exchange1 = "Exchange1";
        private const string Exchange2 = "Exchange2";
        private const string MessageType1 = "MessageType1";
        private const string MessageType2 = "MessageType2";

        private class MessageHandler
        {
            public int Count { get; private set; }
            public object Message { get; private set; }

            public void SuccessHandle(dynamic message)
            {
                Count++;
                Message = message;
            }

            public void ErrorHandle(dynamic message)
            {
                Count++;
                Message = message;
                throw new Exception();
            }
        }


        [Test]
        public void ShouldDeliverMessageToValidSubscribers()
        {
            // Given

            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);

            var messageOfType1 = new object();
            var messageOfType2 = new object();

            var handlerForType1 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handlerForType1.SuccessHandle);

            var handlerForType2 = new MessageHandler();
            messageExchange.Subscribe(MessageType2, handlerForType2.SuccessHandle);

            // When
            Task sendMessageOfType1Task = messageExchange.SendAsync(MessageType1, messageOfType1);
            Task sendMessageOfType2Task = messageExchange.SendAsync(MessageType2, messageOfType2);
            bool sendMessageOfType1Result = sendMessageOfType1Task.Wait(MaxTimeout);
            bool sendMessageOfType2Result = sendMessageOfType2Task.Wait(MaxTimeout);

            // Then
            Assert.IsTrue(sendMessageOfType1Result);
            Assert.IsTrue(sendMessageOfType2Result);
            Assert.AreEqual(1, handlerForType1.Count);
            Assert.AreEqual(messageOfType1, handlerForType1.Message);
            Assert.AreEqual(1, handlerForType2.Count);
            Assert.AreEqual(messageOfType2, handlerForType2.Message);
        }

        [Test]
        public void ShouldDisposeBus()
        {
            // Given
            var messageBus = new MessageBus();

            // When
            TestDelegate disposeBus = messageBus.Dispose;

            // Then
            Assert.DoesNotThrow(disposeBus);
        }

        [Test]
        public void ShouldIgnoreHandlerErrors()
        {
            // Given
            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);

            var handler1 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler1.ErrorHandle);

            var handler2 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler2.SuccessHandle);

            // When
            Task sendTask = messageExchange.SendAsync(MessageType1, message);
            Assert.Throws<AggregateException>(() => sendTask.Wait(MaxTimeout));

            // Then
            Assert.AreEqual(1, handler1.Count);
            Assert.AreEqual(message, handler1.Message);
            Assert.AreEqual(1, handler2.Count);
            Assert.AreEqual(message, handler2.Message);
        }

        [Test]
        public void ShouldIsolateExchanges()
        {
            // Given
            var message1 = new object();
            var message2 = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange1 = messageBus.GetExchange(Exchange1);
            IMessageExchange messageExchange2 = messageBus.GetExchange(Exchange2);

            var handler1OfExchange1 = new MessageHandler();
            messageExchange1.Subscribe(MessageType1, handler1OfExchange1.SuccessHandle);

            var handler1OfExchange2 = new MessageHandler();
            messageExchange2.Subscribe(MessageType1, handler1OfExchange2.SuccessHandle);

            // When
            Task sendExchange1Task = messageExchange1.SendAsync(MessageType1, message1);
            Task sendExchange2Task = messageExchange2.SendAsync(MessageType1, message2);
            bool sendResult = Task.WaitAll(new[] {sendExchange1Task, sendExchange2Task}, MaxTimeout);

            // Then
            Assert.IsTrue(sendResult);
            Assert.AreEqual(1, handler1OfExchange1.Count);
            Assert.AreEqual(message1, handler1OfExchange1.Message);
            Assert.AreEqual(1, handler1OfExchange2.Count);
            Assert.AreEqual(message2, handler1OfExchange2.Message);
        }

        [Test]
        public void ShouldSendAsyncWhenMultipleSubscribers()
        {
            // Given
            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);

            var handler1 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler1.SuccessHandle);

            var handler2 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler2.SuccessHandle);

            var handler3 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler3.SuccessHandle);

            // When
            Task sendTask = messageExchange.SendAsync(MessageType1, message);
            bool sendResult = sendTask.Wait(MaxTimeout);

            // Then
            Assert.IsTrue(sendResult);
            Assert.AreEqual(1, handler1.Count);
            Assert.AreEqual(message, handler1.Message);
            Assert.AreEqual(1, handler2.Count);
            Assert.AreEqual(message, handler2.Message);
            Assert.AreEqual(1, handler3.Count);
            Assert.AreEqual(message, handler3.Message);
        }

        [Test]
        public void ShouldSendAsyncWhenNoSubscribers()
        {
            // Given
            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);

            // When
            Task sendTask = messageExchange.SendAsync(MessageType1, message);
            bool sendResult = sendTask.Wait(MaxTimeout);

            // Then
            Assert.IsTrue(sendResult);
        }

        [Test]
        public void ShouldSendAsyncWhenOneSubscribers()
        {
            // Given
            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);

            var handler1 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler1.SuccessHandle);

            // When
            Task sendTask = messageExchange.SendAsync(MessageType1, message);
            bool sendResult = sendTask.Wait(MaxTimeout);

            // Then
            Assert.IsTrue(sendResult);
            Assert.AreEqual(1, handler1.Count);
            Assert.AreEqual(message, handler1.Message);
        }

        [Test]
        public void ShouldSendSync()
        {
            // Given
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);
            var completeHandle = new CountdownEvent(3);

            messageExchange.Subscribe(MessageType1, m =>
                {
                    Task.Delay(500);
                    completeHandle.Signal();
                });
            messageExchange.Subscribe(MessageType1, m =>
                {
                    Task.Delay(500);
                    completeHandle.Signal();
                });
            messageExchange.Subscribe(MessageType1, m =>
                {
                    Task.Delay(500);
                    completeHandle.Signal();
                });

            // When
            messageExchange.Send(MessageType1, new object());
            bool isComplete = completeHandle.Wait(0);

            // Then
            Assert.IsTrue(isComplete);
        }

        [Test]
        public void ShouldSendSyncWhenMultipleSubscribers()
        {
            // Given
            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);

            var handler1 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler1.SuccessHandle);

            var handler2 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler2.SuccessHandle);

            var handler3 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler3.SuccessHandle);

            // When
            messageExchange.Send(MessageType1, message);

            // Then
            Assert.AreEqual(1, handler1.Count);
            Assert.AreEqual(message, handler1.Message);
            Assert.AreEqual(1, handler2.Count);
            Assert.AreEqual(message, handler2.Message);
            Assert.AreEqual(1, handler3.Count);
            Assert.AreEqual(message, handler3.Message);
        }

        [Test]
        public void ShouldSendSyncWhenNoSubscribers()
        {
            // Given
            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);

            // When
            messageExchange.Send(MessageType1, message);
        }

        [Test]
        public void ShouldSendSyncWhenOneSubscribers()
        {
            // Given
            var message = new object();
            var messageBus = new MessageBus();
            IMessageExchange messageExchange = messageBus.GetExchange(Exchange1);

            var handler1 = new MessageHandler();
            messageExchange.Subscribe(MessageType1, handler1.SuccessHandle);

            // When
            messageExchange.Send(MessageType1, message);

            // Then
            Assert.AreEqual(1, handler1.Count);
            Assert.AreEqual(message, handler1.Message);
        }
    }
}