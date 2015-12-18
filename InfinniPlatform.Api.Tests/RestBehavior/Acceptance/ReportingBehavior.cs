using System;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.NodeServiceHost;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ReportingBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string DocumentType = "PrefillDocument";

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
        public void ShouldGetPrintViewPdf()
        {
            // Given

            var restQueryApi = new RestQueryApi((c, d, a) => new RestQueryBuilder(c, d, a));
            var documentApi = new DocumentApi(restQueryApi);

            var document = new { TestProperty = Guid.NewGuid() };

            // When

            documentApi.SetDocument(ConfigurationId, DocumentType, document);

            var filter = new FilterBuilder().AddCriteria(cr => cr.Property("TestProperty").IsEquals(document.TestProperty)).GetFilter();

            var queryParam = new
            {
                ConfigId = ConfigurationId,
                DocumentId = DocumentType,
                PrintViewId = "TestPrintView",
                PrintViewType = "ListView",
                PageNumber = 0,
                PageSize = 100,
                Query = filter
            };

            var response = restQueryApi.QueryPostUrlEncodedData("SystemConfig", "Reporting", "GetPrintView", queryParam);

            // Then

            Assert.IsNotNull(response);
        }
    }
}