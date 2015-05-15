using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;

using NUnit.Framework;

namespace InfinniPlatform.TestData.TestDatabases
{
	[TestFixture]
	[Ignore]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class QueryDesignerDatabase
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
		public void ShoudlCreateQueryDatabase()
		{
			IndexApi.RebuildIndex("TestQueryConfig", "Patient");
			IndexApi.RebuildIndex("TestQueryConfig", "Address");
			IndexApi.RebuildIndex("TestQueryConfig", "Policy");

			var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager();
			string configurationId = "TestQueryConfig";
			var testconfig = managerConfig.CreateItem(configurationId);

            managerConfig.MergeItem(testconfig);

			var managerDocument = new ManagerFactoryConfiguration(configurationId).BuildDocumentManager();

			dynamic documentMetadata1 = managerDocument.CreateItem("Patient");
			dynamic documentMetadata2 = managerDocument.CreateItem("Address");
			dynamic documentMetadata3 = managerDocument.CreateItem("Policy");


			documentMetadata1.Schema = new DynamicWrapper();
			documentMetadata1.Schema.Name = "Patient";
			documentMetadata1.Schema.Caption = "Patient";
			documentMetadata1.Schema.Properties = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Id.Type = "Uuid";
			documentMetadata1.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata1.Schema.Properties.Name = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Name.Type = "String";
			documentMetadata1.Schema.Properties.Name.Caption = "Patient name";

			//weak reference
			documentMetadata1.Schema.Properties.Address = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.Type = "Object";
			documentMetadata1.Schema.Properties.Address.TypeInfo = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.ConfigId = configurationId;
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.DocumentId = "Address";
			documentMetadata1.Schema.Properties.Address.Caption = "Patient address";

			//inline reference
			documentMetadata1.Schema.Properties.Policies = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Policies.Type = "Array";
			documentMetadata1.Schema.Properties.Policies.Caption = "Policies";
			documentMetadata1.Schema.Properties.Policies.Items = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Policies.Items.Type = "Object";
			documentMetadata1.Schema.Properties.Policies.Items.TypeInfo = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Policies.Items.TypeInfo.DocumentLink = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Policies.Items.TypeInfo.DocumentLink.ConfigId = configurationId;
			documentMetadata1.Schema.Properties.Policies.Items.TypeInfo.DocumentLink.DocumentId = "Policy";
			documentMetadata1.Schema.Properties.Policies.Items.TypeInfo.DocumentLink.Inline = true;

			documentMetadata1.Schema.Properties.Phones = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Phones.Type = "Array";
			documentMetadata1.Schema.Properties.Phones.Caption = "Phones";
			documentMetadata1.Schema.Properties.Phones.Items = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Phones.Items.Type = "String";

			documentMetadata2.Schema = new DynamicWrapper();
			documentMetadata2.Schema.Name = "Address";
			documentMetadata2.Schema.Caption = "Address";
			documentMetadata2.Schema.Properties = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id.Type = "Uuid";
			documentMetadata2.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata2.Schema.Properties.Street = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.Type = "String";
			documentMetadata2.Schema.Properties.Street.Caption = "Address street";

			documentMetadata3.Schema = new DynamicWrapper();
			documentMetadata3.Schema.Name = "Policy";
			documentMetadata3.Schema.Caption = "Policy";
			documentMetadata3.Schema.Properties = new DynamicWrapper();

			documentMetadata3.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata3.Schema.Properties.Id.Type = "Uuid";
			documentMetadata3.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata3.Schema.Properties.Number = new DynamicWrapper();
			documentMetadata3.Schema.Properties.Number.Type = "String";
			documentMetadata3.Schema.Properties.Number.Caption = "Policy number";


            managerDocument.MergeItem(documentMetadata1);
            managerDocument.MergeItem(documentMetadata2);
            managerDocument.MergeItem(documentMetadata3);

			RestQueryApi.QueryPostNotify(configurationId);

			var addressId = Guid.NewGuid().ToString();
			var policy1Id = Guid.NewGuid().ToString();
			var policy2Id = Guid.NewGuid().ToString();

			dynamic testDocument1 = new DynamicWrapper();
			testDocument1.Id = Guid.NewGuid().ToString();
			testDocument1.Name = "Ivanov";
			testDocument1.Address = new DynamicWrapper();
			testDocument1.Address.Id = addressId;
			testDocument1.Address.DisplayName = "г. Челябинск";

			testDocument1.Policies = new List<dynamic>();
			testDocument1.Policies.Add(new DynamicWrapper());
			testDocument1.Policies.Add(new DynamicWrapper());

			testDocument1.Policies[0].Id = policy1Id;
			testDocument1.Policies[0].Number = "7070";

			testDocument1.Policies[1].Id = policy2Id;
			testDocument1.Policies[1].Number = "7070";

			new DocumentApi().SetDocument("TestQueryConfig", "Patient", testDocument1);

			dynamic testAddress1 = new DynamicWrapper();
			testAddress1.Id = addressId;
			testAddress1.Street = "г. Челябинск, пр. Ленина";

			new DocumentApi().SetDocument("TestQueryConfig", "Address", testAddress1);
		}
	}
}
