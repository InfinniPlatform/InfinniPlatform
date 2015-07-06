using System;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;

namespace InfinniPlatform.SystemConfig.Tests
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ControllerBehavior
    {
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldGetConfigurationHelp()
        {
            var builder = new RestQueryBuilder("SystemConfig", "metadata", "helpconfiguration", null);

            RestQueryResponse help = builder.QueryHelp("dmd");

            Assert.IsNotNull(help);

            // Для того, чтобы следующая проверка прошла, нужно предварительно
            // сгенерировать документацию
            // Assert.True(help.Content.Contains("<html>"));
        }

        [Test]
        public void ShouldGetPrefillUnits()
        {
            RestQueryResponse response = RestQueryApi.QueryPostJsonRaw("systemconfig", "prefill", "getfillitems", null,
                                                                       null);
            Assert.AreEqual(true, response.IsAllOk);
        }


        [Test]
        public void ShouldGetRegisteredConfigList()
        {
            var builder = new RestQueryBuilder( "SystemConfig", "metadata", "getregisteredconfiglist", null);
            RestQueryResponse result = builder.QueryPostJson(null, null);

            Assert.True(result.IsAllOk);

            Assert.IsNotNull(result.ToDynamic().ConfigList);
        }


        [Test]
        public void ShouldGetServiceTypes()
        {
            var builder = new RestQueryBuilder( "SystemConfig", "metadata", "getservicemetadata", null);

            RestQueryResponse response = builder.QueryGet(null, 0, 1000);

            Assert.AreEqual(true, response.IsAllOk);
            Assert.True(
                response.Content.Contains(
                    "[{\"Name\":\"applyevents\",\"WorkflowExtensionPoints\":[{\"Name\":\"FilterEvents\",\"ContextType\":4,\"Caption\":\"Document filter events context\"},{\"Name\":\"Move\",\"ContextType\":2,\"Caption\":\"Document move context\"},{\"Name\":\"GetResult\",\"ContextType\":8,\"Caption\":\"Document move result context\"}]},{\"Name\":\"applyjson\",\"WorkflowExtensionPoints\":[{\"Name\":\"FilterEvents\",\"ContextType\":4,\"Caption\":\"Document filter events context\"},{\"Name\":\"Move\",\"ContextType\":2,\"Caption\":\"Document move context\"},{\"Name\":\"GetResult\",\"ContextType\":8,\"Caption\":\"Document move result context\"}]},{\"Name\":\"notify\",\"WorkflowExtensionPoints\":[]},{\"Name\":\"search\",\"WorkflowExtensionPoints\":[{\"Name\":\"ValidateFilter\",\"ContextType\":16,\"Caption\":\"Document search context\"},{\"Name\":\"SearchModel\",\"ContextType\":16,\"Caption\":\"Document search context\"}]},{\"Name\":\"upload\",\"WorkflowExtensionPoints\":[{\"Name\":\"Upload\",\"ContextType\":32,\"Caption\":\"File upload context\"}]},{\"Name\":\"urlencodeddata\",\"WorkflowExtensionPoints\":[{\"Name\":\"ProcessUrlEncodedData\",\"ContextType\":64,\"Caption\":\"Unknown context type\"}]},{\"Name\":\"aggregation\",\"WorkflowExtensionPoints\":[{\"Name\":\"Join\",\"ContextType\":16,\"Caption\":\"Document search context\"},{\"Name\":\"TransformResult\",\"ContextType\":16,\"Caption\":\"Document search context\"}]}]"));
        }

        [Test]
        public void ShouldGetStandardExtensionPoints()
        {
            var builder = new RestQueryBuilder("SystemConfig", "metadata", "getstandardextensionpoints", null);

            RestQueryResponse response = builder.QueryGet(null, 0, 1000);

            Assert.AreEqual(true, response.IsAllOk);
            Assert.True(response.Content.Contains("[\"Action\",\"OnSuccess\",\"OnFail\",\"Validation\"]"));
        }
    }
}