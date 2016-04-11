using System;
using System.IO;

using InfinniPlatform.Sdk.BlobStorage;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.BlobStorage.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class FileSystemBlobStorageTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var mimeTypeResolverMock = new Mock<IMimeTypeResolver>();
            var performanceLogMock = new Mock<IPerformanceLog>();

            _blobStorage = new FileSystemBlobStorage(
                new FileSystemBlobStorageSettings(),
                JsonObjectSerializer.Default,
                mimeTypeResolverMock.Object,
                performanceLogMock.Object);
        }


        private IBlobStorage _blobStorage;


        private static void AssertDateTime(DateTime expected, DateTime actual)
        {
            Assert.IsTrue(Math.Abs((expected - actual).TotalMinutes) <= 1, @"Expected: '{0}', but was: '{1}'.", expected, actual);
        }

        [Test]
        public void ShouldCreateReadUpdateDelete()
        {
            // GIVEN

            const string blobNameV1 = "Attachment.pdf";
            const string blobTypeV1 = "application/pdf";
            var blobDataV1 = new byte[] { 1, 2, 3 };

            const string blobNameV2 = "Attachment.doc";
            const string blobTypeV2 = "application/msword";
            var blobDataV2 = new byte[] { 3, 2, 1 };

            // WHEN

            // Create
            var blobId = _blobStorage.CreateBlob(blobNameV1, blobTypeV1, blobDataV1).Id;

            // Read
            var timeV1 = DateTime.UtcNow;
            var infoV1 = _blobStorage.GetBlobInfo(blobId);
            var dataV1 = _blobStorage.GetBlobData(blobId);
            var dataV1Bytes = GetBlobDataBytes(dataV1.Data);

            // Update
            _blobStorage.UpdateBlob(blobId, blobNameV2, blobTypeV2, blobDataV2);

            // Read
            var timeV2 = DateTime.UtcNow;
            var infoV2 = _blobStorage.GetBlobInfo(blobId);
            var dataV2 = _blobStorage.GetBlobData(blobId);
            var dataV2Bytes = GetBlobDataBytes(dataV2.Data);

            // Delete
            _blobStorage.DeleteBlob(blobId);

            // Read
            var infoV3 = _blobStorage.GetBlobInfo(blobId);
            var dataV3 = _blobStorage.GetBlobData(blobId);

            // THEN

            Assert.IsNotNull(infoV1);
            Assert.IsNotNull(dataV1);
            Assert.IsNotNull(dataV1.Info);
            Assert.IsNotNull(dataV1.Data);
            Assert.AreEqual(blobId, infoV1.Id);
            Assert.AreEqual(blobId, dataV1.Info.Id);
            Assert.AreEqual(blobNameV1, infoV1.Name);
            Assert.AreEqual(blobNameV1, dataV1.Info.Name);
            Assert.AreEqual(blobTypeV1, infoV1.Type);
            Assert.AreEqual(blobTypeV1, dataV1.Info.Type);
            Assert.AreEqual(blobDataV1.LongLength, infoV1.Size);
            Assert.AreEqual(blobDataV1.LongLength, dataV1.Info.Size);
            Assert.AreEqual(infoV1.Time, dataV1.Info.Time);
            AssertDateTime(timeV1, infoV1.Time);
            CollectionAssert.AreEqual(blobDataV1, dataV1Bytes);

            Assert.IsNotNull(infoV2);
            Assert.IsNotNull(dataV2);
            Assert.IsNotNull(dataV2.Info);
            Assert.IsNotNull(dataV2.Data);
            Assert.AreEqual(blobId, infoV2.Id);
            Assert.AreEqual(blobId, dataV2.Info.Id);
            Assert.AreEqual(blobNameV2, infoV2.Name);
            Assert.AreEqual(blobNameV2, dataV2.Info.Name);
            Assert.AreEqual(blobTypeV2, infoV2.Type);
            Assert.AreEqual(blobTypeV2, dataV2.Info.Type);
            Assert.AreEqual(blobDataV2.LongLength, infoV2.Size);
            Assert.AreEqual(blobDataV2.LongLength, dataV2.Info.Size);
            Assert.AreEqual(infoV2.Time, dataV2.Info.Time);
            AssertDateTime(timeV2, infoV2.Time);
            CollectionAssert.AreEqual(blobDataV2, dataV2Bytes);

            Assert.GreaterOrEqual(infoV2.Time, infoV1.Time);

            Assert.IsNull(infoV3);
            Assert.IsNull(dataV3);
        }

        private static byte[] GetBlobDataBytes(Func<Stream> blobData)
        {
            if (blobData != null)
            {
                using (var result = new MemoryStream())
                using (var dataStream = blobData())
                {
                    dataStream.CopyTo(result);
                    result.Flush();

                    return result.ToArray();
                }
            }

            return null;
        }
    }
}