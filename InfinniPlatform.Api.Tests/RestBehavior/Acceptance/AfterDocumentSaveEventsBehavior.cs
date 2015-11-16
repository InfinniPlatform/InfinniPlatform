using System;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.NodeServiceHost;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class AfterDocumentSaveEventsBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
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
            var documentApi = new DocumentApi();
            var document = new { Id = Guid.NewGuid().ToString(), LastName = "123" };

            // When
            documentApi.SetDocument(ConfigurationId, DocumentType, document, false);
            var documents = documentApi.GetDocument(ConfigurationId, DocumentType, f => f.AddCriteria(cr => cr.Property("TestValue").IsEquals("Test")), 0, 1);

            // Then
            Assert.AreEqual("Test", documents?.FirstOrDefault()?.TestValue);
        }
    }
}