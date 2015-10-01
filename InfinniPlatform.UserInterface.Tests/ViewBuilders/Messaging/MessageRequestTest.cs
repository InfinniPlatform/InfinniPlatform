using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.UserInterface.ViewBuilders.Messaging;
using NUnit.Framework;

namespace InfinniPlatform.UserInterface.Tests.ViewBuilders.Messaging
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class MessageRequestTest
    {
        private const string ExchangeName = "Exchange1";

        private static EventWaitHandle CreateRequestHandler(Action handler)
        {
            var startEvent = new ManualResetEvent(false);

            Task.Factory.StartNew(() =>
                {
                    startEvent.WaitOne();
                    handler();
                });

            return startEvent;
        }


        [Test]
        public void ShouldCreateMessageRequest()
        {
            // Given
            const string messageType = "Type1";
            dynamic messageBody = new object();

            // When
            var result = new MessageRequest(ExchangeName, messageType, messageBody);

            // Then
            Assert.AreEqual(ExchangeName, result.ExchangeName);
            Assert.AreEqual(messageType, result.MessageType);
            Assert.AreEqual(messageBody, result.MessageBody);
            Assert.IsNotNull(result.RequestTask);
        }

        [Test]
        public void ShouldErrorComplete()
        {
            // Given
            var error = new Exception();
            var request = new MessageRequest(ExchangeName, "Type1", new object());
            EventWaitHandle startEvent = CreateRequestHandler(() => request.OnErrorComplete(error));

            // When
            startEvent.Set();
            var resultError = Assert.Throws<AggregateException>(() => request.RequestTask.Wait());

            // Then
            CollectionAssert.AreEqual(new[] {error}, resultError.InnerExceptions);
        }

        [Test]
        public void ShouldSuccessComplete()
        {
            // Given
            var request = new MessageRequest(ExchangeName, "Type1", new object());
            EventWaitHandle startEvent = CreateRequestHandler(request.OnSuccessComplete);

            // When
            startEvent.Set();
            request.RequestTask.Wait();

            // Then
            Assert.IsTrue(request.RequestTask.IsCompleted);
        }
    }
}