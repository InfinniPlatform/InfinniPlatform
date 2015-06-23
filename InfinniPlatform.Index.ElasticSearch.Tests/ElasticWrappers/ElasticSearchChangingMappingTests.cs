using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.ElasticSearch.Implementation.Versioning;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    /// <summary>
    ///     Проверка функционала по изменению настроек маппинга
    /// </summary>
    [TestFixture]
    [NUnit.Framework.Category(TestCategories.IntegrationTest)]
    public sealed class ElasticSearchChangingMappingTests
    {
        [TearDown]
        public void DeleteIndex()
        {
            _indexStateProvider.DeleteIndexType(IndexName, IndexName);
        }

        private const string IndexName = "mapping_unit_tests_index";

        private IIndexStateProvider _indexStateProvider;

        [Test]
        public void AddingObjectWithCorrectMappingTest()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

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

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(initialMapping));

            // Пробуем добавить в индекс объект с корректной схемой данных
            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName, IndexName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            var expando = new DynamicWrapper();
            var hobby = new DynamicWrapper();
            hobby["ActivityName"] = "Football";
            hobby["StartingDate"] = new DateTime(1980, 1, 1);
            expando["FirstName"] = "Ivan";
            expando["IsActive"] = true;
            expando["Birthdate"] = new DateTime(1975, 1, 1);
            expando["Int"] = 2;
            expando["Float"] = 2.3;
            expando["Hobbies"] = hobby;
            expando["AttachedData"] = new
                {
                    Info = new
                        {
                            Comment = "some binary data info"
                        }
                };
            expando["Id"] = Guid.NewGuid().ToString().ToLowerInvariant();

            elasticSearchProvider.Set(expando);
            elasticSearchProvider.Refresh();

            Assert.AreEqual(1, elasticSearchProvider.GetTotalCount());
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void AddingObjectWithIncorrectMappingTest()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

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

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(initialMapping));

            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName, IndexName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            IDictionary<string, object> expando = new ExpandoObject();
            IDictionary<string, object> hobby = new ExpandoObject();
            hobby.Add("ActivityName", "Football");
            hobby.Add("StartingDate", new DateTime(1980, 1, 1));
            expando.Add("FirstName", "Ivan");
            expando.Add("IsActive", true);
            expando.Add("Birthdate", new DateTime(1975, 1, 1));
            expando.Add("Int", "2incorrect"); // incorrect value 
            expando.Add("Hobbies", hobby);

            dynamic dynObject1 = expando as ExpandoObject;
            dynObject1.Id = Guid.NewGuid().ToString().ToLowerInvariant();

            // Пробуем добавить в индекс объект с некорректной схемой данных
            // Вызов метода приведет к исключению
            elasticSearchProvider.Set(dynObject1);
        }

        [Test]
        public void ChangingIndexMappingTest()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

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

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(initialMapping));

            // Пробуем добавить в индекс объект с корректной схемой данных
            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName, IndexName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            IDictionary<string, object> expando = new ExpandoObject();
            IDictionary<string, object> hobby = new ExpandoObject();
            hobby.Add("ActivityName", "Football");
            hobby.Add("StartingDate", new DateTime(1980, 1, 1));
            expando.Add("FirstName", "Ivan");
            expando.Add("IsActive", true);
            expando.Add("Birthdate", new DateTime(1975, 1, 1));
            expando.Add("Int", 2);
            expando.Add("Hobbies", hobby);

            dynamic dynObject1 = expando as ExpandoObject;
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

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(newMapping));


            IDictionary<string, object> expando2 = new ExpandoObject();
            expando2.Add("FirstName", "Oleg");
            expando2.Add("IsActive", false);
            expando2.Add("Birthdate", new DateTime(1970, 1, 1));
            expando2.Add("Int", "2correct"); // still correct value 
            expando2.Add("Hobbies", hobby);

            dynamic dynObject2 = expando2 as ExpandoObject;
            dynObject2.Id = Guid.NewGuid().ToString().ToLowerInvariant();

            elasticSearchProvider.RefreshMapping();
            // Вызов метода не должен привести к исключению
            elasticSearchProvider.Set(dynObject2);
            elasticSearchProvider.Refresh();

            Assert.NotNull(elasticSearchProvider.GetItem(dynObject1.Id));
            Assert.NotNull(elasticSearchProvider.GetItem(dynObject2.Id));

            Assert.AreEqual(2, elasticSearchProvider.GetTotalCount());
        }

        [Test]
        public void ChangingIndexTypeMappingTest()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

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

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(initialMapping));

            _indexStateProvider.Refresh();

            var vbuilder = new VersionBuilder(_indexStateProvider, IndexName, IndexName,
                                              SearchAbilityType.KeywordBasedSearch);

            Assert.IsTrue(vbuilder.VersionExists(new IndexTypeMapping(initialMapping)));

            var conflictMapping1 = new List<PropertyMapping>
                {
                    new PropertyMapping("FirstName", PropertyDataType.Date),
                };
            Assert.IsFalse(vbuilder.VersionExists(new IndexTypeMapping(conflictMapping1)));

            var conflictMapping2 = new List<PropertyMapping>
                {
                    new PropertyMapping("AttachedData", new[]
                        {
                            new PropertyMapping("ActivityName", PropertyDataType.String),
                            new PropertyMapping("StartingDate", PropertyDataType.Date)
                        })
                };
            Assert.IsFalse(vbuilder.VersionExists(new IndexTypeMapping(conflictMapping2)));

            var conflictMapping3 = new List<PropertyMapping>
                {
                    new PropertyMapping("FirstName", PropertyDataType.Integer),
                    new PropertyMapping("NewProperty", PropertyDataType.Date),
                };
            Assert.IsFalse(vbuilder.VersionExists(new IndexTypeMapping(conflictMapping3)));
        }

        [Test]
        public void ChangingIndexWithVersionHistoryMappingTest()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

            IVersionBuilder versionBuilder = new ElasticFactory(new RoutingFactoryBase()).BuildVersionBuilder(
                IndexName, IndexName, SearchAbilityType.KeywordBasedSearch);

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

            Assert.IsFalse(versionBuilder.VersionExists(new IndexTypeMapping(initialMapping)));

            versionBuilder.CreateVersion(false, new IndexTypeMapping(initialMapping));

            // Пробуем добавить в индекс объект с корректной схемой данных
            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName, IndexName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            IDictionary<string, object> expando = new ExpandoObject();
            IDictionary<string, object> hobby = new ExpandoObject();
            hobby.Add("ActivityName", "Football");
            hobby.Add("StartingDate", new DateTime(1980, 1, 1));
            expando.Add("FirstName", "Ivan");
            expando.Add("IsActive", new {SomePropert = 1, Active = true});
            expando.Add("Birthdate", new DateTime(1975, 1, 1));
            expando.Add("Number", 2);
            expando.Add("Hobbies", hobby);

            dynamic dynObject1 = expando as ExpandoObject;
            dynObject1.Id = Guid.NewGuid().ToString().ToLowerInvariant();

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

            Assert.IsTrue(versionBuilder.VersionExists(new IndexTypeMapping(initialMapping)));
            Assert.IsFalse(versionBuilder.VersionExists(new IndexTypeMapping(newMapping)));

            versionBuilder.CreateVersion(false, new IndexTypeMapping(newMapping));

            elasticSearchProvider.Refresh();

            Assert.IsTrue(versionBuilder.VersionExists(new IndexTypeMapping(newMapping)));

            IDictionary<string, object> expando2 = new ExpandoObject();
            expando2.Add("FirstName", "Oleg");
            expando2.Add("IsActive", false);
            expando2.Add("Birthdate", new DateTime(1970, 1, 1));
            expando2.Add("Number", 2);
            expando2.Add("Hobbies", hobby);

            dynamic dynObject2 = expando2 as ExpandoObject;
            dynObject2.Id = Guid.NewGuid().ToString().ToLowerInvariant();

            elasticSearchProvider.RefreshMapping();
            // Вызов метода не должен привести к исключению
            elasticSearchProvider.Set(dynObject2);

            elasticSearchProvider.Refresh();

            Assert.NotNull(elasticSearchProvider.GetItem(dynObject1.Id));
            Assert.NotNull(elasticSearchProvider.GetItem(dynObject2.Id));

            Assert.AreEqual(2, elasticSearchProvider.GetTotalCount());

            newMapping.Add(new PropertyMapping("ObjectProperty", PropertyDataType.Object));

            Assert.IsFalse(versionBuilder.VersionExists(new IndexTypeMapping(newMapping)));

            versionBuilder.CreateVersion(false, new IndexTypeMapping(newMapping));
            Assert.IsTrue(versionBuilder.VersionExists(new IndexTypeMapping(newMapping)));
        }

        [Test]
        public void GetIndexTypeMappingTest()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

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

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(initialMapping));

            IList<PropertyMapping> mapping = new ElasticConnection().GetIndexTypeMapping(IndexName, IndexName);

            PropertyMapping firstNameProp = mapping.FirstOrDefault(p => p.Name == "FirstName");

            Assert.IsNotNull(firstNameProp);
            Assert.AreEqual(PropertyDataType.String, firstNameProp.DataType);

            PropertyMapping hobbiesProp = mapping.FirstOrDefault(p => p.Name == "Hobbies");

            Assert.IsNotNull(hobbiesProp);
            Assert.AreEqual(2, hobbiesProp.ChildProperties.Count);
        }

        [Test]
        public void QueryOnIndexWithChangedMapping()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

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
                        }),
                };

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(initialMapping));

            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName, IndexName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            foreach (School school in SchoolsFactory.CreateSchools())
            {
                // Преобразование школы в объект типа dynamic
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                    expando.Add(property.Name, property.GetValue(school));

                dynamic dynSchool = expando as ExpandoObject;
                dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();
                dynSchool.ExtraField = "15";

                elasticSearchProvider.Set(dynSchool);
            }

            elasticSearchProvider.Refresh();


            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            IFilterBuilder filterFactory = FilterBuilderFactory.GetInstance();
            searchModel.AddFilter(filterFactory.Get("ExtraField", "15", CriteriaType.IsEquals));

            SearchViewModel result = executor.Query(searchModel);

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
                        }),
                };

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(changedMapping));

            foreach (School school in SchoolsFactory.CreateSchools())
            {
                // Преобразование школы в объект типа dynamic
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                    expando.Add(property.Name, property.GetValue(school));

                dynamic dynSchool = expando as ExpandoObject;
                dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();
                dynSchool.ExtraField = "15";

                elasticSearchProvider.Set(dynSchool);
            }

            elasticSearchProvider.Refresh();

            SearchViewModel resultAfterMappingChanges = executor.Query(searchModel);

            Assert.AreEqual(6, resultAfterMappingChanges.HitsCount);

            // several additional search to make sure that searching under 2 indeces works
            searchModel = new SearchModel();
            IFilter filter = filterFactory.Get("Principal.LastName", "nak", CriteriaType.IsNotContains);
            searchModel.AddFilter(filter);
            SearchViewModel responce = executor.Query(searchModel);
            int cnt = responce.HitsCount;
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
        [ExpectedException(typeof (ArgumentException))]
        public void QueryOnIndexWithIncorrectInitialMapping()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

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
                        }),
                };

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(initialMapping));

            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName, IndexName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            School school = SchoolsFactory.CreateSchools().FirstOrDefault();

            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                expando.Add(property.Name, property.GetValue(school));

            dynamic dynSchool = expando as ExpandoObject;
            dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();

            // this call will rise an exception
            elasticSearchProvider.Set(dynSchool);
        }

        [Test]
        public void QueryOnIndexWithInitialMappingSet()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

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
                        }),
                };

            _indexStateProvider.CreateIndexType(IndexName, IndexName, deleteExistingVersion: false,
                                                mappingUpdates: new IndexTypeMapping(initialMapping));

            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName, IndexName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            foreach (School school in SchoolsFactory.CreateSchools())
            {
                // Преобразование школы в объект типа dynamic
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                    expando.Add(property.Name, property.GetValue(school));

                dynamic dynSchool = expando as ExpandoObject;
                dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();

                elasticSearchProvider.Set(dynSchool);
            }

            elasticSearchProvider.Refresh();

            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            IFilterBuilder filterFactory = FilterBuilderFactory.GetInstance();
            searchModel.AddFilter(filterFactory.Get("Principal.LastName", "Monakhov", CriteriaType.IsEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }
    }
}