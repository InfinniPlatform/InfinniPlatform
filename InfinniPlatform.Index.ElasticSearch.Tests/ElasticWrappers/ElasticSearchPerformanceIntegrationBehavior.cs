using System;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Sdk.Environment.Index;

using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
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
			var elasticConnection = new ElasticConnection();
            elasticConnection.DeleteType("testindex", "testindex");
            elasticConnection.CreateType("testindex", "testindex");
            var elasticSearchProvider = new ElasticFactory().BuildCrudOperationProvider("testindex", "testindex", null);

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
			var elasticConnection = new ElasticConnection();
            elasticConnection.DeleteType("testindex", "testindex");
            elasticConnection.CreateType("testindex", "testindex");
            var elasticSearchProvider = new ElasticFactory().BuildCrudOperationProvider("testindex", "testindex");

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
			var indexProvider = new ElasticConnection();
            indexProvider.DeleteType("testindex", "testindex");
            indexProvider.CreateType("testindex", "testindex");
            var queryWrapper = new IndexQueryExecutor(new IndexToTypeAccordanceProvider().GetIndexTypeAccordances("testindex", new[] { "testindex" }));
			var elasticSearchProvider = new ElasticFactory().BuildCrudOperationProvider("testindex", "testindex");
            
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
