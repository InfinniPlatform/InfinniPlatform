using System.Linq;

using InfinniPlatform.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

using Nest;

using NUnit.Framework;

namespace InfinniPlatform.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    [Ignore("TODO: Dont work. Refactor!")]
    public class ElasticSearchBehavior
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            _elasticClient = ElasticFactoryBuilder.ElasticConnection.Value.Client;
        }


        private ElasticClient _elasticClient;


        [Test]
        public void ShouldCreateIndex()
        {
            _elasticClient.CreateIndex("testindex");
            Assert.True(_elasticClient.GetIndexSettings(i => i.Index("testindex")) != null);

            _elasticClient.DeleteIndex(i => i.Index("testindex"));
            Assert.True(_elasticClient.GetIndexSettings(i => i.Index("testindex")) == null);
        }

        [Test]
        public void ShouldRemoveFromIndex()
        {
            _elasticClient.DeleteByQuery<dynamic>(q => q.MatchAll());

            Assert.AreEqual(0, _elasticClient.Search<dynamic>(q => q.MatchAll()).Documents.Count());
        }

        [Test]
        public void ShouldReindexWithoutDataLostTest()
        {
            var elasticTypeManager = ElasticFactoryBuilder.ElasticTypeManager.Value;
            elasticTypeManager.DeleteType("testperson", "testperson");
            elasticTypeManager.CreateType("testperson", "testperson");

            var elasticSearchProvider = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider("testperson", "testperson");

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


            var queryExecutor = ElasticFactoryBuilder.GetElasticFactory().BuildIndexQueryExecutor("testperson", "testperson");
            var elasticConnection = ElasticFactoryBuilder.ElasticConnection.Value;
            elasticConnection.Refresh();

            Assert.AreEqual(2, queryExecutor.Query(new SearchModel()).HitsCount);


            // Количество документов в индексе не должно измениться
            Assert.AreEqual(2, queryExecutor.Query(new SearchModel()).HitsCount);

            elasticTypeManager.DeleteType("testperson", "testperson");
            elasticTypeManager.CreateType("testperson", "testperson");
            elasticConnection.Refresh();

            // Документы должны пропасть из индекса
            Assert.AreEqual(0, queryExecutor.Query(new SearchModel()).HitsCount);
        }


        [Test]
        public void ShouldSortIndex()
        {
            _elasticClient.CreateIndex("test", c => c.AddMapping<TestPerson>(m => m.MapFromAttributes()));

            var test = new TestPerson
                       {
                           Id = "898989",
                           LastName = "Бюль бюль оглы",
                           FirstName = "Степан",
                           Patronimic = "Степанович"
                       };


            try
            {
                _elasticClient.Index(test);
                _elasticClient.Refresh();

                var items = _elasticClient.Search<TestPerson>(q => q.Query(qr => qr.MatchAll()).SortAscending(f => f.LastName)).Documents.ToList();

                Assert.True(items.Count > 0);
                Assert.AreEqual("Бюль бюль оглы", items[0].LastName);
            }
            catch
            {
                _elasticClient.DeleteIndex(i => i.Index("test"));
                _elasticClient.Refresh();
            }
        }

        [Test]
        public void ShouldFilterOnFields()
        {
            _elasticClient.DeleteIndex(i => i.Index("testindex"));
            _elasticClient.Refresh();
            _elasticClient.CreateIndex("testindex", c => c.AddMapping<TestPerson>(m => m.MapFromAttributes()));

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

                _elasticClient.Index(test);
                _elasticClient.Index(test1);
                _elasticClient.Index(test2);
                _elasticClient.Index(test3);
                _elasticClient.Refresh();

                var item = _elasticClient.Get<TestPerson>("345243");
                Assert.IsNotNull(item);


                //term should be camel-case field name!
                var filter = Filter<TestPerson>.Term(f => f.LastName, "сидоров");
                items = _elasticClient.Search<TestPerson>(f => f.Filter(ff => ff.Term(i => i.LastName, "сидоров")));

                results = _elasticClient.Search<TestPerson>(s => s
                    .From(0)
                    .Size(10)
                    .Fields(f => f.LastName, f => f.FirstName)
                    .Query(q => q.Wildcard(f => f.LastName, "*оров*")
                    ));

                results1 = _elasticClient.Search<TestPerson>(s => s
                    .From(0)
                    .Size(10)
                    .Fields(f => f.LastName, f => f.FirstName)
                    .Query(q => q.Term(f => f.Id, "83453")
                    ));

                results2 = _elasticClient.Search<TestPerson>(s => s
                    .From(0)
                    .Size(10)
                    .Fields(f => f.LastName, f => f.FirstName)
                    .Query(q => q.Term(f => f.Another, "234233")
                    ));

                results3 = _elasticClient.Search<TestPerson>(s => s
                    .From(0)
                    .Size(10)
                    .Fields(f => f.LastName, f => f.FirstName)
                    .Query(q => q.Term(f => f.LastName, "сидоров")
                    ));
            }
            catch
            {
                _elasticClient.DeleteIndex(i => i.Index("test"));
                _elasticClient.Refresh();
            }

            Assert.AreEqual(results.Hits.Count(), 1);
            Assert.AreEqual(results1.Hits.Count(), 1);
            Assert.AreEqual(results2.Hits.Count(), 1);
            Assert.AreEqual(results3.Hits.Count(), 1);
            Assert.AreEqual(items.Hits.Count(), 1);
        }
    }
}