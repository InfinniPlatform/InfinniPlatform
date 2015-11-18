using System;
using System.IO;

using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    public sealed class FileApiTest
    {
        private const string Route = "1";

        private InfinniFileApi _fileApi;
        private InfinniDocumentApi _documentApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _fileApi = new InfinniFileApi(HostingConfig.Default.ServerName, HostingConfig.Default.ServerPort.ToString(), Route);
            _documentApi = new InfinniDocumentApi(HostingConfig.Default.ServerName, HostingConfig.Default.ServerPort.ToString(), Route);
        }

        [Test]
        public void ShouldUploadAndDownloadFile()
        {
            var document = new
            {
                FirstName = "Ronald",
                LastName = "McDonald",
            };

            var profileId = _documentApi.SetDocument("Gameshop", "UserProfile", document).Id.ToString();

            using (var fileStream = new MemoryStream(Resources.Avatar))
            {
                _fileApi.UploadFile("Gameshop", "UserProfile", profileId, "Avatar", "Avatar.gif", fileStream);
            }

            var documentSaved = _documentApi.GetDocumentById("Gameshop", "UserProfile",profileId);

            var result = _fileApi.DownloadFile(documentSaved.Avatar.Info.ContentId);

            Assert.IsNotNull(result);
            Assert.AreEqual(((string)result.Content.ToString()).Length, 9928);
        }

        [Test]
        public void ShouldAttachFileInSession()
        {
            var document = new
            {
                FirstName = "Ronald",
                LastName = "McDonald",
            };

            var sessionId = _documentApi.CreateSession().SessionId.ToString();

            var instanceId = _documentApi.Attach(sessionId, "Gameshop", "UserProfile", Guid.NewGuid().ToString(), document).Id.ToString();


            using (var fileStream = new MemoryStream(Resources.Avatar))
            {
                _documentApi.AttachFile(sessionId, "Gameshop", "UserProfile", instanceId, "Avatar", "Avatar.gif", fileStream);
            }

            _documentApi.SaveSession(sessionId);

            var contentId = _documentApi.GetDocumentById("Gameshop", "UserProfile", instanceId).Avatar.Info.ContentId;

            var result = _fileApi.DownloadFile(contentId);

            Assert.IsNotNull(result);
            Assert.AreEqual(((string)result.Content.ToString()).Length, 9928);
        }


        [Test]
        public void ShouldDetachFileInSession()
        {
            var document = new
            {
                FirstName = "Ronald",
                LastName = "McDonald",
            };

            var sessionId = _documentApi.CreateSession().SessionId.ToString();

            var instanceId = _documentApi.Attach(sessionId, "Gameshop", "UserProfile", Guid.NewGuid().ToString(), document).Id.ToString();


            using (var fileStream = new MemoryStream(Resources.Avatar))
            {
                _documentApi.AttachFile(sessionId, "Gameshop", "UserProfile", instanceId, "Avatar", "Avatar.gif", fileStream);
            }

            _documentApi.DetachFile(sessionId, instanceId, "Avatar");

            _documentApi.SaveSession(sessionId);

            var contentId = _documentApi.GetDocumentById("Gameshop", "UserProfile", instanceId).Avatar;

            Assert.AreEqual(null, contentId);

        }
    }
}
