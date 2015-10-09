﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using Newtonsoft.Json;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    //[Ignore("Тесты SDK не выполняют запуск сервера InfinniPlatform. Необходимо существование уже запущенного сервера на localhost : 9900")]
    public sealed class FileApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string Route = "1";

        private InfinniFileApi _fileApi;
        private InfinniDocumentApi _documentApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _fileApi = new InfinniFileApi(InfinniSessionServer, InfinniSessionPort,Route);
            _documentApi = new InfinniDocumentApi(InfinniSessionServer, InfinniSessionPort,Route);
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

            using (var fileStream = new FileStream(@"TestData\avatar.gif", FileMode.Open))
            {
                _fileApi.UploadFile("Gameshop", "UserProfile", profileId, "Avatar", "avatar.gif", fileStream);
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


            using (var fileStream = new FileStream(@"TestData\avatar.gif", FileMode.Open))
            {
                _documentApi.AttachFile(sessionId, "Gameshop", "UserProfile", instanceId, "Avatar", "avatar.gif", fileStream);
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


            using (var fileStream = new FileStream(@"TestData\avatar.gif", FileMode.Open))
            {
                _documentApi.AttachFile(sessionId, "Gameshop", "UserProfile", instanceId, "Avatar", "avatar.gif", fileStream);
            }

            _documentApi.DetachFile(sessionId, instanceId, "Avatar");

            _documentApi.SaveSession(sessionId);

            var contentId = _documentApi.GetDocumentById("Gameshop", "UserProfile", instanceId).Avatar;

            Assert.AreEqual(null, contentId);

        }
    }
}