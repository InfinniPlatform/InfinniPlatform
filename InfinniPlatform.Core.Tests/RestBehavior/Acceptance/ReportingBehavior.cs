using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ReportingBehavior
    {
        private const string DocumentType = "TestDocument";

        private IDisposable _server;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _server = InfinniPlatformInprocessHost.Start();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldGetPrintViewPdf()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);
            var printViewApi = new PrintViewApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            var document = new { TestProperty = Guid.NewGuid() };

            // When

            documentApi.SetDocument(DocumentType, document);

            var response = printViewApi.GetPrintView(DocumentType, "TestPrintView", "ListView", 0, 10, f => f.AddCriteria(cr => cr.Property("TestProperty").IsEquals(document.TestProperty)));

            // Then

            Assert.IsNotNull(response);
        }
    }
}