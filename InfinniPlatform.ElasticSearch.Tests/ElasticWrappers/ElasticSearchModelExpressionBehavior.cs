using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class ElasticSearchModelExpressionBehavior
    {
        private const string IndexName = "testperson";
        private const string TypeName = "testperson_typeschema_0";

        private readonly INestFilterBuilder _filterFactory = FilterBuilderFactory.GetInstance();

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            var elasticConnection = ElasticFactoryBuilder.ElasticConnection.Value;


            dynamic test = ElasticDocument.Create();
            test.Values.Id = "898989";
            test.Values.LastName = "Иванов";
            test.Values.FirstName = "Степан";
            test.Values.Patronimic = "Степанович";
            test.Values.NestedObj = new DynamicWrapper();
            test.Values.NestedObjId = "111";
            test.Values.NestedObj.Code = "12345";
            test.Values.NestedObj.Name = "test123";
            elasticConnection.Client.Index((object)test, i => i.Index(IndexName).Type(TypeName));

            dynamic test1 = ElasticDocument.Create();
            test1.Values.Id = "24342";
            test1.Values.LastName = "Иванов";
            test1.Values.FirstName = "Владимир";
            test1.Values.Patronimic = "Степанович";
            test1.Values.NestedObj = new DynamicWrapper();
            test1.Values.NestedObj.Id = "112";
            test1.Values.NestedObj.Code = "12345";
            test1.Values.NestedObj.Name = "test12345";
            elasticConnection.Client.Index((object)test1, i => i.Index(IndexName).Type(TypeName));

            dynamic test2 = ElasticDocument.Create();
            test2.Values.Id = "83453";
            test2.Values.LastName = "Петров";
            test2.Values.FirstName = "Федор";
            test2.Values.Patronimic = "Сергеевич";
            test2.Values.NestedObj = new DynamicWrapper();
            test2.Values.NestedObj.Id = "235";
            test2.Values.NestedObj.Code = "1232342";
            test2.Values.NestedObj.Name = "test2456";
            elasticConnection.Client.Index((object)test2, i => i.Index(IndexName).Type(TypeName));

            elasticConnection.Refresh();
        }

        [Test]
        public void ShouldExecuteSearchByExecutor()
        {
            var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor(IndexName, IndexName);
            var searchModel = new SearchModel();

            var filter = _filterFactory.Get("Patronimic", "СТЕПАНОВИч", CriteriaType.IsEquals)
                                       .And(_filterFactory.Get("NestedObj.Code", "12345", CriteriaType.IsEquals))
                                       .And(_filterFactory.Get("LastName", "иванов", CriteriaType.IsEndsWith));

            searchModel.AddFilter(filter);
            searchModel.AddSort("FirstName", SortOrder.Descending);
            searchModel.SetPageSize(3);

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
            Assert.AreEqual("Степанович", (string)result.Items.First().Patronimic);
            Assert.AreEqual("12345", (string)result.Items.First().NestedObj.Code);
        }

        [Test]
        public void ShouldExecuteSearchWithFacetFilter()
        {
            var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor(IndexName, IndexName);
            var searchModel = new SearchModel();
            searchModel.AddFilter(_filterFactory.Get("Patronimic", "СТЕПАНОВИч", CriteriaType.IsEquals));
            searchModel.AddFilter(_filterFactory.Get("NestedObj.Code", "12345", CriteriaType.IsEquals));
            searchModel.AddSort("LastName", SortOrder.Descending);
            searchModel.SetPageSize(3);

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void ShouldExecuteSearchWithFacetSearchstring()
        {
            var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor(IndexName, IndexName);
            var searchModel = new SearchModel();
            searchModel.AddSort("LastName", SortOrder.Descending);
            searchModel.SetPageSize(3);
            searchModel.AddFilter(_filterFactory.Get("LastName", "иванов", CriteriaType.IsEndsWith));

            var result = executor.Query(searchModel);

            Assert.AreEqual(2, result.HitsCount);
        }

        [Test]
        public void ShouldExecuteSearchWithoutFilters()
        {
            var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor(IndexName, IndexName);
            var searchModel = new SearchModel();

            var result = executor.Query(searchModel);

            Assert.AreEqual(3, result.HitsCount);
        }
    }
}