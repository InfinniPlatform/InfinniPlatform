using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public class ConfigurationVersionBehavior
    {
        private IDisposable _server;
        private const string ConfigurationId = "testconfigversion9";
        private const string DocumentId = "testdocument";

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
        public void ShouldUseSomeConfigurationVersion()
        {
            //Given
            CreateTestConfig("1", "TestAction");
            CreateTestConfig("2", "TestAction_v1");
            CreateTestConfig("3", "TestAction_v2");

            dynamic testDoc = new
            {
                Name = "TestDocument"
            };

            var apiV1 = new DocumentApi("1");
            var apiV2 = new DocumentApi("2");
            var apiV3 = new DocumentApi("3");

            //When
            apiV1.SetDocument(ConfigurationId, DocumentId, testDoc);
            apiV2.SetDocument(ConfigurationId, DocumentId, testDoc);
            apiV3.SetDocument(ConfigurationId, DocumentId, testDoc);

            //Then
            //проверяем, что были созданы документы разных версий ActionUnit
            var checkDoc1 = apiV1.GetDocument(ConfigurationId, DocumentId,
                f => f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction")), 0, 1).FirstOrDefault();
            var checkDoc2 = apiV1.GetDocument(ConfigurationId, DocumentId,
                f => f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v1")), 0, 1).FirstOrDefault();
            var checkDoc3 = apiV1.GetDocument(ConfigurationId, DocumentId,
                f => f.AddCriteria(cr => cr.Property("Name").IsEquals("Name_TestAction_v2")), 0, 1).FirstOrDefault();

            Assert.IsNotNull(checkDoc1);
            Assert.IsNotNull(checkDoc2);
            Assert.IsNotNull(checkDoc3);
        }


        private void CreateTestConfig(string version, string scenarioId)
        {
            string configurationId = ConfigurationId;
            string documentId = DocumentId;

            new IndexApi().RebuildIndex(configurationId, documentId);

            var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager(version); //указана версия конфигурации

            var config = managerConfiguration.CreateItem(configurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            var managerDocument = new ManagerFactoryConfiguration(version, configurationId).BuildDocumentManager();
            dynamic documentMetadata1 = managerDocument.CreateItem(documentId);

            documentMetadata1.Schema = new DynamicWrapper();
            documentMetadata1.Schema.Type =  documentId;
            documentMetadata1.Schema.Caption = documentId;
            documentMetadata1.Schema.Properties = new DynamicWrapper();
            documentMetadata1.Schema.Properties.Name = new DynamicWrapper();
            documentMetadata1.Schema.Properties.Name.Type = "String";
            documentMetadata1.Schema.Properties.Name.Caption = "Patient name";

            managerDocument.MergeItem(documentMetadata1);

            //добавляем бизнес-процесс по умолчанию
            var processManager = new ManagerFactoryDocument(version, configurationId, documentId).BuildProcessManager();
            var defaultProcess = processManager.CreateItem("Default");

            dynamic instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);
            
            instance.SuccessPoint = new DynamicWrapper();
            instance.SuccessPoint.TypeName = "Action";
            instance.SuccessPoint.ScenarioId = scenarioId;

            processManager.MergeItem(defaultProcess);

            var scenarioManager = new ManagerFactoryDocument(version,configurationId, documentId).BuildScenarioManager();

            dynamic scenarioItem = scenarioManager.CreateItem(scenarioId);
            scenarioItem.ScenarioId = scenarioId;
            scenarioItem.Id = scenarioId;
            scenarioItem.Name = scenarioId;
            scenarioItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioItem);

            var assemblyManager = new ManagerFactoryConfiguration(version, configurationId).BuildAssemblyManager();
            dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            //разворачиваем версию с идентификатором версии "0"
            var package = new PackageBuilder().BuildPackage(configurationId, version, GetType().Assembly.Location);
            new UpdateApi(version).InstallPackages(new[] { package });

            RestQueryApi.QueryPostNotify(version, configurationId);
        }


    }
}
