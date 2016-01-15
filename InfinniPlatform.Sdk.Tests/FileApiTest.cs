using System;
using System.IO;

using InfinniPlatform.Sdk.RestApi;
using InfinniPlatform.Sdk.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    public sealed class FileApiTest
    {
        private InfinniDocumentApi _documentApi;
        private InfinniFileApi _fileApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _fileApi = new InfinniFileApi(HostingConfig.Default.Name, HostingConfig.Default.Port);
            _documentApi = new InfinniDocumentApi(HostingConfig.Default.Name, HostingConfig.Default.Port);
        }

        [Test]
        public void ShouldUploadAndDownloadFile()
        {
            var document = new
                           {
                               FirstName = "Ronald",
                               LastName = "McDonald"
                           };

            var profileId = _documentApi.SetDocument("Gameshop", "UserProfile", document).Id.ToString();

            using (var fileStream = new MemoryStream(Resources.Avatar))
            {
                _documentApi.AttachFile("Gameshop", "UserProfile", profileId, "Avatar", fileStream);
            }

            var documentSaved = _documentApi.GetDocumentById("Gameshop", "UserProfile", profileId);

            Stream result = _fileApi.DownloadFile(documentSaved.Avatar.Info.ContentId);

            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldAttachFileInSession()
        {
            var document = new
                           {
                               Id = Guid.NewGuid().ToString(),
                               FirstName = "Ronald",
                               LastName = "McDonald"
                           };

            _documentApi.SetDocument("Gameshop", "UserProfile", document);

            using (var fileStream = new MemoryStream(Resources.Avatar))
            {
                _documentApi.AttachFile("Gameshop", "UserProfile", document.Id, "Avatar", fileStream);
            }

            dynamic actualDocument = _documentApi.GetDocumentById("Gameshop", "UserProfile", document.Id);

            Assert.IsNotNull(actualDocument?.Avatar?.Info?.ContentId);

            var contentId = actualDocument.Avatar.Info.ContentId;

            Stream result = _fileApi.DownloadFile(contentId);

            Assert.IsNotNull(result);
        }
    }
}