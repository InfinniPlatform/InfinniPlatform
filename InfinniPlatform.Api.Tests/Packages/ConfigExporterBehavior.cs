using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Packages
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
	[Ignore("Manual")]
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

            Assert.AreEqual(1, testDoc1.Services.Count);
            Assert.AreEqual(1, testDoc1.Scenarios.Count);
            Assert.AreEqual(1, testDoc1.Processes.Count);
            Assert.AreEqual(1, testDoc1.Generators.Count);
            Assert.AreEqual(1, testDoc1.Views.Count);
        }


        private void CheckConfiguration()
        {
            IDataReader managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationMetadataReader("2.0.0.0");
            dynamic config = managerConfiguration.GetItem(_configurationId);

            Assert.IsNotNull(config);

            IDataReader managerMenu = new ManagerFactoryConfiguration("2.0.0.0", _configurationId).BuildMenuMetadataReader();
            Assert.IsNotNull(managerMenu.GetItem("testmenu"));

            IDataReader managerReport =
                new ManagerFactoryConfiguration("2.0.0.0", _configurationId).BuildReportMetadataReader();
            Assert.IsNotNull(managerReport.GetItem("testreport"));

            IDataReader managerDocument =
                new ManagerFactoryConfiguration("2.0.0.0", _configurationId).BuildDocumentMetadataReader();
            Assert.IsNotNull(managerDocument.GetItem("testdoc1"));
            Assert.IsNotNull(managerDocument.GetItem("testdoc2"));

            IDataReader managerAssembly =
                new ManagerFactoryConfiguration("2.0.0.0", _configurationId).BuildAssemblyMetadataReader();
            Assert.IsNotNull(managerAssembly.GetItem(_configurationId));

            IDataReader managerWarning =
                new ManagerFactoryDocument("2.0.0.0", _configurationId, "testdoc1").BuildValidationWarningsMetadataReader();
            Assert.IsNotNull(managerWarning.GetItem("TestWarning1"));

            IDataReader managerErrors =
                new ManagerFactoryDocument("2.0.0.0", _configurationId, "testdoc1").BuildValidationErrorsMetadataReader();
            Assert.IsNotNull(managerErrors.GetItem("TestError1"));

            IDataReader managerView =
                new ManagerFactoryDocument("2.0.0.0", _configurationId, "testdoc1").BuildViewMetadataReader();
            Assert.IsNotNull(managerView.GetItem("TestView1"));

            IDataReader managerScenario =
                new ManagerFactoryDocument("2.0.0.0", _configurationId, "testdoc1").BuildScenarioMetadataReader();
            Assert.IsNotNull(managerScenario.GetItem("TestScenario1"));

            IDataReader managerProcess =
                new ManagerFactoryDocument("2.0.0.0", _configurationId, "testdoc1").BuildProcessMetadataReader();
            Assert.IsNotNull(managerProcess.GetItem("TestProcess1"));

            IDataReader managerService =
                new ManagerFactoryDocument("2.0.0.0", _configurationId, "testdoc1").BuildServiceMetadataReader();
            Assert.IsNotNull(managerService.GetItem("TestService1"));

            IDataReader managerGenerator =
                new ManagerFactoryDocument("2.0.0.0", _configurationId, "testdoc1").BuildGeneratorMetadataReader();
            Assert.IsNotNull(managerGenerator.GetItem("TestGenerator1"));
        }

        private void DeleteTestConfiguration()
        {
            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager("1.0.0.0");
            IDataReader readerConfiguration = ManagerFactoryConfiguration.BuildConfigurationMetadataReader("1.0.0.0");

            dynamic config = readerConfiguration.GetItem(_configurationId);
            if (config != null)
            {
                managerConfiguration.DeleteItem(config);
            }
        }

        private static void CreateTestConfiguration()
        {
            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager("1.0.0.0");
            dynamic config = managerConfiguration.CreateItem(_configurationId);

            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            MetadataManagerElement menuManager =
                new ManagerFactoryConfiguration("1.0.0.0", _configurationId).BuildMenuManager();
            dynamic menu = menuManager.CreateItem("testmenu");
            menuManager.MergeItem(menu);


            MetadataManagerElement reportManager =
                new ManagerFactoryConfiguration("1.0.0.0", _configurationId).BuildReportManager();
            dynamic report = reportManager.CreateItem("testreport");
            reportManager.MergeItem(report);

            MetadataManagerElement assemblyManager =
                new ManagerFactoryConfiguration("1.0.0.0", _configurationId).BuildAssemblyManager();

            dynamic assembly = new DynamicWrapper();
            assembly.Id = Guid.NewGuid().ToString();
            assembly.Name = _configurationId;

            dynamic instanceView = new DynamicWrapper();
            instanceView.Id = Guid.NewGuid().ToString();
            instanceView.Name = "TestView1";

            dynamic instanceService = new DynamicWrapper();
            instanceService.Id = Guid.NewGuid().ToString();
            instanceService.Name = "TestService1";

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

            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration("1.0.0.0", _configurationId).BuildDocumentManager();
            dynamic documentMetadata1 = managerDocument.CreateItem("testdoc1");
            dynamic documentMetadata2 = managerDocument.CreateItem("testdoc2");

            managerDocument.MergeItem(documentMetadata1);
            managerDocument.MergeItem(documentMetadata2);

            MetadataManagerElement managerWarning =
                new ManagerFactoryDocument("1.0.0.0", _configurationId, "testdoc1").BuildValidationWarningsManager();
            managerWarning.MergeItem(instanceWarning);

            MetadataManagerElement managerError =
                new ManagerFactoryDocument("1.0.0.0", _configurationId, "testdoc1").BuildValidationErrorsManager();
            managerError.MergeItem(instanceError);

            MetadataManagerElement managerView =
                new ManagerFactoryDocument("1.0.0.0", _configurationId, "testdoc1").BuildViewManager();
            managerView.MergeItem(instanceView);

            MetadataManagerElement managerScenario =
                new ManagerFactoryDocument("1.0.0.0", _configurationId, "testdoc1").BuildScenarioManager();
            managerScenario.MergeItem(instanceScenario);

            MetadataManagerElement managerProcess =
                new ManagerFactoryDocument("1.0.0.0", _configurationId, "testdoc1").BuildProcessManager();
            managerProcess.MergeItem(instanceProcess);

            MetadataManagerElement managerService =
                new ManagerFactoryDocument("1.0.0.0", _configurationId, "testdoc1").BuildServiceManager();
            managerService.MergeItem(instanceService);


            MetadataManagerElement managerGenerator =
                new ManagerFactoryDocument("1.0.0.0", _configurationId, "testdoc1").BuildGeneratorManager();
            managerGenerator.MergeItem(instanceGenerator);

            RestQueryApi.QueryPostNotify("1.0.0.0", _configurationId);

            new UpdateApi("1.0.0.0").UpdateStore(_configurationId);
        }

        [Test]
        public void ShouldExportAndImportConfiguration()
        {
            DeleteTestConfiguration();

            CreateTestConfiguration();

            var exporter = new ConfigExporter(new ZipStructure(@"TestData\TestZip.zip"));

            exporter.ExportHeaderToStructure("1.0.0.0", "2.0.0.0", _configurationId);

            DeleteTestConfiguration();

            exporter = new ConfigExporter(new ZipStructure(@"TestData\TestZip.zip", @"TestData\UnzipTest"));

            dynamic config = exporter.ImportHeaderFromStructure("2.0.0.0");

            CheckDynamicConfig(config);

            CheckConfiguration();
        }

        [Test]
        public void ShouldExportAndImportConfigurationToDirectory()
        {
            DeleteTestConfiguration();

            CreateTestConfiguration();

            var exporter = new ConfigExporter(new DirectoryStructure(@"TestData\TestExportToDirectory"));

            exporter.ExportHeaderToStructure("1.0.0.0", "2.0.0.0", _configurationId);

            exporter = new ConfigExporter(new DirectoryStructure(@"TestData\TestExportToDirectory"));

            exporter.ImportHeaderFromStructure("2.0.0.0");

            CheckConfiguration();
        }
    }
}