using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class ElasticSearchBehavior
    {
        [Test]
        public void ShouldConnectElasticSearch()
        {
            ConnectionSettings elasticSettings = new ConnectionSettings(new Uri("http://127.0.0.1:9200"))
                .SetDefaultIndex("testindex");
            var client = new ElasticClient(elasticSettings);

            Assert.True(client.Connection.HeadSync(new Uri("http://127.0.0.1:9200")).Success);
        }


        [Test]
        public void ShouldCreateIndex()
        {
            var elasticSettings = new ConnectionSettings(new Uri("http://localhost:9200"));
            var client = new ElasticClient(elasticSettings);

            if (!client.Connection.HeadSync(new Uri("http://127.0.0.1:9200")).Success)
            {
                client.CreateIndex("testindex");
                Assert.True(client.GetIndexSettings(i => i.Index("testindex")) != null);
                client.DeleteIndex(i => i.Index("testindex"));
                Assert.True(client.GetIndexSettings(i => i.Index("testindex")) == null);
            }
        }

        [Test]
        public void ShouldFilterOnFields()
        {
            ConnectionSettings elasticSettings = new ConnectionSettings(new Uri("http://127.0.0.1:9200"))
                .SetDefaultIndex("testindex");


            var client = new ElasticClient(elasticSettings);
            client.DeleteIndex(i => i.Index("testindex"));
            client.Refresh();
            client.CreateIndex("testindex", c => c.AddMapping<TestPerson>(m => m.MapFromAttributes())
                );

            ISearchResponse<TestPerson> items = null;
            ISearchResponse<TestPerson> results = null;
            ISearchResponse<TestPerson> results1 = null;
            ISearchResponse<TestPerson> results2 = null;
            ISearchResponse<TestPerson> results3 = null;

            try
            {
                var test = new TestPerson
                    {
                        Id = "898989",
                        LastName = "Иванов",
                        FirstName = "Степан",
                        Patronimic = "Степанович",
                        NestedObj = new NestedObj
                            {
                                Code = "12345",
                                Name = "test123"
                            }
                    };

                var test1 = new TestPerson
                    {
                        Id = "83453",
                        LastName = "Петров",
                        FirstName = "Владимир",
                        Patronimic = "Степанович"
                    };

                var test2 = new TestPerson
                    {
                        Id = "345243",
                        LastName = "Сидоров",
                        FirstName = "Петр",
                        Patronimic = "Федорович",
                        Another = "234233"
                    };

                var test3 = new TestPerson
                    {
                        Id = "324323",
                        LastName = "123",
                        FirstName = "Ivan",
                        Patronimic = "cfac56ef-268f-483e-9c7d-6fb1346a0054",
                        HospitalId = "cfac56ef-268f-483e-9c7d-6fb1346a0054"
                    };

                if (client.Connection.HeadSync(new Uri("http://127.0.0.1:9200")).Success)
                {
                    client.Index(test);
                    client.Index(test1);
                    client.Index(test2);
                    client.Index(test3);
                    client.Refresh();

                    IGetResponse<TestPerson> item = client.Get<TestPerson>("345243");
                    Assert.IsNotNull(item);


                    //term should be camel-case field name!
                    FilterContainer filter = Filter<TestPerson>.Term(f => f.LastName, "сидоров");
                    items = client.Search<TestPerson>(f => f.Filter(ff => ff.Term(i => i.LastName, "сидоров")));

                    results = client.Search<TestPerson>(s => s
                                                                 .From(0)
                                                                 .Size(10)
                                                                 .Fields(f => f.LastName, f => f.FirstName)
                                                                 .Query(q => q.Wildcard(f => f.LastName, "*оров*")
                                                                 ));

                    results1 = client.Search<TestPerson>(s => s
                                                                  .From(0)
                                                                  .Size(10)
                                                                  .Fields(f => f.LastName, f => f.FirstName)
                                                                  .Query(q => q.Term(f => f.Id, "83453")
                                                                  ));

                    results2 = client.Search<TestPerson>(s => s
                                                                  .From(0)
                                                                  .Size(10)
                                                                  .Fields(f => f.LastName, f => f.FirstName)
                                                                  .Query(q => q.Term(f => f.Another, "234233")
                                                                  ));

                    results3 = client.Search<TestPerson>(s => s
                                                                  .From(0)
                                                                  .Size(10)
                                                                  .Fields(f => f.LastName, f => f.FirstName)
                                                                  .Query(q => q.Term(f => f.LastName, "сидоров")
                                                                  ));
                }
            }
            catch
            {
                client.DeleteIndex(i => i.Index("test"));
                client.Refresh();
            }


            Assert.AreEqual(results.Hits.Count(), 1);
            Assert.AreEqual(results1.Hits.Count(), 1);
            Assert.AreEqual(results2.Hits.Count(), 1);
            Assert.AreEqual(results3.Hits.Count(), 1);
            Assert.AreEqual(items.Hits.Count(), 1);
        }

        [Test]
        public void ShouldReindexWithoutDataLostTest()
        {
            IIndexStateProvider indexProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexProvider.RecreateIndex("testperson", "testperson");

            ICrudOperationProvider elasticSearchProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("testperson", "testperson",
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);

            dynamic person1 = new DynamicWrapper();
            person1.Id = "11111";
            person1.LastName = "Иванов";
            person1.FirstName = "Петр";
            person1.Patronimic = "Степанович";

            dynamic person2 = new DynamicWrapper();
            person2.Id = "2222";
            person2.LastName = "Сидор";
            person2.FirstName = "Иван";
            person2.Patronimic = "Степанович";

            elasticSearchProvider.Set(person1);
            elasticSearchProvider.Set(person2);


            IIndexQueryExecutor queryExecutor =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexQueryExecutor("testperson", "testperson",
                                                                                     AuthorizationStorageExtensions
                                                                                         .AnonimousUser);
            indexProvider.Refresh();

            Assert.AreEqual(2, queryExecutor.Query(new SearchModel()).HitsCount);


            // Количество документов в индексе не должно измениться
            Assert.AreEqual(2, queryExecutor.Query(new SearchModel()).HitsCount);

            indexProvider.RecreateIndex("testperson", "testperson");
            indexProvider.Refresh();

            // Документы должны пропасть из индекса
            Assert.AreEqual(0, queryExecutor.Query(new SearchModel()).HitsCount);
        }

        [Test]
        public void ShouldRemoveFromIndex()
        {
            ConnectionSettings elasticSettings =
                new ConnectionSettings(new Uri("http://localhost:9200")).SetDefaultIndex("testindex");
            var client = new ElasticClient(elasticSettings);

            if (client.Connection.HeadSync(new Uri("http://127.0.0.1:9200")).Success)
            {
                client.DeleteByQuery<dynamic>(q => q.MatchAll());
            }

            Assert.AreEqual(0, client.Search<dynamic>(q => q.MatchAll()).Documents.Count());
        }


        [Test]
        public void ShouldSortIndex()
        {
            ConnectionSettings elasticSettings = new ConnectionSettings(new Uri("http://127.0.0.1:9200"))
                .SetDefaultIndex("test");

            var client = new ElasticClient(elasticSettings);
            client.CreateIndex("test", c => c
                                                .AddMapping<TestPerson>(m => m.MapFromAttributes())
                );


            var test = new TestPerson
                {
                    Id = "898989",
                    LastName = "Бюль бюль оглы",
                    FirstName = "Степан",
                    Patronimic = "Степанович"
                };


            try
            {
                if (client.Connection.HeadSync(new Uri("http://127.0.0.1:9200")).Success)
                {
                    client.Index(test);
                    client.Refresh();

                    List<TestPerson> items =
                        client.Search<TestPerson>(q => q.Query(qr => qr.MatchAll()).SortAscending(f => f.LastName))
                              .Documents.ToList();
                    Assert.True(items.Count > 0);
                    Assert.AreEqual("Бюль бюль оглы", items[0].LastName);
                }
            }
            catch
            {
                client.DeleteIndex(i => i.Index("test"));
                client.Refresh();
            }
        }
    }
}