using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Tests.RestBehavior.ConfiguratorApiBehavior
{	
	public sealed class MetadataCacheRefresherBehavior
	{
		private IDisposable _server;
		private const string ConfigurationId = "TestConfigRefreshCache";
		private const string DocumentId = "TestDoc1";
		private const string ViewName = "TestViewRefresh";
		private const string LayoutPanel = "{\r\n  \"GridPanel\": {\r\n    \"Rows\": [\r\n      {\r\n        \"Cells\": [\r\n          {\r\n            \"ColumnSpan\": 6,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"Главное меню\",\r\n                  \"Name\": \"MainMenuPanel\",\r\n                  \"Items\": [\r\n                    {\r\n                      \"MenuBar\": {\r\n                        \"Name\": \"HomePage\",\r\n                        \"LayoutPanel\": {\r\n                          \"StackPanel\": {\r\n                            \"Name\": \"MainStackPanel\",\r\n                            \"Items\": [\r\n                              {\r\n                                \"MenuBar\": {\r\n                                  \"Name\": \"MainMenu\",\r\n                                  \"ConfigId\": \"VeteransHospital\"\r\n                                }\r\n                              }\r\n                            ]\r\n                          }\r\n                        }\r\n                      }\r\n                    }\r\n                  ]\r\n                }\r\n              }\r\n            ]\r\n          },\r\n          {\r\n            \"ColumnSpan\": 6,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"Какой-нибудь виджет\",\r\n                  \"Name\": \"Widget1Panel\",\r\n                  \"Items\": []\r\n                }\r\n              }\r\n            ]\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"Cells\": [\r\n          {\r\n            \"ColumnSpan\": 12,\r\n            \"Items\": [\r\n              {\r\n                \"Panel\": {\r\n                  \"Text\": \"И тут еще виджет\",\r\n                  \"Name\": \"Widget2Panel\",\r\n                  \"Items\": []\r\n                }\r\n              }\r\n            ]\r\n          }\r\n        ]\r\n      }\r\n    ]\r\n  }\r\n}";

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

		[Test]
		public void ShouldRefreshViewMetadata()
		{
			CreateTestConfig();

			var managerDocument = new ManagerFactoryDocument(ConfigurationId, DocumentId);
			var managerView = managerDocument.BuildViewManager();
			var view = managerView.CreateItem(ViewName);
			view.MetadataType = ViewType.EditView;
			view.LayoutPanel = new DynamicWrapper();
            managerView.MergeItem(view);

			dynamic body = new DynamicWrapper();
			body.Configuration = ConfigurationId;
			body.MetadataObject = DocumentId;
			body.MetadataType = ViewType.EditView;
			body.MetadataName = ViewName;

			//метаданные представления должны обновиться без перезагрузки конфигурации
			var result = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getmanagedmetadata", null, body);

			Assert.AreEqual(result.ToDynamic().LayoutPanel.ToString(),"{}");
		}

		private void CreateTestConfig()
		{
			//добавили конфигурацию
			var manager = ManagerFactoryConfiguration.BuildConfigurationManager();

			var item = manager.CreateItem(ConfigurationId);
            manager.DeleteItem(item);
            manager.MergeItem(item);

			//добавляем именованный View в конкретном документе
			//---------------------------------------------------
			var documentManager = new ManagerFactoryConfiguration(ConfigurationId).BuildDocumentManager();

			var doc = documentManager.CreateItem(DocumentId);
            documentManager.MergeItem(doc);

			var managerDocument = new ManagerFactoryDocument(ConfigurationId, DocumentId);
			var managerView = managerDocument.BuildViewManager();
			var view = managerView.CreateItem(ViewName);
			view.MetadataType = ViewType.EditView;
            view.LayoutPanel = DynamicWrapperExtensions.ToDynamic((string)LayoutPanel);
            managerView.MergeItem(view);
			//----------------------------------------------------

			//перезагрузим конфигурации
			RestQueryApi.QueryPostNotify(ConfigurationId);

		}

	}
}
