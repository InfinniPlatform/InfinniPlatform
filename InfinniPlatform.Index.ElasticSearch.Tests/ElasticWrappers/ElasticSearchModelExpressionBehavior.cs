using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class ElasticSearchModelExpressionBehavior
    {
        private readonly IFilterBuilder _filterFactory = FilterBuilderFactory.GetInstance();

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            new TestPersonIndexBuilder().BuildIndexObjectForSearchModelAndSetItems("testperson");
        }

        [Test]
        public void ShouldExecuteSearchByExecutor()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor("testperson", "testperson",
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);
            var searchModel = new SearchModel();

            IFilter filter = _filterFactory.Get("Patronimic", "СТЕПАНОВИч", CriteriaType.IsEquals)
                                           .And(_filterFactory.Get("NestedObj.Code", "12345", CriteriaType.IsEquals))
                                           .And(_filterFactory.Get("LastName", "иванов", CriteriaType.IsEndsWith));

            searchModel.AddFilter(filter);
            searchModel.AddSort("FirstName", SortOrder.Descending);
            searchModel.SetPageSize(3);

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
            Assert.AreEqual("Степанович", (string) result.Items.First().Patronimic);
            Assert.AreEqual("12345", (string) result.Items.First().NestedObj.Code);
        }


        [Test]
        public void ShouldExecuteSearchWithFacetFilter()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor("testperson", "testperson",
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);
            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Patronimic", "СТЕПАНОВИч", CriteriaType.IsEquals));
            searchModel.AddFilter(_filterFactory.Get("NestedObj.Code", "12345", CriteriaType.IsEquals));
            searchModel.AddSort("LastName", SortOrder.Descending);
            searchModel.SetPageSize(3);

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void ShouldExecuteSearchWithFacetSearchstring()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor("testperson", "testperson",
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);
            var searchModel = new SearchModel();
            searchModel.AddSort("LastName", SortOrder.Descending);
            searchModel.SetPageSize(3);
            searchModel.AddFilter(_filterFactory.Get("LastName", "иванов", CriteriaType.IsEndsWith));

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void ShouldExecuteSearchWithoutFilters()
        {
            IIndexQueryExecutor executor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor("testperson", "testperson",
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);
            var searchModel = new SearchModel();

            SearchViewModel result = executor.Query(searchModel);

            Assert.AreEqual(3, result.HitsCount);
        }
    }
}