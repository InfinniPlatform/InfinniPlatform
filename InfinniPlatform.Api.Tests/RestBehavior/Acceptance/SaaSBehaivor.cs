using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Tests.Extensions;
using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
	[Ignore("Необходимо создать конфигурацию метаданных на диске, т.к. теперь метаданные загружаются только с диска")]
	public sealed class SaaSBehavior
    {
        private IDisposable _server;

        //[TestFixtureSetUp]
        public void FixtureSetup()
        {
			_server = InfinniPlatformInprocessHost.Start();
		}

        //[TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        private string configId = "TestConfigSaaS";

        private string documentId = "TestDocumentSaaS";


        private void CreateTestConfig()
        {
            MetadataManagerConfiguration managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager();

            dynamic config = managerConfig.CreateItem(configId);
            managerConfig.DeleteItem(config);
            managerConfig.MergeItem(config);

            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration(configId).BuildDocumentManager();


            new IndexApi().RebuildIndex(configId, documentId);

            dynamic documentMetadata1 = managerDocument.CreateItem(documentId);

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

            managerDocument.MergeItem(documentMetadata1);

            RestQueryApi.QueryPostNotify(configId);

            new UpdateApi().UpdateStore(configId);
        }

        [Test]
        public void ShouldReturnOnlyOwnDocuments()
        {
            CreateTestConfig();

            AuthorizationExtensionsTest.ClearAuthConfig();

            //залогиниваемся под админом, чтобы добавить пользователя
            new SignInApi().SignInInternal("Admin", "Admin", false);

            var aclApi = new AuthApi();

            aclApi.AddUser("TestUser1", "Password1");
            aclApi.AddUser("TestUser2", "Password2");

            aclApi.AddClaim("TestUser1", AuthorizationStorageExtensions.OrganizationClaim, "Organization1");
            aclApi.AddClaim("TestUser2", AuthorizationStorageExtensions.OrganizationClaim, "Organization2");

            new SignInApi().SignOutInternal();


            //создаем экземпляр документа
            dynamic documentForUser1 = new DynamicWrapper();
            documentForUser1.Id = Guid.NewGuid().ToString();
            documentForUser1.Name = "Document for user 1";

            dynamic documentForUser2 = new DynamicWrapper();
            documentForUser2.Id = Guid.NewGuid().ToString();
            documentForUser2.Name = "Document for user 2";

            //when
            new SignInApi().SignInInternal("TestUser1", "Password1", false);

            new DocumentApi().SetDocument(configId, documentId, documentForUser1);

            new SignInApi().SignOutInternal();

            new SignInApi().SignInInternal("TestUser2", "Password2", false);

            new DocumentApi().SetDocument(configId, documentId, documentForUser2);

            new SignInApi().SignOutInternal();


            //then

            //проверяем документы первого пользователя
            new SignInApi().SignInInternal("TestUser1", "Password1", false);

            IEnumerable<dynamic> docsForUser1 =
                new DocumentApi().GetDocument(configId, documentId, null, 0, 100).ToList();

            Assert.AreEqual(1, docsForUser1.Count());
            Assert.AreEqual(documentForUser1.Id, docsForUser1.First().Id);

            new SignInApi().SignOutInternal();

            new SignInApi().SignInInternal("TestUser2", "Password2", false);

            //проверяем документы второго пользователя
            IEnumerable<dynamic> docsForUser2 =
                new DocumentApi().GetDocument(configId, documentId, null, 0, 100).ToList();

            Assert.AreEqual(1, docsForUser2.Count());
            Assert.AreEqual(documentForUser2.Id, docsForUser2.First().Id);

            new SignInApi().SignOutInternal();
        }
    }
}