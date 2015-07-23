using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Packages
{
    [TestFixture]
    public class SolutionExporterBehavior
    {
        private IDisposable _server;
        private static string _solutionId = "SystemConfiguration";

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
        public void ShouldImportAndExportSystemConfigurations()
        {
            var importDir = @"..\SystemConfiguration_1.0.0.0";
            var solutionExporter = new SolutionExporter(new DirectoryStructure(importDir),
                config => new DirectoryStructure(importDir + string.Format(@"\{0}_{1}", config.Name, config.Version)));

            solutionExporter.ImportHeaderFromStructure("1.0.0.0");

            IDataReader managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationMetadataReader("1.0.0.0");
            dynamic config1 = managerConfiguration.GetItem("Administration");
            dynamic config2 = managerConfiguration.GetItem("AdministrationCustomization");
            dynamic config3 = managerConfiguration.GetItem("Authorization");

            Assert.IsNotNull(config1);
            Assert.IsNotNull(config2);
            Assert.IsNotNull(config3);

            var exportDir = Directory.GetCurrentDirectory() +  @"\Export\SystemConfiguration_1.0.0.0";
            solutionExporter = new SolutionExporter(new DirectoryStructure(exportDir),
                config => new DirectoryStructure(exportDir + string.Format(@"\{0}.Configuration_{1}", config.Name, config.Version)));

            solutionExporter.ExportSolutionToStructure("SystemConfiguration", "1.0.0.0");

            File.Exists(Path.Combine(exportDir, @"\Administration.Configuration_1.0.0.0\Configuration.json"));
            File.Exists(Path.Combine(exportDir, @"\Authorization.Configuration_1.0.0.0\Configuration.json"));
            File.Exists(Path.Combine(exportDir, @"\AdministrationCustomization.Configuration_1.0.0.0\Configuration.json"));

        }


    }
}
