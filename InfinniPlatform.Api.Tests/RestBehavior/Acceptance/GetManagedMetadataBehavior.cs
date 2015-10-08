using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Hosting;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
	[Ignore("Необходимо создать конфигурацию метаданных на диске, т.к. теперь метаданные загружаются только с диска")]
	public sealed class GeneratorBehavior
    {
        private const string LayoutPanel =
            "{\r\n  \"GridPanel\": {\r\n    \"Rows\": [\r\n      {\r\n        \"Cells\": [\r\n          {\r\n            \"ColumnSpan\": 6,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"Главное меню\",\r\n                  \"Name\": \"MainMenuPanel\",\r\n                  \"Items\": [\r\n                    {\r\n                      \"MenuBar\": {\r\n                        \"Name\": \"HomePage\",\r\n                        \"LayoutPanel\": {\r\n                          \"StackPanel\": {\r\n                            \"Name\": \"MainStackPanel\",\r\n                            \"Items\": [\r\n                              {\r\n                                \"MenuBar\": {\r\n                                  \"Name\": \"MainMenu\",\r\n                                  \"ConfigId\": \"VeteransHospital\"\r\n                                }\r\n                              }\r\n                            ]\r\n                          }\r\n                        }\r\n                      }\r\n                    }\r\n                  ]\r\n                }\r\n              }\r\n            ]\r\n          },\r\n          {\r\n            \"ColumnSpan\": 6,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"Какой-нибудь виджет\",\r\n                  \"Name\": \"Widget1Panel\",\r\n                  \"Items\": []\r\n                }\r\n              }\r\n            ]\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"Cells\": [\r\n          {\r\n            \"ColumnSpan\": 12,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"И тут еще виджет\",\r\n                  \"Name\": \"Widget2Panel\",\r\n                  \"Items\": []\r\n                }\r\n              }\r\n            ]\r\n          }\r\n        ]\r\n      }\r\n    ]\r\n  }\r\n}";

        private const string TestViewName = "TestView";
        private const string ExistsViewName = "TestExistsView";

        private IDisposable _server;

        //[TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);
        }

        //[TestFixtureTearDown]
        public void TearDownFixture()
        {
            _server.Dispose();
        }

        private const string ConfigurationId = "TestConfigView";
        private const string DocumentId = "TestDoc1";

        [TestCase(DocumentId)]
        [TestCase("Common")]
        public void ShouldGenerateGeneratorViewFromDocument(string documentId)
        {
            CreateTestConfig();

            RestQueryBuilder builder;
            dynamic result;

            //получаем сгенерированное представление через менеджер метаданных

            var bodyMetadata = new
                {
                    Configuration = ConfigurationId,
                    MetadataObject = documentId,
                    MetadataType = MetadataType.View,
                    MetadataName = TestViewName
                };

            builder = new RestQueryBuilder("SystemConfig", "metadata", "getmanagedmetadata", null);

            builder.QueryPostJson(null, bodyMetadata).ToDynamic();

            Stopwatch watch = Stopwatch.StartNew();

            result = builder.QueryPostJson(null, bodyMetadata).ToDynamic();

            watch.Stop();

            const int expectedMs = 50;

            if (watch.ElapsedMilliseconds > expectedMs)
            {
                Console.WriteLine(@"Response time more than {0} milliseconds", expectedMs);
            }

            Console.WriteLine(@"Elapsed: {0}", watch.ElapsedMilliseconds);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.TestValue, "Test");
        }

        [TestCase(ExistsViewName, ViewType.ListView)]
        //ViewType не соответствует значению в метаданных, но в случае доступа по имени, игнорируем тип
        [TestCase("", ViewType.EditView)]
        [TestCase("", "")]
        public void ShouldReturnExistingView(string viewName, string viewType)
        {
            CreateTestConfig();

            RestQueryBuilder builder;
            dynamic result;

            //получаем сгенерированное представление через менеджер метаданных

            dynamic parameters = new DynamicWrapper();
            parameters.TestParameter = 1;

            var bodyMetadata = new
                {
                    Configuration = ConfigurationId,
                    MetadataObject = DocumentId,
                    MetadataType = viewType,
                    MetadataName = viewName,
                    Parameters = parameters
                };

            builder = new RestQueryBuilder( "SystemConfig", "metadata", "getmanagedmetadata", null);

            builder.QueryPostJson(null, bodyMetadata).ToDynamic();

            Stopwatch watch = Stopwatch.StartNew();

            result = builder.QueryPostJson(null, bodyMetadata).ToDynamic();

            watch.Stop();

            const int expectedMs = 50;

            if (watch.ElapsedMilliseconds > expectedMs)
            {
                Console.WriteLine(@"Response time more than {0} milliseconds", expectedMs);
            }

            Console.WriteLine(@"Elapsed {0}", watch.ElapsedMilliseconds);


            Assert.IsNotNull(result);
            Assert.AreEqual(result.LayoutPanel.ToString(), LayoutPanel);

            IEnumerable<dynamic> parametersResult = DynamicWrapperExtensions.ToEnumerable(result.RequestParameters);
            Assert.IsNotNull(parametersResult);


            Assert.AreEqual(parametersResult.Count(), 1);
            Assert.AreEqual(parametersResult.First().Name, "TestParameter");
            Assert.AreEqual(parametersResult.First().Value, 1);
        }


        private void CreateTestConfig()
        {
            //добавили конфигурацию
            MetadataManagerConfiguration manager = ManagerFactoryConfiguration.BuildConfigurationManager("1.0.0.0");

            dynamic item = manager.CreateItem(ConfigurationId);
            manager.DeleteItem(item);

            dynamic assembly = new DynamicWrapper();
            assembly.Name = "InfinniPlatform.Api.Tests";
            item.Assemblies.Add(assembly);
            manager.MergeItem(item);

            //добавляем именованный View в конкретном документе
            //---------------------------------------------------
            MetadataManagerDocument documentManager =
                new ManagerFactoryConfiguration("1.0.0.0", ConfigurationId).BuildDocumentManager();

            dynamic doc = documentManager.CreateItem(DocumentId);
            documentManager.MergeItem(doc);

            var managerDocument = new ManagerFactoryDocument("1.0.0.0", ConfigurationId, DocumentId);
            MetadataManagerElement managerView = managerDocument.BuildViewManager();
            dynamic view = managerView.CreateItem(ExistsViewName);
            view.MetadataType = ViewType.EditView;
            view.LayoutPanel = DynamicWrapperExtensions.ToDynamic(LayoutPanel);
            managerView.MergeItem(view);
            //----------------------------------------------------

            //перезагрузим конфигурации
            RestQueryApi.QueryPostNotify("1.0.0.0", ConfigurationId);

            //добавляем генератор в документе Common
            //----------------------------------------------------

            var builder = new RestQueryBuilder("SystemConfig", "metadata", "creategenerator", null);

            var eventObject = new
                {
                    GeneratorName = TestViewName,
                    ActionUnit = "ActionUnitGeneratorTest",
                    Configuration = ConfigurationId,
                    Metadata = "Common",
                    MetadataType = MetadataType.View,
                    ContextTypeKind = ContextTypeKind.ApplyMove
                };

            builder.QueryPostJson(null, eventObject);
            //----------------------------------------------------
            //добавляем генератор в документе _documentId
            builder = new RestQueryBuilder("SystemConfig", "metadata", "creategenerator", null);

            eventObject = new
                {
                    GeneratorName = TestViewName,
                    ActionUnit = "ActionUnitGeneratorTest",
                    Configuration = ConfigurationId,
                    Metadata = DocumentId,
                    MetadataType = MetadataType.View,
                    ContextTypeKind = ContextTypeKind.ApplyMove
                };

            builder.QueryPostJson(null, eventObject);


            //создание пакета со сборкой
            dynamic package = new PackageBuilder().BuildPackage(ConfigurationId, "1.0.0.0",
                                                                @"..\Assemblies\InfinniPlatform.Api.Tests.dll");
            //установка пакета
            new UpdateApi("1.0.0.0").InstallPackages(new[] { package });

            //обновление конфигурации
            RestQueryApi.QueryPostNotify("1.0.0.0", ConfigurationId);

            //генерируем метаданные напрямую
            builder = new RestQueryBuilder("SystemConfig", "metadata", "generatemetadata", null);


            var body = new
                {
                    Metadata = "common",
                    GeneratorName = TestViewName,
                    Configuration = ConfigurationId,
                };

            dynamic result = builder.QueryPostJson(null, body).ToDynamic();
            Assert.AreEqual(result.TestValue, "Test");
        }
    }
}