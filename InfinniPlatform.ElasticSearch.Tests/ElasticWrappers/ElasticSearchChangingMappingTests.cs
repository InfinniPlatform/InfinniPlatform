using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.ElasticSearch.Tests.Builders;
using InfinniPlatform.ElasticSearch.Versioning;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

using NUnit.Framework;

namespace InfinniPlatform.ElasticSearch.Tests.ElasticWrappers
{
    /// <summary>
    /// Проверка функционала по изменению настроек маппинга
    /// </summary>
    [TestFixture]
	[NUnit.Framework.Category(TestCategories.IntegrationTest)]
    public sealed class ElasticSearchChangingMappingTests
    {
        private const string IndexName = "mapping_unit_tests_index";
        
        private ElasticConnection _elasticConnection;
        private ElasticTypeManager _elasticTypeManager;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _elasticConnection = ElasticFactoryBuilder.ElasticConnection.Value;
            _elasticTypeManager = ElasticFactoryBuilder.ElasticTypeManager.Value;
        }

        [Test]
        public void AddingObjectWithCorrectMappingTest()
        {
            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.String),
                new PropertyMapping("IsActive", PropertyDataType.Boolean),
                new PropertyMapping("Birthdate", PropertyDataType.Date),
                new PropertyMapping("Int", PropertyDataType.Integer),
                new PropertyMapping("Float", PropertyDataType.Float),
                new PropertyMapping("AttachedData", PropertyDataType.Binary),
                new PropertyMapping("Hobbies", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String),
                        new PropertyMapping("StartingDate", PropertyDataType.Date)
                    })
            };

            _elasticTypeManager.CreateType(IndexName,IndexName, initialMapping);
            
            // Пробуем добавить в индекс объект с корректной схемой данных
			var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);

            
            var hobby = new DynamicWrapper
                        {
                            ["ActivityName"] = "Football",
                            ["StartingDate"] = new DateTime(1980, 1, 1)
                        };
            var expando = new DynamicWrapper
                          {
                              ["FirstName"] = "Ivan",
                              ["IsActive"] = true,
                              ["Birthdate"] = new DateTime(1975, 1, 1),
                              ["Int"] = 2,
                              ["Float"] = 2.3,
                              ["Hobbies"] = hobby,
                              ["AttachedData"] = new
                                                 {
                                                     Info = new
                                                            {
                                                                Comment = "some binary data info"
                                                            }
                                                 },
                              ["Id"] = Guid.NewGuid().ToString().ToLowerInvariant()
                          };

            elasticSearchProvider.Set(expando);
            elasticSearchProvider.Refresh();
            
            Assert.AreEqual(1, elasticSearchProvider.GetTotalCount());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddingObjectWithIncorrectMappingTest()
        {
            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.String),
                new PropertyMapping("IsActive", PropertyDataType.Boolean),
                new PropertyMapping("Birthdate", PropertyDataType.Date),
                new PropertyMapping("Int", PropertyDataType.Integer),
                new PropertyMapping("Float", PropertyDataType.Float),
                new PropertyMapping("Hobbies", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String),
                        new PropertyMapping("StartingDate", PropertyDataType.Date)
                    })
            };

            _elasticTypeManager.CreateType(IndexName, IndexName, initialMapping);

			var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);

            IDictionary<string, object> expando = new ExpandoObject();
            IDictionary<string, object> hobby = new ExpandoObject();
            hobby.Add("ActivityName", "Football");
            hobby.Add("StartingDate", new DateTime(1980, 1, 1));
            expando.Add("FirstName", "Ivan"); 
            expando.Add("IsActive", true);
            expando.Add("Birthdate", new DateTime(1975, 1, 1));
            expando.Add("Int", "2incorrect"); // incorrect value 
            expando.Add("Hobbies", hobby);

            dynamic dynObject1 = (ExpandoObject)expando;
            dynObject1.Id = Guid.NewGuid().ToString().ToLowerInvariant();

            // Пробуем добавить в индекс объект с некорректной схемой данных
            // Вызов метода приведет к исключению
            elasticSearchProvider.Set(dynObject1);
        }

        [Test]
        public void GetIndexTypeMappingTest()
        {
            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.String),
                new PropertyMapping("IsActive", PropertyDataType.Boolean),
                new PropertyMapping("Birthdate", PropertyDataType.Date),
                new PropertyMapping("Int", PropertyDataType.Integer),
                new PropertyMapping("Float", PropertyDataType.Float),
                new PropertyMapping("AttachedData", PropertyDataType.Binary),
                new PropertyMapping("Hobbies", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String),
                        new PropertyMapping("StartingDate", PropertyDataType.Date)
                    })
            };

            _elasticTypeManager.CreateType(IndexName, IndexName, initialMapping);

            var mapping = _elasticTypeManager.GetPropertyMappings(IndexName, IndexName);

            var firstNameProp = mapping.FirstOrDefault(p => p.Name == "FirstName");

            Assert.IsNotNull(firstNameProp);
            Assert.AreEqual(PropertyDataType.String, firstNameProp.DataType);

            var hobbiesProp = mapping.FirstOrDefault(p => p.Name == "Hobbies");

            Assert.IsNotNull(hobbiesProp);
            Assert.AreEqual(2, hobbiesProp.ChildProperties.Count);
        }

        [Test]
        public void ChangingIndexMappingTest()
        {
            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.String, true),
                new PropertyMapping("IsActive", PropertyDataType.Boolean, true),
                new PropertyMapping("Birthdate", PropertyDataType.Date, true),
                new PropertyMapping("Int", PropertyDataType.Integer),
                new PropertyMapping("Float", PropertyDataType.Float),
                new PropertyMapping("Hobbies", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String, true),
                        new PropertyMapping("StartingDate", PropertyDataType.Date, true)
                    })
            };

            _elasticTypeManager.CreateType(IndexName,IndexName, initialMapping);

            // Пробуем добавить в индекс объект с корректной схемой данных
			var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);

            IDictionary<string, object> expando = new ExpandoObject();
            IDictionary<string, object> hobby = new ExpandoObject();
            hobby.Add("ActivityName", "Football");
            hobby.Add("StartingDate", new DateTime(1980, 1, 1));
            expando.Add("FirstName", "Ivan");
            expando.Add("IsActive", true);
            expando.Add("Birthdate", new DateTime(1975, 1, 1));
            expando.Add("Int", 2);
            expando.Add("Hobbies", hobby);

            dynamic dynObject1 = (ExpandoObject)expando;
            dynObject1.Id = Guid.NewGuid().ToString().ToLowerInvariant();

            elasticSearchProvider.Set(dynObject1);
            elasticSearchProvider.Refresh();

            Assert.AreEqual(1, elasticSearchProvider.GetTotalCount());
            
            var newMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.String),
                new PropertyMapping("IsActive", PropertyDataType.Boolean),
                new PropertyMapping("Birthdate", PropertyDataType.Date),
                new PropertyMapping("Int", PropertyDataType.String), // was int
                new PropertyMapping("Hobbies", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String),
                        new PropertyMapping("StartingDate", PropertyDataType.Date)
                    })
            };

            _elasticTypeManager.CreateType(IndexName, IndexName, newMapping);
            

            IDictionary<string, object> expando2 = new ExpandoObject();
            expando2.Add("FirstName", "Oleg");
            expando2.Add("IsActive", false);
            expando2.Add("Birthdate", new DateTime(1970, 1, 1));
            expando2.Add("Int", "2correct"); // still correct value 
            expando2.Add("Hobbies", hobby);

            dynamic dynObject2 = (ExpandoObject)expando2;
            dynObject2.Id = Guid.NewGuid().ToString().ToLowerInvariant();

			elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);
			// Вызов метода не должен привести к исключению
            elasticSearchProvider.Set(dynObject2);
            elasticSearchProvider.Refresh();

            Assert.NotNull(elasticSearchProvider.GetItem(dynObject1.Id));
            Assert.NotNull(elasticSearchProvider.GetItem(dynObject2.Id));

            Assert.AreEqual(2, elasticSearchProvider.GetTotalCount());
        }

        [Test]
        public void QueryOnIndexWithInitialMappingSet()
        {
            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FoundationDate", PropertyDataType.Date),
                new PropertyMapping("Street", PropertyDataType.String),
                new PropertyMapping("HouseNumber", PropertyDataType.Integer),
                new PropertyMapping("Name", PropertyDataType.String),
                new PropertyMapping("Rating", PropertyDataType.Float),
                new PropertyMapping("Principal", new[]
                    {
                        new PropertyMapping("Name", PropertyDataType.String),
                        new PropertyMapping("LastName", PropertyDataType.String),
                        new PropertyMapping("BirthDate", PropertyDataType.Date),
                        new PropertyMapping("Grade", PropertyDataType.Integer),
                        new PropertyMapping("KnowledgeRating", PropertyDataType.Float)
                    }),
                new PropertyMapping("Students", new[]
                    {
                        new PropertyMapping("Name", PropertyDataType.String),
                        new PropertyMapping("LastName", PropertyDataType.String),
                        new PropertyMapping("BirthDate", PropertyDataType.Date),
                        new PropertyMapping("FavoriteSubject", PropertyDataType.String),
                        new PropertyMapping("KnowledgeRating", PropertyDataType.Float),
                        new PropertyMapping("CountOfFriends", PropertyDataType.Integer)
                    })
            };

            _elasticTypeManager.CreateType(IndexName, IndexName, initialMapping);

			var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);

            foreach (var school in SchoolsFactory.CreateSchools())
            {
                // Преобразование школы в объект типа dynamic
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                    expando.Add(property.Name, property.GetValue(school));

                dynamic dynSchool = (ExpandoObject)expando;
                dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();

                elasticSearchProvider.Set(dynSchool);
            }

            elasticSearchProvider.Refresh();

			var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor(IndexName, IndexName);

            var searchModel = new SearchModel();
            var filterFactory = FilterBuilderFactory.GetInstance();
            searchModel.AddFilter(filterFactory.Get("Principal.LastName", "Monakhov", CriteriaType.IsEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void QueryOnIndexWithIncorrectInitialMapping()
        {
            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FoundationDate", PropertyDataType.Date),
                new PropertyMapping("Street", PropertyDataType.String),
                new PropertyMapping("HouseNumber", PropertyDataType.Integer),
                new PropertyMapping("Name", PropertyDataType.Integer), // should be string here
                new PropertyMapping("Rating", PropertyDataType.Float),
                new PropertyMapping("Principal", new[]
                    {
                        new PropertyMapping("Name", PropertyDataType.String),
                        new PropertyMapping("LastName", PropertyDataType.String),
                        new PropertyMapping("BirthDate", PropertyDataType.Date),
                        new PropertyMapping("Grade", PropertyDataType.Integer),
                        new PropertyMapping("KnowledgeRating", PropertyDataType.Float)
                    }),
                new PropertyMapping("Students", new[]
                    {
                        new PropertyMapping("Name", PropertyDataType.String),
                        new PropertyMapping("LastName", PropertyDataType.String),
                        new PropertyMapping("BirthDate", PropertyDataType.Date),
                        new PropertyMapping("FavoriteSubject", PropertyDataType.String),
                        new PropertyMapping("KnowledgeRating", PropertyDataType.Float),
                        new PropertyMapping("CountOfFriends", PropertyDataType.Integer)
                    })
            };

            _elasticTypeManager.CreateType(IndexName, IndexName, initialMapping);

			var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);

            var school = SchoolsFactory.CreateSchools().FirstOrDefault();
            
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                expando.Add(property.Name, property.GetValue(school));
            
            dynamic dynSchool = (ExpandoObject)expando;
            dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();
            
            // this call will rise an exception
            elasticSearchProvider.Set(dynSchool);
        }

        [Test]
        public void QueryOnIndexWithChangedMapping()
        {
            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("ExtraField", PropertyDataType.String),
                new PropertyMapping("FoundationDate", PropertyDataType.Date),
                new PropertyMapping("Street", PropertyDataType.String),
                new PropertyMapping("HouseNumber", PropertyDataType.Integer),
                new PropertyMapping("Name", PropertyDataType.String),
                new PropertyMapping("Rating", PropertyDataType.Float),
                new PropertyMapping("Principal", new[]
                    {
                        new PropertyMapping("Name", PropertyDataType.String),
                        new PropertyMapping("LastName", PropertyDataType.String),
                        new PropertyMapping("BirthDate", PropertyDataType.Date),
                        new PropertyMapping("Grade", PropertyDataType.Integer),
                        new PropertyMapping("KnowledgeRating", PropertyDataType.Float)
                    }),
                new PropertyMapping("Students", new[]
                    {
                        new PropertyMapping("Name", PropertyDataType.String),
                        new PropertyMapping("LastName", PropertyDataType.String),
                        new PropertyMapping("BirthDate", PropertyDataType.Date),
                        new PropertyMapping("FavoriteSubject", PropertyDataType.String),
                        new PropertyMapping("KnowledgeRating", PropertyDataType.Float),
                        new PropertyMapping("CountOfFriends", PropertyDataType.Integer)
                    })
            };

            _elasticTypeManager.CreateType(IndexName, IndexName, initialMapping);

            var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);

            foreach (var school in SchoolsFactory.CreateSchools())
            {
                // Преобразование школы в объект типа dynamic
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                    expando.Add(property.Name, property.GetValue(school));

                dynamic dynSchool = (ExpandoObject)expando;
                dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();
                dynSchool.ExtraField = "15";

                elasticSearchProvider.Set(dynSchool);
            }

            elasticSearchProvider.Refresh();


            var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor(IndexName,IndexName);

            var searchModel = new SearchModel();
            var filterFactory = FilterBuilderFactory.GetInstance();
            searchModel.AddFilter(filterFactory.Get("ExtraField", "15", CriteriaType.IsEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(3, result.HitsCount);
            
            // Change mapping 
            var changedMapping = new List<PropertyMapping>
            {
                new PropertyMapping("ExtraField", PropertyDataType.Integer),
                new PropertyMapping("FoundationDate", PropertyDataType.Date),
                new PropertyMapping("Street", PropertyDataType.String),
                new PropertyMapping("HouseNumber", PropertyDataType.Integer),
                new PropertyMapping("Name", PropertyDataType.String),
                new PropertyMapping("Rating", PropertyDataType.Float),
                new PropertyMapping("Principal", new[]
                    {
                        new PropertyMapping("Name", PropertyDataType.String),
                        new PropertyMapping("LastName", PropertyDataType.String),
                        new PropertyMapping("BirthDate", PropertyDataType.Date),
                        new PropertyMapping("Grade", PropertyDataType.Integer),
                        new PropertyMapping("KnowledgeRating", PropertyDataType.Float)
                    }),
                new PropertyMapping("Students", new[]
                    {
                        new PropertyMapping("Name", PropertyDataType.String),
                        new PropertyMapping("LastName", PropertyDataType.String),
                        new PropertyMapping("BirthDate", PropertyDataType.Date),
                        new PropertyMapping("FavoriteSubject", PropertyDataType.String),
                        new PropertyMapping("KnowledgeRating", PropertyDataType.Float),
                        new PropertyMapping("CountOfFriends", PropertyDataType.Integer)
                    })
            };

            _elasticTypeManager.CreateType(IndexName, IndexName, changedMapping);

            foreach (var school in SchoolsFactory.CreateSchools())
            {
                // Преобразование школы в объект типа dynamic
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                    expando.Add(property.Name, property.GetValue(school));

                dynamic dynSchool = (ExpandoObject)expando;
                dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();
                dynSchool.ExtraField = "15";

                elasticSearchProvider.Set(dynSchool);
            }

            elasticSearchProvider.Refresh();
            
            var resultAfterMappingChanges = executor.Query(searchModel);

            Assert.AreEqual(6, resultAfterMappingChanges.HitsCount);

            // several additional search to make sure that searching under 2 indeces works
            searchModel = new SearchModel();
            var filter = filterFactory.Get("Principal.LastName", "nak", CriteriaType.IsNotContains);
            searchModel.AddFilter(filter);
            var responce = executor.Query(searchModel);
            var cnt = responce.HitsCount;
            Assert.AreEqual(4, cnt);

            searchModel.AddFilter(filterFactory.Get("Rating", 3.1, CriteriaType.IsMoreThan));
            Assert.AreEqual(2, executor.Query(searchModel).HitsCount);
            
            searchModel = new SearchModel();
            searchModel.AddFilter(filterFactory.Get("HouseNumber", 31, CriteriaType.IsEquals));
            Assert.AreEqual(2, executor.Query(searchModel).HitsCount);

            searchModel = new SearchModel();
            searchModel.AddFilter(filterFactory.Get("FoundationDate", new DateTime(1988, 10, 2), CriteriaType.IsLessThan));
            Assert.AreEqual(4, executor.Query(searchModel).HitsCount);
        }

        [Test]
        public void ChangingIndexTypeMappingTest()
        {
            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.String),
                new PropertyMapping("IsActive", PropertyDataType.Boolean),
                new PropertyMapping("Birthdate", PropertyDataType.Date),
                new PropertyMapping("Number", PropertyDataType.Integer),
                new PropertyMapping("AttachedData", PropertyDataType.Binary),
                new PropertyMapping("Hobbies", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String),
                        new PropertyMapping("StartingDate", PropertyDataType.Date)
                    })
            };

            _elasticTypeManager.CreateType(IndexName, IndexName, initialMapping);

            _elasticConnection.Refresh();

            var vbuilder = new VersionBuilder(_elasticTypeManager, IndexName, IndexName);

            Assert.IsTrue(vbuilder.VersionExists(initialMapping));

            var conflictMapping1 = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.Date)
            };
            Assert.IsFalse(vbuilder.VersionExists(conflictMapping1));

            var conflictMapping2 = new List<PropertyMapping>
            {
                new PropertyMapping("AttachedData", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String),
                        new PropertyMapping("StartingDate", PropertyDataType.Date)
                    })
            };
            Assert.IsFalse(vbuilder.VersionExists(conflictMapping2));

            var conflictMapping3 = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.Integer),
                new PropertyMapping("NewProperty", PropertyDataType.Date)
            };
            Assert.IsFalse(vbuilder.VersionExists(conflictMapping3));
        }

        [Test]
        public void ChangingIndexWithVersionHistoryMappingTest()
        {
            var versionBuilder = ElasticFactoryBuilder.GetElasticFactory().BuildVersionBuilder(IndexName, IndexName);

            var initialMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.String),
                new PropertyMapping("IsActive", PropertyDataType.Object),
                new PropertyMapping("Birthdate", PropertyDataType.Date),
                new PropertyMapping("Number", PropertyDataType.Integer),
                new PropertyMapping("Hobbies", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String),
                        new PropertyMapping("StartingDate", PropertyDataType.Date)
                    })
            };

            Assert.IsFalse(versionBuilder.VersionExists(initialMapping));

            versionBuilder.CreateVersion(false, initialMapping);
            
            // Пробуем добавить в индекс объект с корректной схемой данных
			var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);

            var hobby = new DynamicWrapper
                        {
                            ["ActivityName"] = "Football",
                            ["StartingDate"] = new DateTime(1980, 1, 1)
                        };

            dynamic dynObject1 = new DynamicWrapper
                                 {
                                     ["FirstName"] = "Ivan",
                                     ["IsActive"] = new { SomePropert = 1, Active = true },
                                     ["Birthdate"] = new DateTime(1975, 1, 1),
                                     ["Number"] = 2,
                                     ["Hobbies"] = hobby,
                                     ["Id"] = Guid.NewGuid().ToString().ToLowerInvariant()
                                 };

            elasticSearchProvider.Set(dynObject1);
            elasticSearchProvider.Refresh();

            Assert.AreEqual(1, elasticSearchProvider.GetTotalCount());

            var newMapping = new List<PropertyMapping>
            {
                new PropertyMapping("FirstName", PropertyDataType.String),
                new PropertyMapping("IsActive", PropertyDataType.Boolean), // was object
                new PropertyMapping("Birthdate", PropertyDataType.Date),
                new PropertyMapping("Number", PropertyDataType.Integer), 
                new PropertyMapping("Hobbies", new[]
                    {
                        new PropertyMapping("ActivityName", PropertyDataType.String),
                        new PropertyMapping("StartingDate", PropertyDataType.Date)
                    })
            };

            Assert.IsTrue(versionBuilder.VersionExists(initialMapping));
            Assert.IsFalse(versionBuilder.VersionExists(newMapping));
            
            versionBuilder.CreateVersion(false, newMapping);

            elasticSearchProvider.Refresh();

            Assert.IsTrue(versionBuilder.VersionExists(newMapping));

            dynamic dynObject2 = new DynamicWrapper();
            dynObject2["FirstName"] = "Oleg";
            dynObject2["IsActive"] = false;
            dynObject2["Birthdate"] = new DateTime(1970, 1, 1);
            dynObject2["Number"] = 2;
            dynObject2["Hobbies"] = hobby;
            dynObject2["Id"] = Guid.NewGuid().ToString().ToLowerInvariant();
            
			elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider(IndexName, IndexName);
            // Вызов метода не должен привести к исключению
            elasticSearchProvider.Set(dynObject2);

            elasticSearchProvider.Refresh();
            
            Assert.NotNull(elasticSearchProvider.GetItem(dynObject1.Id));
            Assert.NotNull(elasticSearchProvider.GetItem(dynObject2.Id));

            Assert.AreEqual(2, elasticSearchProvider.GetTotalCount());

            newMapping.Add(new PropertyMapping("ObjectProperty", PropertyDataType.Object));

            Assert.IsFalse(versionBuilder.VersionExists(newMapping));

            versionBuilder.CreateVersion(false, newMapping);
            Assert.IsTrue(versionBuilder.VersionExists(newMapping));
        }

        [TearDown]
        public void DeleteIndex()
        {
            _elasticTypeManager.DeleteType(IndexName,IndexName);
        }

    }
}
