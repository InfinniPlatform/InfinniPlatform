using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Metadata.MetadataContainers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Api.Validation.Serialization;
using InfinniPlatform.Api.Validation.ValidationBuilders;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.DataSources
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class MetadataDataSourceBehavior
    {
        private IDisposable _server;
        private static string _configurationId = "testconfig1";

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

        private static void CreateTestConfiguration()
        {
            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager(null);
            dynamic config = managerConfiguration.CreateItem(_configurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            MetadataManagerElement menuManager =
                new ManagerFactoryConfiguration(null, _configurationId).BuildMenuManager();
            dynamic menu = menuManager.CreateItem("testmenu");
            menuManager.MergeItem(menu);


            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration(null, _configurationId).BuildDocumentManager();
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
            documentMetadata1.Schema.Properties.Address.Caption = "Patient address";

            documentMetadata1.ValidationErrors = new List<dynamic>();
            documentMetadata1.ValidationWarnings = new List<dynamic>();


            documentMetadata1.Views = new List<dynamic>();
            dynamic instance1 = new DynamicWrapper();
            instance1.Id = Guid.NewGuid();
            instance1.Name = "TestView";
            documentMetadata1.Views.Add(instance1);

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


            dynamic validationError =
                DynamicWrapperExtensions.ToDynamic(
                    ValidationOperatorSerializer.Instance.Serialize(
                        ValidationBuilder.ForObject(builder => builder.And(rules => rules
                                                                                        .IsEqual("LastName", "test",
                                                                                                 "Last name not satisfy")
                                                                                        .IsNotNullOrWhiteSpace(
                                                                                            "LastName",
                                                                                            "LastName is empty")))));
            validationError.Id = Guid.NewGuid().ToString();
            validationError.Name = "testvalidationAnd";

            dynamic validationWarning =
                DynamicWrapperExtensions.ToDynamic(
                    ValidationOperatorSerializer.Instance.Serialize(
                        ValidationBuilder.ForObject(builder => builder.Or(rules => rules
                                                                                       .IsEqual("LastName", "test",
                                                                                                "Last name not satisfy")
                                                                                       .IsNotNullOrWhiteSpace(
                                                                                           "LastName",
                                                                                           "LastName is empty")))));
            validationWarning.Id = Guid.NewGuid().ToString();
            validationWarning.Name = "testvalidationOr";


            MetadataManagerElement managerWarning =
                new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildValidationWarningsManager();
            managerWarning.MergeItem(validationWarning);

            MetadataManagerElement managerError =
                new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildValidationErrorsManager();
            managerError.MergeItem(validationError);


            RestQueryApi.QueryPostNotify(null, _configurationId);

            new UpdateApi(null).UpdateStore(_configurationId);
        }

        [Test]
        public void ShouldGetMetadata()
        {
            CreateTestConfiguration();


            //проверка получения списка заголовков всех конфигураций
            dynamic bodyQuery = DynamicWrapperExtensions.ToDynamic(QueryMetadata.GetConfigurationShortListIql());

            dynamic response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);

            IEnumerable<dynamic> queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);
            Assert.True(queryResult.Select(q => q.Result).Any(r => r.Name == _configurationId));

            IEnumerable<dynamic> resultConfig =
                RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getregisteredconfiglist", null, null)
                            .ToDynamic()
                            .ConfigList;
            Assert.True(resultConfig.Any(r => r.Name == _configurationId));

            //проверка получения метаданных конфигураций

            dynamic metadataSource = new DynamicWrapper();
            metadataSource.ConfigId = _configurationId;

            bodyQuery = new DynamicWrapper();
            bodyQuery.ConfigId = metadataSource.ConfigId;

            response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
            queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);
            Assert.AreEqual(1, queryResult.Count());

            //проверка получения метаданных документов
            dynamic metadataSourceDocument = new DynamicWrapper();
            metadataSourceDocument.ConfigId = _configurationId;
            metadataSourceDocument.DocumentId = "testdoc1";

            //проверяем получение списка заголовков метаданных всех документов
            bodyQuery =
                DynamicWrapperExtensions.ToDynamic(
                    QueryMetadata.GetConfigurationMetadataShortListIql(metadataSourceDocument.ConfigId,
                                                                       new MetadataContainerDocument()
                                                                           .GetMetadataContainerName()));

            response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);

            queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);

            IEnumerable<dynamic> docs = queryResult.First().Result.Documents;

            Assert.True(docs.Any(d => d.Name == "testdoc1"));
            Assert.True(docs.Any(d => d.Name == "testdoc2"));

            //проверяем получение подробных метаданных документа

            bodyQuery =
                DynamicWrapperExtensions.ToDynamic(
                    (string)
                    QueryMetadata.GetConfigurationMetadataByNameIql(metadataSourceDocument.ConfigId,
                                                                    metadataSourceDocument.DocumentId,
                                                                    new MetadataContainerDocument()
                                                                        .GetMetadataContainerName(),
                                                                    new MetadataContainerDocument().GetMetadataTypeName()));

            response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
            queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);

            docs = queryResult.First().Result.Documents;
            Assert.AreEqual(1, docs.Count());
            Assert.True(docs.Any(d => d.DocumentFull.Name == "testdoc1"));

            //проверяем получение метаданных элемента документа
            dynamic metadataSourceDocumentElement = new DynamicWrapper();
            metadataSourceDocumentElement.ConfigId = _configurationId;
            metadataSourceDocumentElement.DocumentId = "testdoc1";
            metadataSourceDocumentElement.MetadataType = "View";

            bodyQuery =
                DynamicWrapperExtensions.ToDynamic(
                    (string)
                    QueryMetadata.GetDocumentMetadataShortListIql(metadataSourceDocumentElement.ConfigId,
                                                                  metadataSourceDocumentElement.DocumentId,
                                                                  new MetadataContainerView().GetMetadataContainerName()));

            response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
            queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);


            docs = queryResult.First().Result.Views;
            Assert.AreEqual(1, docs.Count());
            Assert.True(docs.Any(d => d.Name == "TestView"));

            //проверка получения заголовков метаданных указанного типа

            dynamic metadataSourceDocumentMenuList = new DynamicWrapper();
            metadataSourceDocumentMenuList.ConfigId = _configurationId;
            metadataSourceDocumentMenuList.MetadataType = "Menu";

            bodyQuery =
                DynamicWrapperExtensions.ToDynamic(
                    (string)
                    QueryMetadata.GetConfigurationMetadataShortListIql(metadataSourceDocumentMenuList.ConfigId,
                                                                       metadataSourceDocumentMenuList.MetadataType));

            response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
            queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);

            Assert.AreEqual(1, queryResult.First().Result.Menu.Count);
            Assert.AreEqual("testmenu", queryResult.First().Result.Menu[0].Name);

            //проверка получения метаданных указанного типа
            dynamic metadataSourceDocumentMenu = new DynamicWrapper();
            metadataSourceDocumentMenu.ConfigId = _configurationId;
            metadataSourceDocumentMenu.MetadataType = "Menu";
            metadataSourceDocumentMenu.MetadataName = "testmenu";


            bodyQuery =
                DynamicWrapperExtensions.ToDynamic(
                    (string)
                    QueryMetadata.GetConfigurationMetadataByNameIql(metadataSourceDocumentMenu.ConfigId,
                                                                    metadataSourceDocumentMenu.MetadataName, "Menu",
                                                                    metadataSourceDocumentMenu.MetadataType));
            response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);

            queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);

            Assert.AreEqual(1, queryResult.First().Result.Menu.Count);
            Assert.AreEqual("testmenu", queryResult.First().Result.Menu[0].MenuFull.Name);
        }

        [Test]
        [Ignore("Не будет использоваться")]
        public void ShouldGetMetadataByApi()
        {
            CreateTestConfiguration();

            //проверка получения списка заголовков всех конфигураций
            IEnumerable<dynamic> resultConfig =
                RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getregisteredconfiglist", null, null)
                            .ToDynamic()
                            .ConfigList;
            Assert.True(resultConfig.Any(r => r.Name == _configurationId));

            //проверка получения метаданных конфигураций

            dynamic metadataSource = new DynamicWrapper();
            metadataSource.ConfigId = _configurationId;

            dynamic result =
                RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getconfigurationmetadata", null,
                                              metadataSource).ToDynamic();
            Assert.AreEqual(_configurationId, result.Name);

            //проверка получения метаданных документов
            dynamic metadataSourceDocumentList = new DynamicWrapper();
            metadataSourceDocumentList.ConfigId = _configurationId;
            //проверяем получение списка заголовков метаданных всех документов

            IEnumerable<dynamic> docs =
                RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getdocumentlistmetadata", null,
                                              metadataSourceDocumentList).ToDynamicList();

            Assert.True(docs.Any(d => d.Name == "testdoc1"));
            Assert.True(docs.Any(d => d.Name == "testdoc2"));

            //проверяем получение подробных метаданных документа

            dynamic metadataSourceDocument = new DynamicWrapper();
            metadataSourceDocument.ConfigId = _configurationId;
            metadataSourceDocument.DocumentId = "testdoc1";

            result =
                RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getdocumentmetadata", null,
                                              metadataSourceDocument).ToDynamic();

            Assert.True(result.Name == "testdoc1");

            //проверяем получение метаданных элемента документа
            dynamic metadataSourceDocumentElement = new DynamicWrapper();
            metadataSourceDocumentElement.ConfigId = _configurationId;
            metadataSourceDocumentElement.DocumentId = "testdoc1";
            metadataSourceDocumentElement.MetadataType = "View";

            IEnumerable<dynamic> elements =
                RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getdocumentelementlistmetadata", null,
                                              metadataSourceDocumentElement).ToDynamicList();

            Assert.AreEqual(1, elements.Count());
            Assert.True(elements.Any(d => d.Name == "TestView"));

            //проверка получения заголовков метаданных указанного типа

            dynamic metadataSourceDocumentMenuList = new DynamicWrapper();
            metadataSourceDocumentMenuList.ConfigId = _configurationId;
            metadataSourceDocumentMenuList.MetadataType = "Menu";

            IEnumerable<dynamic> resultList =
                RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmenulistmetadata", null,
                                              metadataSourceDocumentMenuList).ToDynamicList();

            Assert.AreEqual(1, resultList.Count());
            Assert.AreEqual("testmenu", resultList.First().Name);

            //проверка получения метаданных указанного типа
            dynamic metadataSourceDocumentMenu = new DynamicWrapper();
            metadataSourceDocumentMenu.ConfigId = _configurationId;
            metadataSourceDocumentMenu.MetadataType = "Menu";
            metadataSourceDocumentMenu.MetadataName = "testmenu";

            result =
                RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmenumetadata", null,
                                              metadataSourceDocumentMenu).ToDynamic();

            Assert.AreEqual("testmenu", result.Name);

            //проверка получения метаданных валидации
            dynamic metadataSourceDocumentValidationWarning = new DynamicWrapper();
            metadataSourceDocumentValidationWarning.ConfigId = _configurationId;
            metadataSourceDocumentValidationWarning.DocumentId = "testdoc1";
            metadataSourceDocumentValidationWarning.MetadataName = "testvalidationOr";

            result = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getvalidationwarningmetadata", null,
                                                   metadataSourceDocumentValidationWarning).ToDynamic();

            Assert.IsNotNull(result.Or);

            dynamic metadataSourceDocumentValidationError = new DynamicWrapper();
            metadataSourceDocumentValidationError.ConfigId = _configurationId;
            metadataSourceDocumentValidationError.DocumentId = "testdoc1";
            metadataSourceDocumentValidationError.MetadataName = "testvalidationAnd";

            result = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getvalidationerrormetadata", null,
                                                   metadataSourceDocumentValidationError).ToDynamic();

            Assert.IsNotNull(result.And);
        }
    }
}