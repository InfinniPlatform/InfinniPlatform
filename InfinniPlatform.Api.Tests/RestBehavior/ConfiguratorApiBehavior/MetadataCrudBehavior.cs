using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Tests.Builders;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;
using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.ConfiguratorApiBehavior
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class MetadataCrudBehavior
    {
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            try
            {
                // TODO Тест работает только на чистой базе.
                var indexProvider = new MultipleTypeIndex();
                indexProvider.DeleteIndex("_all");
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }

            _server = InfinniPlatformInprocessHost.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }


        private static readonly string ConfigurationFirstUid = "C306107D-F279-489F-A251-262D5445AAD1";

        private static readonly string ConfigurationSecondUiod = "C37E7AF6-B460-45D3-80C4-9767BFC308B1";

        private static readonly CrudSettings[] CrudOperationSettings =
        {
            new CrudSettings(MetadataType.Configuration)
            {
                FirstMetadataId = ConfigurationFirstUid,
                FirstMetadataName = CrudSettings.ConfigurationFirstId,
                SecondMetadataId = ConfigurationSecondUiod,
                SecondMetadataName = CrudSettings.ConfigurationSecondId,
                BuildInstanceAction =
                    (metadataId, metadataName) => { return CrudSettings.BuildTestConfig(metadataId, metadataName); },
                Reader = () => new MetadataReaderConfiguration(null),
                Manager = () => ManagerFactoryConfiguration.BuildConfigurationManager(null)
            },
            new CrudSettings(MetadataType.Menu)
            {
                FirstMetadataId = "4306107D-F279-489F-A251-262D5445AAD5",
                FirstMetadataName = "TestMenu",
                SecondMetadataId = "B37E7AF6-B460-45D3-80C4-9767BFC308BE",
                SecondMetadataName = "TestMenu1",
                BuildInstanceAction =
                    (metadataId, metadataName) => new DynamicWrapper().BuildSampleMenu(metadataName, metadataId),
                Reader =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId)
                    .BuildMenuMetadataReader(),
                Manager =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId).BuildMenuManager(),
                AdditionalOperationCheck = null,
                InitTest =
                    () => CrudSettings.BuildTestConfig(ConfigurationFirstUid, CrudSettings.ConfigurationFirstId)
            },
            new CrudSettings(MetadataType.Register)
            {
                FirstMetadataId = "4306107D-F279-489F-A251-262D5445AAD5",
                FirstMetadataName = "TestRegister",
                SecondMetadataId = "B37E7AF6-B460-45D3-80C4-9767BFC308BE",
                SecondMetadataName = "TestRegister1",
                BuildInstanceAction =
                    (metadataId, metadataName) =>
                    new DynamicWrapper().BuildSampleRegister(metadataName, metadataId),
                Reader =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId)
                    .BuildRegisterMetadataReader(),
                Manager =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId)
                    .BuildRegisterManager(),
                AdditionalOperationCheck = null,
                InitTest =
                    () => CrudSettings.BuildTestConfig(ConfigurationFirstUid, CrudSettings.ConfigurationFirstId)
            },
            new CrudSettings(MetadataType.Report)
            {
                FirstMetadataId = "4306107D-F279-489F-A251-262D5445AAD5",
                FirstMetadataName = "TestReport",
                SecondMetadataId = "B37E7AF6-B460-45D3-80C4-9767BFC308BE",
                SecondMetadataName = "TestReport1",
                BuildInstanceAction =
                    (metadataId, metadataName) =>
                    new DynamicWrapper().BuildSampleReport(metadataName, metadataId),
                Reader =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId)
                    .BuildReportMetadataReader(),
                Manager =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId).BuildReportManager(),
                AdditionalOperationCheck = null,
                InitTest =
                    () => CrudSettings.BuildTestConfig(ConfigurationFirstUid, CrudSettings.ConfigurationFirstId)
            },
            new CrudSettings(MetadataType.Assembly)
            {
                FirstMetadataId = "457BAE50-A68A-40B8-BD48-719D086EE708",
                FirstMetadataName = "TestAssembly",
                SecondMetadataId = "C4608DB0-73E3-4CDC-B0E6-2A8396CEBDB5",
                SecondMetadataName = "TestAssembly1",
                BuildInstanceAction =
                    (metadataId, metadataName) =>
                    new DynamicWrapper().BuildSampleAssembly(metadataName, metadataId),
                Reader =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId)
                    .BuildAssemblyMetadataReader(),
                Manager =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId)
                    .BuildAssemblyManager(),
                AdditionalOperationCheck = null,
                InitTest =
                    () => CrudSettings.BuildTestConfig(ConfigurationFirstUid, CrudSettings.ConfigurationFirstId)
            },
            new CrudSettings(MetadataType.Document)
            {
                FirstMetadataId = "838C8086-845E-47E8-A625-16531F9EDC0F",
                FirstMetadataName = "TestDocument",
                SecondMetadataId = "F4C963EC-C764-4F16-9706-C2176FEDA7FC",
                SecondMetadataName = "TestDocument1",
                BuildInstanceAction =
                    (metadataId, metadataName) =>
                    SampleMetadataBuilder.BuildEmptyDocument(metadataName, metadataId),
                Reader =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId)
                    .BuildDocumentMetadataReader(),
                Manager =
                    () =>
                    new ManagerFactoryConfiguration(null, CrudSettings.ConfigurationFirstId)
                    .BuildDocumentManager(),
                AdditionalOperationCheck = settings =>
                                           {
                                               var nameView = CheckAddView(settings);
                                               CheckDeleteView(settings, nameView);
                                               var name = CheckAddScenario(settings);
                                               CheckDeleteScenario(settings, name);
                                               var servicename = CheckAddService(settings);
                                               CheckDeleteService(settings, servicename);
                                               var nameProcess = CheckAddProcess(settings);
                                               CheckDeleteProcess(settings, nameProcess);
                                               var nameWarnings = CheckAddValidationWarning(settings);
                                               CheckDeleteValidationWarning(settings, nameWarnings);
                                               var nameErrors = CheckAddValidationError(settings);
                                               CheckDeleteValidationError(settings, nameErrors);
                                           },
                InitTest =
                    () => CrudSettings.BuildTestConfig(ConfigurationFirstUid, CrudSettings.ConfigurationFirstId)
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
            var description = "Изменение тестового объекта метаданных";
            updatedMetadata.Description = description;

            manager.MergeItem(updatedMetadata);

            //ищем метаданные с указанным наименованием, проверяем что свойство действительно изменилось
            dynamic metadata = reader.GetItem(settings.FirstMetadataName);
            Assert.AreEqual(metadata.Description, description);

            //добавляем еще один объект метаданных
            metadata = settings.SecondInstance;
            manager.MergeItem(metadata);

            //проверяем дополнительные зарегистрированные операции метаданных для данного типа
            settings.CheckAdditionalMetadataOperations();

            //проверяем, что метаданные создались и старые не удалились
            metadataList = reader.GetItems();
            Assert.IsNotNull(metadataList.FirstOrDefault(m => m.Name == metadata.Name));

            dynamic secondInstance = reader.GetItem(settings.SecondMetadataName);
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


            var managerView =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildViewManager();

            managerView.InsertItem(view);

            var views = managerView.MetadataReader.GetItems().ToEnumerable();
            dynamic viewMetadata = managerView.MetadataReader.GetItem("TestView1");

            Assert.True(views.Any(sc => sc.Name == view.Name));

            Assert.IsNotNull(viewMetadata);

            dynamic item = managerView.MetadataReader.GetItem("TestView1");
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

            var managerScenario =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildScenarioManager();

            managerScenario.InsertItem(scenario);

            var scenarios = managerScenario.MetadataReader.GetItems().ToEnumerable();
            dynamic scenarioMetadata = managerScenario.MetadataReader.GetItem("TestScenario1");

            Assert.True(scenarios.Any(sc => sc.Name == scenario.Name));
            Assert.IsNotNull(scenarioMetadata);

            return scenario.Name;
        }

        private static void CheckDeleteScenario(CrudSettings settings, string scenarioName)
        {
            var managerScenario =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildScenarioManager();

            dynamic scenario = managerScenario.MetadataReader.GetItem(scenarioName);

            managerScenario.DeleteItem(scenario);

            dynamic documentMetadata = managerScenario.MetadataReader.GetItem(settings.FirstMetadataName);
            var scenarios =
                managerScenario.MetadataReader.GetItems().ToEnumerable();

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

            var managerService =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildServiceManager();

            managerService.MergeItem(service);


            var services =
                managerService.MetadataReader.GetItems().ToEnumerable();
            dynamic serviceMetadata = managerService.MetadataReader.GetItem("TestService1");

            Assert.True(services.Any(sc => sc.Name == service.Name));
            Assert.IsNotNull(serviceMetadata);

            return service.Name;
        }

        private static void CheckDeleteService(CrudSettings settings, string serviceName)
        {
            var managerService =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildServiceManager();

            dynamic service = managerService.MetadataReader.GetItem(serviceName);

            managerService.DeleteItem(service);

            dynamic documentMetadata = managerService.MetadataReader.GetItem(settings.FirstMetadataName);
            var services =
                managerService.MetadataReader.GetItems().ToEnumerable();

            Assert.AreEqual(0, services.Count(sc => sc.Name == serviceName));
            Assert.IsNull(documentMetadata);
        }

        private static string CheckAddProcess(CrudSettings settings)
        {
            dynamic process = new DynamicWrapper();
            process.Id = Guid.NewGuid().ToString();
            process.Name = "TestProcess1";

            var managerProcess =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildProcessManager();

            managerProcess.InsertItem(process);


            var processes =
                managerProcess.MetadataReader.GetItems().ToEnumerable();
            dynamic processMetadata = managerProcess.MetadataReader.GetItem("TestProcess1");

            Assert.True(processes.Any(sc => sc.Name == process.Name));
            Assert.IsNotNull(processMetadata);

            return process.Name;
        }

        private static void CheckDeleteProcess(CrudSettings settings, string processName)
        {
            var managerProcess =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildProcessManager();

            dynamic process = managerProcess.MetadataReader.GetItem(processName);

            managerProcess.DeleteItem(process);

            dynamic documentMetadata = managerProcess.MetadataReader.GetItem(settings.FirstMetadataName);
            var processes =
                managerProcess.MetadataReader.GetItems().ToEnumerable();

            Assert.AreEqual(0, processes.Count(sc => sc.Name == processName));
            Assert.IsNull(documentMetadata);
        }

        private static string CheckAddValidationWarning(CrudSettings settings)
        {
            dynamic validationWarning = new DynamicWrapper();
            validationWarning.Id = Guid.NewGuid().ToString();
            validationWarning.Name = "TestValidationWarning";

            var manager =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildValidationWarningsManager();

            manager.InsertItem(validationWarning);

            var validationWarnings =
                manager.MetadataReader.GetItems().ToEnumerable();
            dynamic metadata = manager.MetadataReader.GetItem("TestValidationWarning");

            Assert.True(validationWarnings.Any(sc => sc.Name == validationWarning.Name));
            Assert.IsNotNull(metadata);

            return validationWarning.Name;
        }

        private static void CheckDeleteValidationWarning(CrudSettings settings, string validationName)
        {
            var managerValidation =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildValidationWarningsManager();

            dynamic validator = managerValidation.MetadataReader.GetItem(validationName);
            managerValidation.DeleteItem(validator);

            dynamic documentMetadata = managerValidation.MetadataReader.GetItem(settings.FirstMetadataName);
            var validationWarnings =
                managerValidation.MetadataReader.GetItems().ToEnumerable();

            Assert.AreEqual(0, validationWarnings.Count(sc => sc.Name == validationName));
            Assert.IsNull(documentMetadata);
        }

        private static string CheckAddValidationError(CrudSettings settings)
        {
            dynamic validationError = new DynamicWrapper();
            validationError.Id = Guid.NewGuid().ToString();
            validationError.Name = "TestValidationError";

            var manager =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildValidationErrorsManager();
            manager.InsertItem(validationError);

            var validationErrors =
                manager.MetadataReader.GetItems().ToEnumerable();
            dynamic metadata = manager.MetadataReader.GetItem("TestValidationError");

            Assert.True(validationErrors.Any(sc => sc.Name == validationError.Name));
            Assert.IsNotNull(metadata);

            return validationError.Name;
        }

        private static void CheckDeleteValidationError(CrudSettings settings, string validationName)
        {
            var managerValidation =
                settings.MetadataFactoryDocument(settings.FirstMetadataName).BuildValidationErrorsManager();

            dynamic validator = managerValidation.MetadataReader.GetItem(validationName);

            managerValidation.DeleteItem(validator);


            var validationErrors =
                managerValidation.MetadataReader.GetItems().ToEnumerable();
            dynamic validationMetadata = managerValidation.MetadataReader.GetItem(settings.FirstMetadataName);

            Assert.AreEqual(0, validationErrors.Count(sc => sc.Name == validationName));
            Assert.IsNull(validationMetadata);
        }
    }
}