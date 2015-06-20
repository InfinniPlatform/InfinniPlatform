using System;
using System.Threading;
using InfinniPlatform.UserInterface.ViewBuilders.Messaging;
using NUnit.Framework;

namespace InfinniPlatform.UserInterface.Tests.ViewBuilders.Messaging
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class MessageQueueTest
    {
        private class SomeMessage
        {
            public bool IsHandled;
        }

        [Test]
        public void ShouldDisposeQueue()
        {
            // Given
            var messageQueue = new MessageQueue<SomeMessage>(message => { });

            // When
            TestDelegate disposeMessageQueue = messageQueue.Dispose;

            // Then
            Assert.DoesNotThrow(disposeMessageQueue);
        }

        [Test]
        public void ShouldHandleAllMessages()
        {
            // Given

            var message1 = new SomeMessage();
            var message2 = new SomeMessage();
            var message3 = new SomeMessage();
            var messageCount = new CountdownEvent(3);
            var messageQueue = new MessageQueue<SomeMessage>(message =>
                {
                    message.IsHandled = true;
                    messageCount.Signal();
                });

            // When
            messageQueue.Enqueue(message1);
            messageQueue.Enqueue(message2);
            messageQueue.Enqueue(message3);
            messageCount.Wait();

            // Then
            Assert.IsTrue(message1.IsHandled);
            Assert.IsTrue(message2.IsHandled);
            Assert.IsTrue(message3.IsHandled);
        }

        [Test]
        public void ShouldIgnoreHandleErrors()
        {
            // Given

            var message1 = new SomeMessage();
            var message2 = new SomeMessage();
            var message3 = new SomeMessage();
            var messageCount = new CountdownEvent(3);

            var messageQueue = new MessageQueue<SomeMessage>(message =>
                {
                    try
                    {
                        message.IsHandled = true;
                        throw new Exception();
                    }
                    finally
                    {
                        messageCount.Signal();
                    }
                });

            // When
            messageQueue.Enqueue(message1);
            messageQueue.Enqueue(message2);
            messageQueue.Enqueue(message3);
            messageCount.Wait();

            // Then
            Assert.IsTrue(message1.IsHandled);
            Assert.IsTrue(message2.IsHandled);
            Assert.IsTrue(message3.IsHandled);
        }
    }
}