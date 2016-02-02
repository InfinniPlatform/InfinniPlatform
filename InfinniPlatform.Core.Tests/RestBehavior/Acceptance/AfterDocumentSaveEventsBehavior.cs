using System;
using System.Linq;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class AfterDocumentSaveEventsBehavior
    {
        private const string DocumentType = "AfterDocumentSaveDocument";

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
        public void ShouldInvokeSuccessActionOnSuccessSaveDocument()
        {
            // Given
            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);
            var document = new { Id = Guid.NewGuid().ToString(), LastName = "123" };

            // When
            documentApi.SetDocument(DocumentType, document);
            var documents = documentApi.GetDocument(DocumentType, f => f.AddCriteria(cr => cr.Property("TestValue").IsEquals("Test")), 0, 1);

            // Then
            Assert.IsNotNull(documents);
            Assert.IsNotNull(documents.FirstOrDefault());
            Assert.AreEqual("Test", documents.FirstOrDefault().TestValue);
        }
    }
}