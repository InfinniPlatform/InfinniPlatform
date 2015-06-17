using System;
using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Index;

using NUnit.Framework;

namespace InfinniPlatform.Metadata.Tests.HandlersBehavior
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class SearchAggregationBehavior
    {
        private const string Organization1PublicId = "86C26C50-C804-47A0-ADCD-88C52ACFBCDC";
        private const string Organization2PublicId = "3E51A7FF-D240-4E5E-B94F-045E872CD024";
        private const string Organization3PublicId = "5C7D7B7C-8FFE-4C24-8236-4F665F3F7AF3";
        private const string DocumentType1 = "3F95F4C5-CA9C-4F4F-A744-4C21F56E416C";
        private const string DocumentType2 = "EFDE8450-7E37-4FF7-B084-E642E7EEAA4F";

		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(p => p.SetHostingConfig(TestSettings.DefaultHostingConfig)
												.AddConfigurationFromAssembly("InfinniPlatform.Metadata.Tests"));

            InitData();
        }

        private void InitData()
        {
            new IndexApi().RebuildIndex("integration", "document");
            new IndexApi().RebuildIndex("integration", "organization");

            //документы первой организации
            dynamic document1 = new
            {
                Id = Guid.NewGuid().ToString(),
                Type = new
                {
                    Code = DocumentType1,
                    DisplayName = "Талон",
                },
                OrganizationId = Organization1PublicId,
                Rating = 1,
                Rating1 = 3.5
            };

            dynamic document2 = new
            {
                Id = Guid.NewGuid().ToString(),
                Type = new
                {
                    Code = DocumentType1,
                    DisplayName = "Талон"
                },
                OrganizationId = Organization1PublicId,
                Rating = 2,
                Rating1 = 0.6
            };

            dynamic document3 = new
            {
                Id = Guid.NewGuid().ToString(),
                Type = new
                {
                    Code = DocumentType2,
                    DisplayName = "Статформа"
                },
                OrganizationId = Organization1PublicId,
                Rating = 1,
                Rating1 = 1
            };

            dynamic document4 = new
            {
                Id = Guid.NewGuid().ToString(),
                Type = new
                {
                    Code = DocumentType1,
                    DisplayName = "Еще какой-то документ"
                },
                OrganizationId = Organization1PublicId,
                Rating = 1,
                Rating1 = 1
            };

            //документы второй организации
            dynamic document5 = new
            {
                Id = Guid.NewGuid().ToString(),
                Type = new
                {
                    Code = DocumentType2,
                    DisplayName = "Еще какой-то документ"
                },
                OrganizationId = Organization2PublicId,
                Rating = 1,
                Rating1 = 1
            };

            dynamic document6 = new
            {
                Id = Guid.NewGuid().ToString(),
                Type = new
                {
                    Code = DocumentType1,
                    DisplayName = "Талон"
                },
                OrganizationId = Organization2PublicId,
                Rating = 1,
                Rating1 = 1
            };
            //у третьей организации нет документов

            //создаем организации
            dynamic organization1 = new
            {
                Id = Guid.NewGuid().ToString(),
                Code = "001",
                ShortName = "ГКБ №1",
                FullName = "Больница 1",
                PublicId = Organization1PublicId
            };

            dynamic organization2 = new
            {
                Id = Guid.NewGuid().ToString(),
                PublicId = Organization2PublicId,
                Code = "002",
                ShortName = "ГКБ №2",
                FullName = "Больница 2"
            };

            dynamic organization3 = new
            {
                Id = Guid.NewGuid().ToString(),
                PublicId = Organization3PublicId,
                Code = "003",
                ShortName = "ГКБ №3",
                FullName = "Больница 3"
            };

            new IndexApi().InsertDocumentWithTimestamp(organization1, DateTime.Now, "integration", "organization");
            new IndexApi().InsertDocumentWithTimestamp(organization2, DateTime.Now, "integration", "organization");
            new IndexApi().InsertDocumentWithTimestamp(organization3, DateTime.Now, "integration", "organization");

            new IndexApi().InsertDocumentWithTimestamp(document1, DateTime.Now.AddDays(-25), "integration", "document");
            new IndexApi().InsertDocumentWithTimestamp(document2, DateTime.Now.AddDays(-20), "integration", "document");
            new IndexApi().InsertDocumentWithTimestamp(document3, DateTime.Now, "integration", "document");
            new IndexApi().InsertDocumentWithTimestamp(document4, DateTime.Now, "integration", "document");
            new IndexApi().InsertDocumentWithTimestamp(document5, DateTime.Now.AddDays(20), "integration", "document");
            new IndexApi().InsertDocumentWithTimestamp(document6, DateTime.Now.AddDays(25), "integration", "document");

        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldSearchDocumentWithoutFilter()
        {
            dynamic result = RestQueryApi.QueryAggregationRaw(
                "Handlers",
                "patienttest",
                "checkaggregation",
                "integration",
                "document",
                null,
                new[]
                {
                    new
                    {
                        Label = "organization_term",
                        FieldName = "OrganizationId",
                        DimensionType = DimensionType.Term
                    },
                    new
                    {
                        Label = "type_term",
                        FieldName = "Type.Code",
                        DimensionType = DimensionType.Term
                    }
                }, new AggregationType[0], new string[0],
                0,
                10)
                .ToDynamicList();

            // Количество разных организаций, имеющих документы
            Assert.AreEqual(2, result.Count);

            foreach (var organization in result)
            {
                if (organization.Name == Organization1PublicId.ToLowerInvariant())
                {
                    Assert.AreEqual(2, organization.Buckets.Count);

                    // Количество документов первого типа в первой организации
                    foreach (var type in organization.Buckets)
                    {
                        if (type.Name == DocumentType1.ToLowerInvariant())
                        {
                            Assert.AreEqual(3, type.DocCount);
                        }
                        else if (type.Name == DocumentType2.ToLowerInvariant())
                        {
                            Assert.AreEqual(1, type.DocCount);
                        }
                        else
                        {
                            Assert.Fail("Wrong document type");
                        }
                    }
                }
                else if (organization.Name == Organization2PublicId.ToLowerInvariant())
                {
                    Assert.AreEqual(2, organization.Buckets.Count);
                }
                else
                {
                    Assert.Fail("Wrong organization id");
                }
            }

        }

        [Test]
        public void ShouldSearchDocumentByRange()
        {
            dynamic result = RestQueryApi.QueryAggregationRaw(
                "Handlers",
                "patienttest",
                "checkaggregation",
                "integration",
                "document",
                null,
                new object[]
                {
                    new
                    {
                        Label = "type_term",
                        FieldName = "Type.Code",
                        DimensionType = DimensionType.Term
                    },
                    new
                    {
                        Label = "rating_1",
                        FieldName = "Rating1",
                        DimensionType = DimensionType.Range,
                        ValueSet = new
                        {
                            Property = "Rating",
                            CriteriaType = CriteriaType.ValueSet,
                            Value = "0.9\n1.1"
                        }
                    }
                },
                new AggregationType[0], new string[0],
                0,
                10)
                .ToDynamicList();

            // Количество разных организаций, имеющих документы
            Assert.AreEqual(2, result.Count);

            foreach (var docCode in result)
            {
                if (docCode.Name == DocumentType1.ToLowerInvariant())
                {
                    Assert.AreEqual(3, docCode.Buckets.Count);

                    // rating 1 < 0.9
                    Assert.AreEqual(1, docCode.Buckets.ToArray()[0].DocCount);
                    // rating 1 = 1
                    Assert.AreEqual(2, docCode.Buckets.ToArray()[1].DocCount);
                    // rating 1 > 1.1
                    Assert.AreEqual(1, docCode.Buckets.ToArray()[2].DocCount);
                }
                else if (docCode.Name == DocumentType2.ToLowerInvariant())
                {
                    Assert.AreEqual(3, docCode.Buckets.Count);
                }
                else
                {
                    Assert.Fail("Wrong organization id");
                }
            }

        }

        [Test]
        public void ShouldSearchDocumentWithFilter()
        {
            var filterObject = new List<dynamic>();
            dynamic filterContentId = new
            {
                Property = "Type.DisplayName",
                Value = "Талон",
                CriteriaType = CriteriaType.IsEquals
            };

            filterObject.Add(filterContentId);
            
            dynamic result = RestQueryApi.QueryAggregationRaw(
                "Handlers",
                "patienttest",
                "checkaggregation",
                "integration",
                "document",
                filterObject,
                new[]
                {
                    new
                    {
                        Label = "organization_term",
                        FieldName = "OrganizationId",
                        DimensionType = DimensionType.Term
                    },
                    new
                    {
                        Label = "type_term",
                        FieldName = "Type.Code",
                        DimensionType = DimensionType.Term
                    }
                },
                new AggregationType[0], new string[0],
                0,
                10)
                .ToDynamicList();

            // Количество разных организаций, имеющих документы
            Assert.AreEqual(2, result.Count);

            foreach (var organization in result)
            {
                if (organization.Name == Organization1PublicId.ToLowerInvariant())
                {
                    Assert.AreEqual(1, organization.Buckets.Count);

                    // Количество документов первого типа в первой организации
                    foreach (var type in organization.Buckets)
                    {
                        if (type.Name == DocumentType1.ToLowerInvariant())
                        {
                            Assert.AreEqual(2, type.DocCount);
                        }
                        else if (type.Name == DocumentType2.ToLowerInvariant())
                        {
                            Assert.AreEqual(1, type.DocCount);
                        }
                        else
                        {
                            Assert.Fail("Wrong document type");
                        }
                    }
                }
                else if (organization.Name == Organization2PublicId.ToLowerInvariant())
                {
                    Assert.AreEqual(1, organization.Buckets.Count);
                }
                else
                {
                    Assert.Fail("Wrong organization id");
                }
            }

        }
    }
}
