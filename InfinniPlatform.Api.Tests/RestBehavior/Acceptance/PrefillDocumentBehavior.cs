using System;
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
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class PrefillDocumentBehavior
	{
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

		[Test]
		public void ShouldPrefillDocumentBySchema()
		{
			//создаем конфигурацию с документом для предзаполнения поля "Дата осмотра"

			var configId = "TestConfigPrefillSchema";

			string documentId = "TestDocument";

			string documentIdReferenceInline = "TestDocument1";

			string documentIdReferenceInlineInner = "TestDocument2";


			var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager();

            

			dynamic config = managerConfig.CreateItem(configId);

		    managerConfig.DeleteItem(config);

            managerConfig.MergeItem(config);

			var managerDocument = new ManagerFactoryConfiguration(configId).BuildDocumentManager();


			IndexApi.RebuildIndex(configId, documentId);

			var documentMetadata1 = managerDocument.CreateItem(documentId);

			documentMetadata1.Schema = new DynamicWrapper();
			documentMetadata1.Schema.Name = documentId;
			documentMetadata1.Schema.Caption = documentId;
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
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.ConfigId = configId;
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.DocumentId = documentIdReferenceInline;
			documentMetadata1.Schema.Properties.Address.TypeInfo.DocumentLink.Inline = true;

			documentMetadata1.Schema.Properties.ObservationDate = new DynamicWrapper();
			documentMetadata1.Schema.Properties.ObservationDate.Type = "DateTime";
			documentMetadata1.Schema.Properties.ObservationDate.Caption = "Дата осмотра";

			documentMetadata1.Schema.Properties.RecursiveLink = new DynamicWrapper();
			documentMetadata1.Schema.Properties.RecursiveLink.Type = "Object";
			documentMetadata1.Schema.Properties.RecursiveLink.TypeInfo = new DynamicWrapper();
			documentMetadata1.Schema.Properties.RecursiveLink.TypeInfo.DocumentLink = new DynamicWrapper();
			documentMetadata1.Schema.Properties.RecursiveLink.TypeInfo.DocumentLink.ConfigId = configId;
			documentMetadata1.Schema.Properties.RecursiveLink.TypeInfo.DocumentLink.DocumentId = documentId;


			var documentMetadata2 = managerDocument.CreateItem(documentIdReferenceInline);

			documentMetadata2.Schema = new DynamicWrapper();
			documentMetadata2.Schema.Type = documentIdReferenceInline;
			documentMetadata2.Schema.Caption = documentIdReferenceInline;
			documentMetadata2.Schema.Properties = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Id.Type = "Uuid";
			documentMetadata2.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata2.Schema.Properties.Street = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.Type = "Object";
			documentMetadata2.Schema.Properties.Street.Caption = "Address street";
			documentMetadata2.Schema.Properties.Street.TypeInfo = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.TypeInfo.DocumentLink = new DynamicWrapper();
			documentMetadata2.Schema.Properties.Street.TypeInfo.DocumentLink.ConfigId = configId;
			documentMetadata2.Schema.Properties.Street.TypeInfo.DocumentLink.DocumentId = documentIdReferenceInlineInner;
			documentMetadata2.Schema.Properties.Street.TypeInfo.DocumentLink.Inline = true;

			var documentMetadata3 = managerDocument.CreateItem(documentIdReferenceInlineInner);

			documentMetadata3.Schema = new DynamicWrapper();
			documentMetadata3.Schema.Type = documentIdReferenceInlineInner;
			documentMetadata3.Schema.Caption = documentIdReferenceInlineInner;
			documentMetadata3.Schema.Properties = new DynamicWrapper();
			documentMetadata3.Schema.Properties.Id = new DynamicWrapper();
			documentMetadata3.Schema.Properties.Id.Type = "Uuid";
			documentMetadata3.Schema.Properties.Id.Caption = "Unique identifier";

			documentMetadata3.Schema.Properties.House = new DynamicWrapper();
			documentMetadata3.Schema.Properties.House.Type = "String";
			documentMetadata3.Schema.Properties.House.Caption = "House";

			documentMetadata3.Schema.Properties.PostIndex = new DynamicWrapper();
			documentMetadata3.Schema.Properties.PostIndex.Type = "String";
			documentMetadata3.Schema.Properties.PostIndex.Caption = "Post index";


			//Структура
			// TestDocument
			//  |---TestDocument1
			//     |-- TestDocument2



            managerDocument.MergeItem(documentMetadata1);
            managerDocument.MergeItem(documentMetadata2);
            managerDocument.MergeItem(documentMetadata3);

			RestQueryApi.QueryPostNotify(configId);

			UpdateApi.UpdateStore(configId);

			//указываем ссылку на тестовый сценарий комплексного предзаполнения
			var scenarioManager = new ManagerFactoryDocument(configId, documentId).BuildScenarioManager();
			string scenarioId = "TestComplexFillDocumentAction";
			dynamic scenarioItem = scenarioManager.CreateItem(scenarioId);
			scenarioItem.ScenarioId = scenarioId;
			scenarioItem.Id = scenarioId;
			scenarioItem.Name = scenarioId;
			scenarioItem.ScriptUnitType = ScriptUnitType.Action;
			scenarioItem.ContextType = ContextTypeKind.ApplyMove;

            scenarioManager.MergeItem(scenarioItem);

			//добавляем ссылку на сборку, в которой находится прикладной модуль

			var assemblyManager = new ManagerFactoryConfiguration(configId).BuildAssemblyManager();
			dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);


			//Описываем схему предзаполнения в умолчательном бизнес-процессе
			var processManager = new ManagerFactoryDocument(configId, documentId).BuildProcessManager();
			var process = processManager.CreateItem("Default");

			process.Type = WorkflowTypes.WithoutState;
			process.Transitions = new List<dynamic>();
			process.Transitions.Add(new DynamicWrapper());
			process.Transitions[0].Id = Guid.NewGuid().ToString();
			process.Transitions[0].SchemaPrefill = documentMetadata1.Schema;
		    process.Transitions[0].SchemaPrefill.Properties = new DynamicWrapper();
            process.Transitions[0].SchemaPrefill.Properties.Name = new DynamicWrapper();
			process.Transitions[0].SchemaPrefill.Properties.Name.DefaultValue = "ИВАНОВ";
		    process.Transitions[0].SchemaPrefill.Properties.ObservationDate = new DynamicWrapper();
			process.Transitions[0].SchemaPrefill.Properties.ObservationDate.DefaultValue = "prefilldatetimenow";

			process.Transitions[0].SchemaPrefill = process.Transitions[0].SchemaPrefill.ToString();

			process.Transitions[0].ActionPoint = new DynamicWrapper();
			process.Transitions[0].ActionPoint.TypeName = "Action";
			process.Transitions[0].ActionPoint.ScenarioId = scenarioId;

            processManager.MergeItem(process);

			var package = new PackageBuilder().BuildPackage(configId, "test_version", GetType().Assembly.Location);
			UpdateApi.InstallPackages(new[] { package });


			RestQueryApi.QueryPostNotify(configId);

			UpdateApi.UpdateStore(configId);


			//вызываем предзаполнение
			dynamic item = new DocumentApi().CreateDocument(configId, documentId);

			Assert.IsNotNull(item);
			Assert.AreEqual(item.Name, "ИВАНОВ");
			Assert.IsNotNull(item.ObservationDate);

			Assert.AreEqual(item.PrefiledField, "TestValue");
		}
	}
}