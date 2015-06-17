using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.TestEnvironment;

using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class GeneratorBehavior
	{
		private const string LayoutPanel = "{\r\n  \"GridPanel\": {\r\n    \"Rows\": [\r\n      {\r\n        \"Cells\": [\r\n          {\r\n            \"ColumnSpan\": 6,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"Главное меню\",\r\n                  \"Name\": \"MainMenuPanel\",\r\n                  \"Items\": [\r\n                    {\r\n                      \"MenuBar\": {\r\n                        \"Name\": \"HomePage\",\r\n                        \"LayoutPanel\": {\r\n                          \"StackPanel\": {\r\n                            \"Name\": \"MainStackPanel\",\r\n                            \"Items\": [\r\n                              {\r\n                                \"MenuBar\": {\r\n                                  \"Name\": \"MainMenu\",\r\n                                  \"ConfigId\": \"VeteransHospital\"\r\n                                }\r\n                              }\r\n                            ]\r\n                          }\r\n                        }\r\n                      }\r\n                    }\r\n                  ]\r\n                }\r\n              }\r\n            ]\r\n          },\r\n          {\r\n            \"ColumnSpan\": 6,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"Какой-нибудь виджет\",\r\n                  \"Name\": \"Widget1Panel\",\r\n                  \"Items\": []\r\n                }\r\n              }\r\n            ]\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"Cells\": [\r\n          {\r\n            \"ColumnSpan\": 12,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"И тут еще виджет\",\r\n                  \"Name\": \"Widget2Panel\",\r\n                  \"Items\": []\r\n                }\r\n              }\r\n            ]\r\n          }\r\n        ]\r\n      }\r\n    ]\r\n  }\r\n}";
		private const string TestViewName = "TestView";
		private const string ExistsViewName = "TestExistsView";

		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

			TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);

			
		}

		[TestFixtureTearDown]
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

			builder = new RestQueryBuilder(null, "SystemConfig", "metadata", "getmanagedmetadata", null);

			builder.QueryPostJson(null, bodyMetadata).ToDynamic();

			var watch = Stopwatch.StartNew();

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

		[TestCase(ExistsViewName,ViewType.ListView)] //ViewType не соответствует значению в метаданных, но в случае доступа по имени, игнорируем тип
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

			builder = new RestQueryBuilder(null, "SystemConfig", "metadata", "getmanagedmetadata", null);

			builder.QueryPostJson(null, bodyMetadata).ToDynamic();

			var watch = Stopwatch.StartNew();

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
			var manager = ManagerFactoryConfiguration.BuildConfigurationManager(null);

			var item = manager.CreateItem(ConfigurationId);
            manager.DeleteItem(item);

			dynamic assembly = new DynamicWrapper();
			assembly.Name = "InfinniPlatform.Api.Tests";
			item.Assemblies.Add(assembly);
            manager.MergeItem(item);

			//добавляем именованный View в конкретном документе
			//---------------------------------------------------
			var documentManager = new ManagerFactoryConfiguration(null, ConfigurationId).BuildDocumentManager();

			var doc = documentManager.CreateItem(DocumentId);
            documentManager.MergeItem(doc);

			var managerDocument = new ManagerFactoryDocument(null,ConfigurationId, DocumentId);
			var managerView = managerDocument.BuildViewManager();
			var view = managerView.CreateItem(ExistsViewName);
			view.MetadataType = ViewType.EditView;
            view.LayoutPanel = DynamicWrapperExtensions.ToDynamic((string)LayoutPanel);
            managerView.MergeItem(view);
			//----------------------------------------------------

			//перезагрузим конфигурации
			RestQueryApi.QueryPostNotify(null, ConfigurationId);

			//добавляем генератор в документе Common
			//----------------------------------------------------

			var builder = new RestQueryBuilder(null, "SystemConfig", "metadata", "creategenerator", null);

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
			builder = new RestQueryBuilder(null, "SystemConfig", "metadata", "creategenerator", null);

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
			var package = new PackageBuilder().BuildPackage(ConfigurationId, "test",
															"InfinniPlatform.Api.Tests.dll");
			//установка пакета
			new UpdateApi(null).InstallPackages(new[] { package });

			//обновление конфигурации
			RestQueryApi.QueryPostNotify(null, ConfigurationId);

			//генерируем метаданные напрямую
			builder = new RestQueryBuilder(null, "SystemConfig", "metadata", "generatemetadata", null);



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