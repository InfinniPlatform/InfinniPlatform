﻿using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Api.Validation.Serialization;
using InfinniPlatform.Api.Validation.ValidationBuilders;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class ValidationDocumentBehavior
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

		[TestCase(true, true, false, false, false)]
		[TestCase(true, true, false, true, false)]
		[TestCase(true, false, false, true, true)]
		[TestCase(false, false, false, false, true)]
		[TestCase(false, true, false, false, false)]
		[TestCase(true, false, false, false, false)]
		[TestCase(false, false, true, false, false)]
		[TestCase(true, false, true, true, false)]
		public void ShouldValidateDocumentOnSet(bool addWarnings, bool addErrors, bool addComplexErrors, bool ignoreWarnings, bool resultValidation)
		{
			var document = new
			{
				Id = Guid.NewGuid(),
				LastName = "123",
			};

			CreateTestConfig(addWarnings, addErrors, addComplexErrors);

		    if (resultValidation)
		    {
		        dynamic result = new DocumentApi().SetDocument("testconfigvalidator", "testdoc1", document, ignoreWarnings);
		        Assert.IsTrue(result.IsValid);
		    }
		    else
		    {
		        try
		        {
                    new DocumentApi().SetDocument("testconfigvalidator", "testdoc1", document, ignoreWarnings);
		        }
		        catch (ArgumentException e)
		        {
		            Assert.IsTrue(e.Message.Contains("\"IsValid\":false"));
		        }
		    }
		}

		[Test]
		public void ShouldValidateDocumentOnEmptyDocument()
		{
			var document = new
			{

			};

			CreateTestConfig(true, true, true);

		    try
		    {
                new DocumentApi().SetDocument("testconfigvalidator", "testdoc1", document, false);
		    }
		    catch (Exception e)
		    {
                Assert.IsTrue(e.Message.Contains("\"IsValid\":false"));
		    }
		}

		[Test]
		public void ShouldFillValidationMessageOnException()
		{
			var document = new
			{
				Id = Guid.NewGuid(),
				LastName = "123",
			};

			CreateTestConfig(false, false, false, true);

			try
			{
				new DocumentApi().SetDocument("testconfigvalidator", "testdoc1", document, false);
			}
			catch (Exception e)
			{
				Assert.True(e.Message.ToLowerInvariant().Contains("exception"));
				return;
			}
			Assert.Fail("Should be an exception throws");
		}

		private void CreateTestConfig(bool addWarnings, bool addErrors, bool addComplexErrors, bool addLogicException = false)
		{

			string configurationId = "testconfigvalidator";
			string documentId = "testdoc1";

			var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager();

			var config = managerConfiguration.CreateItem(configurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

			var managerDocument = new ManagerFactoryConfiguration(configurationId).BuildDocumentManager();
			dynamic documentMetadata1 = managerDocument.CreateItem(documentId);
            managerDocument.MergeItem(documentMetadata1);


			dynamic validationErrorOperator = DynamicWrapperExtensions.ToDynamic((object) (ValidationOperatorSerializer.Instance.Serialize(ValidationBuilder.ForObject(builder => builder.And(rules => rules
			    .IsEqual("LastName", "test", "Last name not satisfy")
			    .IsNotNullOrWhiteSpace("LastName",
			        "LastName is empty"))))));

			dynamic validationError = new DynamicWrapper();
			validationError.ValidationOperator = validationErrorOperator;
			validationError.Id = Guid.NewGuid().ToString();
			validationError.Name = "testvalidationError";

            dynamic validationWarningOperator = DynamicWrapperExtensions.ToDynamic(ValidationOperatorSerializer.Instance.Serialize(ValidationBuilder.ForObject(builder => builder.And(rules => rules
																			.IsEqual("LastName", "test", "Last name not satisfy")
																			.IsNotNullOrWhiteSpace("LastName",
																								   "LastName is empty")))));
			dynamic validationWarning = new DynamicWrapper();
			validationWarning.ValidationOperator = validationWarningOperator;
			validationWarning.Id = Guid.NewGuid().ToString();
			validationWarning.Name = "testvalidationWarning";


			if (addWarnings)
			{
				var managerWarning = new ManagerFactoryDocument(configurationId, documentId).BuildValidationWarningsManager();
                managerWarning.MergeItem(validationWarning);
			}

			if (addErrors)
			{
				var managerError = new ManagerFactoryDocument(configurationId, documentId).BuildValidationErrorsManager();
                managerError.MergeItem(validationError);
			}

			//добавляем бизнес-процесс по умолчанию
			var processManager = new ManagerFactoryDocument(configurationId, documentId).BuildProcessManager();
			var defaultProcess = processManager.CreateItem("Default");

			dynamic instance = new DynamicWrapper();
			instance.Name = "Default transition";
			instance.Type = (int) WorkflowTypes.WithoutState;
			defaultProcess.Transitions = new List<dynamic>();
			defaultProcess.Transitions.Add(instance);

			if (addErrors)
			{
				instance.ValidationRuleError = "testvalidationError";
			}
			if (addWarnings)
			{
				instance.ValidationRuleWarning = "testvalidationWarning";	
			}			

			if (addComplexErrors)
			{
				instance.ValidationPointError = new DynamicWrapper();
				instance.ValidationPointError.TypeName = "Validation";
				instance.ValidationPointError.ScenarioId = "testcomplexvalidator";
			}

			if (addLogicException)
			{
				instance.SuccessPoint = new DynamicWrapper();
				instance.SuccessPoint.TypeName = "Action";
				instance.SuccessPoint.ScenarioId = "testactionexception";
			}

            processManager.MergeItem(defaultProcess);

			RestQueryApi.QueryPostNotify(configurationId);

			UpdateApi.UpdateStore(configurationId);

			if (addComplexErrors)
			{


				//указываем ссылку на тестовый сценарий комплексного предзаполнения
				var scenarioManager = new ManagerFactoryDocument(configurationId, documentId).BuildScenarioManager();
				string scenarioId = "TestComplexValidator";
				dynamic scenarioItem = scenarioManager.CreateItem(scenarioId);
				scenarioItem.ScenarioId = scenarioId;
				scenarioItem.Id = scenarioId;
				scenarioItem.Name = scenarioId;
				scenarioItem.ScriptUnitType = ScriptUnitType.Validator;
				scenarioItem.ContextType = ContextTypeKind.ApplyMove;
                scenarioManager.MergeItem(scenarioItem);

				//добавляем ссылку на сборку, в которой находится прикладной модуль

				var assemblyManager = new ManagerFactoryConfiguration(configurationId).BuildAssemblyManager();
				dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
                assemblyManager.MergeItem(assemblyItem);

				var package = new PackageBuilder().BuildPackage(configurationId, "test_version", this.GetType().Assembly.Location);
				UpdateApi.InstallPackages(new[] { package });
			}
			RestQueryApi.QueryPostNotify(configurationId);

			UpdateApi.UpdateStore(configurationId);
		}


	}
}
