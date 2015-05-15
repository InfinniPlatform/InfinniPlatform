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
	public sealed class SignalRNotificationBehavior
	{
		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));
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

		private string _configId = "testsignalr";

		private string _documentId = "testdocsignalr";

		private void CreateTestConfig()
		{
			var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager();

			var config = managerConfiguration.CreateItem(_configId);
			managerConfiguration.DeleteItem(config);
			managerConfiguration.MergeItem(config);

			var managerDocument = new ManagerFactoryConfiguration(_configId).BuildDocumentManager();
			dynamic documentMetadata1 = managerDocument.CreateItem(_documentId);
			managerDocument.MergeItem(documentMetadata1);

			//добавляем бизнес-процесс по умолчанию
			var processManager = new ManagerFactoryDocument(_configId, _documentId).BuildProcessManager();
			var defaultProcess = processManager.CreateItem("Default");

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
			var scenarioManager = new ManagerFactoryDocument(_configId, _documentId).BuildScenarioManager();


			string scenarioSuccessId = "TestSignalRAction";
			dynamic scenarioSuccessItem = scenarioManager.CreateItem(scenarioSuccessId);
			scenarioSuccessItem.ScenarioId = scenarioSuccessId;
			scenarioSuccessItem.Id = scenarioSuccessId;
			scenarioSuccessItem.Name = scenarioSuccessId;
			scenarioSuccessItem.ScriptUnitType = ScriptUnitType.Action;
			scenarioSuccessItem.ContextType = ContextTypeKind.ApplyMove;
			scenarioManager.MergeItem(scenarioSuccessItem);


			//добавляем ссылку на сборку, в которой находится прикладной модуль

			var assemblyManager = new ManagerFactoryConfiguration(_configId).BuildAssemblyManager();
			dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
			assemblyManager.MergeItem(assemblyItem);

			var package = new PackageBuilder().BuildPackage(_configId, "test_version", this.GetType().Assembly.Location);
			UpdateApi.InstallPackages(new[] { package });

			RestQueryApi.QueryPostNotify(_configId);

			UpdateApi.UpdateStore(_configId);
		}

	}
}
