using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Api.Tests.Packages
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public class ConfigExporterBehavior
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


		[Test]
		public void ShouldExportAndImportConfiguration()
		{
            DeleteTestConfiguration();

			CreateTestConfiguration();

			var exporter = new ConfigExporter(new ZipStructure(@"TestData\TestZip.zip"));

			exporter.ExportHeaderToStructure(null, _configurationId);

			DeleteTestConfiguration();

			exporter = new ConfigExporter(new ZipStructure(@"TestData\TestZip.zip", @"TestData\UnzipTest"));

			dynamic config = exporter.ImportHeaderFromStructure("testversionimport");

            CheckDynamicConfig(config);

			CheckConfiguration();

			


		}

		private void CheckDynamicConfig(dynamic config)
		{
			Assert.True(config.Menu != null);
			Assert.True(config.Documents != null);

			IEnumerable<dynamic> menu = config.Menu;
			dynamic menuItem = menu.FirstOrDefault(d => d.Name == "testmenu");

			Assert.IsNotNull(menuItem);

			IEnumerable<dynamic> reports = config.Reports;
			dynamic reportItem = reports.FirstOrDefault(d => d.Name == "testreport");

			Assert.IsNotNull(reportItem);

			IEnumerable<dynamic> docs = config.Documents;

			dynamic testDoc1 = docs.FirstOrDefault(d => d.Name == "testdoc1");

			Assert.IsNotNull(testDoc1);

			Assert.IsNotNull(testDoc1.Services);
			Assert.IsNotNull(testDoc1.Scenarios);
			Assert.IsNotNull(testDoc1.Processes);
			Assert.IsNotNull(testDoc1.Generators);
			Assert.IsNotNull(testDoc1.Views);
			Assert.IsNotNull(testDoc1.DocumentStatuses);

			Assert.AreEqual(1, testDoc1.Services.Count);
			Assert.AreEqual(1, testDoc1.Scenarios.Count);
			Assert.AreEqual(1, testDoc1.Processes.Count);
			Assert.AreEqual(1, testDoc1.Generators.Count);
			Assert.AreEqual(1, testDoc1.Views.Count);
			Assert.AreEqual(1, testDoc1.DocumentStatuses.Count);
		}

		[Test]
		public void ShouldExportAndImportConfigurationToDirectory()
		{
            DeleteTestConfiguration();

			CreateTestConfiguration();

			var exporter = new ConfigExporter(new DirectoryStructure(@"TestData\TestExportToDirectory"));

			exporter.ExportHeaderToStructure(null, _configurationId);

			exporter = new ConfigExporter(new DirectoryStructure(@"TestData\TestExportToDirectory"));

			exporter.ImportHeaderFromStructure("testversionimport");

			CheckConfiguration();
		}



		private void CheckConfiguration()
		{
			var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(null);
			var config = managerConfiguration.GetItem(_configurationId);

			Assert.IsNotNull(config);

			var managerMenu = new ManagerFactoryConfiguration(null, _configurationId).BuildMenuMetadataReader();
			Assert.IsNotNull(managerMenu.GetItem("testmenu"));

			var managerReport = new ManagerFactoryConfiguration(null, _configurationId).BuildReportMetadataReader();
			Assert.IsNotNull(managerReport.GetItem("testreport"));

			var managerDocument = new ManagerFactoryConfiguration(null, _configurationId).BuildDocumentMetadataReader();
			Assert.IsNotNull(managerDocument.GetItem("testdoc1"));
			Assert.IsNotNull(managerDocument.GetItem("testdoc2"));

			var managerAssembly = new ManagerFactoryConfiguration(null, _configurationId).BuildAssemblyMetadataReader();
			Assert.IsNotNull(managerAssembly.GetItem(_configurationId));

			var managerWarning = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildValidationWarningsMetadataReader();
			Assert.IsNotNull(managerWarning.GetItem("TestWarning1"));

			var managerErrors = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildValidationErrorsMetadataReader();
			Assert.IsNotNull(managerErrors.GetItem("TestError1"));

			var managerView = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildViewMetadataReader();
			Assert.IsNotNull(managerView.GetItem("TestView1"));

			var managerScenario = new ManagerFactoryDocument(null,_configurationId, "testdoc1").BuildScenarioMetadataReader();
			Assert.IsNotNull(managerScenario.GetItem("TestScenario1"));

			var managerProcess = new ManagerFactoryDocument(null,_configurationId, "testdoc1").BuildProcessMetadataReader();
			Assert.IsNotNull(managerProcess.GetItem("TestProcess1"));

			var managerService = new ManagerFactoryDocument(null,_configurationId, "testdoc1").BuildServiceMetadataReader();
			Assert.IsNotNull(managerService.GetItem("TestService1"));

			var managerGenerator = new ManagerFactoryDocument(null,_configurationId, "testdoc1").BuildGeneratorMetadataReader();
			Assert.IsNotNull(managerGenerator.GetItem("TestGenerator1"));

			var managerStatus = new ManagerFactoryDocument(null,_configurationId, "testdoc1").BuildStatusMetadataReader();
			Assert.IsNotNull(managerStatus.GetItem("JustCreated"));

		}

		private void DeleteTestConfiguration()
		{
			var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager(null);
		    var readerConfiguration = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(null);

		    var config = readerConfiguration.GetItem(_configurationId);
		    if (config != null)
		    {
		        managerConfiguration.DeleteItem(config);
		    }

		}

		private static void CreateTestConfiguration()
		{
			var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager(null);
			var config = managerConfiguration.CreateItem(_configurationId);

			managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

			var menuManager = new ManagerFactoryConfiguration(null, _configurationId).BuildMenuManager();
			dynamic menu = menuManager.CreateItem("testmenu");
            menuManager.MergeItem(menu);


			var reportManager = new ManagerFactoryConfiguration(null, _configurationId).BuildReportManager();
			dynamic report = reportManager.CreateItem("testreport");
            reportManager.MergeItem(report);

			var assemblyManager = new ManagerFactoryConfiguration(null, _configurationId).BuildAssemblyManager();

			dynamic assembly = new DynamicWrapper();
			assembly.Id = Guid.NewGuid().ToString();
			assembly.Name = _configurationId;

			dynamic instanceView = new DynamicWrapper();
			instanceView.Id = Guid.NewGuid().ToString();
			instanceView.Name = "TestView1";

			dynamic instanceService = new DynamicWrapper();
			instanceService.Id = Guid.NewGuid().ToString();
			instanceService.Name = "TestService1";

			dynamic instanceStatus = new DynamicWrapper();
			instanceStatus.Id = Guid.NewGuid().ToString();
			instanceStatus.Name = "JustCreated";

			dynamic instanceProcess = new DynamicWrapper();
			instanceProcess.Id = Guid.NewGuid().ToString();
			instanceProcess.Name = "TestProcess1";

			dynamic instanceScenario = new DynamicWrapper();
			instanceScenario.Id = Guid.NewGuid().ToString();
			instanceScenario.Name = "TestScenario1";

			dynamic instanceGenerator = new DynamicWrapper();
			instanceGenerator.Id = Guid.NewGuid().ToString();
			instanceGenerator.Name = "TestGenerator1";

			dynamic instanceWarning = new DynamicWrapper();
			instanceWarning.Id = Guid.NewGuid().ToString();
			instanceWarning.Name = "TestWarning1";

			dynamic instanceError = new DynamicWrapper();
			instanceError.Id = Guid.NewGuid().ToString();
			instanceError.Name = "TestError1";

            assemblyManager.MergeItem(assembly);

			var managerDocument = new ManagerFactoryConfiguration(null, _configurationId).BuildDocumentManager();
			dynamic documentMetadata1 = managerDocument.CreateItem("testdoc1");
			dynamic documentMetadata2 = managerDocument.CreateItem("testdoc2");

            managerDocument.MergeItem(documentMetadata1);
            managerDocument.MergeItem(documentMetadata2);

			var managerWarning = new ManagerFactoryDocument(null,_configurationId, "testdoc1").BuildValidationWarningsManager();
            managerWarning.MergeItem(instanceWarning);

            var managerError = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildValidationErrorsManager();
            managerError.MergeItem(instanceError);

            var managerView = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildViewManager();
            managerView.MergeItem(instanceView);

            var managerScenario = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildScenarioManager();
            managerScenario.MergeItem(instanceScenario);

            var managerProcess = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildProcessManager();
            managerProcess.MergeItem(instanceProcess);

            var managerService = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildServiceManager();
            managerService.MergeItem(instanceService);

            var managerStatus = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildStatusManager();
            managerStatus.MergeItem(instanceStatus);

            var managerGenerator = new ManagerFactoryDocument(null, _configurationId, "testdoc1").BuildGeneratorManager();
            managerGenerator.MergeItem(instanceGenerator);

			RestQueryApi.QueryPostNotify(null, _configurationId);

            new UpdateApi(null).UpdateStore(_configurationId);
		}
	}
}