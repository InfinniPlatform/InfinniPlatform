using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Documents;

using NUnit.Framework;

namespace InfinniPlatform.ElasticSearch.Tests.ElasticWrappers
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
			var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
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
			var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
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
			var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
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
			var executor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
			var searchModel = new SearchModel();

			var result = executor.Query(searchModel);

			Assert.AreEqual(3, result.HitsCount);
		}



	}
}
