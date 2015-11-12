﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
	[Ignore("Необходимо создать конфигурацию метаданных на диске, т.к. теперь метаданные загружаются только с диска")]
	public sealed class AfterDocumentSaveEventsBehavior
    {
        private IDisposable _server;
        private string _configurationId = "testconfigsuccesssave";

        //[TestFixtureSetUp]
        public void FixtureSetup()
        {
			_server = InfinniPlatformInprocessHost.Start();
		}

        //[TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        private void CreateTestConfig(bool hasOnSuccessPoint, bool hasRegisterMovePoint)
        {
			_server = InfinniPlatformInprocessHost.Start();

            string configurationId = _configurationId;
            string documentId = "testdoc1";

            new IndexApi().RebuildIndex(configurationId, documentId);

            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager();

            dynamic config = managerConfiguration.CreateItem(configurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration(configurationId).BuildDocumentManager();
            dynamic documentMetadata1 = managerDocument.CreateItem(documentId);
            managerDocument.MergeItem(documentMetadata1);

            //добавляем бизнес-процесс по умолчанию
            MetadataManagerElement processManager =
                new ManagerFactoryDocument(configurationId, documentId).BuildProcessManager();
            dynamic defaultProcess = processManager.CreateItem("Default");

            dynamic instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);

            if (hasOnSuccessPoint)
            {
                instance.SuccessPoint = new DynamicWrapper();
                instance.SuccessPoint.TypeName = "Action";
                instance.SuccessPoint.ScenarioId = "testcomplexaction";
            }

            if (hasRegisterMovePoint)
            {
                instance.RegisterPoint = new DynamicWrapper();
                instance.RegisterPoint.TypeName = "Action";
                instance.RegisterPoint.ScenarioId = "testregistermoveAction";
            }

            processManager.MergeItem(defaultProcess);

            //указываем ссылку на тестовый сценарий комплексного предзаполнения
            MetadataManagerElement scenarioManager =
                new ManagerFactoryDocument(configurationId, documentId).BuildScenarioManager();

            if (hasOnSuccessPoint)
            {
                string scenarioSuccessId = "TestComplexAction";
                dynamic scenarioSuccessItem = scenarioManager.CreateItem(scenarioSuccessId);
                scenarioSuccessItem.ScenarioId = scenarioSuccessId;
                scenarioSuccessItem.Id = scenarioSuccessId;
                scenarioSuccessItem.Name = scenarioSuccessId;
                scenarioSuccessItem.ScriptUnitType = ScriptUnitType.Action;
                scenarioSuccessItem.ContextType = ContextTypeKind.ApplyMove;
                scenarioManager.MergeItem(scenarioSuccessItem);

                string otherScenarioSuccessId = "YetAnotherTestComplexAction";
                scenarioSuccessItem = scenarioManager.CreateItem(otherScenarioSuccessId);
                scenarioSuccessItem.ScenarioId = otherScenarioSuccessId;
                scenarioSuccessItem.Id = otherScenarioSuccessId;
                scenarioSuccessItem.Name = otherScenarioSuccessId;
                scenarioSuccessItem.ScriptUnitType = ScriptUnitType.Action;
                scenarioSuccessItem.ContextType = ContextTypeKind.ApplyMove;
                scenarioManager.MergeItem(scenarioSuccessItem);
            }



            //добавляем ссылку на сборку, в которой находится прикладной модуль

            MetadataManagerElement assemblyManager =
                new ManagerFactoryConfiguration(configurationId).BuildAssemblyManager();
            dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            dynamic package = new PackageBuilder().BuildPackage(configurationId, "1.0.0.0", GetType().Assembly.Location);
            new UpdateApi().InstallPackages(new[] { package });

            RestQueryApi.QueryPostNotify(configurationId);

            new UpdateApi().UpdateStore(configurationId);

            _server.Dispose();

			_server = InfinniPlatformInprocessHost.Start();
		}

        [Test]       
        public void ShouldInvokeSuccessActionAfterChangingScenario()
        {
            var document = new
                {
                    Id = Guid.NewGuid(),
                    LastName = "123",
                };

            CreateTestConfig(true, false);

            // Замена точки расширения
            MetadataManagerElement processManager =
                new ManagerFactoryDocument(_configurationId, "testdoc1").BuildProcessManager();
            dynamic defaultProcess = processManager.MetadataReader.GetItem("Default");
            defaultProcess.Transitions[0].SuccessPoint.ScenarioId = "yetanothertestcomplexaction";

            processManager.MergeItem(defaultProcess);

            new DocumentApi().SetDocument(_configurationId, "testdoc1", document);

            IEnumerable<dynamic> documents = new DocumentApi().GetDocument(_configurationId, "testdoc1",
                                                                               f =>
                                                                               f.AddCriteria(
                                                                                   cr =>
                                                                                   cr.Property("TestValue")
                                                                                     .IsEquals("AnotherTest")), 0, 1);

            Assert.AreEqual("AnotherTest", documents.First().TestValue);
        }

        [Test]
        public void ShouldInvokeSuccessActionOnSuccessSaveDocument()
        {
            var document = new
                {
                    Id = Guid.NewGuid().ToString(),
                    LastName = "123",
                };


            CreateTestConfig(true, false);

            new DocumentApi().SetDocument(_configurationId, "testdoc1", document, false);

            IEnumerable<dynamic> documents = new DocumentApi().GetDocument(_configurationId, "testdoc1",
                                                                               f =>
                                                                               f.AddCriteria(
                                                                                   cr =>
                                                                                   cr.Property("TestValue")
                                                                                     .IsEquals("Test")), 0, 1);

            Assert.AreEqual("Test", documents.First().TestValue);
        }


    }
}