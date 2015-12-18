using System;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.NodeServiceHost;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ActionUnitExceptionHandlingBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string DocumentType = "ExceptionHandlingDocument";

        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = InfinniPlatformInprocessHost.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldFormatExceptionMessage()
        {
            // Given
            var restQueryApi = new RestQueryApi((c, d, a) => new RestQueryBuilder(c, d, a));
            var documentApi = new DocumentApi(restQueryApi);
            var document = new { Id = Guid.NewGuid(), LastName = "123" };

            // When
            var result = Assert.Catch(() => documentApi.SetDocument(ConfigurationId, DocumentType, document));

            // Then
            Assert.IsTrue(result.Message.Contains("Important exception details"));
        }
    }
}