using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Hosting;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
	[Ignore("Необходимо создать конфигурацию метаданных на диске, т.к. теперь метаданные загружаются только с диска")]
    public sealed class ActionUnitExceptionHandlingBehavior
    {
        private IDisposable _server;
        private const string ConfigurationId = "exceptionhandlingconfig";

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
			_server = InfinniPlatformInprocessHost.Start();
		}

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        private void CreateTestConfig()
        {
            const string documentId = "testdoc1";

            new IndexApi().RebuildIndex(ConfigurationId, documentId);

            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager("1.0.0.0");

            dynamic config = managerConfiguration.CreateItem(ConfigurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration("1.0.0.0", ConfigurationId).BuildDocumentManager();
            dynamic documentMetadata1 = managerDocument.CreateItem(documentId);
            managerDocument.MergeItem(documentMetadata1);

            //добавляем бизнес-процесс по умолчанию
            MetadataManagerElement processManager =
                new ManagerFactoryDocument("1.0.0.0", ConfigurationId, documentId).BuildProcessManager();
            dynamic defaultProcess = processManager.CreateItem("Default");

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
            MetadataManagerElement scenarioManager =
                new ManagerFactoryDocument("1.0.0.0", ConfigurationId, documentId).BuildScenarioManager();

            const string scenarioSuccessId = "ActionWithException";
            dynamic scenarioSuccessItem = scenarioManager.CreateItem(scenarioSuccessId);
            scenarioSuccessItem.ScenarioId = scenarioSuccessId;
            scenarioSuccessItem.Id = scenarioSuccessId;
            scenarioSuccessItem.Name = scenarioSuccessId;
            scenarioSuccessItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioSuccessItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioSuccessItem);

            //добавляем ссылку на сборку, в которой находится прикладной модуль
            MetadataManagerElement assemblyManager =
                new ManagerFactoryConfiguration("1.0.0.0", ConfigurationId).BuildAssemblyManager();
            dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            dynamic package = new PackageBuilder().BuildPackage(ConfigurationId, "1.0.0.0", GetType().Assembly.Location);
            new UpdateApi("1.0.0.0").InstallPackages(new[] { package });

            RestQueryApi.QueryPostNotify(null, ConfigurationId);

            new UpdateApi("1.0.0.0").UpdateStore(ConfigurationId);
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
                dynamic result = new DocumentApi().SetDocument(ConfigurationId, "testdoc1", document);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Important exception details"));
            }
        }
    }
}