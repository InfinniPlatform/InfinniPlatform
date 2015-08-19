using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;
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
            var solution = _metadataApi.CreateSolution();
            
            Assert.IsNotNull(solution);
            Assert.IsNotNull(solution.Name);

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

            var solutions = _metadataApi.GetSolutionItems();
            Assert.True(solutions.Count > 0);

            _metadataApi.DeleteSolution(solution.Version, solution.Name);
            solutionRead = _metadataApi.GetSolution(solution.Version, solution.Name);
            Assert.IsNull(solutionRead);
        }

        [Test]
        public void ShouldInsertUpdateDeleteConfig()
        {
            var config = _metadataApi.CreateConfig();
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

            IEnumerable<dynamic> configList = _metadataApi.GetConfigList();
            Assert.True(configList.Any());

            _metadataApi.DeleteConfig(config.Version, config.Name);
            configRead = _metadataApi.GetConfig(config.Version, config.Name);
            Assert.IsNull(configRead);
        }

        [Test()]
        public void ShouldInsertUpdateDeleteMenu()
        {
            dynamic config = _metadataApi.CreateConfig();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var menu = _metadataApi.CreateMenu(config.Version, config.Name);
            menu.Id = Guid.NewGuid().ToString();
            menu.Name = "MainMenu";

            _metadataApi.InsertMenu(menu, config.Version, config.Name);

            var menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.IsNotNull(menuRead);

            menu.Name = "MainMenu_v1";

            _metadataApi.UpdateMenu(menu, config.Version, config.Name);

            menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.AreEqual(menuRead.Name, menu.Name);

            IEnumerable<dynamic> menuList = _metadataApi.GetMenuList(config.Version, config.Name);

            Assert.True(menuList.Any());

            _metadataApi.DeleteMenu(config.Version, config.Name, menu.Name);

            menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.IsNull(menuRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test()]
        public void ShouldInsertUpdateDeleteAssembly()
        {
            dynamic config = _metadataApi.CreateConfig();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var assemblyMetadata = _metadataApi.CreateAssembly(config.Version, config.Name);
            assemblyMetadata.Id = Guid.NewGuid().ToString();
            assemblyMetadata.Name = "Assembly1";

            _metadataApi.InsertAssembly(assemblyMetadata, config.Version, config.Name);

            var assemblyRead = _metadataApi.GetAssembly(config.Version, config.Name, assemblyMetadata.Name);

            Assert.IsNotNull(assemblyRead);

            assemblyMetadata.Name = "Assembly1_v1";

            _metadataApi.UpdateAssembly(assemblyMetadata, config.Version, config.Name);

            assemblyRead = _metadataApi.GetAssembly(config.Version, config.Name, assemblyMetadata.Name);

            Assert.AreEqual(assemblyRead.Name, assemblyMetadata.Name);

            IEnumerable<dynamic> assemblyList = _metadataApi.GetAssemblyList(config.Version, config.Name);

            Assert.True(assemblyList.Any());

            _metadataApi.DeleteAssembly(config.Version, config.Name, assemblyMetadata.Name);

            assemblyRead = _metadataApi.GetAssembly(config.Version, config.Name, assemblyMetadata.Name);

            Assert.IsNull(assemblyRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test()]
        public void ShouldInsertUpdateDeleteRegister()
        {
            var config = _metadataApi.CreateConfig();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var registerMetadata = _metadataApi.CreateRegister(config.Version,config.Name);
            registerMetadata.Id = Guid.NewGuid().ToString();
            registerMetadata.Name = "Register1";

            _metadataApi.InsertRegister(registerMetadata, config.Version, config.Name);

            var registerRead = _metadataApi.GetRegister(config.Version, config.Name, registerMetadata.Name);

            Assert.IsNotNull(registerRead);

            registerMetadata.Name = "Register1_v1";

            _metadataApi.UpdateRegister(registerMetadata, config.Version, config.Name);

            registerRead = _metadataApi.GetRegister(config.Version, config.Name, registerMetadata.Name);

            Assert.AreEqual(registerRead.Name, registerMetadata.Name);

            IEnumerable<dynamic> registers = _metadataApi.GetRegisterList(config.Version, config.Name);
            Assert.True(registers.Any());

            _metadataApi.DeleteRegister(config.Version, config.Name, registerMetadata.Name);

            registerRead = _metadataApi.GetRegister(config.Version, config.Name, registerMetadata.Name);

            Assert.IsNull(registerRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test()]
        public void ShouldInsertUpdateDeleteDocument()
        {
            var config = _metadataApi.CreateConfig();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var documentMetadata = _metadataApi.CreateDocument(config.Version, config.Name);
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.Name = "Document1";

            _metadataApi.InsertDocument(documentMetadata, config.Version, config.Name);

            var documentRead = _metadataApi.GetDocument(config.Version, config.Name, documentMetadata.Name);

            Assert.IsNotNull(documentRead);

            documentMetadata.Name = "Document1_v1";

            _metadataApi.UpdateDocument(documentMetadata, config.Version, config.Name);

            documentRead = _metadataApi.GetDocument(config.Version, config.Name, documentMetadata.Name);

            Assert.AreEqual(documentRead.Name, documentMetadata.Name);

            IEnumerable<dynamic> documents = _metadataApi.GetDocumentList(config.Version, config.Name);
            Assert.True(documents.Any());

            _metadataApi.DeleteDocument(config.Version, config.Name, documentMetadata.Name);

            documentRead = _metadataApi.GetAssembly(config.Version, config.Name, documentMetadata.Name);

            Assert.IsNull(documentRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }


        [Test]
        public void ShouldInsertUpdateDeleteDocumentScenario()
        {
            var config = _metadataApi.CreateConfig();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var documentMetadata = _metadataApi.CreateDocument(config.Version, config.Name);
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.Name = "Document1";

            _metadataApi.InsertDocument(documentMetadata, config.Version, config.Name);

            var scenarioMetadata = _metadataApi.CreateScenario(config.Version, config.Name, documentMetadata.Name);
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

            IEnumerable<dynamic> scenarios = _metadataApi.GetScenarioItems(config.Version, config.Name,
                documentMetadata.Name);
            Assert.True(scenarios.Any());

            _metadataApi.DeleteScenario(config.Version, config.Name, documentMetadata.Name, scenarioMetadata.Name);

            scenarioRead = _metadataApi.GetScenario(config.Version, config.Name, documentMetadata.Name, scenarioMetadata.Name);

            Assert.IsNull(scenarioRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test]
        public void ShouldInsertUpdateDeleteDocumentProcess()
        {
            var config = _metadataApi.CreateConfig();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var documentMetadata = _metadataApi.CreateDocument(config.Version, config.Name);
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.Name = "Document1";

            _metadataApi.InsertDocument(documentMetadata, config.Version, config.Name);

            var processMetadata = _metadataApi.CreateProcess(config.Version, config.Name,documentMetadata.Name);
            processMetadata.Id = Guid.NewGuid().ToString();
            processMetadata.Name = "Process1";
            processMetadata.SettingsType = "Default";
            processMetadata.Type = 1;

            _metadataApi.InsertProcess(processMetadata, config.Version, config.Name, documentMetadata.Name);

            dynamic processMetadataRead = _metadataApi.GetProcess(config.Version, config.Name, documentMetadata.Name, processMetadata.Name);

            Assert.IsNotNull(processMetadataRead);
            Assert.AreEqual(processMetadataRead.Id, processMetadata.Id);

            processMetadata.Name = "Process1_v1";

            _metadataApi.UpdateProcess(processMetadata, config.Version, config.Name, documentMetadata.Name);

            var processRead = _metadataApi.GetProcess(config.Version, config.Name, documentMetadata.Name, processMetadata.Name);

            Assert.AreEqual(processRead.Name, processMetadata.Name);

            IEnumerable<dynamic> processes = _metadataApi.GetProcessItems(config.Version, config.Name, documentMetadata.Name);
            Assert.True(processes.Any());

            _metadataApi.DeleteProcess(config.Version, config.Name, documentMetadata.Name, processMetadata.Name);

            processRead = _metadataApi.GetProcess(config.Version, config.Name, documentMetadata.Name, processMetadata.Name);

            Assert.IsNull(processRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test]
        public void ShouldInsertUpdateDeleteDocumentService()
        {
            var config = _metadataApi.CreateConfig();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var documentMetadata = _metadataApi.CreateDocument(config.Version, config.Name);
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.Name = "Document1";

            _metadataApi.InsertDocument(documentMetadata, config.Version, config.Name);

            var serviceMetadata = _metadataApi.CreateService(config.Version, config.Name,documentMetadata.Name);
            serviceMetadata.Id = Guid.NewGuid().ToString();
            serviceMetadata.Name = "Service1";

            _metadataApi.InsertService(serviceMetadata, config.Version, config.Name, documentMetadata.Name);

            dynamic serviceMetadataRead = _metadataApi.GetService(config.Version, config.Name, documentMetadata.Name, serviceMetadata.Name);

            Assert.IsNotNull(serviceMetadataRead);
            Assert.AreEqual(serviceMetadataRead.Id, serviceMetadata.Id);

            serviceMetadata.Name = "Service1_v1";

            _metadataApi.UpdateService(serviceMetadata, config.Version, config.Name, documentMetadata.Name);

            var serviceRead = _metadataApi.GetService(config.Version, config.Name, documentMetadata.Name, serviceMetadata.Name);

            Assert.AreEqual(serviceRead.Name, serviceMetadata.Name);

            IEnumerable<dynamic> services = _metadataApi.GetServiceItems(config.Version, config.Name,
                documentMetadata.Name);
            Assert.True(services.Any());

            _metadataApi.DeleteService(config.Version, config.Name, documentMetadata.Name, serviceMetadata.Name);

            serviceRead = _metadataApi.GetService(config.Version, config.Name, documentMetadata.Name, serviceMetadata.Name);

            Assert.IsNull(serviceRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test]
        public void ShouldInsertUpdateDeleteDocumentView()
        {
            var config = _metadataApi.CreateConfig();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var documentMetadata = _metadataApi.CreateDocument(config.Version, config.Name);
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.Name = "Document1";

            _metadataApi.InsertDocument(documentMetadata, config.Version, config.Name);

            var viewMetadata = new ViewMetadata();
            viewMetadata.Id = Guid.NewGuid().ToString();
            viewMetadata.Name = "View1";

            _metadataApi.InsertView(viewMetadata, config.Version, config.Name, documentMetadata.Name);

            dynamic viewMetadataRead = _metadataApi.GetView(config.Version, config.Name, documentMetadata.Name, viewMetadata.Name);

            Assert.IsNotNull(viewMetadataRead);
            Assert.AreEqual(viewMetadataRead.Id, viewMetadata.Id);

            viewMetadata.Name = "View1_v1";

            _metadataApi.UpdateView(viewMetadata, config.Version, config.Name, documentMetadata.Name);

            var viewMetadataREad = _metadataApi.GetView(config.Version, config.Name, documentMetadata.Name, viewMetadata.Name);

            Assert.AreEqual(viewMetadataREad.Name, viewMetadata.Name);

            IEnumerable<dynamic> views = _metadataApi.GetViewItems(config.Version, config.Name, documentMetadata.Name);
            Assert.True(views.Any());


            _metadataApi.DeleteView(config.Version, config.Name, documentMetadata.Name, viewMetadata.Name);

            viewMetadataREad = _metadataApi.GetView(config.Version, config.Name, documentMetadata.Name, viewMetadata.Name);

            Assert.IsNull(viewMetadataREad);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }

        [Test]
        public void ShouldInsertUpdateDeleteDocumentPrintView()
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

            var printViewMetadata = new PrintViewMetadata();
            printViewMetadata.Id = Guid.NewGuid().ToString();
            printViewMetadata.Name = "PrintView1";

            _metadataApi.InsertPrintView(printViewMetadata, config.Version, config.Name, documentMetadata.Name);

            dynamic viewMetadataRead = _metadataApi.GetPrintView(config.Version, config.Name, documentMetadata.Name, printViewMetadata.Name);

            Assert.IsNotNull(viewMetadataRead);
            Assert.AreEqual(viewMetadataRead.Id, printViewMetadata.Id);

            printViewMetadata.Name = "View1_v1";

            _metadataApi.UpdatePrintView(printViewMetadata, config.Version, config.Name, documentMetadata.Name);

            var viewMetadataREad = _metadataApi.GetPrintView(config.Version, config.Name, documentMetadata.Name, printViewMetadata.Name);

            Assert.AreEqual(viewMetadataREad.Name, printViewMetadata.Name);

            _metadataApi.DeletePrintView(config.Version, config.Name, documentMetadata.Name, printViewMetadata.Name);

            viewMetadataREad = _metadataApi.GetPrintView(config.Version, config.Name, documentMetadata.Name, printViewMetadata.Name);

            Assert.IsNull(viewMetadataREad);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }
    }
}
