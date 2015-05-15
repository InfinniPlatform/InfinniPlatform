using System;
using System.Diagnostics;
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
	public sealed class CreateDocumentBehavior
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
		public void ShouldCreateDocument()
		{
			//создаем конфигурацию с документом для предзаполнения поля "Дата осмотра"


			var configId = "TestConfigPrefillSchema";

			string documentId = "TestDocument";

			string documentIdReferenceInline = "TestDocument1";

			string documentIdReferenceInlineInner = "TestDocument2";


			var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager();

			dynamic config = managerConfig.CreateItem(configId);
            managerConfig.DeleteItem(config);
            managerConfig.MergeItem(config);

			var managerDocument = new ManagerFactoryConfiguration(configId).BuildDocumentManager();


			IndexApi.RebuildIndex(configId, documentId);

			var documentMetadata1 = managerDocument.CreateItem(documentId);

			documentMetadata1.Schema = new DynamicWrapper();
			documentMetadata1.Schema.Type = documentId;
			documentMetadata1.Schema.Caption = documentId;
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
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.ConfigId = configId;
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.DocumentId = documentIdReferenceInline;
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.Inline = true;

			documentMetadata1.Schema.Properties.ObservationDate = new DynamicWrapper();
			documentMetadata1.Schema.Properties.ObservationDate.Type = "DateTime";
			documentMetadata1.Schema.Properties.ObservationDate.Caption = "Дата осмотра";

			var documentMetadata2 = managerDocument.CreateItem(documentIdReferenceInline);

			documentMetadata2.Schema = new DynamicWrapper();
			documentMetadata2.Schema.Type = documentIdReferenceInline;
			documentMetadata2.Schema.Caption = documentIdReferenceInline;
			documentMetadata2.Schema.Properties = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id.Type = "Uuid";
			documentMetadata2.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata2.Schema.Properties.Street = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.Type = "Object";
			documentMetadata2.Schema.Properties.Street.Caption = "Address street";
			documentMetadata2.Schema.Properties.Street.TypeInfo = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.TypeInfo.DocumentLink = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.TypeInfo.DocumentLink.ConfigId = configId;
			documentMetadata2.Schema.Properties.Street.TypeInfo.DocumentLink.DocumentId = documentIdReferenceInlineInner;
			documentMetadata2.Schema.Properties.Street.TypeInfo.DocumentLink.Inline = true;

			documentMetadata2.Schema.Properties.Street.PostIndex = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.PostIndex.Type = "String";
			documentMetadata2.Schema.Properties.Street.PostIndex.Caption = "Post index";


			var documentMetadata3 = managerDocument.CreateItem(documentIdReferenceInlineInner);

			documentMetadata3.Schema = new DynamicWrapper();
			documentMetadata3.Schema.Type = documentIdReferenceInlineInner;
			documentMetadata3.Schema.Caption = documentIdReferenceInlineInner;
			documentMetadata3.Schema.Properties = new DynamicWrapper();
			documentMetadata3.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata3.Schema.Properties.Id.Type = "Uuid";
			documentMetadata3.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata3.Schema.Properties.House = new DynamicWrapper();
			documentMetadata3.Schema.Properties.House.Type = "String";
			documentMetadata3.Schema.Properties.House.Caption = "House";

			//Структура
			// TestDocument
			//  |---TestDocument1
			//     |-- TestDocument2


            managerDocument.MergeItem(documentMetadata1);
            managerDocument.MergeItem(documentMetadata2);
            managerDocument.MergeItem(documentMetadata3);

			RestQueryApi.QueryPostNotify(configId);

			UpdateApi.UpdateStore(configId);

			//создаем экземпляр документа
			new DocumentApi().CreateDocument(configId, documentId);

			//после повторного вызова (выполнилась загрузка модуля)
			var watch = Stopwatch.StartNew();

			dynamic item = new DocumentApi().CreateDocument(configId, documentId);

			watch.Stop();

			Assert.IsNotNull(item);
			Assert.AreEqual(item.ToString(),"{\r\n  \"Address\": {\r\n    \"Street\": {}\r\n  }\r\n}");
			Assert.True(watch.ElapsedMilliseconds < 100, string.Format("Elapsed ms: {0}", watch.ElapsedMilliseconds));


		}
	}
}