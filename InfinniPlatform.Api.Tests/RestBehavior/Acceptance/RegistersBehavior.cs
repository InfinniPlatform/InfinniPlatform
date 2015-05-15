using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    /// <summary>
    /// В тесте воспроизводится сценарий накопления информации по свободным
    /// койкам стационара с последующей агрегацией по различным критериям
    /// </summary>
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class RegistersBehavior
    {
        private static readonly Guid TestGuid = new Guid("194E44BD-3E55-4945-AEEF-8AB9BE51DC61");

        private const string ConfigurationId = "hospitalregisterconfiguration";
        private const string PatientMovementDocumentId = "patientmovement";
        private const string BedsRegistrationDocumentId = "bedsregistration";
        private const string AvailableBedsRegisterId = "availablebeds";
        private const string InfoRegisterId = "inforegister";

        private readonly Stopwatch _sw = new Stopwatch();

        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);

            _sw.Restart();


            CreateTestConfig();

            _sw.Stop();
            Console.WriteLine("Configuration creation time: " + _sw.ElapsedMilliseconds + " ms");

            _sw.Restart();

            AddTestDocuments();

            _sw.Stop();
            Console.WriteLine("Adding 15 documents: " + _sw.ElapsedMilliseconds + " ms");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldAggregateHospitalData()
        {
            // Тестовая конфигурация будет содержать документы двух типов: 
            // BedsRegistrationDocumentId (реестр коек) и PatientMovementDocumentId (движение пациентов по палате).
            // При сохранении документов этих типов, будут добавляться записи в регистр AvailableBedsRegisterId,
            // накапливающий изменения состояния коек в палатах.

            _sw.Restart();

            // Получение информации по занятности коек на 1 февраля 2014
            var aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId,
                new DateTime(2014, 2, 1), new[] {"Room", "Bed"});

            _sw.Stop();
            Console.WriteLine("Aggregation by date time: " + _sw.ElapsedMilliseconds + " ms");

            // Все койки свободны, ни одного пациента еще не было

            foreach (var bucket in aggregationInfo)
            {
                if (bucket.Room.Contains("54") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("2"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("3"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }
            }
            
            _sw.Restart();

            // Получение информации по наличию свободных коек по палатам на 12 августа 2014
            aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId,
                new DateTime(2014, 08, 12), new[] {"Room"});

            _sw.Stop();
            Console.WriteLine("Aggregation by date under Rooms time: " + _sw.ElapsedMilliseconds + " ms");

            // 2 пациента в палате 33, все койки палаты 54 свободны
            Assert.AreEqual(3, aggregationInfo.First(a => a.Room.Contains("54")).Value);
            Assert.AreEqual(1, aggregationInfo.First(a => a.Room.Contains("33")).Value);

            _sw.Restart();

            // Получение информации по наличию свободных коек с различными номерами на 12 августа 2014
            aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId,
                new DateTime(2014, 08, 12), new[] {"Bed"}, new[] {"Value"});

            _sw.Stop();
            Console.WriteLine("Aggregation by date under Beds time: " + _sw.ElapsedMilliseconds + " ms");

            // Койки с номером 1 свободны в обеих палатах, с номером 2 и 3 - только только в одной
            Assert.AreEqual(2, aggregationInfo.First(a => a.Bed.Contains("1")).Value);
            Assert.AreEqual(1, aggregationInfo.First(a => a.Bed.Contains("2")).Value);
            Assert.AreEqual(1, aggregationInfo.First(a => a.Bed.Contains("3")).Value);

            _sw.Restart();

            // Получение информации по изменению занятности коек между 12 августа и 14 августа 2014 года
            aggregationInfo = new RegisterApi().GetValuesBetweenDates(ConfigurationId, AvailableBedsRegisterId,
                new DateTime(2014, 08, 12), new DateTime(2014, 08, 14));

            _sw.Stop();
            Console.WriteLine("Aggregation between dates time: " + _sw.ElapsedMilliseconds + " ms");

            // За этот период освободилась первая койка в палате 33
            Assert.AreEqual(1, aggregationInfo.First(a => a.Room.Contains("33") && a.Bed.Contains("1")).Value);

            // Получение информации по типу регистратора
            aggregationInfo = new RegisterApi().GetValuesBуRegistrarType(ConfigurationId, AvailableBedsRegisterId,
                BedsRegistrationDocumentId);

            _sw.Stop();
            Console.WriteLine("Aggregation by registrar type time: " + _sw.ElapsedMilliseconds + " ms");

            // Были добавлены все койки
            foreach (var bucket in aggregationInfo)
            {
                if (bucket.Room.Contains("54") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("2"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("3"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }
            }

            // Получение информации по регистратору
            aggregationInfo = new RegisterApi().GetValuesBуRegistrar(ConfigurationId, AvailableBedsRegisterId,
                TestGuid.ToString());

            _sw.Stop();
            Console.WriteLine("Aggregation by registrar time: " + _sw.ElapsedMilliseconds + " ms");

            // В рамках этого перевода койка из палаты 54 освободилась, из палаты 33 была занята
            Assert.AreEqual(1, aggregationInfo.First(a => a.Room.Contains("54") && a.Bed.Contains("1")).Value);
            Assert.AreEqual(-1, aggregationInfo.First(a => a.Room.Contains("33") && a.Bed.Contains("3")).Value);

            // Получение информации по занятности коек на 18 августа 2014 года
            aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId,
                new DateTime(2014, 8, 18));

            // Все койки в палате 54 должны освободиться
            // Петров и Сидоров не были выписаны 18 августа 2014 года
            foreach (var bucket in aggregationInfo)
            {
                if (bucket.Room.Contains("54") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("2"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("3"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(0, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(0, bucket.Value);
                }
            }

            // Получение информации по занятности коек на 1 октября 2014 года
            aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId,
                new DateTime(2014, 10, 1));

            // Все койки должны освободиться
            foreach (var bucket in aggregationInfo)
            {
                if (bucket.Room.Contains("54") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("2"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("3"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }
            }

            // Получение агрегации с группировкой по месяцам

            aggregationInfo = new RegisterApi().GetValuesByPeriods(ConfigurationId, AvailableBedsRegisterId,
                DateTime.MinValue, DateTime.MaxValue, RegisterPeriod.Month);
            Assert.IsTrue(aggregationInfo.Any());

            // Тот же запрос с установленной временной зоной
            aggregationInfo = new RegisterApi().GetValuesByPeriods(ConfigurationId, AvailableBedsRegisterId,
                DateTime.MinValue, DateTime.MaxValue, RegisterPeriod.Month, null, null, "+05:00");
            Assert.IsTrue(aggregationInfo.Any());
        }

        [Test]
        public void ShouldCalculateTotals()
        {
            // Тестирование функционала таблицы итогов:
            // помещаем содержимое регистра в таблицу итогов (миграция поместит имеющиеся на данный момент записи в таблицу итогов)
            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "runmigration", null,
                new
                {
                    MigrationName = "CalculateTotalsForRegisters",
                    ConfigurationName = ConfigurationId
                });

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Иванов",
                OldRoom = "Палата 33",
                OldBed = "Койка 3",
                Date = DateTime.Now.AddYears(2)
            }, false, true);

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Петров",
                OldRoom = "Палата 33",
                OldBed = "Койка 2",
                Date = DateTime.Now.AddYears(2)
            }, false, true);

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Сидоров",
                OldRoom = "Палата 33",
                OldBed = "Койка 1",
                Date = DateTime.Now.AddYears(2)
            }, false, true);

            // В данном случае расчет будет произведен с учетом данных из таблицы итогов
            var aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId,
                DateTime.Now.AddYears(3));

            foreach (var bucket in aggregationInfo)
            {
                if (bucket.Room.Contains("54") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("2"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("54") && bucket.Bed.Contains("3"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(2, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(2, bucket.Value);
                }

                if (bucket.Room.Contains("33") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(2, bucket.Value);
                }
            }

            var docs = new RegisterApi().GetRegisterTotals(ConfigurationId, AvailableBedsRegisterId, DateTime.Now);

            // Три палаты по три койки в каждой
            Assert.GreaterOrEqual(docs.Count(), 6);
        }

        [Test]
        public void ShouldDeleteRegisterEntriesAfterDeletingDocument()
        {
            var testingDocumentGuid = Guid.NewGuid().ToString();

            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Info = true,
                Id = testingDocumentGuid,
                Room = "Палата 123",
                Bed = "Койка 321",
                Date = new DateTime(2214, 01, 01)
            });

            var docs = new RegisterApi().GetRegisterEntries(
                ConfigurationId,
                InfoRegisterId,
                f =>
                    f.AddCriteria(
                        c => c.Property(RegisterConstants.DocumentDateProperty).IsMoreThan(new DateTime(2213, 01, 01))),
                0, 1000);

            // Проверяем, что в регистр была помещена новая запись
            Assert.AreEqual(1, docs.Count());

            // Удаляем документ
            new DocumentApi().DeleteDocument(ConfigurationId, BedsRegistrationDocumentId, testingDocumentGuid);

            docs = new RegisterApi().GetRegisterEntries(
                ConfigurationId,
                InfoRegisterId,
                f =>
                    f.AddCriteria(
                        c => c.Property(RegisterConstants.DocumentDateProperty).IsMoreThan(new DateTime(2213, 01, 01)))
                , 0, 1000);

            // Проверяем, что соответствующая запись была удалена из регистра
            Assert.AreEqual(0, docs.Count());

        }

        [Test]
        public void ShouldAddOnlyOneRegisterEntryToInfoRegisterPerPeriod()
        {
            // Период для регистра сведений - 1 месяц, следовательно за каждый месяц будем иметь одну запись в регистре
            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Info = true,
                Id = Guid.NewGuid(),
                Room = "Палата 33",
                Bed = "Койка 1",
                Date = new DateTime(2114, 01, 01, 12, 1, 1)
            });

            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Info = true,
                Id = Guid.NewGuid(),
                Room = "Палата 33",
                Bed = "Койка 1",
                Date = new DateTime(2114, 01, 15, 1, 2, 3)
            });

            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Info = true,
                Id = Guid.NewGuid(),
                Room = "Палата 1",
                Bed = "Койка 3",
                Date = new DateTime(2114, 02, 01, 7, 6, 5)
            });

            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Info = true,
                Id = Guid.NewGuid(),
                Room = "Палата 1",
                Bed = "Койка 3",
                Date = new DateTime(2114, 02, 02, 4, 4, 4)
            });

            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Info = true,
                Id = Guid.NewGuid(),
                Room = "Палата 1",
                Bed = "Койка 3",
                Date = new DateTime(2114, 02, 04, 3, 2, 1)
            });

            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Info = true,
                Id = Guid.NewGuid(),
                Room = "Палата 1",
                Bed = "Койка 6",
                Date = new DateTime(2114, 03, 01, 3, 2, 1)
            });

            var docs = new DocumentApi().GetDocument(
                ConfigurationId,
                RegisterConstants.RegisterNamePrefix + InfoRegisterId,
                f =>
                    f.AddCriteria(
                        c => c.Property(RegisterConstants.DocumentDateProperty).IsMoreThan(new DateTime(2113, 01, 01)))
                , 0, 1000);

            // Должно добавиться толко три записи в регистр, каждая на определенный месяц
            Assert.AreEqual(3, docs.Count());
        }

        [Test]
        public void ShouldAggregateHospitalDataAfterMappingChanged()
        {
            // Добавляем новые койки
            
            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 6",
                Bed = "Койка 1",
                Date = new DateTime(2014, 01, 01)
            });
            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 6",
                Bed = "Койка 2",
                Date = new DateTime(2014, 01, 01)
            });
            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 6",
                Bed = "Койка 3",
                Date = new DateTime(2014, 01, 01)
            });
            
            // Получение информации по занятности коек на 1 февраля 2014
            var aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId,
                new DateTime(2014, 2, 1), new[] { "Room", "Bed" });
            
            // Все койки свободны, ни одного пациента еще не было

            foreach (var bucket in aggregationInfo)
            {
                if (bucket.Room.Contains("6") && bucket.Bed.Contains("1"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("6") && bucket.Bed.Contains("2"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }

                if (bucket.Room.Contains("6") && bucket.Bed.Contains("3"))
                {
                    Assert.AreEqual(1, bucket.Value);
                }
            }
            
            // Добавляем документ (схема данных не изменилась)

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Иванов",
                NewRoom = "Палата 6",
                NewBed = "Койка 1",
                Date = new DateTime(2014, 08, 10)
            });

            // Получение информации по наличию свободных коек по палатам на 12 августа 2014
            aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId,
                new DateTime(2014, 08, 12), new[] { "Room" });
            
            // Занята только первая койка палаты номер 6
            Assert.AreEqual(2, aggregationInfo.First(a => a.Room.Contains("6")).Value);

            // Меняем схему данных документа PatientMovementDocumentId: теперь номер койки становится числом
            
            var managerDocument = new ManagerFactoryConfiguration(ConfigurationId).BuildDocumentManager();
            
            var documentMetadata = managerDocument.CreateItem(BedsRegistrationDocumentId);

            dynamic schemaProperties = new DynamicWrapper();

            dynamic roomPropertyModel = new DynamicWrapper();
            roomPropertyModel.Type = DataType.String.ToString();
            roomPropertyModel.Caption = "Палата";
            schemaProperties.Room = roomPropertyModel;

            dynamic bedPropertyModel = new DynamicWrapper();
            bedPropertyModel.Type = DataType.Integer.ToString();
            bedPropertyModel.Caption = "Койка";
            schemaProperties.Bed = bedPropertyModel;

            dynamic datetimePropertyModel = new DynamicWrapper();
            datetimePropertyModel.Type = DataType.DateTime.ToString();
            datetimePropertyModel.Caption = "Дата";
            schemaProperties.Date = datetimePropertyModel;

            dynamic infoPropertyModel = new DynamicWrapper();
            infoPropertyModel.Type = DataType.Bool.ToString();
            infoPropertyModel.Caption = "Инфо";
            schemaProperties.Info = infoPropertyModel;

            documentMetadata.Schema = new
            {
                Type = "Object",
                Caption = "Beds register",
                Properties = schemaProperties
            }.ToDynamic();

            managerDocument.MergeItem(documentMetadata);

            // добавляем бизнес-процесс по умолчанию
            var processManager =
                new ManagerFactoryDocument(ConfigurationId, BedsRegistrationDocumentId).BuildProcessManager();
            var defaultProcess = processManager.CreateItem("Default");

            dynamic instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);

            instance.SuccessPoint = new DynamicWrapper();
            instance.SuccessPoint.TypeName = "Action";
            instance.SuccessPoint.ScenarioId = "AddNewBedToRoomMoveAction";
            instance.SuccessPoint.DocumentDateProperty = "Date";

            instance.DeletePoint = new DynamicWrapper();
            instance.DeletePoint.TypeName = "Action";
            instance.DeletePoint.ScenarioId = "DeleteRegisterEntriesAction";

            processManager.MergeItem(defaultProcess);

            // указываем ссылку на тестовый сценарий комплексного предзаполнения
            var scenarioManager =
                new ManagerFactoryDocument(ConfigurationId, BedsRegistrationDocumentId).BuildScenarioManager();

            const string addScenarioRegisterId = "AddNewBedToRoomMoveAction";
            dynamic scenarioRegisterItem = scenarioManager.CreateItem(addScenarioRegisterId);
            scenarioRegisterItem.ScenarioId = addScenarioRegisterId;
            scenarioRegisterItem.Id = addScenarioRegisterId;
            scenarioRegisterItem.Name = addScenarioRegisterId;
            scenarioRegisterItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioRegisterItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioRegisterItem);

            const string deleteRegisterEntriesId = "DeleteRegisterEntriesAction";
            dynamic scenarioDeleteRegisterItem = scenarioManager.CreateItem(deleteRegisterEntriesId);
            scenarioDeleteRegisterItem.ScenarioId = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.Id = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.Name = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioDeleteRegisterItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioDeleteRegisterItem);
            
            // добавляем ссылку на сборку, в которой находится прикладной модуль

            var assemblyManager = new ManagerFactoryConfiguration(ConfigurationId).BuildAssemblyManager();
            dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            // Обновляем регистр, который будет накапливать информацию о занятости коек

            var managerRegister = new ManagerFactoryConfiguration(ConfigurationId).BuildRegisterManager();

            var registerMetadata = managerRegister.CreateItem(AvailableBedsRegisterId);

            registerMetadata.Id = Guid.NewGuid().ToString();

            registerMetadata.Properties = new List<dynamic>();

            registerMetadata.Properties.Add(
                new { Property = RegisterConstants.RegistrarProperty, Type = RegisterPropertyType.Info });
            registerMetadata.Properties.Add(
                new { Property = RegisterConstants.RegistrarTypeProperty, Type = RegisterPropertyType.Info });
            registerMetadata.Properties.Add(
                new { Property = RegisterConstants.DocumentDateProperty, Type = RegisterPropertyType.Info });
            registerMetadata.Properties.Add(
                new { Property = RegisterConstants.EntryTypeProperty, Type = RegisterPropertyType.Info });
            registerMetadata.Properties.Add(new { Property = "Room", Type = RegisterPropertyType.Dimension });
            registerMetadata.Properties.Add(new { Property = "Bed", Type = RegisterPropertyType.Dimension });
            registerMetadata.Properties.Add(new { Property = "Value", Type = RegisterPropertyType.Value });
            registerMetadata.Period = RegisterPeriod.Month;

            registerMetadata.DocumentId = CreateRegisterDocuments(ConfigurationId, AvailableBedsRegisterId, true);

            managerRegister.MergeItem(registerMetadata);

            // Создаем регистр сведений
            var registerInfoMetadata = managerRegister.CreateItem(InfoRegisterId);

            registerInfoMetadata.Id = Guid.NewGuid().ToString();
            registerInfoMetadata.Period = RegisterPeriod.Month;

            registerInfoMetadata.Properties = new List<dynamic>();

            registerInfoMetadata.Properties.Add(
                new { Property = RegisterConstants.RegistrarProperty, Type = RegisterPropertyType.Info });
            registerInfoMetadata.Properties.Add(
                new { Property = RegisterConstants.RegistrarTypeProperty, Type = RegisterPropertyType.Info });
            registerInfoMetadata.Properties.Add(
                new { Property = RegisterConstants.DocumentDateProperty, Type = RegisterPropertyType.Info });
            registerInfoMetadata.Properties.Add(
                new { Property = RegisterConstants.EntryTypeProperty, Type = RegisterPropertyType.Info });
            registerInfoMetadata.Properties.Add(new { Property = "Room", Type = RegisterPropertyType.Dimension });
            registerInfoMetadata.Properties.Add(new { Property = "Bed", Type = RegisterPropertyType.Dimension });
            registerInfoMetadata.Properties.Add(new { Property = "Value", Type = RegisterPropertyType.Value });

            registerInfoMetadata.DocumentId = CreateRegisterDocuments(ConfigurationId, InfoRegisterId, true);

            managerRegister.MergeItem(registerInfoMetadata);

            var package = new PackageBuilder().BuildPackage(ConfigurationId, "test_version", GetType().Assembly.Location);

            UpdateApi.InstallPackages(new[] { package });

            UpdateApi.ForceReload(ConfigurationId);

            UpdateApi.UpdateStore(ConfigurationId);


            // Добавляем документ (схема данных изменилась)
            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Петров",
                NewRoom = "Палата 6",
                NewBed = 2,
                Date = new DateTime(2014, 08, 10)
            });
            
            
            aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId, new DateTime(2014, 09, 11), new[] { "Room" });

            // Заняты 2 койки палаты номер 6 (одна занята после проводки документа со старой схемой и одна после проводки документа с новой схемой)
            Assert.AreEqual(1, aggregationInfo.First(a => a.Room.Contains("6")).Value);

            // Возвращаем старую схему
            
            bedPropertyModel = new DynamicWrapper();
            bedPropertyModel.Type = DataType.String.ToString();
            bedPropertyModel.Caption = "Койка";
            schemaProperties.Bed = bedPropertyModel;
            
            documentMetadata.Schema = new
            {
                Type = "Object",
                Caption = "Beds register",
                Properties = schemaProperties
            }.ToDynamic();

            managerDocument.MergeItem(documentMetadata);

            // добавляем бизнес-процесс по умолчанию
            
            instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);

            instance.SuccessPoint = new DynamicWrapper();
            instance.SuccessPoint.TypeName = "Action";
            instance.SuccessPoint.ScenarioId = "AddNewBedToRoomMoveAction";
            instance.SuccessPoint.DocumentDateProperty = "Date";

            instance.DeletePoint = new DynamicWrapper();
            instance.DeletePoint.TypeName = "Action";
            instance.DeletePoint.ScenarioId = "DeleteRegisterEntriesAction";

            processManager.MergeItem(defaultProcess);

            // указываем ссылку на тестовый сценарий комплексного предзаполнения
            scenarioManager =
                new ManagerFactoryDocument(ConfigurationId, BedsRegistrationDocumentId).BuildScenarioManager();

            scenarioRegisterItem = scenarioManager.CreateItem(addScenarioRegisterId);
            scenarioRegisterItem.ScenarioId = addScenarioRegisterId;
            scenarioRegisterItem.Id = addScenarioRegisterId;
            scenarioRegisterItem.Name = addScenarioRegisterId;
            scenarioRegisterItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioRegisterItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioRegisterItem);

            scenarioDeleteRegisterItem = scenarioManager.CreateItem(deleteRegisterEntriesId);
            scenarioDeleteRegisterItem.ScenarioId = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.Id = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.Name = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioDeleteRegisterItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioDeleteRegisterItem);

            // добавляем ссылку на сборку, в которой находится прикладной модуль

            assemblyManager = new ManagerFactoryConfiguration(ConfigurationId).BuildAssemblyManager();
            assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            // Обновляем регистр, который будет накапливать информацию о занятости коек

            managerRegister = new ManagerFactoryConfiguration(ConfigurationId).BuildRegisterManager();

            registerMetadata = managerRegister.CreateItem(AvailableBedsRegisterId);

            registerMetadata.Id = Guid.NewGuid().ToString();

            registerMetadata.Properties = new List<dynamic>();

            registerMetadata.Properties.Add(
                new { Property = RegisterConstants.RegistrarProperty, Type = RegisterPropertyType.Info });
            registerMetadata.Properties.Add(
                new { Property = RegisterConstants.RegistrarTypeProperty, Type = RegisterPropertyType.Info });
            registerMetadata.Properties.Add(
                new { Property = RegisterConstants.DocumentDateProperty, Type = RegisterPropertyType.Info });
            registerMetadata.Properties.Add(
                new { Property = RegisterConstants.EntryTypeProperty, Type = RegisterPropertyType.Info });
            registerMetadata.Properties.Add(new { Property = "Room", Type = RegisterPropertyType.Dimension });
            registerMetadata.Properties.Add(new { Property = "Bed", Type = RegisterPropertyType.Dimension });
            registerMetadata.Properties.Add(new { Property = "Value", Type = RegisterPropertyType.Value });
            registerMetadata.Period = RegisterPeriod.Month;

            registerMetadata.DocumentId = CreateRegisterDocuments(ConfigurationId, AvailableBedsRegisterId);

            managerRegister.MergeItem(registerMetadata);

            // Создаем регистр сведений
            registerInfoMetadata = managerRegister.CreateItem(InfoRegisterId);

            registerInfoMetadata.Id = Guid.NewGuid().ToString();
            registerInfoMetadata.Period = RegisterPeriod.Month;

            registerInfoMetadata.Properties = new List<dynamic>();

            registerInfoMetadata.Properties.Add(
                new { Property = RegisterConstants.RegistrarProperty, Type = RegisterPropertyType.Info });
            registerInfoMetadata.Properties.Add(
                new { Property = RegisterConstants.RegistrarTypeProperty, Type = RegisterPropertyType.Info });
            registerInfoMetadata.Properties.Add(
                new { Property = RegisterConstants.DocumentDateProperty, Type = RegisterPropertyType.Info });
            registerInfoMetadata.Properties.Add(
                new { Property = RegisterConstants.EntryTypeProperty, Type = RegisterPropertyType.Info });
            registerInfoMetadata.Properties.Add(new { Property = "Room", Type = RegisterPropertyType.Dimension });
            registerInfoMetadata.Properties.Add(new { Property = "Bed", Type = RegisterPropertyType.Dimension });
            registerInfoMetadata.Properties.Add(new { Property = "Value", Type = RegisterPropertyType.Value });

            registerInfoMetadata.DocumentId = CreateRegisterDocuments(ConfigurationId, InfoRegisterId);

            managerRegister.MergeItem(registerInfoMetadata);

            package = new PackageBuilder().BuildPackage(ConfigurationId, "test_version", GetType().Assembly.Location);

            UpdateApi.InstallPackages(new[] { package });

            UpdateApi.ForceReload(ConfigurationId);

            UpdateApi.UpdateStore(ConfigurationId);
            
            // Добавляем документ (схема данных изменилась)
            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Петров",
                NewRoom = "Палата 6",
                NewBed = 10,
                Date = new DateTime(2014, 08, 10)
            });
            
            aggregationInfo = new RegisterApi().GetValuesByDate(ConfigurationId, AvailableBedsRegisterId, new DateTime(2014, 09, 11), new[] { "Room" });
            
            Assert.AreEqual(0, aggregationInfo.First(a => a.Room.Contains("6")).Value);
        }

        private void CreateTestConfig()
        {
            //пересоздаем таблицы данных
            //TODO: Необходимо реализовать механизм очистки существующих данных (архивирование?)
            IndexApi.RebuildIndex(ConfigurationId, BedsRegistrationDocumentId);
            IndexApi.RebuildIndex(ConfigurationId, PatientMovementDocumentId);
            IndexApi.RebuildIndex(ConfigurationId, AvailableBedsRegisterId);
            IndexApi.RebuildIndex(ConfigurationId, InfoRegisterId);
            IndexApi.RebuildIndex(ConfigurationId, RegisterConstants.RegisterNamePrefix + AvailableBedsRegisterId);
            IndexApi.RebuildIndex(ConfigurationId, RegisterConstants.RegisterNamePrefix + InfoRegisterId);
            IndexApi.RebuildIndex(ConfigurationId, RegisterConstants.RegisterTotalNamePrefix + AvailableBedsRegisterId);
            IndexApi.RebuildIndex(ConfigurationId, RegisterConstants.RegisterTotalNamePrefix + InfoRegisterId);
            IndexApi.RebuildIndex(ConfigurationId, ConfigurationId + RegisterConstants.RegistersCommonInfo);


            var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager();

            var config = managerConfiguration.CreateItem(ConfigurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            var managerDocument = new ManagerFactoryConfiguration(ConfigurationId).BuildDocumentManager();

            // Документ BedsRegistrationDocumentId позволяет добавить новую койку в определенную палату
            /* Поля документа
             * 
             * Room      - номер палаты
             * Bed       - номер койки
             */

            var documentMetadata = managerDocument.CreateItem(BedsRegistrationDocumentId);

            dynamic schemaProperties = new DynamicWrapper();

            dynamic roomPropertyModel = new DynamicWrapper();
            roomPropertyModel.Type = DataType.String.ToString();
            roomPropertyModel.Caption = "Палата";
            schemaProperties.Room = roomPropertyModel;

            dynamic bedPropertyModel = new DynamicWrapper();
            bedPropertyModel.Type = DataType.String.ToString();
            bedPropertyModel.Caption = "Койка";
            schemaProperties.Bed = bedPropertyModel;

            dynamic datetimePropertyModel = new DynamicWrapper();
            datetimePropertyModel.Type = DataType.DateTime.ToString();
            datetimePropertyModel.Caption = "Дата";
            schemaProperties.Date = datetimePropertyModel;

            dynamic infoPropertyModel = new DynamicWrapper();
            infoPropertyModel.Type = DataType.Bool.ToString();
            infoPropertyModel.Caption = "Инфо";
            schemaProperties.Info = infoPropertyModel;

            documentMetadata.Schema = new
            {
                Type = "Object",
                Caption = "Beds register",
                Properties = schemaProperties
            }.ToDynamic();

            managerDocument.MergeItem(documentMetadata);

            // добавляем бизнес-процесс по умолчанию
            var processManager =
                new ManagerFactoryDocument(ConfigurationId, BedsRegistrationDocumentId).BuildProcessManager();
            var defaultProcess = processManager.CreateItem("Default");

            dynamic instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);

            instance.SuccessPoint = new DynamicWrapper();
            instance.SuccessPoint.TypeName = "Action";
            instance.SuccessPoint.ScenarioId = "AddNewBedToRoomMoveAction";
            instance.SuccessPoint.DocumentDateProperty = "Date";

            instance.DeletePoint = new DynamicWrapper();
            instance.DeletePoint.TypeName = "Action";
            instance.DeletePoint.ScenarioId = "DeleteRegisterEntriesAction";

            processManager.MergeItem(defaultProcess);

            // указываем ссылку на тестовый сценарий комплексного предзаполнения
            var scenarioManager =
                new ManagerFactoryDocument(ConfigurationId, BedsRegistrationDocumentId).BuildScenarioManager();

            const string addScenarioRegisterId = "AddNewBedToRoomMoveAction";
            dynamic scenarioRegisterItem = scenarioManager.CreateItem(addScenarioRegisterId);
            scenarioRegisterItem.ScenarioId = addScenarioRegisterId;
            scenarioRegisterItem.Id = addScenarioRegisterId;
            scenarioRegisterItem.Name = addScenarioRegisterId;
            scenarioRegisterItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioRegisterItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioRegisterItem);

            const string deleteRegisterEntriesId = "DeleteRegisterEntriesAction";
            dynamic scenarioDeleteRegisterItem = scenarioManager.CreateItem(deleteRegisterEntriesId);
            scenarioDeleteRegisterItem.ScenarioId = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.Id = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.Name = deleteRegisterEntriesId;
            scenarioDeleteRegisterItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioDeleteRegisterItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioDeleteRegisterItem);

            // Документ PatientMovementDocumentId c движением пациентов описывает перевод пациента с одной койки на другую.

            /* Поля документа
             * 
             * PatientName  - имя пациента
             * OldRoom      - номер палаты, в которой пациент находился ранее
             * OldBed       - номер койки, на которой пациент находился ранее
             * NewRoom      - номер палаты, в которую переведен пациент
             * NewBed       - номер койки, на которую переведен пациент
             * Date         - дата перевода пациента
             */

            documentMetadata = managerDocument.CreateItem(PatientMovementDocumentId);

            schemaProperties = new DynamicWrapper();

            dynamic patientNamePropertyModel = new DynamicWrapper();
            patientNamePropertyModel.Type = DataType.String.ToString();
            patientNamePropertyModel.Caption = "Имя пациента";
            schemaProperties.PatientName = patientNamePropertyModel;

            dynamic oldRoomPropertyModel = new DynamicWrapper();
            oldRoomPropertyModel.Type = DataType.String.ToString();
            oldRoomPropertyModel.Caption = "Прежняя палата";
            schemaProperties.OldRoom = oldRoomPropertyModel;

            dynamic oldBedPropertyModel = new DynamicWrapper();
            oldBedPropertyModel.Type = DataType.String.ToString();
            oldBedPropertyModel.Caption = "Прежняя койка";
            schemaProperties.OldBed = oldBedPropertyModel;

            dynamic newRoomPropertyModel = new DynamicWrapper();
            newRoomPropertyModel.Type = DataType.String.ToString();
            newRoomPropertyModel.Caption = "Новая палата";
            schemaProperties.NewRoom = newRoomPropertyModel;

            dynamic newBedPropertyModel = new DynamicWrapper();
            newBedPropertyModel.Type = DataType.String.ToString();
            newBedPropertyModel.Caption = "Новая койка";
            schemaProperties.NewBed = newBedPropertyModel;

            dynamic datePropertyModel = new DynamicWrapper();
            datePropertyModel.Type = DataType.DateTime.ToString();
            datePropertyModel.Caption = "Дата перевода";
            schemaProperties.Date = datePropertyModel;

            documentMetadata.Schema = new
            {
                Type = "Object",
                Caption = "Patient movement document",
                Description = "Patient movement document schema",
                Properties = schemaProperties
            }.ToDynamic();

            managerDocument.MergeItem(documentMetadata);

            // добавляем бизнес-процесс по умолчанию
            processManager =
                new ManagerFactoryDocument(ConfigurationId, PatientMovementDocumentId).BuildProcessManager();
            defaultProcess = processManager.CreateItem("Default");

            instance = new DynamicWrapper();
            instance.Name = "Default transition";
            defaultProcess.Type = WorkflowTypes.WithoutState;
            defaultProcess.Transitions = new List<dynamic>();
            defaultProcess.Transitions.Add(instance);

            instance.SuccessPoint = new DynamicWrapper();
            instance.SuccessPoint.TypeName = "Action";
            instance.SuccessPoint.ScenarioId = "HospitalRegisterMoveAction";
            instance.SuccessPoint.DocumentDateProperty = "Date";

            instance.DeletePoint = new DynamicWrapper();
            instance.DeletePoint.TypeName = "Action";
            instance.DeletePoint.ScenarioId = "DeleteRegisterEntriesAction";

            processManager.MergeItem(defaultProcess);

            // указываем ссылку на тестовый сценарий комплексного предзаполнения
            scenarioManager =
                new ManagerFactoryDocument(ConfigurationId, PatientMovementDocumentId).BuildScenarioManager();

            const string scenarioRegisterId = "HospitalRegisterMoveAction";
            scenarioRegisterItem = scenarioManager.CreateItem(scenarioRegisterId);
            scenarioRegisterItem.ScenarioId = scenarioRegisterId;
            scenarioRegisterItem.Id = scenarioRegisterId;
            scenarioRegisterItem.Name = scenarioRegisterId;
            scenarioRegisterItem.ScriptUnitType = ScriptUnitType.Action;
            scenarioRegisterItem.ContextType = ContextTypeKind.ApplyMove;
            scenarioManager.MergeItem(scenarioRegisterItem);

            // добавляем ссылку на сборку, в которой находится прикладной модуль

            var assemblyManager = new ManagerFactoryConfiguration(ConfigurationId).BuildAssemblyManager();
            dynamic assemblyItem = assemblyManager.CreateItem("InfinniPlatform.Api.Tests");
            assemblyManager.MergeItem(assemblyItem);

            // Создаем новый регистр, который будет накапливать информацию о занятости коек

            var managerRegister = new ManagerFactoryConfiguration(ConfigurationId).BuildRegisterManager();

            var registerMetadata = managerRegister.CreateItem(AvailableBedsRegisterId);

            registerMetadata.Id = Guid.NewGuid().ToString();

            registerMetadata.Properties = new List<dynamic>();

            registerMetadata.Properties.Add(
                new {Property = RegisterConstants.RegistrarProperty, Type = RegisterPropertyType.Info});
            registerMetadata.Properties.Add(
                new {Property = RegisterConstants.RegistrarTypeProperty, Type = RegisterPropertyType.Info});
            registerMetadata.Properties.Add(
                new {Property = RegisterConstants.DocumentDateProperty, Type = RegisterPropertyType.Info});
            registerMetadata.Properties.Add(
                new {Property = RegisterConstants.EntryTypeProperty, Type = RegisterPropertyType.Info});
            registerMetadata.Properties.Add(new {Property = "Room", Type = RegisterPropertyType.Dimension});
            registerMetadata.Properties.Add(new {Property = "Bed", Type = RegisterPropertyType.Dimension});
            registerMetadata.Properties.Add(new {Property = "Value", Type = RegisterPropertyType.Value});
            registerMetadata.Period = RegisterPeriod.Month;

            registerMetadata.DocumentId = CreateRegisterDocuments(ConfigurationId, AvailableBedsRegisterId);

            managerRegister.MergeItem(registerMetadata);

            // Создаем регистр сведений
            var registerInfoMetadata = managerRegister.CreateItem(InfoRegisterId);

            registerInfoMetadata.Id = Guid.NewGuid().ToString();
            registerInfoMetadata.Period = RegisterPeriod.Month;

            registerInfoMetadata.Properties = new List<dynamic>();

            registerInfoMetadata.Properties.Add(
                new {Property = RegisterConstants.RegistrarProperty, Type = RegisterPropertyType.Info});
            registerInfoMetadata.Properties.Add(
                new {Property = RegisterConstants.RegistrarTypeProperty, Type = RegisterPropertyType.Info});
            registerInfoMetadata.Properties.Add(
                new {Property = RegisterConstants.DocumentDateProperty, Type = RegisterPropertyType.Info});
            registerInfoMetadata.Properties.Add(
                new {Property = RegisterConstants.EntryTypeProperty, Type = RegisterPropertyType.Info});
            registerInfoMetadata.Properties.Add(new {Property = "Room", Type = RegisterPropertyType.Dimension});
            registerInfoMetadata.Properties.Add(new {Property = "Bed", Type = RegisterPropertyType.Dimension});
            registerInfoMetadata.Properties.Add(new {Property = "Value", Type = RegisterPropertyType.Value});

            registerInfoMetadata.DocumentId = CreateRegisterDocuments(ConfigurationId, InfoRegisterId);

            managerRegister.MergeItem(registerInfoMetadata);

            var package = new PackageBuilder().BuildPackage(ConfigurationId, "test_version", GetType().Assembly.Location);

            UpdateApi.InstallPackages(new[] {package});

            UpdateApi.ForceReload(ConfigurationId);

            UpdateApi.UpdateStore(ConfigurationId);

            var registerInfoDocument = new
            {
                Id = InfoRegisterId
            };
            new DocumentApi().SetDocument(ConfigurationId, ConfigurationId + RegisterConstants.RegistersCommonInfo,
                registerInfoDocument);

            registerInfoDocument = new
            {
                Id = AvailableBedsRegisterId
            };
            new DocumentApi().SetDocument(ConfigurationId, ConfigurationId + RegisterConstants.RegistersCommonInfo,
                registerInfoDocument);
        }

        private static string CreateRegisterDocuments(string configId, string registerName, bool changeMapping = false)
        {
            var managerDocument = new ManagerFactoryConfiguration(configId).BuildDocumentManager();

            dynamic documentMetadata = managerDocument.CreateItem(RegisterConstants.RegisterNamePrefix + registerName);
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.SearchAbility = 0; // SearchAbilityType.KeywordBasedSearch;
            documentMetadata.Description = string.Format("Storage for register {0} data", registerName);

            dynamic schemaProperties = new DynamicWrapper();

            // Общие свойства для всех регистров

            dynamic registrarPropertyModel = new DynamicWrapper();
            registrarPropertyModel.Type = DataType.String.ToString();
            registrarPropertyModel.Caption = "Registrar";
            registrarPropertyModel.Description = "Регистратор";
            schemaProperties[RegisterConstants.RegistrarProperty] = registrarPropertyModel;

            dynamic registrarTypePropertyModel = new DynamicWrapper();
            registrarTypePropertyModel.Type = DataType.String.ToString();
            registrarTypePropertyModel.Caption = "Registrar type";
            registrarTypePropertyModel.Description = "Тип регистратора";
            schemaProperties[RegisterConstants.RegistrarTypeProperty] = registrarTypePropertyModel;

            dynamic documentDatePropertyModel = new DynamicWrapper();
            documentDatePropertyModel.Type = DataType.DateTime.ToString();
            documentDatePropertyModel.Caption = "Document date";
            documentDatePropertyModel.Description = "Дата регистрируемого документа";
            schemaProperties[RegisterConstants.DocumentDateProperty] = documentDatePropertyModel;

            dynamic entryTypePropertyModel = new DynamicWrapper();
            entryTypePropertyModel.Type = DataType.String.ToString();
            entryTypePropertyModel.Caption = "EntryType";
            entryTypePropertyModel.Description = "Тип записи регистра";
            schemaProperties[RegisterConstants.EntryTypeProperty] = entryTypePropertyModel;

            // Специфичные свойства регистра

            dynamic roomPropertyModel = new DynamicWrapper();
            roomPropertyModel.Type = DataType.String.ToString();
            roomPropertyModel.Caption = "Room";
            roomPropertyModel.Description = "Номер палаты";
            schemaProperties.Room = roomPropertyModel;

            dynamic bedPropertyModel = new DynamicWrapper();
            bedPropertyModel.Type = changeMapping ? DataType.Integer.ToString() : DataType.String.ToString();
            bedPropertyModel.Caption = "Bed";
            bedPropertyModel.Description = "Номер койки";
            schemaProperties.Bed = bedPropertyModel;

            dynamic changesPropertyModel = new DynamicWrapper();
            changesPropertyModel.Type = DataType.Integer.ToString();
            changesPropertyModel.Caption = "Value";
            changesPropertyModel.Description = "Изменение количества коек";
            schemaProperties.Value = changesPropertyModel;

            documentMetadata.Schema = new
            {
                Type = "Object",
                Caption = "Register document",
                Description = "Register document schema",
                Properties = schemaProperties
            }.ToDynamic();

            managerDocument.MergeItem(documentMetadata);

            // Документ для хранения промежуточных итогов

            dynamic documentTotalMetadata =
                managerDocument.CreateItem(RegisterConstants.RegisterTotalNamePrefix + registerName);
            documentTotalMetadata.Id = Guid.NewGuid().ToString();
            documentTotalMetadata.SearchAbility = 0; // SearchAbilityType.KeywordBasedSearch;
            documentTotalMetadata.Description = string.Format("Storage for register {0} totals", registerName);

            dynamic schemaTotalProperties = new DynamicWrapper();

            dynamic documentCalculationDatePropertyModel = new DynamicWrapper();
            documentCalculationDatePropertyModel.Type = DataType.DateTime.ToString();
            documentCalculationDatePropertyModel.Caption = "Date of total calculation";
            documentCalculationDatePropertyModel.Description = "Дата регистрируемого документа";
            schemaTotalProperties[RegisterConstants.DocumentDateProperty] = documentCalculationDatePropertyModel;

            // Специфичные свойства регистра
            schemaTotalProperties.Room = roomPropertyModel;
            schemaTotalProperties.Bed = bedPropertyModel;
            schemaTotalProperties.Value = changesPropertyModel;

            documentTotalMetadata.Schema = new
            {
                Type = "Object",
                Caption = "Register document",
                Description = "Register document schema",
                Properties = schemaTotalProperties
            }.ToDynamic();

            managerDocument.MergeItem(documentTotalMetadata);

            return RegisterConstants.RegisterNamePrefix + registerName;
        }

        private static void AddTestDocuments()
        {
            // Добавляем новые койки
            var result = new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 33",
                Bed = "Койка 1",
                Date = new DateTime(2014, 01, 01)
            });

            // Console.WriteLine(result.ToString());

            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 33",
                Bed = "Койка 2",
                Date = new DateTime(2014, 01, 01)
            });
            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 33",
                Bed = "Койка 3",
                Date = new DateTime(2014, 01, 01)
            });
            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 54",
                Bed = "Койка 1",
                Date = new DateTime(2014, 01, 01)
            });
            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 54",
                Bed = "Койка 2",
                Date = new DateTime(2014, 01, 01)
            });
            new DocumentApi().SetDocument(ConfigurationId, BedsRegistrationDocumentId, new
            {
                Id = Guid.NewGuid(),
                Room = "Палата 54",
                Bed = "Койка 3",
                Date = new DateTime(2014, 01, 01)
            });

            // Новые пациенты
            result = new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Иванов",
                NewRoom = "Палата 54",
                NewBed = "Койка 3",
                Date = new DateTime(2014, 08, 10)
            });

            // Console.WriteLine(result.ToString());

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Петров",
                NewRoom = "Палата 54",
                NewBed = "Койка 2",
                Date = new DateTime(2014, 08, 10)
            });

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Сидоров",
                NewRoom = "Палата 54",
                NewBed = "Койка 1",
                Date = new DateTime(2014, 08, 10)
            });

            // Переводы пациентов в другие палаты
            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Иванов",
                OldRoom = "Палата 54",
                OldBed = "Койка 3",
                NewRoom = "Палата 33",
                NewBed = "Койка 1",
                Date = new DateTime(2014, 08, 11)
            });

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Петров",
                OldRoom = "Палата 54",
                OldBed = "Койка 2",
                NewRoom = "Палата 33",
                NewBed = "Койка 2",
                Date = new DateTime(2014, 08, 11)
            });

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = TestGuid,
                PatientName = "Сидоров",
                OldRoom = "Палата 54",
                OldBed = "Койка 1",
                NewRoom = "Палата 33",
                NewBed = "Койка 3",
                Date = new DateTime(2014, 08, 11)
            });

            // Выписка пациентов
            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Иванов",
                OldRoom = "Палата 33",
                OldBed = "Койка 1",
                Date = new DateTime(2014, 08, 12)
            });

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Петорв",
                OldRoom = "Палата 33",
                OldBed = "Койка 2",
                Date = new DateTime(2014, 09, 12)
            });

            new DocumentApi().SetDocument(ConfigurationId, PatientMovementDocumentId, new
            {
                Id = Guid.NewGuid(),
                PatientName = "Сидоров",
                OldRoom = "Палата 33",
                OldBed = "Койка 3",
                Date = new DateTime(2014, 09, 12)
            });
        }
    }
}
