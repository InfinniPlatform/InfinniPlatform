using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Api.Tests.Builders;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Api.Tests.RestBehavior.ConfiguratorApiBehavior
{
    public class CrudSettings
    {
        private readonly string _metadataType;

        public CrudSettings(string metadataType)
        {
            _metadataType = metadataType;
        }

        public const string ConfigurationFirstId = "TestConfigurationCRUD";

        public const string ConfigurationSecondId = "TestConfigurationCRUD1";

        public const string ConfigurationDescription = "Тестовая конфигурация";

        public string MetadataType
        {
            get { return _metadataType; }
        }

        public string FirstMetadataName { get; set; }

        public string SecondMetadataName { get; set; }

        public string FirstMetadataId { get; set; }

        public string SecondMetadataId { get; set; }

        public Func<string, string, dynamic> BuildInstanceAction { get; set; }

        public Action<CrudSettings> AdditionalOperationCheck { get; set; }

        public dynamic FirstInstance
        {
            get { return BuildInstanceAction(FirstMetadataId, FirstMetadataName); }
        }

        public dynamic SecondInstance
        {
            get { return BuildInstanceAction(SecondMetadataId, SecondMetadataName); }
        }

        public Func<IDataReader> Reader { get; set; }

        public override string ToString()
        {
            return _metadataType;
        }


        public ManagerFactoryDocument MetadataFactoryDocument(string documentId)
        {
            return new ManagerFactoryDocument(ConfigurationFirstId, documentId);
        }

        public Func<IDataManager> Manager { get; set; }

        public Action InitTest { get; set; }

        public void CheckAdditionalMetadataOperations()
        {
            if (AdditionalOperationCheck != null)
            {
                AdditionalOperationCheck(this);
            }
        }

        public static dynamic BuildTestConfig(string configUid, string configName)
        {
			var managerConfig = ManagerFactoryConfiguration.BuildConfigurationManager();
			dynamic existConfig = managerConfig.CreateItem(configName);
	        existConfig.Id = configUid;
			existConfig.Name = configName;

			managerConfig.DeleteItem(existConfig);
			managerConfig.MergeItem(existConfig);

	        return existConfig;
        }

    }

	[TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class MetadataCrudBehavior
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

		


        private static string ConfigurationFirstUid = "C306107D-F279-489F-A251-262D5445AAD1";

        private static string ConfigurationSecondUiod = "C37E7AF6-B460-45D3-80C4-9767BFC308B1";

        private static readonly CrudSettings[] CrudOperationSettings =
			{
				new CrudSettings(MetadataType.Configuration)
					{
						FirstMetadataId = ConfigurationFirstUid ,
						FirstMetadataName = CrudSettings.ConfigurationFirstId,
						SecondMetadataId = ConfigurationSecondUiod,
						SecondMetadataName = CrudSettings.ConfigurationSecondId,
						BuildInstanceAction = (metadataId, metadataName) =>
							                      {
								                    
						       return CrudSettings.BuildTestConfig(metadataId, metadataName);

						    },
						Reader = () => new MetadataReaderConfiguration(),
						Manager = () => ManagerFactoryConfiguration.BuildConfigurationManager(),						
					},
				new CrudSettings(MetadataType.Menu)
					{
						FirstMetadataId = "4306107D-F279-489F-A251-262D5445AAD5",
						FirstMetadataName = "TestMenu",
						SecondMetadataId = "B37E7AF6-B460-45D3-80C4-9767BFC308BE",
						SecondMetadataName = "TestMenu1",
						BuildInstanceAction = (metadataId,metadataName) => new DynamicWrapper().BuildSampleMenu(metadataName, metadataId),
						Reader = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildMenuMetadataReader(),
						Manager = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildMenuManager(),	
						AdditionalOperationCheck = null,
						InitTest = () => CrudSettings.BuildTestConfig(ConfigurationFirstUid,CrudSettings.ConfigurationFirstId)
					},
                new CrudSettings(MetadataType.Register)
					{
						FirstMetadataId = "4306107D-F279-489F-A251-262D5445AAD5",
						FirstMetadataName = "TestRegister",
						SecondMetadataId = "B37E7AF6-B460-45D3-80C4-9767BFC308BE",
						SecondMetadataName = "TestRegister1",
						BuildInstanceAction = (metadataId,metadataName) => new DynamicWrapper().BuildSampleRegister(metadataName, metadataId),
						Reader = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildRegisterMetadataReader(),
						Manager = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildRegisterManager(),	
						AdditionalOperationCheck = null,
						InitTest = () => CrudSettings.BuildTestConfig(ConfigurationFirstUid,CrudSettings.ConfigurationFirstId)
					},
				new CrudSettings(MetadataType.Report)
					{
						FirstMetadataId = "4306107D-F279-489F-A251-262D5445AAD5",
						FirstMetadataName = "TestReport",
						SecondMetadataId = "B37E7AF6-B460-45D3-80C4-9767BFC308BE",
						SecondMetadataName = "TestReport1",
						BuildInstanceAction = (metadataId,metadataName) => new DynamicWrapper().BuildSampleReport(metadataName, metadataId),
						Reader = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildReportMetadataReader(),
						Manager = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildReportManager(),	
						AdditionalOperationCheck = null,
                        InitTest = () => CrudSettings.BuildTestConfig(ConfigurationFirstUid,CrudSettings.ConfigurationFirstId)
					},
				new CrudSettings(MetadataType.Assembly)
					{
						FirstMetadataId = "457BAE50-A68A-40B8-BD48-719D086EE708",
						FirstMetadataName = "TestAssembly",
						SecondMetadataId = "C4608DB0-73E3-4CDC-B0E6-2A8396CEBDB5",
						SecondMetadataName = "TestAssembly1",
						BuildInstanceAction = (metadataId,metadataName) => new DynamicWrapper().BuildSampleAssembly(metadataName, metadataId),
						Reader = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildAssemblyMetadataReader(),
						Manager = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildAssemblyManager(),	
						AdditionalOperationCheck = null,
						InitTest = () => CrudSettings.BuildTestConfig(ConfigurationFirstUid,CrudSettings.ConfigurationFirstId)
					},
				new CrudSettings(MetadataType.Document)
					{
						FirstMetadataId = "838C8086-845E-47E8-A625-16531F9EDC0F",
						FirstMetadataName = "TestDocument",
						SecondMetadataId = "F4C963EC-C764-4F16-9706-C2176FEDA7FC",
						SecondMetadataName = "TestDocument1",
						BuildInstanceAction = (metadataId,metadataName) => SampleMetadataBuilder.BuildEmptyDocument(metadataName, metadataId),
						Reader = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildDocumentMetadataReader(),
						Manager = () => new ManagerFactoryConfiguration(CrudSettings.ConfigurationFirstId).BuildDocumentManager(),
						AdditionalOperationCheck = settings =>
							                           {
								                           var nameView = CheckAddView(settings);
								                           CheckDeleteView(settings, nameView);
														   var name= CheckAddScenario(settings);
														   CheckDeleteScenario(settings,name);
														   var servicename = CheckAddService(settings);
														   CheckDeleteService(settings,servicename);
														   var nameProcess = CheckAddProcess(settings);
														   CheckDeleteProcess(settings,nameProcess);
														   var nameWarnings = CheckAddValidationWarning(settings);
														   CheckDeleteValidationWarning(settings,nameWarnings);
														   var nameErrors = CheckAddValidationError(settings);
                                                           CheckDeleteValidationError(settings, nameErrors);
                                                           var nameStatus = CheckAddStatus(settings);
                                                           CheckDeleteStatus(settings, nameStatus);
													   },
						InitTest = () => CrudSettings.BuildTestConfig(ConfigurationFirstUid,CrudSettings.ConfigurationFirstId)
					}
			};


        [TestCaseSource("CrudOperationSettings")]
        public void ShouldCrudOperationMetadata(CrudSettings settings)
        {
            if (settings.InitTest != null)
            {
                settings.InitTest();
            }

            var manager = settings.Manager();
            var reader = settings.Reader();

            //создаем метаданные объекта
            manager.MergeItem(settings.FirstInstance);

            //получаем созданный объект метаданных
            IEnumerable<dynamic> metadataList = reader.GetItems().ToList();

            //проверяем, что метаданные действительно созданы
            Assert.IsNotNull(metadataList);

            var countBeforeupdate = metadataList.Count();
            Assert.True(countBeforeupdate > 0);

            Assert.True(metadataList.Any(m => m.Name == settings.FirstMetadataName));

            //изменяем существующие метаданные объекта
            dynamic updatedMetadata = settings.FirstInstance;
            string description = "Изменение тестового объекта метаданных";
            updatedMetadata.Description = description;

            manager.MergeItem(updatedMetadata);

            //ищем метаданные с указанным наименованием, проверяем что свойство действительно изменилось
            var metadata = reader.GetItem(settings.FirstMetadataName);
            Assert.AreEqual(metadata.Description, description);

            //добавляем еще один объект метаданных
            metadata = settings.SecondInstance;
            manager.MergeItem(metadata);

            //проверяем дополнительные зарегистрированные операции метаданных для данного типа
            settings.CheckAdditionalMetadataOperations();

            //проверяем, что метаданные создались и старые не удалились
            metadataList = reader.GetItems();
            Assert.IsNotNull(metadataList.FirstOrDefault(m => m.Name == metadata.Name));

            var secondInstance = reader.GetItem(settings.SecondMetadataName);
            Assert.IsNotNull(secondInstance);
            //удаляем метаданные
            manager.DeleteItem(secondInstance);

            //ищем метаданные с указанным наименованием
            metadata = reader.GetItem(settings.SecondMetadataName);

            //проверяем, что метаданные с таким наименованием не найдены
            Assert.IsNull(metadata);

            //проверяем, что не удалился первый документ 
            metadata = reader.GetItem(settings.FirstMetadataName);

            Assert.IsNotNull(metadata);

        }

		private static string CheckAddView(CrudSettings settings)
		{
			dynamic view = new DynamicWrapper();
			view.Id = Guid.NewGuid().ToString();
			view.Name = "TestView1";
			view.ScriptUnitType = ScriptUnitType.Action;

			dynamic layoutPanel = new DynamicWrapper();
			layoutPanel.Id = Guid.NewGuid().ToString();
			layoutPanel.Schema = new DynamicWrapper();
			layoutPanel.Schema.TestProperty = 1;
			view.LayoutPanel = layoutPanel;

			dynamic childViews = new List<dynamic>();
			childViews.Add(new DynamicWrapper());
			childViews[0].Name = "test";
			childViews[0].TestPropertyChildViews = 2;
			view.ChildViews = childViews;


			var managerView = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildViewManager();

			managerView.InsertItem(view);

			IEnumerable<dynamic> views = managerView.MetadataReader.GetItems().ToEnumerable();
			var viewMetadata = managerView.MetadataReader.GetItem("TestView1");

			Assert.True(views.Any(sc => sc.Name == view.Name));
			
			Assert.IsNotNull(viewMetadata);

			var item = managerView.MetadataReader.GetItem("TestView1");
			Assert.IsNotNull(item.ChildViews);
			Assert.IsNotNull(item.LayoutPanel);

			Assert.AreEqual(item.ChildViews[0].Name, "test");
			Assert.IsNotNull(item.LayoutPanel.Schema.TestProperty);
			return view.Name;
		}

		private static void CheckDeleteView(CrudSettings settings, string nameView)
		{
			
		}


        private static string CheckAddScenario(CrudSettings settings)
        {
            dynamic scenario = new DynamicWrapper();
            scenario.Id = Guid.NewGuid().ToString();
            scenario.Name = "TestScenario1";
            scenario.ScriptUnitType = ScriptUnitType.Action;

            var managerScenario = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildScenarioManager();

            managerScenario.InsertItem(scenario);

            IEnumerable<dynamic> scenarios = managerScenario.MetadataReader.GetItems().ToEnumerable();
            var scenarioMetadata = managerScenario.MetadataReader.GetItem("TestScenario1");

            Assert.True(scenarios.Any(sc => sc.Name == scenario.Name));
            Assert.IsNotNull(scenarioMetadata);

            return scenario.Name;
        }

        private static void CheckDeleteScenario(CrudSettings settings, string scenarioName)
        {
            var managerScenario = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildScenarioManager();

            var scenario = managerScenario.MetadataReader.GetItem(scenarioName);

            managerScenario.DeleteItem(scenario);

            var documentMetadata = managerScenario.MetadataReader.GetItem(settings.FirstMetadataName);
            IEnumerable<dynamic> scenarios = DynamicWrapperExtensions.ToEnumerable(managerScenario.MetadataReader.GetItems());

            Assert.AreEqual(0, scenarios.Count(sc => sc.Name == scenarioName));
            Assert.IsNull(documentMetadata);

        }

        private static string CheckAddService(CrudSettings settings)
        {
            dynamic service = new DynamicWrapper();
            service.Id = Guid.NewGuid().ToString();
            service.Name = "TestService1";
            service.Type = new DynamicWrapper();
            service.Type.Name = "ApplyEvents";

            var managerService = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildServiceManager();

            managerService.MergeItem(service);


            IEnumerable<dynamic> services =
                DynamicWrapperExtensions.ToEnumerable(managerService.MetadataReader.GetItems());
            var serviceMetadata = managerService.MetadataReader.GetItem("TestService1");

            Assert.True(services.Any(sc => sc.Name == service.Name));
            Assert.IsNotNull(serviceMetadata);

            return service.Name;
        }

        private static void CheckDeleteService(CrudSettings settings, string serviceName)
        {
            var managerService = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildServiceManager();

            var service = managerService.MetadataReader.GetItem(serviceName);

            managerService.DeleteItem(service);

            var documentMetadata = managerService.MetadataReader.GetItem(settings.FirstMetadataName);
            IEnumerable<dynamic> services = DynamicWrapperExtensions.ToEnumerable(managerService.MetadataReader.GetItems());

            Assert.AreEqual(0, services.Count(sc => sc.Name == serviceName));
            Assert.IsNull(documentMetadata);

        }

        private static string CheckAddProcess(CrudSettings settings)
        {
            dynamic process = new DynamicWrapper();
            process.Id = Guid.NewGuid().ToString();
            process.Name = "TestProcess1";

            var managerProcess = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildProcessManager();

            managerProcess.InsertItem(process);


            IEnumerable<dynamic> processes =
                DynamicWrapperExtensions.ToEnumerable(managerProcess.MetadataReader.GetItems());
            var processMetadata = managerProcess.MetadataReader.GetItem("TestProcess1");

            Assert.True(processes.Any(sc => sc.Name == process.Name));
            Assert.IsNotNull(processMetadata);

            return process.Name;
        }

        private static void CheckDeleteProcess(CrudSettings settings, string processName)
        {
            var managerProcess = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildProcessManager();

            var process = managerProcess.MetadataReader.GetItem(processName);

            managerProcess.DeleteItem(process);

            var documentMetadata = managerProcess.MetadataReader.GetItem(settings.FirstMetadataName);
            IEnumerable<dynamic> processes = DynamicWrapperExtensions.ToEnumerable(managerProcess.MetadataReader.GetItems());

            Assert.AreEqual(0, processes.Count(sc => sc.Name == processName));
            Assert.IsNull(documentMetadata);

        }

        private static string CheckAddValidationWarning(CrudSettings settings)
        {
            dynamic validationWarning = new DynamicWrapper();
            validationWarning.Id = Guid.NewGuid().ToString();
            validationWarning.Name = "TestValidationWarning";

            var manager = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildValidationWarningsManager();

            manager.InsertItem(validationWarning);

            IEnumerable<dynamic> validationWarnings =
                DynamicWrapperExtensions.ToEnumerable(manager.MetadataReader.GetItems());
            var metadata = manager.MetadataReader.GetItem("TestValidationWarning");

            Assert.True(validationWarnings.Any(sc => sc.Name == validationWarning.Name));
            Assert.IsNotNull(metadata);

            return validationWarning.Name;
        }

        private static void CheckDeleteValidationWarning(CrudSettings settings, string validationName)
        {
            var managerValidation = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildValidationWarningsManager();

            var validator = managerValidation.MetadataReader.GetItem(validationName);
            managerValidation.DeleteItem(validator);

            var documentMetadata = managerValidation.MetadataReader.GetItem(settings.FirstMetadataName);
            IEnumerable<dynamic> validationWarnings = DynamicWrapperExtensions.ToEnumerable(managerValidation.MetadataReader.GetItems());

            Assert.AreEqual(0, validationWarnings.Count(sc => sc.Name == validationName));
            Assert.IsNull(documentMetadata);

        }

        private static string CheckAddValidationError(CrudSettings settings)
        {
            dynamic validationError = new DynamicWrapper();
            validationError.Id = Guid.NewGuid().ToString();
            validationError.Name = "TestValidationError";

            var manager = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildValidationErrorsManager();
            manager.InsertItem(validationError);

            IEnumerable<dynamic> validationErrors =
                DynamicWrapperExtensions.ToEnumerable(manager.MetadataReader.GetItems());
            var metadata = manager.MetadataReader.GetItem("TestValidationError");

            Assert.True(validationErrors.Any(sc => sc.Name == validationError.Name));
            Assert.IsNotNull(metadata);

            return validationError.Name;
        }

        private static void CheckDeleteValidationError(CrudSettings settings, string validationName)
        {
            var managerValidation = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildValidationErrorsManager();

            var validator = managerValidation.MetadataReader.GetItem(validationName);

            managerValidation.DeleteItem(validator);


            IEnumerable<dynamic> validationErrors = DynamicWrapperExtensions.ToEnumerable(managerValidation.MetadataReader.GetItems());
            var validationMetadata = managerValidation.MetadataReader.GetItem(settings.FirstMetadataName);

            Assert.AreEqual(0, validationErrors.Count(sc => sc.Name == validationName));
            Assert.IsNull(validationMetadata);

        }

        private static string CheckAddStatus(CrudSettings settings)
        {
            dynamic status = new DynamicWrapper();
            status.Id = Guid.NewGuid().ToString();
            status.Name = "Status1";
            status.Caption = "Первый статус";
            status.Description = "Описание первого статуса";

            var managerStatus = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildStatusManager();

            managerStatus.MergeItem(status);

            IEnumerable<dynamic> statuses =
                DynamicWrapperExtensions.ToEnumerable(managerStatus.MetadataReader.GetItems());
            var statusMetadata = managerStatus.MetadataReader.GetItem(status.Name);

            Assert.True(statuses.Any(sc => sc.Name == status.Name));
            Assert.IsNotNull(statusMetadata);

            return status.Name;
        }

        private static void CheckDeleteStatus(CrudSettings settings, string statusName)
        {
            var managerStatus = settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildStatusManager();

            var status = managerStatus.MetadataReader.GetItem(statusName);

            managerStatus.DeleteItem(status);

            var documentMetadata = managerStatus.MetadataReader.GetItem(settings.FirstMetadataName);
            IEnumerable<dynamic> processes = DynamicWrapperExtensions.ToEnumerable(managerStatus.MetadataReader.GetItems());

            Assert.AreEqual(0, processes.Count(sc => sc.Name == statusName));
            Assert.IsNull(documentMetadata);

        }

    }
}
