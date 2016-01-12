using System;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Documents;

using NUnit.Framework;

namespace InfinniPlatform.ElasticSearch.Tests.ElasticWrappers
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class ElasticSearchPerformanceIntegrationBehavior
	{
		[Test]
		[TestCase(10)]
		//[TestCase(100000)]
		public void ShouldWriteToEmptyIndex(int recordCount)
		{
		    var elasticTypeManager = ElasticFactoryBuilder.ElasticTypeManager.Value;
            elasticTypeManager.DeleteType("testindex", "testindex");
            elasticTypeManager.CreateType("testindex", "testindex");
            var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider("testindex", "testindex");

			dynamic expandoObject = new ExpandoObject();


			var watch = Stopwatch.StartNew();
			for (int i = 0; i < recordCount; i++)
			{
				expandoObject.Id = Guid.NewGuid();
				expandoObject.Value = i;

				elasticSearchProvider.Set(expandoObject, IndexItemStrategy.Insert);
			}
			watch.Stop();

			Console.WriteLine("INSERT {0} records. Elapsed {1} ms.", recordCount, watch.ElapsedMilliseconds);
		}

		[Test]
		[TestCase(10)]
		public void ShouldUpdateExistingItems(int recordCount)
		{
		    var elasticTypeManager = ElasticFactoryBuilder.ElasticTypeManager.Value;
            elasticTypeManager.DeleteType("testindex", "testindex");
            elasticTypeManager.CreateType("testindex", "testindex");
            var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider("testindex", "testindex");

			dynamic expandoObject = new ExpandoObject();
			expandoObject.Id = 1;
			expandoObject.Value = "someValue";


			var watch = Stopwatch.StartNew();
			for (int i = 0; i < recordCount; i++)
			{
				elasticSearchProvider.Set(expandoObject, IndexItemStrategy.Insert);
			}
			watch.Stop();

			Console.WriteLine("UPDATE {0} records. Elapsed {1} ms.", recordCount, watch.ElapsedMilliseconds);
		}


		[Test]
		[TestCase(1)]		
		public void ShouldSearchExistingItems(int recordCount)
		{
		    var elasticTypeManager = ElasticFactoryBuilder.ElasticTypeManager.Value;
            elasticTypeManager.DeleteType("testindex", "testindex");
            elasticTypeManager.CreateType("testindex", "testindex");
		    var factory = ElasticFactoryBuilder.GetElasticFactory();
            var queryWrapper = factory.BuildIndexQueryExecutor("testindex", "testindex");
			var elasticSearchProvider = factory.BuildCrudOperationProvider("testindex", "testindex");
            
			for (int i = 0; i < recordCount; i++)
			{
				dynamic expandoObject = new ExpandoObject();
				expandoObject.Id = i;
				expandoObject.Value = "someValue";
				elasticSearchProvider.Set(expandoObject, IndexItemStrategy.Insert);
			}

			elasticSearchProvider.Refresh();

			var watch = Stopwatch.StartNew();
			for (int i = 0; i < recordCount; i++)
			{
                var model = new SearchModel();

			    var filter = FilterBuilderFactory.GetInstance().Get(ElasticConstants.IndexObjectIdentifierField, i.ToString(CultureInfo.InvariantCulture), CriteriaType.IsEquals);

                model.AddFilter(filter);
                var document = queryWrapper.Query(model).Items.FirstOrDefault();
				Assert.IsNotNull(document);
			}

			watch.Stop();

			Console.WriteLine("SEARCH {0} records. Elapsed {1} ms.", recordCount, watch.ElapsedMilliseconds);
		}

	}
}
