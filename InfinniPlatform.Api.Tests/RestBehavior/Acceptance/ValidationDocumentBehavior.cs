using System;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.NodeServiceHost;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ValidationDocumentBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string DocumentType = "ValidationDocument";

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
        public void ShouldValidateDocumentWithActionUnit()
        {
            // Given
            var documentApi = new DocumentApi();
            var document = new { Id = Guid.NewGuid(), LastName = "123" };

            // When
            var error = Assert.Catch(() => documentApi.SetDocument(ConfigurationId, DocumentType, document));

            // Then
            Assert.IsTrue(error.Message.Contains("\"IsValid\":false"));
            Assert.IsTrue(error.Message.Contains("\"TestComplexValidatorMessage\""));
        }
    }
}