using System;
using System.Linq;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture(Description = "Проверка серверной сортировки")]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class SortingBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string OneStringSortingFieldDocument = "OneStringSortingFieldDocument";
        private const string OneIntSortingFieldDocument = "OneIntSortingFieldDocument";
        private const string OneDateSortingFieldDocument = "OneDateSortingFieldDocument";
        private const string OneSortingFieldInArrayDocument = "OneSortingFieldInArrayDocument";
        private const string OneSortingFieldInNestedObjectDocument = "OneSortingFieldInNestedObjectDocument";
        private const string TwoSortingFieldsDocument = "TwoSortingFieldsDocument";
        private const string InlineSortingDocument = "InlineSortingDocument";

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
        public void ShouldSortByArrayField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            var document1 = new
            {
                IntProperty = 2,
                ArrayProperty = new[] { new { SortableStringProperty = "ccc 1" }, new { SortableStringProperty = "yyy 1" } }
            };

            var document2 = new
            {
                IntProperty = 1,
                ArrayProperty = new[] { new { SortableStringProperty = "aaaa 2" }, new { SortableStringProperty = "zzz 2" } }
            };

            var document3 = new
            {
                IntProperty = 3,
                ArrayProperty = new[] { new { SortableStringProperty = "eee 1" }, new { SortableStringProperty = "xxx 1" } }
            };

            // When

            documentApi.SetDocument(ConfigurationId, OneSortingFieldInArrayDocument, document1);
            documentApi.SetDocument(ConfigurationId, OneSortingFieldInArrayDocument, document2);
            documentApi.SetDocument(ConfigurationId, OneSortingFieldInArrayDocument, document3);

            // По умолчанию - сортировка по возрастанию SortableStringProperty
            var ascPage0 = documentApi.GetDocument(ConfigurationId, OneSortingFieldInArrayDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(ConfigurationId, OneSortingFieldInArrayDocument, null, 0, 10, s => s.AddSorting("ArrayProperty.SortableStringProperty", "descending"));

            // Then

            Assert.AreEqual(1, ascPage0?.FirstOrDefault()?.IntProperty);
            Assert.AreEqual(3, descPage0?.FirstOrDefault()?.IntProperty);
        }

        [Test]
        public void ShouldSortByDateField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            var startDate = DateTime.Today;

            // When

            for (var i = 0; i < 100; ++i)
            {
                var document = new { SortableDateProperty = startDate.AddDays(i) };

                documentApi.SetDocument(ConfigurationId, OneDateSortingFieldDocument, document);
            }

            // По умолчанию - сортировка по возрастанию SortableDateProperty
            var ascPage0 = documentApi.GetDocument(ConfigurationId, OneDateSortingFieldDocument, null, 0, 10);
            var ascPage1 = documentApi.GetDocument(ConfigurationId, OneDateSortingFieldDocument, null, 1, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(ConfigurationId, OneDateSortingFieldDocument, null, 0, 10, s => s.AddSorting("SortableDateProperty", "descending"));
            var descPage1 = documentApi.GetDocument(ConfigurationId, OneDateSortingFieldDocument, null, 1, 10, s => s.AddSorting("SortableDateProperty", "descending"));

            // Then

            Assert.AreEqual(startDate.AddDays(0), ascPage0?.FirstOrDefault()?.SortableDateProperty);
            Assert.AreEqual(startDate.AddDays(10), ascPage1?.FirstOrDefault()?.SortableDateProperty);

            Assert.AreEqual(startDate.AddDays(99), descPage0?.FirstOrDefault()?.SortableDateProperty);
            Assert.AreEqual(startDate.AddDays(89), descPage1?.FirstOrDefault()?.SortableDateProperty);
        }

        [Test]
        public void ShouldSortByInlinedDocumentField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            var document1 = new
            {
                IntProperty = 2,
                ObjectProperty = new { SortableStringProperty = "ccc 1" }
            };

            var document2 = new
            {
                IntProperty = 1,
                ObjectProperty = new { SortableStringProperty = "aaa 2" }
            };

            var document3 = new
            {
                IntProperty = 3,
                ObjectProperty = new { SortableStringProperty = "eee 1" }
            };

            // When

            documentApi.SetDocument(ConfigurationId, InlineSortingDocument, document1);
            documentApi.SetDocument(ConfigurationId, InlineSortingDocument, document2);
            documentApi.SetDocument(ConfigurationId, InlineSortingDocument, document3);

            // По умолчанию - сортировка по возрастанию ObjectProperty.SortableStringProperty
            var ascPage0 = documentApi.GetDocument(ConfigurationId, InlineSortingDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(ConfigurationId, InlineSortingDocument, null, 0, 10, s => s.AddSorting("ObjectProperty.SortableStringProperty", "descending"));

            // Then

            Assert.AreEqual(1, ascPage0?.FirstOrDefault()?.IntProperty);
            Assert.AreEqual(3, descPage0?.FirstOrDefault()?.IntProperty);
        }

        [Test]
        public void ShouldSortByIntField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            // When

            for (var i = 0; i < 100; ++i)
            {
                var document = new { SortableIntProperty = i };

                documentApi.SetDocument(ConfigurationId, OneIntSortingFieldDocument, document);
            }

            // По умолчанию - сортировка по возрастанию SortableIntProperty
            var ascPage0 = documentApi.GetDocument(ConfigurationId, OneIntSortingFieldDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(ConfigurationId, OneIntSortingFieldDocument, null, 0, 10, s => s.AddSorting("SortableIntProperty", "descending"));

            // Then

            Assert.AreEqual(0, ascPage0?.FirstOrDefault()?.SortableIntProperty);
            Assert.AreEqual(99, descPage0?.FirstOrDefault()?.SortableIntProperty);
        }

        [Test]
        public void ShouldSortByObjectField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            var document1 = new
            {
                IntProperty = 2,
                ObjectProperty = new { SortableStringProperty = "ccc 1" }
            };

            var document2 = new
            {
                IntProperty = 1,
                ObjectProperty = new { SortableStringProperty = "aaa 2" }
            };

            var document3 = new
            {
                IntProperty = 3,
                ObjectProperty = new { SortableStringProperty = "eee 1" }
            };

            // When

            documentApi.SetDocument(ConfigurationId, OneSortingFieldInNestedObjectDocument, document1);
            documentApi.SetDocument(ConfigurationId, OneSortingFieldInNestedObjectDocument, document2);
            documentApi.SetDocument(ConfigurationId, OneSortingFieldInNestedObjectDocument, document3);

            // По умолчанию - сортировка по возрастанию ObjectProperty.SortableStringProperty
            var ascPage0 = documentApi.GetDocument(ConfigurationId, OneSortingFieldInNestedObjectDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(ConfigurationId, OneSortingFieldInNestedObjectDocument, null, 0, 10, s => s.AddSorting("ObjectProperty.SortableStringProperty", "descending"));

            // Then

            Assert.AreEqual(1, ascPage0?.FirstOrDefault()?.IntProperty);
            Assert.AreEqual(3, descPage0?.FirstOrDefault()?.IntProperty);
        }

        [Test]
        public void ShouldSortByStringField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            // When

            for (var i = 0; i < 100; ++i)
            {
                var document = new { SortableStringProperty = $"эюя абв где {i}" };

                documentApi.SetDocument(ConfigurationId, OneStringSortingFieldDocument, document);
            }

            // По умолчанию - сортировка по возрастанию SortableStringProperty
            var ascPage0 = documentApi.GetDocument(ConfigurationId, OneStringSortingFieldDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(ConfigurationId, OneStringSortingFieldDocument, null, 0, 10, s => s.AddSorting("SortableStringProperty", "descending"));

            // Then

            Assert.AreEqual("эюя абв где 0", ascPage0?.FirstOrDefault()?.SortableStringProperty);
            Assert.AreEqual("эюя абв где 99", descPage0?.FirstOrDefault()?.SortableStringProperty);
        }

        [Test]
        public void ShouldSortByTwoFields()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);

            var document1 = new
            {
                SortableStringProperty = "aaaaa",
                SortableIntProperty = 300
            };

            var document2 = new
            {
                SortableStringProperty = "aaaaa",
                SortableIntProperty = 200
            };

            var document3 = new
            {
                SortableStringProperty = "aaaaa",
                SortableIntProperty = 100
            };

            var document4 = new
            {
                SortableStringProperty = "bbbbb",
                SortableIntProperty = 300
            };

            var document5 = new
            {
                SortableStringProperty = "bbbbb",
                SortableIntProperty = 200
            };

            var document6 = new
            {
                SortableStringProperty = "bbbbb",
                SortableIntProperty = 100
            };

            // When

            documentApi.SetDocument(ConfigurationId, TwoSortingFieldsDocument, document1);
            documentApi.SetDocument(ConfigurationId, TwoSortingFieldsDocument, document2);
            documentApi.SetDocument(ConfigurationId, TwoSortingFieldsDocument, document3);
            documentApi.SetDocument(ConfigurationId, TwoSortingFieldsDocument, document4);
            documentApi.SetDocument(ConfigurationId, TwoSortingFieldsDocument, document5);
            documentApi.SetDocument(ConfigurationId, TwoSortingFieldsDocument, document6);

            // По умолчанию - сортировка по возрастанию SortableStringProperty и SortableIntProperty
            var ascPage0 = documentApi.GetDocument(ConfigurationId, TwoSortingFieldsDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(ConfigurationId, TwoSortingFieldsDocument, null, 0, 10, s => s.AddSorting("SortableStringProperty", "descending").AddSorting("SortableIntProperty"));

            // Then

            var firstAscItem = ascPage0?.FirstOrDefault();
            Assert.AreEqual(100, firstAscItem?.SortableIntProperty);
            Assert.AreEqual("aaaaa", firstAscItem?.SortableStringProperty);

            var firstDescItem = descPage0?.FirstOrDefault();
            Assert.AreEqual(100, firstDescItem?.SortableIntProperty);
            Assert.AreEqual("bbbbb", firstDescItem?.SortableStringProperty);
        }
    }
}