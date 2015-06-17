using System;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class DeleteReferencedDocumentBehavior
	{
		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

			TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_server.Dispose();
		}

        [Test]
		public void ShouldDeleteDocumentWithReferenceCorrectly()
		{
			var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager(null);
			var config = managerConfiguration.CreateItem("testconfig");
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

			var managerDocument = new ManagerFactoryConfiguration(null, "testconfig").BuildDocumentManager();
			dynamic documentMetadata1 = managerDocument.CreateItem("testdoc1");
			dynamic documentMetadata2 = managerDocument.CreateItem("testdoc2");

			documentMetadata1.Schema = new DynamicWrapper();
			documentMetadata1.Schema.Type = "testdoc1";
			documentMetadata1.Schema.Caption = "testdoc1";
			documentMetadata1.Schema.Properties = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Id.Type = "Uuid";
			documentMetadata1.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata1.Schema.Properties.Name = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Name.Type = "String";
			documentMetadata1.Schema.Properties.Name.Caption = "Patient name";

			documentMetadata1.Schema.Properties.Address = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.Type = "Object";
			documentMetadata1.Schema.Properties.Address.TypeInfo = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.ConfigId = "testconfig";
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.DocumentId = "testdoc2";
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.Resolve = true;
			documentMetadata1.Schema.Properties.Address.Caption = "Patient address";


			documentMetadata2.Schema = new DynamicWrapper();
			documentMetadata2.Schema.Type = "testdoc2";
			documentMetadata2.Schema.Caption = "testdoc2";
			documentMetadata2.Schema.Properties = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id.Type = "Uuid";
			documentMetadata2.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata2.Schema.Properties.Street = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.Type = "String";
			documentMetadata2.Schema.Properties.Street.Caption = "Address street";


            managerDocument.MergeItem(documentMetadata1);
            managerDocument.MergeItem(documentMetadata2);


			RestQueryApi.QueryPostNotify(null, "testconfig");

            new UpdateApi(null).UpdateStore("testconfig");


			var uid1 = Guid.NewGuid().ToString();
			var uid2 = Guid.NewGuid().ToString();

			dynamic testDoc1Instance = new DynamicWrapper();
			testDoc1Instance.Id = uid1;
			testDoc1Instance.Name = "Ivanov";
			testDoc1Instance.Address = new DynamicWrapper();
			testDoc1Instance.Address.Id = uid2;
			testDoc1Instance.Address.DisplayName = "Lenina";

			dynamic testDoc2Instance = new DynamicWrapper();
			testDoc2Instance.Id = uid2;
			testDoc2Instance.Street = "Lenina";

			new DocumentApi(null).SetDocument("testconfig", "testdoc1", testDoc1Instance);
			new DocumentApi(null).SetDocument("testconfig", "testdoc2", testDoc2Instance);


			dynamic storedDoc1 = new DocumentApi(null).GetDocument("testconfig", "testdoc1", filter => filter.AddCriteria(c => c.Property("Id").IsEquals(uid1)), 0, 1);
			dynamic storedDoc2 = new DocumentApi(null).GetDocument("testconfig", "testdoc2", filter => filter.AddCriteria(c => c.Property("Id").IsEquals(uid2)), 0, 1);

			Assert.IsNotNull(storedDoc1);
			Assert.IsNotNull(storedDoc2);

			new DocumentApi(null).DeleteDocument("testconfig", "testdoc1", uid1);

			storedDoc1 = new DocumentApi(null).GetDocument("testconfig", "testdoc1", filter => filter.AddCriteria(c => c.Property("Id").IsEquals(uid1)), 0, 1);
			storedDoc2 = new DocumentApi(null).GetDocument("testconfig", "testdoc2", filter => filter.AddCriteria(c => c.Property("Id").IsEquals(uid2)), 0, 1);

			Assert.AreEqual(storedDoc1.Count, 0);
			Assert.AreEqual(storedDoc2.Count, 1);
		}
	}
}
