using System;
using System.Collections;
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
        private const string DocumentType = "ContentUploadDocument";

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

            var saveResult = documentApi.SetDocument(DocumentType, testDocument);
            Assert.AreNotEqual(saveResult.IsValid, false);

            // When & Then

            documentApi.AttachFile(DocumentType, testDocument.Id, "ContentField", "images.jpg", "image/jpeg", contentStream);

            // When & Then

            var storedDocument = documentApi.GetDocument(DocumentType, cr => cr.AddCriteria(f => f.Property("Id").IsEquals(testDocument.Id)), 0, 1).FirstOrDefault();
            Assert.IsNotNull(storedDocument);
            Assert.IsNotNull(storedDocument.ContentField);
            Assert.IsNotNull(storedDocument.ContentField.Info);
            Assert.IsNotNull(storedDocument.ContentField.Info.ContentId);

            // When & Then

            Stream downloadResult = fileApi.DownloadFile(storedDocument.ContentField.Info.ContentId);
            Assert.IsNotNull(downloadResult);
            Assert.AreEqual(Encoding.UTF8.GetString(contentBytes), ReadAsString(downloadResult));
        }

        [Test]
        public void ShouldAttachFileToArrayItem()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);
            var fileApi = new FileApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            var documentId = Guid.NewGuid().ToString();
            var contentBytes = Resources.UploadBinaryContent;

            var document = new DynamicWrapper
                           {
                               { "_id", documentId },
                               { "Id", documentId },
                               {
                                   "subDocument", new DynamicWrapper
                                                  {
                                                      {
                                                          "items", new[]
                                                                   {
                                                                       new DynamicWrapper { { "name", "item0" } },
                                                                       new DynamicWrapper { { "name", "item1" } },
                                                                       new DynamicWrapper { { "name", "item2" } }
                                                                   }
                                                      }
                                                  }
                               }
                           };

            // When & Then

            documentApi.SetDocument(DocumentType, document);
            documentApi.AttachFile(DocumentType, documentId, "subDocument.items.0.file", "file1.zip", "application/zip", new MemoryStream(contentBytes));
            documentApi.AttachFile(DocumentType, documentId, "subDocument.items.1.file", "file2.zip", "application/zip", new MemoryStream(contentBytes));
            documentApi.AttachFile(DocumentType, documentId, "subDocument.items.2.file", "file3.zip", "application/zip", new MemoryStream(contentBytes));

            var storedDocument = documentApi.GetDocument(DocumentType, cr => cr.AddCriteria(f => f.Property("Id").IsEquals(documentId)), 0, 1).FirstOrDefault();

            Assert.IsNotNull(storedDocument);
            Assert.IsNotNull(storedDocument.subDocument);
            Assert.IsInstanceOf<IEnumerable>(storedDocument.subDocument.items);
            Assert.AreEqual(3, ((IEnumerable)storedDocument.subDocument.items).Cast<object>().Count());

            Assert.IsNotNull(storedDocument.subDocument.items[0]);
            Assert.AreEqual("item0", storedDocument.subDocument.items[0].name);
            Assert.IsNotNull(storedDocument.subDocument.items[0].file);
            Assert.IsNotNull(storedDocument.subDocument.items[0].file.Info);
            Assert.IsNotNull(storedDocument.subDocument.items[0].file.Info.ContentId);

            Assert.IsNotNull(storedDocument.subDocument.items[1]);
            Assert.AreEqual("item1", storedDocument.subDocument.items[1].name);
            Assert.IsNotNull(storedDocument.subDocument.items[1].file);
            Assert.IsNotNull(storedDocument.subDocument.items[1].file.Info);
            Assert.IsNotNull(storedDocument.subDocument.items[1].file.Info.ContentId);

            Assert.IsNotNull(storedDocument.subDocument.items[2]);
            Assert.AreEqual("item2", storedDocument.subDocument.items[2].name);
            Assert.IsNotNull(storedDocument.subDocument.items[2].file);
            Assert.IsNotNull(storedDocument.subDocument.items[2].file.Info);
            Assert.IsNotNull(storedDocument.subDocument.items[2].file.Info.ContentId);

            // Then & Then

            Stream file1 = fileApi.DownloadFile(storedDocument.subDocument.items[0].file.Info.ContentId);
            Stream file2 = fileApi.DownloadFile(storedDocument.subDocument.items[0].file.Info.ContentId);
            Stream file3 = fileApi.DownloadFile(storedDocument.subDocument.items[0].file.Info.ContentId);

            FileAssert.AreEqual(new MemoryStream(contentBytes), file1);
            FileAssert.AreEqual(new MemoryStream(contentBytes), file2);
            FileAssert.AreEqual(new MemoryStream(contentBytes), file3);
        }


        private static string ReadAsString(Stream stream)
        {
            return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
        }
    }
}