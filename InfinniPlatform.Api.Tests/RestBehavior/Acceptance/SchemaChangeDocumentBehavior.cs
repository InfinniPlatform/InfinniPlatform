using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    public class SchemaChangeDocumentBehavior
    {
		private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));


        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
		[Repeat(2)]
		[Ignore("Only manual run")]
        public void ShouldSelectAccordingDataSchemaForDocument()
        {
			

            var configId = "TestConfigChangeSchema";

            string documentId = "TestDocument12";

			new IndexApi().RebuildIndex(configId, documentId);

            var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager(null);

            dynamic config = managerConfig.CreateItem(configId);

            managerConfig.MergeItem(config);

            var managerDocument = new ManagerFactoryConfiguration(null, configId).BuildDocumentManager();

            
            

            var documentMetadata1 = managerDocument.CreateItem(documentId);

            documentMetadata1.Schema = new DynamicWrapper();
            documentMetadata1.Schema.Name = documentId;
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
            documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.DocumentId = "TestDocument1";

            managerDocument.MergeItem(documentMetadata1);

            RestQueryApi.QueryPostNotify(null, configId);

            new UpdateApi(null).UpdateStore(configId);

            //сохраняем документ с первой схемой
            dynamic documentFirstSchema = new DynamicWrapper();
	        documentFirstSchema.Id = Guid.NewGuid().ToString();
            documentFirstSchema.Name = "TestPatient1";
            documentFirstSchema.Address = new DynamicWrapper();
            documentFirstSchema.Address.Id = Guid.NewGuid().ToString();
            documentFirstSchema.Address.DisplayName = "Челябинск";

            new DocumentApi(null).SetDocument(configId, documentId, documentFirstSchema);

            //Изменяем схему
            
            documentMetadata1 = managerDocument.CreateItem(documentId);

            documentMetadata1.Schema = new DynamicWrapper();
            documentMetadata1.Schema.Name = documentId;
            documentMetadata1.Schema.Caption = documentId;
            documentMetadata1.Schema.Properties = new DynamicWrapper();
            documentMetadata1.Schema.Properties.Id = new DynamicWrapper();
            documentMetadata1.Schema.Properties.Id.Type = "Uuid";
            documentMetadata1.Schema.Properties.Id.Caption = "Unique identifier";

            documentMetadata1.Schema.Properties.Name = new DynamicWrapper();
            documentMetadata1.Schema.Properties.Name.Type = "String";
            documentMetadata1.Schema.Properties.Name.Caption = "Patient name";

            documentMetadata1.Schema.Properties.Address = new DynamicWrapper();
            documentMetadata1.Schema.Properties.Address.Type = "String";
            documentMetadata1.Schema.Properties.Address.Caption = "Patient address";

            managerDocument.MergeItem(documentMetadata1);

            RestQueryApi.QueryPostNotify(null, configId);

            new UpdateApi(null).UpdateStore(configId);
			
            //Сохраняем новый документ


            dynamic documentSecondSchema = new DynamicWrapper();
			documentSecondSchema.Id = Guid.NewGuid().ToString();
            documentSecondSchema.Name = "TestPatient1";
            documentSecondSchema.Address = "Челябинск";

            new DocumentApi(null).SetDocument(configId, documentId, documentSecondSchema);

            //получаем оба документа
            var documents =  new DocumentApi(null).GetDocument(configId, documentId, null, 0, 10);

            Assert.AreEqual(documents.Count(),2);
        }

    }
}
