using System.Globalization;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Dynamic;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class ElasticSearchPerformanceIntegrationBehavior
	{
		[Test]
		public void ShouldConnect100Times()
		{
			var indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
			indexProvider.RecreateIndex("someindex","someindex");
			indexProvider.RecreateIndex("testindex","testindex");

			int connectionCount = 100;

			var watch = Stopwatch.StartNew();
			var elasticConnection = new ElasticConnection();
		    elasticConnection.ConnectIndex();
			watch.Stop();

			var oneConnectionInitTime = watch.ElapsedMilliseconds;

			watch = Stopwatch.StartNew();
			for (int i = 0; i < connectionCount; i++)
			{
				elasticConnection = new ElasticConnection();
                elasticConnection.ConnectIndex();
			}
			watch.Stop();

			Console.WriteLine(string.Format("Connected {0} times {1} ms. First connection time: {2} ",connectionCount, watch.ElapsedMilliseconds,oneConnectionInitTime ));
		}

		[Test]
		[TestCase(10)]
		//[TestCase(100000)]
		public void ShouldWriteToEmptyIndex(int recordCount)
		{
			var indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("testindex", "testindex");
			var elasticSearchProvider = new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("testindex", "testindex", AuthorizationStorageExtensions.AnonimousUser);

			dynamic expandoObject = new ExpandoObject();


			var watch = Stopwatch.StartNew();
			for (int i = 0; i < recordCount; i++)
			{
				expandoObject.Id = Guid.NewGuid();
				expandoObject.Value = i;

				elasticSearchProvider.Set(expandoObject, IndexItemStrategy.Insert);
			}
			watch.Stop();

			Console.WriteLine(string.Format("INSERT {0} records. Elapsed {1} ms.",recordCount,  watch.ElapsedMilliseconds));
		}

		[Test]
		[TestCase(10)]
		public void ShouldUpdateExistingItems(int recordCount)
		{
			var indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("testindex", "testindex");
			var elasticSearchProvider = new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("testindex", "testindex", null);

			dynamic expandoObject = new ExpandoObject();
			expandoObject.Id = 1;
			expandoObject.Value = "someValue";


			var watch = Stopwatch.StartNew();
			for (int i = 0; i < recordCount; i++)
			{
				elasticSearchProvider.Set(expandoObject, IndexItemStrategy.Insert);
			}
			watch.Stop();

			Console.WriteLine(string.Format("UPDATE {0} records. Elapsed {1} ms.", recordCount, watch.ElapsedMilliseconds));
		}


		[Test]
		[TestCase(1)]		
		public void ShouldSearchExistingItems(int recordCount)
		{
			var indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("testindex", "testindex");
			var queryWrapper = new IndexQueryExecutor("testindex", "testindex", AuthorizationStorageExtensions.AnonimousUser);
			var elasticSearchProvider = new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("testindex", "testindex", AuthorizationStorageExtensions.AnonimousUser);
            
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

			Console.WriteLine(string.Format("SEARCH {0} records. Elapsed {1} ms.", recordCount, watch.ElapsedMilliseconds));
		}

	}
}
