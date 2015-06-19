using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    /// <summary>
    /// Тестирование поиска по различным критериям из следующего списка:
    /// 
    /// IsEquals
	///	IsNotEquals
	///	IsMoreThan
	///	IsLessThan
	///	IsMoreThanOrEquals
	///	IsLessThanOrEquals
	///	IsContains
	///	IsNotContains
	///	IsEmpty
	///	IsNotEmpty
	///	IsStartsWith
	///	IsNotStartsWith
	///	IsEndsWith
	///	IsNotEndsWith
    /// 
    /// </summary>
    [TestFixture]
	[NUnit.Framework.Category(TestCategories.IntegrationTest)]
    public sealed class ElasticSearchFilterBuilderCriteriaTests
    {
        private const string IndexName = "filterunittestindex";

        private ICrudOperationProvider _elasticSearchProvider;
        private IIndexStateProvider _indexStateProvider;
        private readonly IFilterBuilder _filterFactory = FilterBuilderFactory.GetInstance();

        [SetUp]
        public void InitializeElasticSearch()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            
            _indexStateProvider.CreateIndexType(IndexName,IndexName, true);
			_elasticSearchProvider = new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser, null);

            foreach (var school in SchoolsFactory.CreateSchools())
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

        [Test]
        public void IsEqualsCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "Monakhov", CriteriaType.IsEquals));
            
            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        public void IsNotEqualsCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "Monakhov", CriteriaType.IsNotEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsContainsCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "nak", CriteriaType.IsContains));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        public void IsNotContainsCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "nak", CriteriaType.IsNotContains));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsEmptyCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.Name", "put any string here", CriteriaType.IsEmpty));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
        }

        [Test]
        public void IsNotEmptyCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.Name", "put any string here", CriteriaType.IsNotEmpty));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsStartsWithCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "Mon", CriteriaType.IsStartsWith));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        public void IsNotStartsWithCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "Mon", CriteriaType.IsNotStartsWith));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsEndsWithCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "akhov", CriteriaType.IsEndsWith));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(21, result.Items.First().HouseNumber);
            Assert.AreEqual("Far away", result.Items.First().Street);
        }

        [Test]
        public void IsNotEndsWithCriteriaUnderStringFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Principal.LastName", "akhov", CriteriaType.IsNotEndsWith));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsEqualsCriteriaUnderDoubleFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(31, result.Items.First().HouseNumber);
            Assert.AreEqual("Kirova", result.Items.First().Street);
        }

        [Test]
        public void IsNotEqualsCriteriaUnderDoubleFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsNotEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsMoreThanCriteriaUnderDoubleFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsMoreThan));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Very good school", result.Items.First().Name);
        }

        [Test]
        public void IsLessThanCriteriaUnderDoubleFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsLessThan));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Bad school", result.Items.First().Name);
        }

        [Test]
        public void IsMoreThanOrEqualsCriteriaUnderDoubleFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsMoreThanOrEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsLessThanOrEqualsCriteriaUnderDoubleFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Rating", 3.1, CriteriaType.IsLessThanOrEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsEqualsCriteriaUnderIntegerFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(31, result.Items.First().HouseNumber);
            Assert.AreEqual("Kirova", result.Items.First().Street);
        }

        [Test]
        public void IsNotEqualsCriteriaUnderIntegerFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsNotEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsMoreThanCriteriaUnderIntegerFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsMoreThan));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Very good school", result.Items.First().Name);
        }

        [Test]
        public void IsLessThanCriteriaUnderIntegerFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsLessThan));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Bad school", result.Items.First().Name);
        }

        [Test]
        public void IsMoreThanOrEqualsCriteriaUnderIntegerFieldTest()
        {
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsMoreThanOrEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsLessThanOrEqualsCriteriaUnderIntegerFieldTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("HouseNumber", 31, CriteriaType.IsLessThanOrEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsEqualsCriteriaUnderDateTimeFieldTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 11, 1), CriteriaType.IsLessThan));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsNotEqualsCriteriaUnderDateTimeFieldTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1), CriteriaType.IsNotEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsMoreThanCriteriaUnderDateTimeFieldTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1), CriteriaType.IsMoreThan));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Bad school", result.Items.First().Name);
        }

        [Test]
        public void IsLessThanCriteriaUnderDateTimeFieldTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1), CriteriaType.IsLessThan));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Very good school", result.Items.First().Name);
        }

        [Test]
        public void IsMoreThanOrEqualsCriteriaUnderDateTimeFieldTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1), CriteriaType.IsMoreThanOrEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsLessThanOrEqualsCriteriaUnderDateTimeFieldTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("FoundationDate", new DateTime(1988, 10, 1), CriteriaType.IsLessThanOrEquals));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void IsMoreThanCriteriaUnderSetOfFieldsTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();

            var filter = _filterFactory.Get("FoundationDate", new DateTime(1981, 10, 1), CriteriaType.IsMoreThan)
                                       .And(_filterFactory.Get("Principal.Grade", 6, CriteriaType.IsEquals).Not());

            searchModel.AddFilter(filter);

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual("Bad school", result.Items.First().Name);
        }

        [Test]
        public void DifferentCriteriaUnderSetOfFieldsTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();

            var filter = _filterFactory.Get("FoundationDate", new DateTime(1985, 10, 1), CriteriaType.IsMoreThan)
                                       .And(_filterFactory.Get("Principal.KnowledgeRating", 3.9, CriteriaType.IsNotEquals))
                                       .And(_filterFactory.Get("Principal.Name", null, CriteriaType.IsNotEmpty));

            searchModel.AddFilter(filter);

            var result = executor.Query(searchModel);

            Assert.AreEqual(0, result.HitsCount);
        }

		[Test]
		public void IsFullTextSearchCriteriaUnderStringFieldTest()
		{
			var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

			var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Street", "away", CriteriaType.FullTextSearch));

			var result = executor.Query(searchModel);

			Assert.AreEqual(1, result.HitsCount);
			Assert.AreEqual(21, result.Items.First().HouseNumber);
			Assert.AreEqual("Far away", result.Items.First().Street);
		}

        [Test]
        public void IsFullTextSearchCriteriaByTwoWordsTest()
        {
            var executor = new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor(IndexName, IndexName, AuthorizationStorageExtensions.AnonimousUser);

            var searchModel = new SearchModel();
			searchModel.AddFilter(_filterFactory.Get("", "Lenina-Avenue", CriteriaType.FullTextSearch));

            var result = executor.Query(searchModel);

            Assert.AreEqual(1, result.HitsCount);
            Assert.AreEqual(41, result.Items.First().HouseNumber);
            Assert.AreEqual("Lenina-Avenue", result.Items.First().Street);
        }

        [TearDown]
        public void DeleteIndex()
        {
            _indexStateProvider.DeleteIndexType(IndexName,IndexName);
        }
    }
}
