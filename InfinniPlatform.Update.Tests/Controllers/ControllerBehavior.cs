using System;
using System.Collections.Generic;
using System.Net;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Update.Tests.Builders;
using NUnit.Framework;
using RestSharp;

namespace InfinniPlatform.Update.Tests.Controllers
{
    [TestFixture]
    [Category(TestCategories.BusinessLogicTest)]
    public sealed class ControllerBehavior
    {
        private IDisposable _server;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ConfigurationBuilder.InitScriptStorage();

            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldSaveAssemblyVersion()
        {
            dynamic package = new PackageBuilder().BuildPackage("Update", null, "InfinniPlatform.Update.dll");

            new UpdateApi(null).InstallPackages(new[] {package});
        }

        [Test]
        public void ShouldUpdateVersion()
        {
            var body = new Dictionary<string, object>
                {
                    {"metadataConfigurationId", "systemconfig"}
                };

            var restClient =
                new RestClient(string.Format("{0}://{1}:{2}/0/Update/StandardApi/Package/Notify/",
                                             TestSettings.DefaultHostingConfig.ServerScheme,
                                             TestSettings.DefaultHostingConfig.ServerName,
                                             TestSettings.DefaultHostingConfig.ServerPort));

            IRestResponse restResponse = restClient.Post(new RestRequest {RequestFormat = DataFormat.Json}.AddBody(body));

            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
        }
    }
}