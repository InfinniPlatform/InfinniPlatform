using System;
using System.Linq;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture(Description = "Проверка серверной сортировки")]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class SortingBehavior
    {
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

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

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

            documentApi.SetDocument(OneSortingFieldInArrayDocument, document1);
            documentApi.SetDocument(OneSortingFieldInArrayDocument, document2);
            documentApi.SetDocument(OneSortingFieldInArrayDocument, document3);

            // По умолчанию - сортировка по возрастанию SortableStringProperty
            var ascPage0 = documentApi.GetDocument(OneSortingFieldInArrayDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(OneSortingFieldInArrayDocument, null, 0, 10, s => s.AddSorting("ArrayProperty.SortableStringProperty", "descending"));

            // Then

            Assert.IsNotNull(ascPage0);
            Assert.IsNotNull(descPage0);
            Assert.IsNotNull(ascPage0.FirstOrDefault());
            Assert.IsNotNull(descPage0.FirstOrDefault());
            Assert.AreEqual(1, ascPage0.FirstOrDefault().IntProperty);
            Assert.AreEqual(3, descPage0.FirstOrDefault().IntProperty);
        }

        [Test]
        public void ShouldSortByDateField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var startDate = DateTime.Today;

            // When

            for (var i = 0; i < 100; ++i)
            {
                var document = new { SortableDateProperty = startDate.AddDays(i) };

                documentApi.SetDocument(OneDateSortingFieldDocument, document);
            }

            // По умолчанию - сортировка по возрастанию SortableDateProperty
            var ascPage0 = documentApi.GetDocument(OneDateSortingFieldDocument, null, 0, 10);
            var ascPage1 = documentApi.GetDocument(OneDateSortingFieldDocument, null, 1, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(OneDateSortingFieldDocument, null, 0, 10, s => s.AddSorting("SortableDateProperty", "descending"));
            var descPage1 = documentApi.GetDocument(OneDateSortingFieldDocument, null, 1, 10, s => s.AddSorting("SortableDateProperty", "descending"));

            // Then

            Assert.IsNotNull(ascPage0);
            Assert.IsNotNull(ascPage1);
            Assert.IsNotNull(ascPage0.FirstOrDefault());
            Assert.IsNotNull(ascPage1.FirstOrDefault());
            Assert.AreEqual(startDate.AddDays(0), ascPage0.FirstOrDefault().SortableDateProperty);
            Assert.AreEqual(startDate.AddDays(10), ascPage1.FirstOrDefault().SortableDateProperty);

            Assert.IsNotNull(descPage0);
            Assert.IsNotNull(descPage1);
            Assert.IsNotNull(descPage0.FirstOrDefault());
            Assert.IsNotNull(descPage1.FirstOrDefault());
            Assert.AreEqual(startDate.AddDays(99), descPage0.FirstOrDefault().SortableDateProperty);
            Assert.AreEqual(startDate.AddDays(89), descPage1.FirstOrDefault().SortableDateProperty);
        }

        [Test]
        public void ShouldSortByInlinedDocumentField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

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

            documentApi.SetDocument(InlineSortingDocument, document1);
            documentApi.SetDocument(InlineSortingDocument, document2);
            documentApi.SetDocument(InlineSortingDocument, document3);

            // По умолчанию - сортировка по возрастанию ObjectProperty.SortableStringProperty
            var ascPage0 = documentApi.GetDocument(InlineSortingDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(InlineSortingDocument, null, 0, 10, s => s.AddSorting("ObjectProperty.SortableStringProperty", "descending"));

            // Then

            Assert.IsNotNull(ascPage0);
            Assert.IsNotNull(descPage0);
            Assert.IsNotNull(ascPage0.FirstOrDefault());
            Assert.IsNotNull(descPage0.FirstOrDefault());
            Assert.AreEqual(1, ascPage0.FirstOrDefault().IntProperty);
            Assert.AreEqual(3, descPage0.FirstOrDefault().IntProperty);
        }

        [Test]
        public void ShouldSortByIntField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            // When

            for (var i = 0; i < 100; ++i)
            {
                var document = new { SortableIntProperty = i };

                documentApi.SetDocument(OneIntSortingFieldDocument, document);
            }

            // По умолчанию - сортировка по возрастанию SortableIntProperty
            var ascPage0 = documentApi.GetDocument(OneIntSortingFieldDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(OneIntSortingFieldDocument, null, 0, 10, s => s.AddSorting("SortableIntProperty", "descending"));

            // Then

            Assert.IsNotNull(ascPage0);
            Assert.IsNotNull(descPage0);
            Assert.IsNotNull(ascPage0.FirstOrDefault());
            Assert.IsNotNull(descPage0.FirstOrDefault());
            Assert.AreEqual(0, ascPage0.FirstOrDefault().SortableIntProperty);
            Assert.AreEqual(99, descPage0.FirstOrDefault().SortableIntProperty);
        }

        [Test]
        public void ShouldSortByObjectField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

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

            documentApi.SetDocument(OneSortingFieldInNestedObjectDocument, document1);
            documentApi.SetDocument(OneSortingFieldInNestedObjectDocument, document2);
            documentApi.SetDocument(OneSortingFieldInNestedObjectDocument, document3);

            // По умолчанию - сортировка по возрастанию ObjectProperty.SortableStringProperty
            var ascPage0 = documentApi.GetDocument(OneSortingFieldInNestedObjectDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(OneSortingFieldInNestedObjectDocument, null, 0, 10, s => s.AddSorting("ObjectProperty.SortableStringProperty", "descending"));

            // Then

            Assert.IsNotNull(ascPage0);
            Assert.IsNotNull(descPage0);
            Assert.IsNotNull(ascPage0.FirstOrDefault());
            Assert.IsNotNull(descPage0.FirstOrDefault());
            Assert.AreEqual(1, ascPage0.FirstOrDefault().IntProperty);
            Assert.AreEqual(3, descPage0.FirstOrDefault().IntProperty);
        }

        [Test]
        public void ShouldSortByStringField()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            // When

            for (var i = 0; i < 100; ++i)
            {
                var document = new { SortableStringProperty = $"эюя абв где {i}" };

                documentApi.SetDocument(OneStringSortingFieldDocument, document);
            }

            // По умолчанию - сортировка по возрастанию SortableStringProperty
            var ascPage0 = documentApi.GetDocument(OneStringSortingFieldDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(OneStringSortingFieldDocument, null, 0, 10, s => s.AddSorting("SortableStringProperty", "descending"));

            // Then

            Assert.IsNotNull(ascPage0);
            Assert.IsNotNull(descPage0);
            Assert.IsNotNull(ascPage0.FirstOrDefault());
            Assert.IsNotNull(descPage0.FirstOrDefault());
            Assert.AreEqual("эюя абв где 0", ascPage0.FirstOrDefault().SortableStringProperty);
            Assert.AreEqual("эюя абв где 99", descPage0.FirstOrDefault().SortableStringProperty);
        }

        [Test]
        public void ShouldSortByTwoFields()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

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

            documentApi.SetDocument(TwoSortingFieldsDocument, document1);
            documentApi.SetDocument(TwoSortingFieldsDocument, document2);
            documentApi.SetDocument(TwoSortingFieldsDocument, document3);
            documentApi.SetDocument(TwoSortingFieldsDocument, document4);
            documentApi.SetDocument(TwoSortingFieldsDocument, document5);
            documentApi.SetDocument(TwoSortingFieldsDocument, document6);

            // По умолчанию - сортировка по возрастанию SortableStringProperty и SortableIntProperty
            var ascPage0 = documentApi.GetDocument(TwoSortingFieldsDocument, null, 0, 10);

            // Сортировка по убыванию
            var descPage0 = documentApi.GetDocument(TwoSortingFieldsDocument, null, 0, 10, s => s.AddSorting("SortableStringProperty", "descending").AddSorting("SortableIntProperty"));

            // Then

            Assert.IsNotNull(ascPage0);
            var firstAscItem = ascPage0.FirstOrDefault();
            Assert.IsNotNull(firstAscItem);
            Assert.AreEqual(100, firstAscItem.SortableIntProperty);
            Assert.AreEqual("aaaaa", firstAscItem.SortableStringProperty);

            Assert.IsNotNull(descPage0);
            var firstDescItem = descPage0.FirstOrDefault();
            Assert.IsNotNull(firstDescItem);
            Assert.AreEqual(100, firstDescItem.SortableIntProperty);
            Assert.AreEqual("bbbbb", firstDescItem.SortableStringProperty);
        }
    }
}