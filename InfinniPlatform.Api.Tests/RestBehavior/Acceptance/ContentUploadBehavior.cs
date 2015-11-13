using System;
using System.IO;
using System.Linq;
using System.Text;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Tests.Properties;
using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ContentUploadBehavior
    {
        private const string ConfigId = "TestConfiguration";
        private const string DocumentId = "ContentUploadDocument";

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
        public void ShouldUploadContent()
        {
            // Given

            var contentBytes = Resources.UploadBinaryContent;
            var contentStream = new MemoryStream(contentBytes);

            dynamic testDocument = new DynamicWrapper();
            testDocument.Id = Guid.NewGuid().ToString();
            testDocument.ContentField = new DynamicWrapper();
            testDocument.ContentField.Info = new DynamicWrapper();
            testDocument.ContentField.Info.Name = "images.jpg";
            testDocument.ContentField.Info.Type = "image/jpeg";
            testDocument.ContentField.Info.Size = contentBytes.LongLength;

            // When & Then

            var saveResult = new DocumentApi().SetDocument(ConfigId, DocumentId, testDocument);
            Assert.AreNotEqual(saveResult.IsValid, false);

            // When & Then

            var uploadResult = new UploadApi().UploadBinaryContent(ConfigId, DocumentId, testDocument.Id, "ContentField", @"Authorization.zip", contentStream);
            Assert.AreNotEqual(uploadResult.IsValid, false);

            // When & Then

            var storedDocument = new DocumentApi().GetDocument(ConfigId, DocumentId, cr => cr.AddCriteria(f => f.Property("Id").IsEquals(testDocument.Id)), 0, 1).FirstOrDefault();
            Assert.IsNotNull(storedDocument);
            Assert.IsNotNull(storedDocument.ContentField);
            Assert.IsNotNull(storedDocument.ContentField.Info);
            Assert.IsNotNull(storedDocument.ContentField.Info.ContentId);

            // When & Then

            var downloadResult = new UploadApi().DownloadBinaryContent(storedDocument.ContentField.Info.ContentId);
            Assert.IsNotNull(downloadResult);
            Assert.IsNotNull(downloadResult.Content);
            Assert.AreEqual(Encoding.UTF8.GetString(contentBytes), downloadResult.Content);
        }
    }
}