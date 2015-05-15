﻿using System;
using System.Collections.Generic;
using System.Linq;

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
	public sealed class TransactionBehavior
	{
		string _configurationId = "TestTransactionConfig";
		string _documentIdPatient = "Patient";
		string _documentIdAddress = "Address";
		string _documentIdHouse = "AddressHouse";

		private IDisposable _server;

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

		private void CreateDocumentSchema()
		{
			IndexApi.RebuildIndex(_configurationId, _documentIdPatient);
			IndexApi.RebuildIndex(_configurationId, _documentIdAddress);

			var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager();

			dynamic config = managerConfig.CreateItem(_configurationId);
            managerConfig.DeleteItem(config);
            managerConfig.MergeItem(config);

			var managerDocument = new ManagerFactoryConfiguration(_configurationId).BuildDocumentManager();


			var documentMetadata1 = managerDocument.CreateItem(_documentIdPatient);

			documentMetadata1.Schema = new DynamicWrapper();
			documentMetadata1.Schema.Name = _documentIdPatient;
			documentMetadata1.Schema.Caption = _documentIdPatient;
			documentMetadata1.Schema.Properties = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Id.Type = "Uuid";
			documentMetadata1.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata1.Schema.Properties.Name = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Name.Type = "String";
			documentMetadata1.Schema.Properties.Name.Caption = "Patient name";

			documentMetadata1.Schema.Properties.Address = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.Type = "Object";
			documentMetadata1.Schema.Properties.Address.TypeInfo = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink = new DynamicWrapper();
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.ConfigId = _configurationId;
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.DocumentId = _documentIdAddress;
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.Inline = true;

			documentMetadata1.Schema.Properties.ObservationDate = new DynamicWrapper();
			documentMetadata1.Schema.Properties.ObservationDate.Type = "DateTime";
			documentMetadata1.Schema.Properties.ObservationDate.Caption = "Дата осмотра";

			var documentMetadata2 = managerDocument.CreateItem(_documentIdAddress);

			documentMetadata2.Schema = new DynamicWrapper();
			documentMetadata2.Schema.Type = _documentIdAddress;
			documentMetadata2.Schema.Caption = _documentIdAddress;
			documentMetadata2.Schema.Properties = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id.Type = "Uuid";
			documentMetadata2.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata2.Schema.Properties.House = new DynamicWrapper();
			documentMetadata2.Schema.Properties.House.Type = "Object";
			documentMetadata2.Schema.Properties.House.Caption = "House";
			documentMetadata2.Schema.Properties.House.TypeInfo = new DynamicWrapper();
			documentMetadata2.Schema.Properties.House.TypeInfo.DocumentLink = new DynamicWrapper();
			documentMetadata2.Schema.Properties.House.TypeInfo.DocumentLink.ConfigId = _configurationId;
			documentMetadata2.Schema.Properties.House.TypeInfo.DocumentLink.DocumentId = _documentIdHouse;
			documentMetadata2.Schema.Properties.House.TypeInfo.DocumentLink.Inline = true;

			var documentMetadata3 = managerDocument.CreateItem(_documentIdHouse);

			documentMetadata3.Schema = new DynamicWrapper();
			documentMetadata3.Schema.Type = _documentIdHouse;
			documentMetadata3.Schema.Caption = _documentIdHouse;
			documentMetadata3.Schema.Properties = new DynamicWrapper();
			documentMetadata3.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata3.Schema.Properties.Id.Type = "Uuid";
			documentMetadata3.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata3.Schema.Properties.HouseNumber = new DynamicWrapper();
			documentMetadata3.Schema.Properties.HouseNumber.Type = "String";
			documentMetadata3.Schema.Properties.HouseNumber.Caption = "House number";

			documentMetadata3.Schema.Properties.PostIndex = new DynamicWrapper();
			documentMetadata3.Schema.Properties.PostIndex.Type = "String";
			documentMetadata3.Schema.Properties.PostIndex.Caption = "Post index";


			//Структура
			// Patient
			//  |---Address
			//     |-- House           

            managerDocument.MergeItem(documentMetadata1);
            managerDocument.MergeItem(documentMetadata2);
            managerDocument.MergeItem(documentMetadata3);


			//указываем ссылку на тестовый сценарий 
			var scenarioManager = new ManagerFactoryDocument(_configurationId, _documentIdPatient).BuildScenarioManager();
			string scenarioId = "TestSaveDocumentFailAction";
			dynamic scenarioItem = scenarioManager.CreateItem(scenarioId);
			scenarioItem.ScenarioId = scenarioId;
			scenarioItem.Id = scenarioId;
			scenarioItem.Name = scenarioId;
			scenarioItem.ScriptUnitType = ScriptUnitType.Action;
			scenarioItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioItem);

			var assemblyManager = new ManagerFactoryConfiguration(_configurationId).BuildAssemblyManager();
			dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);


			//Описываем схему предзаполнения в умолчательном бизнес-процессе
			var processManager = new ManagerFactoryDocument(_configurationId, _documentIdPatient).BuildProcessManager();
			var process = processManager.CreateItem("Default");

			process.Type = WorkflowTypes.WithoutState;

			process.Transitions = new List<dynamic>();
			process.Transitions.Add(new DynamicWrapper());
			process.Transitions[0].Id = Guid.NewGuid().ToString();
			process.Transitions[0].SuccessPoint = new DynamicWrapper();
			process.Transitions[0].SuccessPoint.TypeName = "Action";
			process.Transitions[0].SuccessPoint.ScenarioId = scenarioId;

            processManager.MergeItem(process);

			var package = new PackageBuilder().BuildPackage(_configurationId, "test_version", GetType().Assembly.Location);
			UpdateApi.InstallPackages(new[] { package });


			RestQueryApi.QueryPostNotify(_configurationId);

			UpdateApi.UpdateStore(_configurationId);

		}


		[Test]
		public void ShouldFailTransaction()
		{
			CreateDocumentSchema();

			dynamic document = new DocumentApi().CreateDocument(_configurationId, _documentIdPatient);


			document.Id = Guid.NewGuid().ToString();
			document.Name = "TestDocument";

			//при обработке документа в OnSuccess возникнет исключение
			try
			{
				new DocumentApi().SetDocument(_configurationId, _documentIdPatient, document);
			}
			catch
			{

			}

			IEnumerable<dynamic> addresses = new DocumentApi().GetDocument(_configurationId, _documentIdAddress, null, 0, 1);

			Assert.AreEqual(0, addresses.Count());


			IEnumerable<dynamic> documents = new DocumentApi().GetDocument(_configurationId, _documentIdPatient, null, 0, 1);

			Assert.AreEqual(0, documents.Count());

		}

	}
}
