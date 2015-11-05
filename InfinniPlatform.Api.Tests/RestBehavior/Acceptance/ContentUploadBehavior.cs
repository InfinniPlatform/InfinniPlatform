using System;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Tests.Properties;
using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	[Ignore("Необходимо создать конфигурацию метаданных на диске, т.к. теперь метаданные загружаются только с диска")]
    public sealed class ContentUploadBehavior
	{
		private IDisposable _server;
		private string _configurationId = "testconfigacontentupload";
		private string _documentId = "testcontentdocument";

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

        private void CreateTestConfig()
        {
            new IndexApi().RebuildIndex(_configurationId, _documentId);

            MetadataManagerConfiguration managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager(null);
            dynamic config = managerConfig.CreateItem(_configurationId);
            managerConfig.DeleteItem(config);
            managerConfig.MergeItem(config);

            var managerFactoryDocument = new ManagerFactoryConfiguration(null, _configurationId);
            MetadataManagerDocument documentManager = managerFactoryDocument.BuildDocumentManager();
            dynamic doc = documentManager.CreateItem(_documentId);

            doc.Schema = new DynamicWrapper();
            doc.Schema.Properties = new DynamicWrapper();

            doc.Schema.Properties.ContentField = new DynamicWrapper();
            doc.Schema.Properties.ContentField.Type = "Binary";
            doc.Schema.Properties.ContentField.Caption = "Field with content";

            documentManager.MergeItem(doc);

            RestQueryApi.QueryPostNotify(null, _configurationId);
            new UpdateApi(null).UpdateStore(_configurationId);
        }

		[Test]
		public void ShouldUploadContent()
		{
			CreateTestConfig();

			var content = new MemoryStream(Resources.UploadBinaryContent);
			
			dynamic testDocument = new DynamicWrapper();
			testDocument.Id = Guid.NewGuid().ToString();
			testDocument.ContentField = new DynamicWrapper();
			testDocument.ContentField.Info = new DynamicWrapper();
			testDocument.ContentField.Info.Name = "images.jpg";
			testDocument.ContentField.Info.Type = "image/jpeg";
			testDocument.ContentField.Info.Size = 11723;
			

            dynamic result = new DocumentApi().SetDocument(_configurationId, _documentId, testDocument);

            Assert.AreNotEqual(result.IsValid, false);

            dynamic uploadResult = new UploadApi().UploadBinaryContent(_configurationId, _documentId, testDocument.Id, "ContentField",
			                                    @"Authorization.zip", content);

            Assert.AreNotEqual(uploadResult.IsValid, false);

            dynamic storedDocument = new DocumentApi().GetDocument(_configurationId, _documentId, cr => cr.AddCriteria(f => f.Property("Id").IsEquals(testDocument.Id)), 0, 1).FirstOrDefault();

            var resultBlob = new UploadApi().DownloadBinaryContent(storedDocument.ContentField.Info.ContentId);

            Assert.IsNotNull(resultBlob);
		}
	}
}
