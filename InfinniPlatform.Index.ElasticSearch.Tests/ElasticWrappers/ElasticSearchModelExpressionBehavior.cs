using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Environment.Index;

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
			var executor = new ElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
			var searchModel = new SearchModel();

		    var filter = _filterFactory.Get("Patronimic", "СТЕПАНОВИч", CriteriaType.IsEquals)
		                               .And(_filterFactory.Get("NestedObj.Code", "12345", CriteriaType.IsEquals))
                                       .And(_filterFactory.Get("LastName", "иванов", CriteriaType.IsEndsWith));

			searchModel.AddFilter(filter);
			searchModel.AddSort("FirstName",SortOrder.Descending);
			searchModel.SetPageSize(3);
			
			var result = executor.Query(searchModel);

			Assert.AreEqual(2,result.HitsCount);
			Assert.AreEqual("Степанович",(string)result.Items.First().Patronimic);
			Assert.AreEqual("12345", (string)result.Items.First().NestedObj.Code);
		}


		[Test]
		public void ShouldExecuteSearchWithFacetSearchstring()
		{
			var executor = new ElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
			var searchModel = new SearchModel();
			searchModel.AddSort("LastName", SortOrder.Descending);
			searchModel.SetPageSize(3);
            searchModel.AddFilter(_filterFactory.Get("LastName", "иванов", CriteriaType.IsEndsWith));

			var result = executor.Query( searchModel);

			Assert.AreEqual(2, result.HitsCount);
		}

		[Test]
		public void ShouldExecuteSearchWithFacetFilter()
		{
			var executor = new ElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
			var searchModel = new SearchModel();
		    searchModel.AddFilter(_filterFactory.Get("Patronimic", "СТЕПАНОВИч", CriteriaType.IsEquals));
		    searchModel.AddFilter(_filterFactory.Get("NestedObj.Code", "12345", CriteriaType.IsEquals));
			searchModel.AddSort("LastName", SortOrder.Descending);
			searchModel.SetPageSize(3);

			var result = executor.Query( searchModel);

			Assert.AreEqual(2, result.HitsCount);
		}

		[Test]
		public void ShouldExecuteSearchWithoutFilters()
		{
			var executor = new ElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
			var searchModel = new SearchModel();

			var result = executor.Query(searchModel);

			Assert.AreEqual(3, result.HitsCount);
		}



	}
}
