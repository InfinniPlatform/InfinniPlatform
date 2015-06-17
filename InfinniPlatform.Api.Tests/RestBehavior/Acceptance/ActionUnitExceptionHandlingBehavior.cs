using System.Collections.Generic;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;
using System;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ActionUnitExceptionHandlingBehavior
    {
        private IDisposable _server;
        private const string ConfigurationId = "exceptionhandlingconfig";

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
        public void ShouldFormatExceptionMessage()
        {
            var document = new
            {
                Id = Guid.NewGuid(),
                LastName = "123",
            };

            CreateTestConfig();

	        try
	        {
		        var result = new DocumentApi(null).SetDocument(ConfigurationId, "testdoc1", document);

	        }
	        catch (Exception e)
	        {
		        Assert.IsTrue(e.Message.Contains("Important exception details"));
	        }
        }
        
        private void CreateTestConfig()
        {
            const string documentId = "testdoc1";

            new IndexApi().RebuildIndex(ConfigurationId, documentId);

            var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager(null);

            var config = managerConfiguration.CreateItem(ConfigurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            var managerDocument = new ManagerFactoryConfiguration(null, ConfigurationId).BuildDocumentManager();
            dynamic documentMetadata1 = managerDocument.CreateItem(documentId);
            managerDocument.MergeItem(documentMetadata1);

            //добавляем бизнес-процесс по умолчанию
            var processManager = new ManagerFactoryDocument(null,ConfigurationId, documentId).BuildProcessManager();
            var defaultProcess = processManager.CreateItem("Default");

            dynamic instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);


            instance.SuccessPoint = new DynamicWrapper();
            instance.SuccessPoint.TypeName = "Action";
            instance.SuccessPoint.ScenarioId = "actionwithexception";
            
            processManager.MergeItem(defaultProcess);

            //указываем ссылку на тестовый сценарий комплексного предзаполнения
            var scenarioManager = new ManagerFactoryDocument(null,ConfigurationId, documentId).BuildScenarioManager();
            
            const string scenarioSuccessId = "ActionWithException";
            dynamic scenarioSuccessItem = scenarioManager.CreateItem(scenarioSuccessId);
            scenarioSuccessItem.ScenarioId = scenarioSuccessId;
            scenarioSuccessItem.Id = scenarioSuccessId;
            scenarioSuccessItem.Name = scenarioSuccessId;
            scenarioSuccessItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioSuccessItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioSuccessItem);

            //добавляем ссылку на сборку, в которой находится прикладной модуль
            var assemblyManager = new ManagerFactoryConfiguration(null, ConfigurationId).BuildAssemblyManager();
            dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            var package = new PackageBuilder().BuildPackage(ConfigurationId, null, GetType().Assembly.Location);
            new UpdateApi(null).InstallPackages(new[] { package });

            RestQueryApi.QueryPostNotify(null, ConfigurationId);

            new UpdateApi(null).UpdateStore(ConfigurationId);
        }

    }
}
