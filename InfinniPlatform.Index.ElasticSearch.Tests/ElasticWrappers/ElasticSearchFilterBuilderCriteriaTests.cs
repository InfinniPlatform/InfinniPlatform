using System;
using System.ComponentModel;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    /// <summary>
    ///     Тестирование поиска по различным критериям из следующего списка:
    ///     IsEquals
    ///     IsNotEquals
    ///     IsMoreThan
    ///     IsLessThan
    ///     IsMoreThanOrEquals
    ///     IsLessThanOrEquals
    ///     IsContains
    ///     IsNotContains
    ///     IsEmpty
    ///     IsNotEmpty
    ///     IsStartsWith
    ///     IsNotStartsWith
    ///     IsEndsWith
    ///     IsNotEndsWith
    /// </summary>
    [TestFixture]
    [NUnit.Framework.Category(TestCategories.IntegrationTest)]
    public sealed class ElasticSearchFilterBuilderCriteriaTests
    {
        [SetUp]
        public void InitializeElasticSearch()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

            _indexStateProvider.CreateIndexType(IndexName, IndexName, true);
            _elasticSearchProvider = new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName,
                                                                                                             IndexName,
                                                                                                             AuthorizationStorageExtensions
                                                                                                                 .AnonimousUser,
                                                                                                             null);

            foreach (School school in SchoolsFactory.CreateSchools())
            {
                // Преобразование школы в объект типа dynamic
                var expando = new DynamicWrapper();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                    expando[property.Name] = property.GetValue(school);

                expando["Id"] = Guid.NewGuid().ToString().ToLowerInvariant();

                _elasticSearchProvider.Set(expando);
            }

            _elasticSearchProvider.Refresh();
        }

        [TearDown]
        public void DeleteIndex()
        {
            _indexStateProvider.DeleteIndexType(IndexName, IndexName);
        }

        private const string IndexName = "filterunittestindex";

        private ICrudOperationProvider _elasticSearchProvider;
        private IIndexStateProvider _indexStateProvider;
        private readonly IFilterBuilder _filterFactory = FilterBuilderFactory.GetInstance();

        [Test]
        public void DifferentCriteriaUnderSetOfFieldsTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();

            IFilter filter = _filterFactory.Get("FoundationDate", new DateTime(1985, 10, 1), CriteriaType.IsMoreThan)
                                           .And(_filterFactory.Get("Principal.KnowledgeRating", 3.9,
                                                                   CriteriaType.IsNotEquals))
                                           .And(_filterFactory.Get("Principal.Name", null, CriteriaType.IsNotEmpty));

            searchModel.AddFilter(filter);

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(0, result.HitsCount);
        }

        [Test]
        public void IsContainsCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "nak", CriteriaType.IsContains));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        public void IsEmptyCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.Name", "put any string here", CriteriaType.IsEmpty));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
        }

        [Test]
        public void IsEndsWithCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "akhov", CriteriaType.IsEndsWith));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        public void IsEqualsCriteriaUnderDateTimeFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 11, 1),
                                                     CriteriaType.IsLessThan));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsEqualsCriteriaUnderDoubleFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(31, result.Items.First().HouseNumber);
            Assert.AreEqual("Kirova", result.Items.First().Street);
        }

        [Test]
        public void IsEqualsCriteriaUnderIntegerFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(31, result.Items.First().HouseNumber);
            Assert.AreEqual("Kirova", result.Items.First().Street);
        }

        [Test]
        public void IsEqualsCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "Monakhov", CriteriaType.IsEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        public void IsFullTextSearchCriteriaByTwoWordsTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("", "Lenina-Avenue", CriteriaType.FullTextSearch));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(41, result.Items.First().HouseNumber);
            Assert.AreEqual("Lenina-Avenue", result.Items.First().Street);
        }

        [Test]
        public void IsFullTextSearchCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Street", "away", CriteriaType.FullTextSearch));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        public void IsLessThanCriteriaUnderDateTimeFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1),
                                                     CriteriaType.IsLessThan));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Very good school", result.Items.First().Name);
        }

        [Test]
        public void IsLessThanCriteriaUnderDoubleFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsLessThan));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Bad school", result.Items.First().Name);
        }

        [Test]
        public void IsLessThanCriteriaUnderIntegerFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsLessThan));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Bad school", result.Items.First().Name);
        }

        [Test]
        public void IsLessThanOrEqualsCriteriaUnderDateTimeFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1),
                                                     CriteriaType.IsLessThanOrEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsLessThanOrEqualsCriteriaUnderDoubleFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsLessThanOrEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsLessThanOrEqualsCriteriaUnderIntegerFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsLessThanOrEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsMoreThanCriteriaUnderDateTimeFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1),
                                                     CriteriaType.IsMoreThan));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Bad school", result.Items.First().Name);
        }

        [Test]
        public void IsMoreThanCriteriaUnderDoubleFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsMoreThan));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Very good school", result.Items.First().Name);
        }

        [Test]
        public void IsMoreThanCriteriaUnderIntegerFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsMoreThan));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Very good school", result.Items.First().Name);
        }

        [Test]
        public void IsMoreThanCriteriaUnderSetOfFieldsTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();

            IFilter filter = _filterFactory.Get("FoundationDate", new DateTime(1981, 10, 1), CriteriaType.IsMoreThan)
                                           .And(_filterFactory.Get("Principal.Grade", 6, CriteriaType.IsEquals).Not());

            searchModel.AddFilter(filter);

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Bad school", result.Items.First().Name);
        }

        [Test]
        public void IsMoreThanOrEqualsCriteriaUnderDateTimeFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1),
                                                     CriteriaType.IsMoreThanOrEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsMoreThanOrEqualsCriteriaUnderDoubleFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsMoreThanOrEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsMoreThanOrEqualsCriteriaUnderIntegerFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsMoreThanOrEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotContainsCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "nak", CriteriaType.IsNotContains));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotEmptyCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.Name", "put any string here", CriteriaType.IsNotEmpty));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotEndsWithCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "akhov", CriteriaType.IsNotEndsWith));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotEqualsCriteriaUnderDateTimeFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1),
                                                     CriteriaType.IsNotEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotEqualsCriteriaUnderDoubleFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsNotEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotEqualsCriteriaUnderIntegerFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsNotEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotEqualsCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "Monakhov", CriteriaType.IsNotEquals));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotStartsWithCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "Mon", CriteriaType.IsNotStartsWith));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsStartsWithCriteriaUnderStringFieldTest()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName,
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "Mon", CriteriaType.IsStartsWith));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }
    }
}