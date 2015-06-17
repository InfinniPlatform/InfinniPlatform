using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    /// <summary>
    /// Проверка серверной сортировки
    /// </summary>
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class SortingBehavior
    {
        const string ConfigurationId = "sortingbehaviorconfiguration";
        const string DocumentWithOneStringSortingField = "OneStringSortingField";
        const string DocumentWithOneIntSortingField = "OneIntSortingField";
        const string DocumentWithOneDateSortingField = "OneDateSortingField";
        const string DocumentWithOneSortingFieldInArray = "OneSortingFieldInArray";
        const string DocumentWithOneSortingFieldInNestedObject = "OneSortingFieldInNestedObject";
        const string DocumentWithTwoSortingFields = "TwoSortingFields";
        const string DocumentWithNoSorting = "NoSortingFields";
        const string DocumentWithInlineSorting = "InlineSorting";
        
        private readonly Stopwatch _sw = new Stopwatch();
        
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

			TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);

            _sw.Restart();
            
            CreateTestConfig();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldSortByStringField()
        {
            var ids = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                var next = new Random().Next(1000);

                ids.Add(next);

                new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneStringSortingField, new
                {
                    Id = Guid.NewGuid(),
                    StringProperty = "где абв эюя" + next,
                    SortableStringProperty = string.Format("эюя абв где {0:D3}", next),
                    RandomNumber = next
                }, false, true);
            }

            ids.Sort();

            _sw.Restart();

            // Автоматически должно отсортироваться по возрастанию SortableStringProperty
            var sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneStringSortingField, null, 0, 10);

            _sw.Stop();
            
            Console.WriteLine("Sorting by string time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(ids.Min(), sortedData.First().RandomNumber);

            _sw.Restart();

            // Постраничная выборка - берем вторую страницу
            sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneStringSortingField, null, 1, 10);

            _sw.Stop();

            Console.WriteLine("Sorting by string time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(ids.Skip(10).First(), sortedData.First().RandomNumber);

            // Crossconfig call
			sortedData = new DocumentApi(null).GetDocumentCrossConfig(null, 0, 10, new[] { ConfigurationId }, new[] { DocumentWithOneStringSortingField }, s => s.AddSorting("SortableStringProperty"));
            Assert.AreEqual(ids.Min(), sortedData.First().RandomNumber);
        }

        [Test]
        public void ShouldSortByIntField()
        {
            var ids = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                var next = new Random().Next(1000);

                ids.Add(next);

                new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneIntSortingField, new
                {
                    Id = Guid.NewGuid(),
                    StringProperty = "где абв эюя" + next,
                    SortableIntProperty = next,
                    IntProperty = -1 * next
                });
            }

            ids.Sort();

            _sw.Restart();

            // Автоматически должно отсортироваться по возрастанию SortableIntProperty
            var sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneIntSortingField, null, 0, 10);

            _sw.Stop();

            Console.WriteLine("Sorting by int time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(ids.Min(), sortedData.First().SortableIntProperty);

            _sw.Restart();

            // Постраничная выборка - берем пятую страницу
            sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneIntSortingField, null, 5, 10);

            _sw.Stop();

            Console.WriteLine("Sorting by int time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(ids.Skip(50).First(), sortedData.First().SortableIntProperty);
        }

        [Test]
        public void ShouldSortByDateField()
        {
            var ids = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                var next = new Random().Next(30);

                ids.Add(next);

                new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneDateSortingField, new
                {
                    Id = Guid.NewGuid(),
                    StringProperty = "где абв эюя" + next,
                    SortableDateProperty = new DateTime(2014, 1, 1 + next),
                    IntProperty = -1 * next
                }.ToDynamic());
            }

            ids.Sort();

            _sw.Restart();

            // Автоматически должно отсортироваться по возрастанию SortableDateProperty
            var sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneDateSortingField, null, 0, 10);

            _sw.Stop();

            Console.WriteLine("Sorting by date time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(new DateTime(2014, 1, 1 + ids.Min()), sortedData.First().SortableDateProperty);

            _sw.Restart();

            // Постраничная выборка - берем пятую страницу
            sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneDateSortingField, null, 5, 5);

            _sw.Stop();

            Console.WriteLine("Sorting by date time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(new DateTime(2014, 1, 1 + ids.Skip(25).First()), sortedData.First().SortableDateProperty);
        }

        [Test]
        public void ShouldSortByArrayField()
        {
            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneSortingFieldInArray, new
            {
                Id = Guid.NewGuid(),
                ArrayProperty = new[]
                {
                    new
                    {
                        SortableStringProperty = "ccc 1"
                    },
                    new
                    {
                        SortableStringProperty = "yyy 1"
                    }
                },
                StringProperty = "где абв эюя",
                IntProperty = 2,
                DateProperty = new DateTime(2014, 01, 02)
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneSortingFieldInArray, new
            {
                Id = Guid.NewGuid(),
                ArrayProperty = new[]
                {
                    new
                    {
                        SortableStringProperty = "aaaa 2"
                    },
                    new
                    {
                        SortableStringProperty = "zzz 2"
                    }
                },
                StringProperty = "где абв эюя",
                IntProperty = 1,
                DateProperty = new DateTime(2014, 01, 02)
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneSortingFieldInArray, new
            {
                Id = Guid.NewGuid(),
                ArrayProperty = new[]
                {
                    new
                    {
                        SortableStringProperty = "eee 1"
                    },
                    new
                    {
                        SortableStringProperty = "xxx 1"
                    }
                },
                StringProperty = "где абв эюя",
                IntProperty = 3,
                DateProperty = new DateTime(2014, 01, 02)
            });

            _sw.Restart();

            // Автоматически должно отсортироваться по возрастанию SortableStringProperty
            var sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneSortingFieldInArray, null, 0, 10);
            
            _sw.Stop();
            Console.WriteLine("Sorting by array time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(1, sortedData.First().IntProperty);

            _sw.Restart();

            // Сортировка по убыванию
            sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneSortingFieldInArray, null, 0, 10, 
                s => s.AddSorting("ArrayProperty.SortableStringProperty", SortOrder.Descending));

            _sw.Stop();
            Console.WriteLine("Sorting by array time: " + _sw.ElapsedMilliseconds + " ms");
            
            Assert.AreEqual(3, sortedData.First().IntProperty);
        }

        [Test]
        public void ShouldSortByObjectField()
        {
            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneSortingFieldInNestedObject, new
            {
                Id = Guid.NewGuid(),
                ObjectProperty = new
                {
                    SortableStringProperty = "ccc 1"
                },
                StringProperty = "где абв эюя",
                IntProperty = 2,
                DateProperty = new DateTime(2014, 01, 02)
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneSortingFieldInNestedObject, new
            {
                Id = Guid.NewGuid(),
                ObjectProperty = new
                {
                    SortableStringProperty = "aaa 2"
                },
                StringProperty = "где абв эюя",
                IntProperty = 1,
                DateProperty = new DateTime(2014, 01, 02)
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneSortingFieldInNestedObject, new
            {
                Id = Guid.NewGuid(),
                ObjectProperty = new
                {
                    SortableStringProperty = "eee 1"
                },
                StringProperty = "где абв эюя",
                IntProperty = 3,
                DateProperty = new DateTime(2014, 01, 02)
            });

            _sw.Restart();

            // Автоматически должно отсортироваться по возрастанию SortableStringProperty
            var sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneSortingFieldInNestedObject, null, 0, 10);

            _sw.Stop();
            Console.WriteLine("Sorting by object field time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(1, sortedData.First().IntProperty);

            _sw.Restart();

            // Сортировка по убыванию
            sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneSortingFieldInNestedObject, null, 0, 10,
                s => s.AddSorting("ObjectProperty.SortableStringProperty", SortOrder.Descending));

            _sw.Stop();
            Console.WriteLine("Sorting by object field time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(3, sortedData.First().IntProperty);
        }

        [Test]
        public void ShouldSortByInlinedDocumentField()
        {
            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithInlineSorting, new
            {
                Id = Guid.NewGuid(),
                ObjectProperty = new
                {
                    SortableStringProperty = "ccc 1"
                },
                StringProperty = "где абв эюя",
                IntProperty = 2,
                DateProperty = new DateTime(2014, 01, 02)
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithInlineSorting, new
            {
                Id = Guid.NewGuid(),
                ObjectProperty = new
                {
                    SortableStringProperty = "aaa 2"
                },
                StringProperty = "где абв эюя",
                IntProperty = 1,
                DateProperty = new DateTime(2014, 01, 02)
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithInlineSorting, new
            {
                Id = Guid.NewGuid(),
                ObjectProperty = new
                {
                    SortableStringProperty = "eee 1"
                },
                StringProperty = "где абв эюя",
                IntProperty = 3,
                DateProperty = new DateTime(2014, 01, 02)
            });

            _sw.Restart();

            // Автоматически должно отсортироваться по возрастанию SortableStringProperty
            var sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithInlineSorting, null, 0, 10);

            _sw.Stop();
            Console.WriteLine("Sorting by inline object field time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(1, sortedData.First().IntProperty);

            _sw.Restart();

            // Сортировка по убыванию
            sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithInlineSorting, null, 0, 10,
                s => s.AddSorting("ObjectProperty.SortableStringProperty", SortOrder.Descending));

            _sw.Stop();
            Console.WriteLine("Sorting by inline object field time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(3, sortedData.First().IntProperty);
        }

        [Test]
        public void ShouldSortByTwoFields()
        {
            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithTwoSortingFields, new
            {
                Id = Guid.NewGuid(),
                SortableStringProperty = "aaaaa",
                StringProperty = "где абв эюя",
                SortableIntProperty = 300,
                IntProperty = 2
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithTwoSortingFields, new
            {
                Id = Guid.NewGuid(),
                SortableStringProperty = "aaaaa",
                StringProperty = "где абв эюя",
                SortableIntProperty = 200,
                IntProperty = 2
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithTwoSortingFields, new
            {
                Id = Guid.NewGuid(),
                SortableStringProperty = "aaaaa",
                StringProperty = "где абв эюя",
                SortableIntProperty = 100,
                IntProperty = 2
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithTwoSortingFields, new
            {
                Id = Guid.NewGuid(),
                SortableStringProperty = "bbbbb",
                StringProperty = "где абв эюя",
                SortableIntProperty = 300,
                IntProperty = 2
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithTwoSortingFields, new
            {
                Id = Guid.NewGuid(),
                SortableStringProperty = "bbbbb",
                StringProperty = "где абв эюя",
                SortableIntProperty = 200,
                IntProperty = 2
            });

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithTwoSortingFields, new
            {
                Id = Guid.NewGuid(),
                SortableStringProperty = "bbbbb",
                StringProperty = "где абв эюя",
                SortableIntProperty = 100,
                IntProperty = 2
            });


            _sw.Restart();

            // Автоматически должно отсортироваться по возрастанию SortableStringProperty и SortableIntProperty
            var sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithTwoSortingFields, null, 0, 10);

            _sw.Stop();
            Console.WriteLine("Sorting by 2 fields time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(100, sortedData.First().SortableIntProperty);
            Assert.AreEqual("aaaaa", sortedData.First().SortableStringProperty);

            _sw.Restart();

            // Сортировка по убыванию
            sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithTwoSortingFields, null, 0, 10,
                s => s.AddSorting("SortableStringProperty", SortOrder.Descending).AddSorting("SortableIntProperty"));

            _sw.Stop();
            Console.WriteLine("Sorting by 2 fields time: " + _sw.ElapsedMilliseconds + " ms");

            Assert.AreEqual(100, sortedData.First().SortableIntProperty);
            Assert.AreEqual("bbbbb", sortedData.First().SortableStringProperty);
        }

        [Test]
        [Ignore("Manual test for performance estimation")]
        public void SortingByStringPerformance()
        {
            var ids = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                var next = new Random().Next();

                ids.Add(next);

                new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithOneStringSortingField, new
                {
                    Id = Guid.NewGuid(),
                    StringProperty = "где абв эюя" + next,
                    SortableStringProperty = string.Format("эюя абв где {0:D10}", next),
                    RandomNumber = next
                });

                new DocumentApi(null).SetDocument(ConfigurationId, DocumentWithNoSorting, new
                {
                    Id = Guid.NewGuid(),
                    StringProperty1 = "где абв эюя" + next,
                    StringProperty2 = string.Format("эюя абв где {0:D10}", next),
                    RandomNumber = next
                });
            }

            ids.Sort();

            _sw.Restart();
            // Автоматически должно отсортироваться по возрастанию SortableStringProperty
            var sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneStringSortingField, null, 0, 10);
            _sw.Stop();

            Assert.AreEqual(ids.Min(), sortedData.First().RandomNumber);

            _sw.Restart();
            new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithNoSorting, null, 0, 10);
            _sw.Stop();

            
            _sw.Restart();
            // Постраничная выборка
            for (var i = 0; i < 10; i++)
                sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneStringSortingField, null, 5, 10);
            
            _sw.Stop();

            Console.WriteLine("Take page 5 with sorting: " + _sw.ElapsedMilliseconds/10 + " ms");

            Assert.AreEqual(ids.Skip(50).First(), sortedData.First().RandomNumber);

            _sw.Restart();
            for (var i = 0; i < 10; i++)
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithNoSorting, null, 5, 10);
            _sw.Stop();

            Console.WriteLine("Take page 5 without sorting: " + _sw.ElapsedMilliseconds/10 + " ms");

            // Изменение размера страницы до 100
            _sw.Restart();
            for (var i = 0; i < 10; i++)
                sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneStringSortingField, null, 5, 100);
            _sw.Stop();

            Console.WriteLine("Take page 5 (pagesize 100) with sorting: " + _sw.ElapsedMilliseconds/10 + " ms");

            Assert.AreEqual(ids.Skip(500).First(), sortedData.First().RandomNumber);

            _sw.Restart();
            for (var i = 0; i < 10; i++)
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithNoSorting, null, 5, 100);
            _sw.Stop();

            Console.WriteLine("Take page 5 (pagesize 100) without sorting: " + _sw.ElapsedMilliseconds/10 + " ms");

            // Изменение размера страницы до 1000
            _sw.Restart();
            for (var i = 0; i < 10; i++)
                sortedData = new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithOneStringSortingField, null, 5, 1000);
            _sw.Stop();

            Console.WriteLine("Take page 5 (pagesize 1000) with sorting: " + _sw.ElapsedMilliseconds/10 + " ms");

            Assert.AreEqual(ids.Skip(5000).First(), sortedData.First().RandomNumber);

            _sw.Restart();
            for (var i = 0; i < 10; i++)
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentWithNoSorting, null, 5, 1000);
            _sw.Stop();

            Console.WriteLine("Take page 5 (pagesize 1000) without sorting: " + _sw.ElapsedMilliseconds/10 + " ms");

            /* Output:
             * Take page 5 with sorting: 19 ms
             * Take page 5 without sorting: 17 ms
             * Take page 5 (pagesize 100) with sorting: 24 ms
             * Take page 5 (pagesize 100) without sorting: 22 ms
             * Take page 5 (pagesize 1000) with sorting: 89 ms
             * Take page 5 (pagesize 1000) without sorting: 84 ms
             */

        }
        
        private void CreateTestConfig()
        {
            new IndexApi().RebuildIndex(ConfigurationId, DocumentWithOneStringSortingField);
            new IndexApi().RebuildIndex(ConfigurationId, DocumentWithOneIntSortingField);
            new IndexApi().RebuildIndex(ConfigurationId, DocumentWithOneDateSortingField);
            new IndexApi().RebuildIndex(ConfigurationId, DocumentWithOneSortingFieldInArray);
            new IndexApi().RebuildIndex(ConfigurationId, DocumentWithOneSortingFieldInNestedObject);
            new IndexApi().RebuildIndex(ConfigurationId, DocumentWithTwoSortingFields);
            new IndexApi().RebuildIndex(ConfigurationId, DocumentWithInlineSorting);
         
            var managerConfiguration = ManagerFactoryConfiguration.BuildConfigurationManager(null);
            var config = managerConfiguration.CreateItem(ConfigurationId);
			managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            var managerDocument = new ManagerFactoryConfiguration(null, ConfigurationId).BuildDocumentManager();
            
            dynamic stringPropertyModel = new DynamicWrapper();
            stringPropertyModel.Type = DataType.String.ToString();
            stringPropertyModel.Caption = "Строковое поле 1";
            
            dynamic stringSortablePropertyModel = new DynamicWrapper();
            stringSortablePropertyModel.Type = DataType.String.ToString();
            stringSortablePropertyModel.Caption = "Строковое поле 2";
            stringSortablePropertyModel.Sortable = true;

            dynamic intPropertyModel = new DynamicWrapper();
            intPropertyModel.Type = DataType.Integer.ToString();
            intPropertyModel.Caption = "Числовое поле 1";
            intPropertyModel.Sortable = false;

            dynamic intSortablePropertyModel = new DynamicWrapper();
            intSortablePropertyModel.Type = DataType.Integer.ToString();
            intSortablePropertyModel.Caption = "Числовое поле 2";
            intSortablePropertyModel.Sortable = true;

            dynamic datePropertyModel = new DynamicWrapper();
            datePropertyModel.Type = DataType.DateTime.ToString();
            datePropertyModel.Caption = "Поле даты 1";
            datePropertyModel.Sortable = false;

            dynamic dateSortablePropertyModel = new DynamicWrapper();
            dateSortablePropertyModel.Type = DataType.DateTime.ToString();
            dateSortablePropertyModel.Caption = "Поле даты 2";
            dateSortablePropertyModel.Sortable = true;

            dynamic arrayPropertyModel = new DynamicWrapper();
            arrayPropertyModel.Type = DataType.Array.ToString();
            arrayPropertyModel.Caption = "Массив";
            arrayPropertyModel.Sortable = false;
            arrayPropertyModel.Items = new DynamicWrapper();
            arrayPropertyModel.Items.Type = DataType.Object.ToString();
            arrayPropertyModel.Items.Properties = new DynamicWrapper();
            arrayPropertyModel.Items.Properties.SortableStringProperty = stringSortablePropertyModel;

            dynamic objectPropertyModel = new DynamicWrapper();
            objectPropertyModel.Type = DataType.Object.ToString();
            objectPropertyModel.Caption = "Объект";
            objectPropertyModel.Sortable = false;
            objectPropertyModel.Properties = new DynamicWrapper();
            objectPropertyModel.Properties.SortableStringProperty = stringSortablePropertyModel;

            dynamic objectWithInlinePropertyModel = new DynamicWrapper();
            objectWithInlinePropertyModel.Type = DataType.Object.ToString();
            objectWithInlinePropertyModel.Caption = "Объект";
            objectWithInlinePropertyModel.Sortable = false;
            objectWithInlinePropertyModel.TypeInfo = new DynamicWrapper();
            objectWithInlinePropertyModel.TypeInfo.DocumentLink = new DynamicWrapper();
            objectWithInlinePropertyModel.TypeInfo.DocumentLink.Inline = true;
            objectWithInlinePropertyModel.TypeInfo.DocumentLink.ConfigId = ConfigurationId;
            objectWithInlinePropertyModel.TypeInfo.DocumentLink.DocumentId = DocumentWithOneStringSortingField;

            var documentWithOneStringSortingField = managerDocument.CreateItem(DocumentWithOneStringSortingField);
            dynamic schemaProperties = new DynamicWrapper();
            schemaProperties.SortableStringProperty = stringSortablePropertyModel;
            schemaProperties.StringProperty = stringPropertyModel;
            documentWithOneStringSortingField.Schema = new DynamicWrapper();
            documentWithOneStringSortingField.Schema.Type = "Object";
            documentWithOneStringSortingField.Schema.Caption = "DocumentWithOneStringSortingField";
            documentWithOneStringSortingField.Schema.Properties = schemaProperties;
            managerDocument.MergeItem(documentWithOneStringSortingField);

            var documentWithOneIntSortingField = managerDocument.CreateItem(DocumentWithOneIntSortingField);
            schemaProperties = new DynamicWrapper();
            schemaProperties.SortableIntProperty = intSortablePropertyModel;
            schemaProperties.StringProperty = stringPropertyModel;
            schemaProperties.IntProperty = intPropertyModel;
            documentWithOneIntSortingField.Schema = new DynamicWrapper();
            documentWithOneIntSortingField.Schema.Type = "Object";
            documentWithOneIntSortingField.Schema.Caption = "DocumentWithOneIntSortingField";
            documentWithOneIntSortingField.Schema.Properties = schemaProperties;

            managerDocument.MergeItem(documentWithOneIntSortingField);

            var documentWithOneDateSortingField = managerDocument.CreateItem(DocumentWithOneDateSortingField);
            schemaProperties = new DynamicWrapper();
            schemaProperties.SortableDateProperty = dateSortablePropertyModel;
            schemaProperties.StringProperty = stringPropertyModel;
            schemaProperties.IntProperty = intPropertyModel;
            documentWithOneDateSortingField.Schema = new DynamicWrapper();
            documentWithOneDateSortingField.Schema.Type = "Object";
            documentWithOneDateSortingField.Schema.Caption = "DocumentWithOneDateSortingField";
            documentWithOneDateSortingField.Schema.Properties = schemaProperties;

            managerDocument.MergeItem(documentWithOneDateSortingField);

            var documentWithOneSortingFieldInArray = managerDocument.CreateItem(DocumentWithOneSortingFieldInArray);
            schemaProperties = new DynamicWrapper();
            schemaProperties.ArrayProperty = arrayPropertyModel;
            schemaProperties.DateProperty = datePropertyModel;
            schemaProperties.StringProperty = stringPropertyModel;
            schemaProperties.IntProperty = intPropertyModel;
            documentWithOneSortingFieldInArray.Schema = new DynamicWrapper();
            documentWithOneSortingFieldInArray.Schema.Type = "Object";
            documentWithOneSortingFieldInArray.Schema.Caption = "DocumentWithOneSortingFieldInArray";
            documentWithOneSortingFieldInArray.Schema.Properties = schemaProperties;
            managerDocument.MergeItem(documentWithOneSortingFieldInArray);

            var documentWithOneSortingFieldInNestedObject = managerDocument.CreateItem(DocumentWithOneSortingFieldInNestedObject);
            schemaProperties = new DynamicWrapper();
            schemaProperties.ObjectProperty = objectPropertyModel;
            schemaProperties.DateProperty = datePropertyModel;
            schemaProperties.StringProperty = stringPropertyModel;
            schemaProperties.IntProperty = intPropertyModel;
            documentWithOneSortingFieldInNestedObject.Schema = new DynamicWrapper();
            documentWithOneSortingFieldInNestedObject.Schema.Type = "Object";
            documentWithOneSortingFieldInNestedObject.Schema.Caption = "DocumentWithOneSortingFieldInNestedObject";
            documentWithOneSortingFieldInNestedObject.Schema.Properties = schemaProperties;

            managerDocument.MergeItem(documentWithOneSortingFieldInNestedObject);

            var documentWithTwoSortingFields = managerDocument.CreateItem(DocumentWithTwoSortingFields);
            schemaProperties = new DynamicWrapper();
            schemaProperties.SortableStringProperty = stringSortablePropertyModel;
            schemaProperties.SortableIntProperty = intSortablePropertyModel;
            schemaProperties.StringProperty = stringPropertyModel;
            schemaProperties.IntProperty = intPropertyModel;
            documentWithTwoSortingFields.Schema = new DynamicWrapper();
            documentWithTwoSortingFields.Schema.Type = "Object";
            documentWithTwoSortingFields.Schema.Caption = "DocumentWithTwoSortingFields";
            documentWithTwoSortingFields.Schema.Properties = schemaProperties;
            managerDocument.MergeItem(documentWithTwoSortingFields);

            var documentWithNoSorting = managerDocument.CreateItem(DocumentWithNoSorting);
            schemaProperties = new DynamicWrapper();
            schemaProperties.StringProperty1 = stringPropertyModel;
            schemaProperties.IntProperty1 = intPropertyModel;
            schemaProperties.StringProperty2 = stringPropertyModel;
            schemaProperties.IntProperty2 = intPropertyModel;
            documentWithNoSorting.Schema = new DynamicWrapper();
            documentWithNoSorting.Schema.Type = "Object";
            documentWithNoSorting.Schema.Caption = "DocumentWithNoSorting";
            documentWithNoSorting.Schema.Properties = schemaProperties;
            managerDocument.MergeItem(documentWithNoSorting);

            var documentWithInlineSorting = managerDocument.CreateItem(DocumentWithInlineSorting);
            schemaProperties = new DynamicWrapper();
            schemaProperties.ObjectProperty = objectWithInlinePropertyModel;
            schemaProperties.DateProperty = datePropertyModel;
            schemaProperties.StringProperty = stringPropertyModel;
            schemaProperties.IntProperty = intPropertyModel;
            documentWithInlineSorting.Schema =  new DynamicWrapper();
            documentWithInlineSorting.Schema.Type = "Object";
            documentWithInlineSorting.Schema.Caption = "DocumentWithInlineSorting";
            documentWithInlineSorting.Schema.Properties = schemaProperties;
            managerDocument.MergeItem(documentWithInlineSorting);

            new UpdateApi(null).ForceReload(ConfigurationId);

            new UpdateApi(null).UpdateStore(ConfigurationId);
        }
    }
}
