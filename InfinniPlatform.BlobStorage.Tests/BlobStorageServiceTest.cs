using System;
using System.IO;
using InfinniPlatform.Tests;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace InfinniPlatform.BlobStorage
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class BlobStorageServiceTest
    {
        [Test]
        public void GetFileResponse_should_return_null_if_not_exist()
        {
            //GIVEN

            Mock<IBlobStorage> blobStorageMock = new Mock<IBlobStorage>();
            Mock<ILogger<BlobStorageService>> loggerMock = new Mock<ILogger<BlobStorageService>>();

            var blobStorageService = new BlobStorageService(blobStorageMock.Object, loggerMock.Object);
            var blobId = "testBlobID";

            //WHEN

            var result = blobStorageService.GetFileResponse(blobId);

            //THEN

            blobStorageMock.Verify(b => b.GetBlobData(blobId), Times.Once);
            Assert.Null(result);
        }

        [Test]
        public void GetFileResponse_should_return_correct_response()
        {
            //GIVEN

            Mock<IBlobStorage> blobStorageMock = new Mock<IBlobStorage>();
            Mock<ILogger<BlobStorageService>> loggerMock = new Mock<ILogger<BlobStorageService>>();

            var blobStorageService = new BlobStorageService(blobStorageMock.Object, loggerMock.Object);

            var blobId = "testBlobID";
            var fileName = "TestFileName";
            int size = 700;
            DateTime time = DateTime.MinValue;;

            var blobData = new BlobData()
            {
                Data = () => Stream.Null,

                Info = new BlobInfo()
                {
                    Id = blobId,
                    Name = fileName,
                    Size = size,
                    Time = time,
                    Type = "image/jpg"
                }
            };

            blobStorageMock.Setup(b => b.GetBlobData(blobId)).Returns(blobData);

            //WHEN

            var result = blobStorageService.GetFileResponse(blobId);

            //THEN

            blobStorageMock.Verify(b => b.GetBlobData(blobId), Times.Once);
            Assert.AreEqual(fileName, result.FileName);
            Assert.AreEqual(size, result.ContentLength);
            Assert.AreEqual(time, result.LastWriteTimeUtc);
            Assert.NotNull(result.Headers);
        }
    }
}
