using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Metadata;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    [TestFixture]
    public class MetadataApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string Route = "1";

        private InfinniMetadataApi _metadataApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _metadataApi = new InfinniMetadataApi(InfinniSessionServer, InfinniSessionPort,Route);
        }

        [Test]
        public void ShouldInsertUpdateDeleteSolution()
        {
            var solution = new SolutionMetadata();
            solution.Id = Guid.NewGuid().ToString();
            solution.Name = "TestSolutionVersion_"+Guid.NewGuid().ToString();
            solution.Version = "2.0.0.0";

            _metadataApi.InsertSolution(solution);
            dynamic solutionRead = _metadataApi.GetSolution(solution.Version, solution.Name);

            Assert.IsNotNull(solutionRead);

            solution.Name = "TestSolutionVersion_" + Guid.NewGuid().ToString() + "Changed";
            _metadataApi.UpdateSolution(solution);

            solutionRead = _metadataApi.GetSolution(solution.Version, solution.Name);
            Assert.AreEqual(solution.Name, solutionRead.Name);

            _metadataApi.DeleteSolution(solution.Version, solution.Name);
            solutionRead = _metadataApi.GetSolution(solution.Version, solution.Name);
            Assert.IsNull(solutionRead);
        }

        [Test]
        public void ShouldInsertUpdateDeleteConfig()
        {
            var config = new ConfigurationMetadata();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";

            _metadataApi.InsertConfig(config);
            dynamic configRead = _metadataApi.GetConfig(config.Version, config.Name);

            Assert.IsNotNull(configRead);

            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString() + "Changed";
            _metadataApi.UpdateConfig(config);

            configRead = _metadataApi.GetConfig(config.Version, config.Name);
            Assert.AreEqual(config.Name, configRead.Name);

            _metadataApi.DeleteConfig(config.Version, config.Name);
            configRead = _metadataApi.GetConfig(config.Version, config.Name);
            Assert.IsNull(configRead);
        }

        [Test()]
        public void ShouldInsertUpdateDeleteMenu()
        {
            var config = new ConfigurationMetadata();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var menu = new MenuMetadata();
            menu.Id = Guid.NewGuid().ToString();
            menu.Name = "MainMenu";

            _metadataApi.InsertMenu(menu, config.Version, config.Name);

            var menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.IsNotNull(menuRead);

            menu.Name = "MainMenu_v1";

            _metadataApi.UpdateMenu(menu, config.Version, config.Name);

            menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.AreEqual(menuRead.Name, menu.Name);

            _metadataApi.DeleteMenu(config.Version, config.Name, menu.Name);

            menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.IsNull(menuRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test()]
        public void ShouldInsertUpdateDeleteAssembly()
        {
            var config = new ConfigurationMetadata();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var assemblyMetadata = new AssemblyMetadata();
            assemblyMetadata.Id = Guid.NewGuid().ToString();
            assemblyMetadata.Name = "Assembly1";

            _metadataApi.InsertAssembly(assemblyMetadata, config.Version, config.Name);

            var assemblyRead = _metadataApi.GetAssembly(config.Version, config.Name, assemblyMetadata.Name);

            Assert.IsNotNull(assemblyRead);

            assemblyMetadata.Name = "Assembly1_v1";

            _metadataApi.UpdateAssembly(assemblyMetadata, config.Version, config.Name);

            assemblyRead = _metadataApi.GetAssembly(config.Version, config.Name, assemblyMetadata.Name);

            Assert.AreEqual(assemblyRead.Name, assemblyMetadata.Name);

            _metadataApi.DeleteAssembly(config.Version, config.Name, assemblyMetadata.Name);

            assemblyRead = _metadataApi.GetAssembly(config.Version, config.Name, assemblyMetadata.Name);

            Assert.IsNull(assemblyRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test()]
        public void ShouldInsertUpdateDeleteRegister()
        {
            var config = new ConfigurationMetadata();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var registerMetadata = new RegisterMetadata();
            registerMetadata.Id = Guid.NewGuid().ToString();
            registerMetadata.Name = "Register1";

            _metadataApi.InsertRegister(registerMetadata, config.Version, config.Name);

            var registerRead = _metadataApi.GetRegister(config.Version, config.Name, registerMetadata.Name);

            Assert.IsNotNull(registerRead);

            registerMetadata.Name = "Register1_v1";

            _metadataApi.UpdateRegister(registerMetadata, config.Version, config.Name);

            registerRead = _metadataApi.GetRegister(config.Version, config.Name, registerMetadata.Name);

            Assert.AreEqual(registerRead.Name, registerMetadata.Name);

            _metadataApi.DeleteRegister(config.Version, config.Name, registerMetadata.Name);

            registerRead = _metadataApi.GetRegister(config.Version, config.Name, registerMetadata.Name);

            Assert.IsNull(registerRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test()]
        public void ShouldInsertUpdateDeleteDocument()
        {
            var config = new ConfigurationMetadata();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var documentMetadata = new DocumentMetadata();
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.Name = "Document1";

            _metadataApi.InsertDocument(documentMetadata, config.Version, config.Name);

            var documentRead = _metadataApi.GetDocument(config.Version, config.Name, documentMetadata.Name);

            Assert.IsNotNull(documentRead);

            documentMetadata.Name = "Document1_v1";

            _metadataApi.UpdateDocument(documentMetadata, config.Version, config.Name);

            documentRead = _metadataApi.GetDocument(config.Version, config.Name, documentMetadata.Name);

            Assert.AreEqual(documentRead.Name, documentMetadata.Name);

            _metadataApi.DeleteDocument(config.Version, config.Name, documentMetadata.Name);

            documentRead = _metadataApi.GetAssembly(config.Version, config.Name, documentMetadata.Name);

            Assert.IsNull(documentRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }


        [Test]
        public void ShouldInsertUpdateDeleteDocumentScenario()
        {
            var config = new ConfigurationMetadata();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var documentMetadata = new DocumentMetadata();
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.Name = "Scenario1";

            _metadataApi.InsertDocument(documentMetadata, config.Version, config.Name);

            var scenarioMetadata = new ScenarioMetadata();
            scenarioMetadata.Id = Guid.NewGuid().ToString();
            scenarioMetadata.Name = Guid.NewGuid().ToString();
            scenarioMetadata.ScenarioId = "ActionUnitThatNotExistsAndOnlyForTestName";

            _metadataApi.InsertScenario(scenarioMetadata, config.Version, config.Name, documentMetadata.Name);

            dynamic scenarioMetadataRead = _metadataApi.GetScenario(config.Version, config.Name, documentMetadata.Name, scenarioMetadata.Name);

            Assert.IsNotNull(scenarioMetadataRead);
            Assert.AreEqual(scenarioMetadataRead.Id, scenarioMetadata.Id);

            scenarioMetadata.Name = "Scenario1_v1";

            _metadataApi.UpdateScenario(scenarioMetadata, config.Version, config.Name, documentMetadata.Name);

            var scenarioRead = _metadataApi.GetScenario(config.Version, config.Name, documentMetadata.Name, scenarioMetadata.Name);

            Assert.AreEqual(scenarioRead.Name, scenarioMetadata.Name);

            _metadataApi.DeleteScenario(config.Version, config.Name, documentMetadata.Name, scenarioMetadata.Name);

            scenarioRead = _metadataApi.GetScenario(config.Version, config.Name, documentMetadata.Name, scenarioMetadata.Name);

            Assert.IsNull(scenarioRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }
    }
}
