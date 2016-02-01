using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Registers;
using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Registers
{
    /// <summary>
    /// В тесте воспроизводится сценарий накопления информации по свободным койкам стационара с последующей агрегацией по различным критериям.
    /// </summary>
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class RegisterApiAcceptanceTest
    {
        public const string ConfigurationId = "TestConfiguration";

        public const string PatientMovementDocument = "RegisterTest_PatientMovementDocument";
        public const string BedsRegistrationDocument = "RegisterTest_BedsRegistrationDocument";

        public const string InfoRegister = "RegisterTest_InfoRegister";
        public const string AvailableBedsRegister = "RegisterTest_AvailableBedsRegister";
        public const string ConfigurationRegistersCommonInfo = ConfigurationId + RegisterConstants.RegistersCommonInfo;


        private IDisposable _server;
        private DocumentApiClient _documentApi;
        private RegisterApiClient _registerApi;


        [TestFixtureSetUp]
        public void SetUp()
        {
            _server = InfinniPlatformInprocessHost.Start();
            _documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);
            _registerApi = new RegisterApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            InitTestData();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _server.Dispose();
        }


        private void InitTestData()
        {
            // Тестовая конфигурация содержит документы двух типов: 
            // * BedsRegistrationDocument (Реестр коек)
            // * PatientMovementDocument (Движения пациентов по палатам)
            //
            // При сохранении документов этих типов, будут добавляться записи в регистры, накапливающие изменения состояния коек в палатах:
            // * InfoRegister
            // * AvailableBedsRegister

            // Регистрация документов регистров

            _documentApi.SetDocuments(ConfigurationId, ConfigurationRegistersCommonInfo, new[] { new { Id = InfoRegister } });
            _documentApi.SetDocuments(ConfigurationId, ConfigurationRegistersCommonInfo, new[] { new { Id = AvailableBedsRegister } });

            // Регистрация коек

            AddBed(new DateTime(2014, 01, 01), "Палата 33", "Койка 1");
            AddBed(new DateTime(2014, 01, 01), "Палата 33", "Койка 2");
            AddBed(new DateTime(2014, 01, 01), "Палата 33", "Койка 3");

            AddBed(new DateTime(2014, 01, 01), "Палата 54", "Койка 1");
            AddBed(new DateTime(2014, 01, 01), "Палата 54", "Койка 2");
            AddBed(new DateTime(2014, 01, 01), "Палата 54", "Койка 3");

            // Поступление новых пациентов

            AddPatientMovement(new DateTime(2014, 08, 10), "Иванов", "", "", "Палата 54", "Койка 3");
            AddPatientMovement(new DateTime(2014, 08, 10), "Петров", "", "", "Палата 54", "Койка 2");
            AddPatientMovement(new DateTime(2014, 08, 10), "Сидоров", "", "", "Палата 54", "Койка 1");

            // Переводы пациентов

            AddPatientMovement(new DateTime(2014, 08, 11), "Иванов", "Палата 54", "Койка 3", "Палата 33", "Койка 1");
            AddPatientMovement(new DateTime(2014, 08, 11), "Петров", "Палата 54", "Койка 2", "Палата 33", "Койка 2");
            AddPatientMovement(new DateTime(2014, 08, 11), "Сидоров", "Палата 54", "Койка 1", "Палата 33", "Койка 3");

            // Выписка пациентов

            AddPatientMovement(new DateTime(2014, 08, 12), "Иванов", "Палата 33", "Койка 1", "", "");
            AddPatientMovement(new DateTime(2014, 09, 12), "Петорв", "Палата 33", "Койка 2", "", "");
            AddPatientMovement(new DateTime(2014, 09, 12), "Сидоров", "Палата 33", "Койка 3", "", "");
        }

        private void AddBed(DateTime date, string room, string bed, string info = null)
        {
            var documentId = Guid.NewGuid().ToString();

            _documentApi.SetDocument(ConfigurationId, BedsRegistrationDocument, new
            {
                Id = documentId,
                Room = room,
                Bed = bed,
                Date = date,
                Info = info
            });
        }

        private string AddPatientMovement(DateTime date, string patient, string oldRoom, string oldBed, string newRoom, string newBed)
        {
            var documentId = Guid.NewGuid().ToString();

            _documentApi.SetDocument(ConfigurationId, PatientMovementDocument, new
            {
                Id = documentId,
                PatientName = patient,
                OldRoom = oldRoom,
                OldBed = oldBed,
                NewRoom = newRoom,
                NewBed = newBed,
                Date = date
            });

            return documentId;
        }

        [Test]
        public void ShouldAddOnlyOneRegisterEntryToInfoRegisterPerPeriod()
        {
            const string infoRegisterDocument = RegisterConstants.RegisterNamePrefix + InfoRegister;

            // Given

            var infoProperty = Guid.NewGuid().ToString();

            // When

            AddBed(new DateTime(2114, 01, 01, 9, 1, 1), "Палата 33", "Койка 1", infoProperty);
            AddBed(new DateTime(2114, 01, 15, 1, 2, 3), "Палата 33", "Койка 1", infoProperty);
            AddBed(new DateTime(2114, 02, 01, 7, 6, 5), "Палата 1", "Койка 3", infoProperty);
            AddBed(new DateTime(2114, 02, 02, 4, 4, 4), "Палата 1", "Койка 3", infoProperty);
            AddBed(new DateTime(2114, 02, 04, 3, 2, 1), "Палата 1", "Койка 3", infoProperty);
            AddBed(new DateTime(2114, 03, 01, 3, 2, 1), "Палата 1", "Койка 6", infoProperty);

            var registerEntries = _documentApi.GetDocument(ConfigurationId, infoRegisterDocument, f => f.AddCriteria(c => c.Property("Info").IsEquals(infoProperty)), 0, 10);

            // Then

            // Период для регистра сведений задан равным в 1 месяц, поэтому за каждый месяц должно быть по одной записи в регистре.
            // Должно добавиться только три записи в регистр, каждая на определенный месяц.

            Assert.AreEqual(3, registerEntries.Count());
        }

        [Test]
        public void ShouldAggregateHospitalData()
        {
            // When & Then

            // 01.02.2014 - Все койки свободны, ни одного пациента еще не было
            IEnumerable<dynamic> aggregationInfo1 = _registerApi.GetValuesByDate(ConfigurationId, AvailableBedsRegister, new DateTime(2014, 2, 1), dimensionsProperties: new[] { "Room", "Bed" }).ToArray();
            Assert.AreEqual(1d, aggregationInfo1.First(i => i.Room == "палата 54" && i.Bed == "койка 3").Value);
            Assert.AreEqual(1d, aggregationInfo1.First(i => i.Room == "палата 33" && i.Bed == "койка 3").Value);

            // 12.08.2014 - Свободные койки по палатам: "Палата 33" - 2 пациента, "Палата 54" - свободна
            IEnumerable<dynamic> aggregationInfo2 = _registerApi.GetValuesByDate(ConfigurationId, AvailableBedsRegister, new DateTime(2014, 08, 12), dimensionsProperties: new[] { "Room" }).ToArray();
            Assert.AreEqual(3d, aggregationInfo2.First(i => i.Room == "палата 54").Value);
            Assert.AreEqual(1d, aggregationInfo2.First(i => i.Room == "палата 33").Value);

            // 12.08.2014 - Свободные койки по номерам
            IEnumerable<dynamic> aggregationInfo3 = _registerApi.GetValuesByDate(ConfigurationId, AvailableBedsRegister, new DateTime(2014, 08, 12), dimensionsProperties: new[] { "Bed" }, valueProperties: new[] { "Value" }).ToArray();
            Assert.AreEqual(2d, aggregationInfo3.First(i => i.Bed == "койка 1").Value);
            Assert.AreEqual(1d, aggregationInfo3.First(i => i.Bed == "койка 2").Value);
            Assert.AreEqual(1d, aggregationInfo3.First(i => i.Bed == "койка 3").Value);

            // 12-14.08.2014 - Изменение занятости коек за период - освободилась "Койка 1" в "Палата 33"
            IEnumerable<dynamic> aggregationInfo4 = _registerApi.GetValuesBetweenDates(ConfigurationId, AvailableBedsRegister, new DateTime(2014, 08, 12), new DateTime(2014, 08, 14));
            Assert.AreEqual(1, aggregationInfo4.First(a => a.Room == "палата 33" && a.Bed == "койка 1").Value);

            // Получение информации по типу регистратора - были добавлены все койки
            //var aggregationInfo5 = registerApi.GetValuesBуRegistrarType(ConfigurationId, AvailableBedsRegister, BedsRegistrationDocument).ToArray();
            //Assert.AreEqual(1d, aggregationInfo5.First(i => i.Room == "палата 54" && i.Bed == "койка 3").Value);
            //Assert.AreEqual(1d, aggregationInfo5.First(i => i.Room == "палата 33" && i.Bed == "койка 3").Value);

            // 18.08.2014 - "Палата 54" свободна, в "Палата 33" занято 2 койки
            IEnumerable<dynamic> aggregationInfo6 = _registerApi.GetValuesByDate(ConfigurationId, AvailableBedsRegister, new DateTime(2014, 8, 18)).ToArray();
            Assert.AreEqual(1d, aggregationInfo6.First(a => a.Room == "палата 54" && a.Bed == "койка 3").Value);
            Assert.AreEqual(0d, aggregationInfo6.First(a => a.Room == "палата 33" && a.Bed == "койка 3").Value);

            // 01.10.2014 - Все койки свободны
            IEnumerable<dynamic> aggregationInfo7 = _registerApi.GetValuesByDate(ConfigurationId, AvailableBedsRegister, new DateTime(2014, 10, 1)).ToArray();
            Assert.AreEqual(1d, aggregationInfo7.First(i => i.Room == "палата 54" && i.Bed == "койка 3").Value);
            Assert.AreEqual(1d, aggregationInfo7.First(i => i.Room == "палата 33" && i.Bed == "койка 3").Value);

            // Получение агрегации с группировкой по месяцам
            //var aggregationInfo8 = registerApi.GetValuesByPeriods(ConfigurationId, AvailableBedsRegister, DateTime.MinValue, DateTime.MaxValue, RegisterPeriod.Month);
            //Assert.IsTrue(aggregationInfo8.Any());

            // Тот же запрос с установленной временной зоной
            //var aggregationInfo9 = registerApi.GetValuesByPeriods(ConfigurationId, AvailableBedsRegister, DateTime.MinValue, DateTime.MaxValue, RegisterPeriod.Month, null, null, "+05:00", new List<dynamic>());
            //Assert.IsTrue(aggregationInfo9.Any());
        }

        [Test]
        public void ShouldAggregateHospitalDataAfterMappingChanged()
        {
            // Given

            AddBed(new DateTime(2014, 01, 01), "Палата 6", "Койка 1");
            AddBed(new DateTime(2014, 01, 01), "Палата 6", "Койка 2");
            AddBed(new DateTime(2014, 01, 01), "Палата 6", "Койка 3");

            // When & Then

            // 01.02.2014 - Все койки свободны, ни одного пациента еще не было
            IEnumerable<dynamic> aggregationInfo1 = _registerApi.GetValuesByDate(ConfigurationId, AvailableBedsRegister, new DateTime(2014, 2, 1), dimensionsProperties: new[] { "Room", "Bed" }).ToArray();
            Assert.AreEqual(1d, aggregationInfo1.First(i => i.Room == "палата 54" && i.Bed == "койка 3").Value);
            Assert.AreEqual(1d, aggregationInfo1.First(i => i.Room == "палата 33" && i.Bed == "койка 3").Value);

            // When & Then

            AddPatientMovement(new DateTime(2014, 08, 10), "Иванов", "", "", "Палата 6", "Койка 1");

            // 12.08.2014 - Занята только первая койка в "Палата 6"
            IEnumerable<dynamic> aggregationInfo2 = _registerApi.GetValuesByDate(ConfigurationId, AvailableBedsRegister, new DateTime(2014, 08, 12), dimensionsProperties: new[] { "Room" }).ToArray();
            Assert.AreEqual(2, aggregationInfo2.First(i => i.Room == "палата 6").Value);
        }

        [Test(Description = "Тестирование функционала таблицы итогов")]
        public void ShouldCalculateTotals()
        {
            // When

            // Миграция поместит имеющиеся на данный момент записи в таблицу итогов
            _registerApi.RecalculateTotals(ConfigurationId);

            AddPatientMovement(DateTime.Now.AddYears(2), "Иванов", "Палата 33", "Койка 3", "", "");
            AddPatientMovement(DateTime.Now.AddYears(2), "Петров", "Палата 33", "Койка 2", "", "");
            AddPatientMovement(DateTime.Now.AddYears(2), "Сидоров", "Палата 33", "Койка 1", "", "");

            // В данном случае расчет будет произведен с учетом данных из таблицы итогов
            IEnumerable<dynamic> aggregationInfo = _registerApi.GetValuesByDate(ConfigurationId, AvailableBedsRegister, DateTime.Now.AddYears(3)).ToArray();

            // Then

            Assert.AreEqual(1d, aggregationInfo.First(i => i.Room == "палата 54" && i.Bed == "койка 3").Value);
            Assert.AreEqual(10d, aggregationInfo.First(i => i.Room == "палата 33" && i.Bed == "койка 3").Value);
        }

        [Test]
        public void ShouldDeleteRegisterEntriesAfterDeletingDocument()
        {
            // Given

            var date = new DateTime(2214, 01, 01);

            // When

            AddBed(date, "Палата 123", "Койка 321");

            var documentId = AddPatientMovement(date, "Иванов", "", "", "Палата 123", "Койка 321");

            var registerEntries1 = _registerApi.GetEntries(ConfigurationId, AvailableBedsRegister,
                new[] { new FilterCriteria(RegisterConstants.DocumentDateProperty, date, CriteriaType.IsMoreThanOrEquals) }, 0, 10);

            _documentApi.DeleteDocument(ConfigurationId, BedsRegistrationDocument, documentId);

            var registerEntries2 = _registerApi.GetEntries(ConfigurationId, AvailableBedsRegister,
                new[] { new FilterCriteria(RegisterConstants.DocumentDateProperty, date, CriteriaType.IsMoreThanOrEquals) }, 0, 10);

            // Then

            Assert.AreEqual(2, registerEntries1.Count());
            Assert.AreEqual(0, registerEntries2.Count());
        }
    }
}