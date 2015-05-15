using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class ContentUploadBehavior
	{
		private IDisposable _server;
		private string _configurationId = "testconfigacontentupload";
		private string _documentId = "testcontentdocument";

		[TestFixtureSetUp]
		public void FixtureSetup()
		{

			_server = TestApi.StartServer(c => c
				.SetHostingConfig(HostingConfig.Default));

            TestApi.InitClientRouting(HostingConfig.Default);
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_server.Dispose();
		}

		[Test]
		public void ShouldUploadContent()
		{
			CreateTestConfig();

			dynamic testDocument = new DynamicWrapper();
			testDocument.Id = Guid.NewGuid().ToString();
			testDocument.ContentField = new DynamicWrapper();
			testDocument.ContentField.Info = new DynamicWrapper();
			testDocument.ContentField.Info.Name = "images.jpg";
			testDocument.ContentField.Info.Type = "image/jpeg";
			testDocument.ContentField.Info.Size = 11723;
			

			var result = new DocumentApi().SetDocument(_configurationId, _documentId, testDocument);

			Assert.AreNotEqual(result.IsValid,false);

			dynamic uploadResult = new UploadApi().UploadBinaryContent(_configurationId, _documentId, testDocument.Id, "ContentField",
			                                    @"TestData\Configurations\Authorization.zip");

			Assert.AreNotEqual(uploadResult.IsValid,false);

			dynamic resultBlob =new UploadApi().DownloadBinaryContent(_configurationId, _documentId, testDocument.Id, "ContentField");

			Assert.IsNotNull(resultBlob);

		}

		private void CreateTestConfig()
		{
			
			IndexApi.RebuildIndex(_configurationId, _documentId);

			var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager();
			dynamic config = managerConfig.CreateItem(_configurationId);
			managerConfig.DeleteItem(config);
			managerConfig.MergeItem(config);

			var managerFactoryDocument = new ManagerFactoryConfiguration(_configurationId);
			var documentManager = managerFactoryDocument.BuildDocumentManager();
			dynamic doc = documentManager.CreateItem(_documentId);
			
			doc.Schema = new DynamicWrapper();
			doc.Schema.Properties = new DynamicWrapper();

			doc.Schema.Properties.ContentField = new DynamicWrapper();
			doc.Schema.Properties.ContentField.Type = "Binary";
			doc.Schema.Properties.ContentField.Caption = "Field with content";

			documentManager.MergeItem(doc);

			RestQueryApi.QueryPostNotify(_configurationId);
			UpdateApi.UpdateStore(_configurationId);

		}


	}
}
