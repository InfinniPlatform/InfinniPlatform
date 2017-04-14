using System;
using InfinniPlatform.MessageQueue.RabbitMQ;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;

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
            var appEnvMock = new Mock<IAppEnvironment>();
            appEnvMock.Setup(env => env.InstanceId).Returns(appId);

            var basicPropertiesProvider = new BasicPropertiesProvider(appEnvMock.Object, jsonObjSerializerMock.Object);

            var basicProperties = basicPropertiesProvider.Get();

            Assert.AreEqual(appId, basicProperties.AppId);
        }

        [Test]
        public void BasicPropertiesProviderHandleEmptyValues()
        {
            var appEnvMock = new Mock<IAppEnvironment>();

            var jsonObjectSerializer = new JsonObjectSerializer();

            var basicPropertiesProvider = new BasicPropertiesProvider(appEnvMock.Object, jsonObjectSerializer);

            var basicProperties = basicPropertiesProvider.Get();

            Assert.AreEqual(0, basicProperties.Headers.Count);
        }
    }
}