using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ValidationDocumentBehavior
    {
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
            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);
            var document = new { Id = Guid.NewGuid(), LastName = "123" };

            // When
            var error = Assert.Catch(() => documentApi.SetDocument(DocumentType, document));

            // Then
            Assert.IsTrue(error.Message.Contains(@"TestComplexValidatorMessage"));
        }
    }
}