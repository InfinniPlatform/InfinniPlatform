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
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Hosting;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    public sealed class SignalRNotificationBehavior
    {
        private string _configId = "testsignalr";

        private string _documentId = "testdocsignalr";
        private IDisposable _server;

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

        [Test]
        public void ShouldNotifyClient()
        {
            CreateTestConfig();

            dynamic document = new DynamicWrapper();
            document.Name = "test";

            new DocumentApi().SetDocument(_configId, _documentId, document);
        }

        private void CreateTestConfig()
        {
            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager();

            dynamic config = managerConfiguration.CreateItem(_configId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration(_configId).BuildDocumentManager();
            dynamic documentMetadata1 = managerDocument.CreateItem(_documentId);
            managerDocument.MergeItem(documentMetadata1);

            //добавляем бизнес-процесс по умолчанию
            MetadataManagerElement processManager =
                new ManagerFactoryDocument(_configId, _documentId).BuildProcessManager();
            dynamic defaultProcess = processManager.CreateItem("Default");

            dynamic instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);

            instance.SuccessPoint = new DynamicWrapper();
            instance.SuccessPoint.TypeName = "Action";
            instance.SuccessPoint.ScenarioId = "TestSignalRAction";

            processManager.MergeItem(defaultProcess);

            //указываем ссылку на тестовый сценарий комплексного предзаполнения
            MetadataManagerElement scenarioManager =
                new ManagerFactoryDocument(_configId, _documentId).BuildScenarioManager();


            string scenarioSuccessId = "TestSignalRAction";
            dynamic scenarioSuccessItem = scenarioManager.CreateItem(scenarioSuccessId);
            scenarioSuccessItem.ScenarioId = scenarioSuccessId;
            scenarioSuccessItem.Id = scenarioSuccessId;
            scenarioSuccessItem.Name = scenarioSuccessId;
            scenarioSuccessItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioSuccessItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioSuccessItem);


            //добавляем ссылку на сборку, в которой находится прикладной модуль

            MetadataManagerElement assemblyManager =
                new ManagerFactoryConfiguration(_configId).BuildAssemblyManager();
            dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            dynamic package = new PackageBuilder().BuildPackage(_configId, "1.0.0.0", GetType().Assembly.Location);
            new UpdateApi().InstallPackages(new[] { package });

            RestQueryApi.QueryPostNotify(_configId);

            new UpdateApi().UpdateStore(_configId);
        }
    }
}