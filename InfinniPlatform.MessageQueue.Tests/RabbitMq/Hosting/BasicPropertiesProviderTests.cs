using System;
using System.Security.Claims;
using System.Security.Principal;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Security;
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
            var userIdentityPorviderMock = new Mock<IUserIdentityProvider>();
            var jsonObjSerializerMock = new Mock<IJsonObjectSerializer>();

            var appId = Guid.NewGuid().ToString();
            var appEnvMock = new Mock<IAppEnvironment>();
            appEnvMock.Setup(env => env.InstanceId).Returns(appId);

            var basicPropertiesProvider = new BasicPropertiesProvider(appEnvMock.Object, userIdentityPorviderMock.Object, jsonObjSerializerMock.Object);

            var basicProperties = basicPropertiesProvider.Get();

            Assert.AreEqual(appId, basicProperties.AppId);
        }

        [Test]
        public void BasicPropertiesProviderFillsHeaders()
        {
            var appEnvMock = new Mock<IAppEnvironment>();

            const string username = "username";
            const string claim = "tenantId";
            var userIdentityPorviderMock = new Mock<IUserIdentityProvider>();

            var identity = new Mock<IIdentity>();
            identity.Setup(i => i.Name)
                    .Returns(username);

            var claimsIdentity = new ClaimsIdentity(identity.Object);
            claimsIdentity.AddClaim(ApplicationClaimTypes.TenantId, claim);

            userIdentityPorviderMock.Setup(provider => provider.GetUserIdentity())
                                    .Returns(claimsIdentity);

            var jsonObjectSerializer = new JsonObjectSerializer();

            var basicPropertiesProvider = new BasicPropertiesProvider(appEnvMock.Object, userIdentityPorviderMock.Object, jsonObjectSerializer);

            var basicProperties = basicPropertiesProvider.Get();

            Assert.AreEqual(2, basicProperties.Headers.Count);
            Assert.AreEqual(username, jsonObjectSerializer.Deserialize<string>((byte[])basicProperties.Headers[MessageHeadersTypes.UserName]));
            Assert.AreEqual(claim, jsonObjectSerializer.Deserialize<string>((byte[])basicProperties.Headers[MessageHeadersTypes.TenantId]));
        }

        [Test]
        public void BasicPropertiesProviderHandleEmptyValues()
        {
            var appEnvMock = new Mock<IAppEnvironment>();

            var userIdentityPorviderMock = new Mock<IUserIdentityProvider>();
            userIdentityPorviderMock.Setup(provider => provider.GetUserIdentity())
                                    .Returns((IIdentity)null);

            var jsonObjectSerializer = new JsonObjectSerializer();

            var basicPropertiesProvider = new BasicPropertiesProvider(appEnvMock.Object, userIdentityPorviderMock.Object, jsonObjectSerializer);

            var basicProperties = basicPropertiesProvider.Get();

            Assert.AreEqual(0, basicProperties.Headers.Count);
        }
    }
}