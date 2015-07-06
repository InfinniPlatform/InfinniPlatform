using System;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class ElasticSearchPerformanceIntegrationBehavior
    {
        [Test]
        public void ShouldConnect100Times()
        {
            IIndexStateProvider indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("someindex", "someindex");
            indexProvider.RecreateIndex("testindex", "testindex");

            int connectionCount = 100;

            Stopwatch watch = Stopwatch.StartNew();
            var elasticConnection = new ElasticConnection();
            elasticConnection.ConnectIndex();
            watch.Stop();

            long oneConnectionInitTime = watch.ElapsedMilliseconds;

            watch = Stopwatch.StartNew();
            for (int i = 0; i < connectionCount; i++)
            {
                elasticConnection = new ElasticConnection();
                elasticConnection.ConnectIndex();
            }
            watch.Stop();

            Console.WriteLine(string.Format("Connected {0} times {1} ms. First connection time: {2} ", connectionCount,
                                            watch.ElapsedMilliseconds, oneConnectionInitTime));
        }


        [Test]
        [TestCase(1)]
        public void ShouldSearchExistingItems(int recordCount)
        {
            IIndexStateProvider indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("testindex", "testindex");
            var queryWrapper = new IndexQueryExecutor("testindex", "testindex",
                                                      AuthorizationStorageExtensions.AnonimousUser);
            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("testindex", "testindex",
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            for (int i = 0; i < recordCount; i++)
            {
                dynamic expandoObject = new ExpandoObject();
                expandoObject.Id = i;
                expandoObject.Value = "someValue";
                elasticSearchProvider.Set(expandoObject, IndexItemStrategy.Insert);
            }

            elasticSearchProvider.Refresh();

            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < recordCount; i++)
            {
                var model = new SearchModel();

                IFilter filter = FilterBuilderFactory.GetInstance()
                                                     .Get(ElasticConstants.IndexObjectIdentifierField,
                                                          i.ToString(CultureInfo.InvariantCulture),
                                                          CriteriaType.IsEquals);

                model.AddFilter(filter);
                dynamic document = queryWrapper.Query(model).Items.FirstOrDefault();
                Assert.IsNotNull(document);
            }

            watch.Stop();

            Console.WriteLine(string.Format("SEARCH {0} records. Elapsed {1} ms.", recordCount,
                                            watch.ElapsedMilliseconds));
        }

        [Test]
        [TestCase(10)]
        public void ShouldUpdateExistingItems(int recordCount)
        {
            IIndexStateProvider indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("testindex", "testindex");
            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("testindex", "testindex", null,
                                                                                        null);

            dynamic expandoObject = new ExpandoObject();
            expandoObject.Id = 1;
            expandoObject.Value = "someValue";


            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < recordCount; i++)
            {
                elasticSearchProvider.Set(expandoObject, IndexItemStrategy.Insert);
            }
            watch.Stop();

            Console.WriteLine(string.Format("UPDATE {0} records. Elapsed {1} ms.", recordCount,
                                            watch.ElapsedMilliseconds));
        }

        [Test]
        [TestCase(10)]
        //[TestCase(100000)]
        public void ShouldWriteToEmptyIndex(int recordCount)
        {
            IIndexStateProvider indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("testindex", "testindex");
            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("testindex", "testindex",
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            dynamic expandoObject = new ExpandoObject();


            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < recordCount; i++)
            {
                expandoObject.Id = Guid.NewGuid();
                expandoObject.Value = i;

                elasticSearchProvider.Set(expandoObject, IndexItemStrategy.Insert);
            }
            watch.Stop();

            Console.WriteLine(string.Format("INSERT {0} records. Elapsed {1} ms.", recordCount,
                                            watch.ElapsedMilliseconds));
        }
    }
}