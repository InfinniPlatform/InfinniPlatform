using System;
using InfinniPlatform.MessageQueue.RabbitMQ;
using InfinniPlatform.Serialization;
using InfinniPlatform.Settings;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq.Hosting
{
    [Category(TestCategories.UnitTest)]
    public class BasicPropertiesProviderTests
    {
        [Test]
        public void BasicPropertiesProviderFillsAppId()
        {
            var jsonObjSerializerMock = new Mock<IJsonObjectSerializer>();

            var appId = Guid.NewGuid().ToString();
            var appOptions = new AppOptions { AppInstance = appId };

            var basicPropertiesProvider = new BasicPropertiesProvider(appOptions, jsonObjSerializerMock.Object);

            var basicProperties = basicPropertiesProvider.Get();

            Assert.AreEqual(appId, basicProperties.AppId);
        }

        [Test]
        public void BasicPropertiesProviderHandleEmptyValues()
        {
            var appOptions = new AppOptions();

            var jsonObjectSerializer = new JsonObjectSerializer();

            var basicPropertiesProvider = new BasicPropertiesProvider(appOptions, jsonObjectSerializer);

            var basicProperties = basicPropertiesProvider.Get();

            Assert.AreEqual(0, basicProperties.Headers.Count);
        }
    }
}