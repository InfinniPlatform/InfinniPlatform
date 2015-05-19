using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    public sealed class FileApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string InfinniSessionVersion = "1";
        private InfinniFileApi _fileApi;
        private InfinniDocumentApi _documentApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _fileApi = new InfinniFileApi(InfinniSessionServer, InfinniSessionPort, InfinniSessionVersion);
            _documentApi = new InfinniDocumentApi(InfinniSessionServer, InfinniSessionPort, InfinniSessionVersion);
        }

        [Test]
        public void ShouldUploadAndDownloadFile()
        {
            var document = new
            {
                FirstName = "Ronald",
                LastName = "McDonald",
            };

            var profileId = _documentApi.SetDocument("Gameshop", "UserProfile", Guid.NewGuid().ToString(), document);

            using (var fileStream = new FileStream(@"TestData\avatar.gif", FileMode.Open))
            {
                _fileApi.UploadFile("Gameshop", "UserProfile", profileId,"Avatar", "avatar.gif", fileStream);
            }

            var result = _fileApi.DownloadFile("Gameshop", "UserProfile", profileId, "Avatar");

            Assert.IsNotNull(result);
            Assert.AreEqual(((string)result.Content.ToString()).Length, 9928);
        }

    }
}
