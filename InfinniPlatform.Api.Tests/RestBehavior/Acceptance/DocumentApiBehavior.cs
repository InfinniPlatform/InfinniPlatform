using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class DocumentApiBehavior
    {
        private IDisposable _server;
        private const string ConfigurationId = "testdocumentapi";
        private const string DocumentId = "documentapitest";

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);

            CreateTestConfig();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        private void CreateTestConfig()
        {
            string configurationId = ConfigurationId;
            string documentId = DocumentId;

            new IndexApi().RebuildIndex(configurationId, documentId);

            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager(null);

            dynamic config = managerConfiguration.CreateItem(configurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration(null, configurationId).BuildDocumentManager();
            dynamic documentMetadata1 = managerDocument.CreateItem(documentId);

            dynamic schemaProperties = new DynamicWrapper();

            dynamic idPropertyModel = new DynamicWrapper();
            idPropertyModel.Type = DataType.String.ToString();
            idPropertyModel.Caption = "Id";
            idPropertyModel.Description = "Идетификатор";
            schemaProperties.Id = idPropertyModel;

            schemaProperties.TestProperty = new DynamicWrapper();
            schemaProperties.TestProperty.Type = DataType.String.ToString();
            schemaProperties.TestProperty.Caption = "TestProperty";
            schemaProperties.TestProperty.Description = "Тестовое свойство";

            schemaProperties.ComplexObject = new DynamicWrapper();
            schemaProperties.ComplexObject.Type = DataType.Object.ToString();
            schemaProperties.ComplexObject.Caption = "ComplexObject";
            schemaProperties.ComplexObject.TypeInfo = new DynamicWrapper();
            schemaProperties.ComplexObject.TypeInfo.Properties = new DynamicWrapper();
            schemaProperties.ComplexObject.TypeInfo.Properties.ValidProperty = new DynamicWrapper();
            schemaProperties.ComplexObject.TypeInfo.Properties.ValidProperty.Type = DataType.Integer.ToString();
            schemaProperties.ComplexObject.TypeInfo.Properties.ValidProperty.Caption = "ValidProperty";

            schemaProperties.ComplexArray = new DynamicWrapper();
            schemaProperties.ComplexArray.Type = DataType.Array.ToString();
            schemaProperties.ComplexArray.Caption = "ComplexArray";
            schemaProperties.ComplexArray.Items = new DynamicWrapper();
            schemaProperties.ComplexArray.Items.TypeInfo = new DynamicWrapper();
            schemaProperties.ComplexArray.Items.TypeInfo.Properties = new DynamicWrapper();
            schemaProperties.ComplexArray.Items.TypeInfo.Properties.ValidProperty = new DynamicWrapper();
            schemaProperties.ComplexArray.Items.TypeInfo.Properties.ValidProperty.Type = DataType.Integer.ToString();
            schemaProperties.ComplexArray.Items.TypeInfo.Properties.ValidProperty.Caption = "ValidProperty";

            documentMetadata1.Schema = new DynamicWrapper();
            documentMetadata1.Schema.Type = "Object";
            documentMetadata1.Schema.Caption = "Register document";
            documentMetadata1.Schema.Description = "Register document schema";
            documentMetadata1.Schema.Properties = schemaProperties;

            managerDocument.MergeItem(documentMetadata1);

            RestQueryApi.QueryPostNotify(null, configurationId);

            new UpdateApi(null).UpdateStore(configurationId);
        }

        [Test]
        public void ShouldDeleteDocument()
        {
            new DocumentApi(null).SetDocument(ConfigurationId, DocumentId,
                                              new
                                                  {
                                                      Id = Guid.NewGuid().ToString(),
                                                      TestProperty = "delete"
                                                  });

            IEnumerable<dynamic> items =
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentId,
                                                  filter =>
                                                  filter.AddCriteria(
                                                      cr => cr.IsEquals("delete").Property("TestProperty")), 0, 1)
                                     .ToEnumerable();

            dynamic itemId = items.First().Id;

            new DocumentApi(null).DeleteDocument(ConfigurationId, DocumentId, itemId);

            items =
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentId,
                                                  f => f.AddCriteria(c => c.Property("Id").IsEquals(itemId)), 0, 2)
                                     .ToEnumerable();
            Assert.AreEqual(0, items.Count());
        }

        [Test]
        public void ShouldDeleteExtraArrayPropertiesDuringSetDocuments()
        {
            var documents = new object[]
                {
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "111",
                            ComplexArray = new[]
                                {
                                    new
                                        {
                                            ValidProperty = 1,
                                            InvalidProperty = 2
                                        }
                                }
                        },
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "111",
                            ComplexArray = new[]
                                {
                                    new
                                        {
                                            ValidProperty = 1,
                                            InvalidProperty = 2
                                        }
                                }
                        }
                };

            new DocumentApi(null).SetDocuments(ConfigurationId, DocumentId, documents, 2);

            IEnumerable<dynamic> items =
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentId,
                                                  f => f.AddCriteria(c => c.Property("TestProperty").IsEquals("111")), 0,
                                                  10).ToEnumerable();
            Assert.AreEqual(items.Count(), 2);

            dynamic firstDoc = items.First();

            Assert.NotNull(firstDoc.ComplexArray);
            Assert.NotNull(firstDoc.ComplexArray[0].ValidProperty);
            Assert.Null(firstDoc.ComplexArray[0].InvalidProperty);
        }

        [Test]
        public void ShouldDeleteExtraObjectPropertiesDuringSetDocuments()
        {
            var documents = new object[]
                {
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "123",
                            ComplexObject = new
                                {
                                    ValidProperty = 1,
                                    InvalidProperty = 2,
                                }
                        },
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "123",
                            ComplexObject = new
                                {
                                    ValidProperty = 1,
                                    InvalidProperty = 2,
                                }
                        }
                };

            new DocumentApi(null).SetDocuments(ConfigurationId, DocumentId, documents, 2);

            IEnumerable<dynamic> items =
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentId,
                                                  f => f.AddCriteria(c => c.Property("TestProperty").IsEquals("123")), 0,
                                                  10).ToEnumerable();
            Assert.Greater(items.Count(), 0);

            dynamic firstDoc = items.First();

            Assert.NotNull(firstDoc.ComplexObject.ValidProperty);
            Assert.Null(firstDoc.ComplexObject.InvalidProperty);
        }

        [Test]
        public void ShouldDeleteExtraPropertiesDuringSetDocuments()
        {
            var documents = new object[]
                {
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "1",
                            PropertyToDelete = "DeleteMe"
                        },
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "2",
                            PropertyToDelete = "DeleteMe"
                        }
                };

            new DocumentApi(null).SetDocuments(ConfigurationId, DocumentId, documents, 2);

            IEnumerable<dynamic> items =
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentId, null, 0, 10).ToEnumerable();
            Assert.Greater(items.Count(), 0);

            dynamic firstDoc = items.First();

            Assert.NotNull(firstDoc.TestProperty);
            Assert.Null(firstDoc.PropertyToDelete);
        }

        [Test]
        public void ShouldGetDocumentCrossConfig()
        {
            new DocumentApi(null).SetDocument(ConfigurationId, DocumentId,
                                              new
                                                  {
                                                      Id = Guid.NewGuid().ToString(),
                                                      TestProperty = "crossget"
                                                  });

            var items =
                new DocumentApi(null).GetDocumentCrossConfig(
                    filter => filter.AddCriteria(cr => cr.IsEquals("crossget").Property("TestProperty")), 0, 10,
                    new[] {ConfigurationId}, new[] {DocumentId}).ToEnumerable();
            Assert.AreEqual(items.Count(), 1);

            items =
                new DocumentApi(null).GetDocumentCrossConfig(
                    filter => filter.AddCriteria(cr => cr.IsEquals("crossget").Property("TestProperty")), 0, 10,
                    new[] {ConfigurationId}, new[] {DocumentId}).ToEnumerable();
            Assert.AreEqual(items.Count(), 1);

            items =
                new DocumentApi(null).GetDocumentCrossConfig(
                    filter => filter.AddCriteria(cr => cr.IsEquals("crossget").Property("TestProperty")), 0, 10,
                    new[] {"sdf"}, new[] {DocumentId, "ds"}).ToEnumerable();
            Assert.AreEqual(items.Count(), 0);

            items =
                new DocumentApi(null).GetDocumentCrossConfig(
                    filter => filter.AddCriteria(cr => cr.IsEquals("crossget").Property("TestProperty")), 0, 10,
                    new[] {ConfigurationId, "update"}, new[] {DocumentId}).ToEnumerable();
            Assert.AreEqual(items.Count(), 1);

            items =
                new DocumentApi(null).GetDocumentCrossConfig(
                    filter => filter.AddCriteria(cr => cr.IsEquals("crossget").Property("TestProperty")), 0, 10,
                    new[] {ConfigurationId}, new[] {"ds"}).ToEnumerable();
            Assert.AreEqual(items.Count(), 0);

            items =
                new DocumentApi(null).GetDocumentCrossConfig(
                    filter => filter.AddCriteria(cr => cr.IsEquals("crossget").Property("TestProperty")), 0, 10,
                    new[] {ConfigurationId, "systemconfig"}, new[] {"sdf", DocumentId, "ds"}).ToEnumerable();
            Assert.AreEqual(items.Count(), 1);
        }

        [Test]
        public void ShouldGetDocuments()
        {
            new DocumentApi(null).SetDocument(ConfigurationId, DocumentId,
                                              new
                                                  {
                                                      Id = Guid.NewGuid().ToString(),
                                                      TestProperty = "get"
                                                  });

            IEnumerable<dynamic> items =
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentId,
                                                  filter =>
                                                  filter.AddCriteria(cr => cr.IsEquals("get").Property("TestProperty")),
                                                  0, 10).ToEnumerable();
            Assert.AreEqual(items.Count(), 1);
        }

        [Test]
        public void ShouldReturnCorrectMessageAfterSetDocumentWithIncorrectSchema()
        {
            new DocumentApi(null).SetDocument(ConfigurationId, DocumentId, new
                {
                    Id = Guid.NewGuid().ToString(),
                    StringProperty = "StringValue1",
                    NumberProperty = 1,
                    DateProperty = DateTime.Now,
                    BoolProperty = true,
                    ObjectProperty = new
                        {
                            NestedStringProperty = "NestedStringValue1",
                            NestedNumberPrperty = 2,
                            NestedDateProperty = DateTime.Now,
                            NestedBoolProperty = false,
                        }
                }, false, true);

            new DocumentApi(null).SetDocument(ConfigurationId, DocumentId, new
                {
                    Id = Guid.NewGuid().ToString(),
                    StringProperty = "StringValue2",
                    NumberProperty = 2,
                    DateProperty = DateTime.Now,
                    BoolProperty = true,
                    ObjectProperty = new
                        {
                            NestedStringProperty = "NestedStringValue2",
                            NestedNumberPrperty = 3,
                            NestedDateProperty = DateTime.Now,
                            NestedBoolProperty = false,
                        }
                }, false, true);

            string errorMessage = "";

            try
            {
                new DocumentApi(null).SetDocument(ConfigurationId, DocumentId, new
                    {
                        Id = Guid.NewGuid().ToString(),
                        StringProperty = 1,
                        NumberProperty = "1",
                        DateProperty = "Now",
                        BoolProperty = "true",
                        ObjectProperty = new
                            {
                                NestedStringProperty = 2,
                                NestedNumberPrperty = "2",
                                NestedDateProperty = 3,
                                NestedBoolProperty = "false",
                            }
                    }, false, true);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            // Сообщение об ошибке должно содержать фразы вида:
            // Expected value for field 'StringProperty' should have String type, but value has System.Int64 type ('1')

            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
            Assert.IsTrue(errorMessage.Contains("Expected value for field"));
        }

        [Test]
        public void ShouldReturnCorrectMessageAfterSetDocumentsWithIncorrectSchema()
        {
            new DocumentApi(null).SetDocuments(ConfigurationId, DocumentId, new[]
                {
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            StringProperty = "StringValue1",
                            NumberProperty = 1,
                            DateProperty = DateTime.Now,
                            BoolProperty = true,
                            ObjectProperty = new
                                {
                                    NestedStringProperty = "NestedStringValue1",
                                    NestedNumberPrperty = 2,
                                    NestedDateProperty = DateTime.Now,
                                    NestedBoolProperty = false,
                                }
                        }
                }, 200, true);

            new DocumentApi(null).SetDocuments(ConfigurationId, DocumentId, new[]
                {
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            StringProperty = "StringValue2",
                            NumberProperty = 2,
                            DateProperty = DateTime.Now,
                            BoolProperty = true,
                            ObjectProperty = new
                                {
                                    NestedStringProperty = "NestedStringValue2",
                                    NestedNumberPrperty = 3,
                                    NestedDateProperty = DateTime.Now,
                                    NestedBoolProperty = false,
                                }
                        }
                }, 200, true);

            string errorMessage = "";

            try
            {
                new DocumentApi(null).SetDocuments(ConfigurationId, DocumentId, new[]
                    {
                        new
                            {
                                Id = Guid.NewGuid().ToString(),
                                StringProperty = 1,
                                NumberProperty = "1",
                                DateProperty = "Now",
                                BoolProperty = "true",
                                ObjectProperty = new
                                    {
                                        NestedStringProperty = 2,
                                        NestedNumberPrperty = "2",
                                        NestedDateProperty = 3,
                                        NestedBoolProperty = "false",
                                    }
                            }
                    }, 200, true);
            }
            catch (ArgumentException e)
            {
                errorMessage = e.Message;
            }

            // Сообщение об ошибке должно содержать фразы вида:
            // Expected value for field 'StringProperty' should have String type, but value has System.Int64 type ('1')

            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
            Assert.IsTrue(errorMessage.Contains("Expected value for field"));
        }

        [Test]
        public void ShouldSetDocuments()
        {
            var documents = new object[]
                {
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "1"
                        },
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "1"
                        },
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "1"
                        },
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "1"
                        },
                    new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TestProperty = "1"
                        }
                };

            new DocumentApi(null).SetDocuments(ConfigurationId, DocumentId, documents, 2);

            IEnumerable<dynamic> items =
                new DocumentApi(null).GetDocument(ConfigurationId, DocumentId,
                                                  filter =>
                                                  filter.AddCriteria(cr => cr.IsEquals(1).Property("TestProperty")), 0,
                                                  10).ToEnumerable();
            Assert.Greater(items.Count(), 0);
        }
    }
}