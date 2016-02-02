using System;
using System.IO;
using System.Linq;
using System.Text;

using InfinniPlatform.Core.Tests.Properties;
using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ContentUploadBehavior
    {
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

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);
            var fileApi = new FileApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

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

            var saveResult = documentApi.SetDocument(DocumentId, testDocument);
            Assert.AreNotEqual(saveResult.IsValid, false);

            // When & Then

            documentApi.AttachFile(DocumentId, testDocument.Id, "ContentField", contentStream);

            // When & Then

            var storedDocument = documentApi.GetDocument(DocumentId, cr => cr.AddCriteria(f => f.Property("Id").IsEquals(testDocument.Id)), 0, 1).FirstOrDefault();
            Assert.IsNotNull(storedDocument);
            Assert.IsNotNull(storedDocument.ContentField);
            Assert.IsNotNull(storedDocument.ContentField.Info);
            Assert.IsNotNull(storedDocument.ContentField.Info.ContentId);

            // When & Then

            Stream downloadResult = fileApi.DownloadFile(storedDocument.ContentField.Info.ContentId);
            Assert.IsNotNull(downloadResult);
            Assert.AreEqual(Encoding.UTF8.GetString(contentBytes), ReadAsString(downloadResult));
        }

        private static string ReadAsString(Stream stream)
        {
            return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
        }
    }
}