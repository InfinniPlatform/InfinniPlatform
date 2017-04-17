using InfinniPlatform.Core.Abstractions.Http;
using InfinniPlatform.DocumentStorage.HttpService;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.HttpService
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class HttpServiceWrapperFactoryTest
    {
        [Test]
        public void TestMethod()
        {
            // Given
            var success = false;
            var realService = new Mock<IHttpService>();
            realService.Setup(m => m.Load(It.IsAny<IHttpServiceBuilder>())).Callback(() => success = true);

            // When
            var serviceWrapperFactory = new HttpServiceWrapperFactory();
            var serviceWrapper = serviceWrapperFactory.CreateServiceWrapper("WrapperName", realService.Object);
            serviceWrapper.Load(null);

            // Then
            Assert.IsTrue(success);
            Assert.AreEqual("WrapperName", serviceWrapper.GetType().Name);
        }
    }
}