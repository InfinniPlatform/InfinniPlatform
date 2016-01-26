using System;
using System.IO;

using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;
using InfinniPlatform.Sdk.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    public sealed class FileApiTest
    {
        private DocumentApiClient _documentApiClient;
        private FileApiClient _fileApiClient;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _fileApiClient = new FileApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);
            _documentApiClient = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);
        }

        [Test]
        public void ShouldUploadAndDownloadFile()
        {
            var document = new
                           {
                               FirstName = "Ronald",
                               LastName = "McDonald"
                           };

            var profileId = _documentApiClient.SetDocument("Gameshop", "UserProfile", document).Id.ToString();

            using (var fileStream = new MemoryStream(Resources.Avatar))
            {
                _documentApiClient.AttachFile("Gameshop", "UserProfile", profileId, "Avatar", fileStream);
            }

            var documentSaved = _documentApiClient.GetDocumentById("Gameshop", "UserProfile", profileId);

            Stream result = _fileApiClient.DownloadFile(documentSaved.Avatar.Info.ContentId);

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

            _documentApiClient.SetDocument("Gameshop", "UserProfile", document);

            using (var fileStream = new MemoryStream(Resources.Avatar))
            {
                _documentApiClient.AttachFile("Gameshop", "UserProfile", document.Id, "Avatar", fileStream);
            }

            dynamic actualDocument = _documentApiClient.GetDocumentById("Gameshop", "UserProfile", document.Id);

            Assert.IsNotNull(actualDocument);
            Assert.IsNotNull(actualDocument.Avatar);
            Assert.IsNotNull(actualDocument.Avatar.Info);
            Assert.IsNotNull(actualDocument.Avatar.Info.ContentId);

            var contentId = actualDocument.Avatar.Info.ContentId;

            Stream result = _fileApiClient.DownloadFile(contentId);

            Assert.IsNotNull(result);
        }
    }
}