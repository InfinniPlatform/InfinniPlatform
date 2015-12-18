using System;
using System.Linq;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.NodeServiceHost;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class DeleteReferencedDocumentBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string MainDocumentType = "DeleteReferencedDocument";
        private const string ReferenceDocumentType = "AddressDocument";

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
        public void ShouldDeleteDocumentWithReferenceCorrectly()
        {
            // Given

            var restQueryApi = new RestQueryApi((c, d, a) => new RestQueryBuilder(c, d, a));
            var documentApi = new DocumentApi(restQueryApi);

            var mainDocumentId = Guid.NewGuid();
            var referenceDocumentId = Guid.NewGuid();

            var mainDocument = new
            {
                Id = mainDocumentId,
                Name = "Ivanov",
                Address = new
                {
                    Id = referenceDocumentId,
                    DisplayName = "Lenina"
                }
            };

            var referenceDocument = new
            {
                Id = referenceDocumentId,
                Street = "Lenina"
            };

            // When

            documentApi.SetDocument(ConfigurationId, ReferenceDocumentType, referenceDocument);
            documentApi.SetDocument(ConfigurationId, MainDocumentType, mainDocument);

            var mainDocumentBeforeDelete = documentApi.GetDocument(ConfigurationId, MainDocumentType, filter => filter.AddCriteria(c => c.Property("Id").IsEquals(mainDocumentId)), 0, 1);
            var referenceDocumentBeforeDelete = documentApi.GetDocument(ConfigurationId, ReferenceDocumentType, filter => filter.AddCriteria(c => c.Property("Id").IsEquals(referenceDocumentId)), 0, 1);

            documentApi.DeleteDocument(ConfigurationId, MainDocumentType, mainDocumentId.ToString());

            var mainDocumentAfterDelete = documentApi.GetDocument(ConfigurationId, MainDocumentType, filter => filter.AddCriteria(c => c.Property("Id").IsEquals(mainDocumentId)), 0, 1);
            var referenceDocumentAfterDelete = documentApi.GetDocument(ConfigurationId, ReferenceDocumentType, filter => filter.AddCriteria(c => c.Property("Id").IsEquals(referenceDocumentId)), 0, 1);

            // Then

            Assert.IsNotNull(mainDocumentBeforeDelete);
            Assert.AreEqual(1, mainDocumentBeforeDelete.Count());
            Assert.IsNotNull(referenceDocumentBeforeDelete);
            Assert.AreEqual(1, referenceDocumentBeforeDelete.Count());

            Assert.IsNotNull(mainDocumentAfterDelete);
            Assert.AreEqual(0, mainDocumentAfterDelete.Count());
            Assert.IsNotNull(referenceDocumentAfterDelete);
            Assert.AreEqual(1, referenceDocumentAfterDelete.Count());
        }
    }
}