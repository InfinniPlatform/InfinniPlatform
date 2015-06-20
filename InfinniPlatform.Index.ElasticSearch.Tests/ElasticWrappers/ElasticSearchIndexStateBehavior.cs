using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class ElasticSearchIndexStateBehavior
    {
        private IIndexStateProvider _indexStateProvider;

        [Test]
        public void ShouldMakeCompleteIndexingProcess()
        {
            var connection = new ElasticConnection();

            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

            _indexStateProvider.DeleteIndex("indexstatebehavior");
            _indexStateProvider.DeleteIndex("indexstatebehavior1");

            Assert.AreEqual(IndexStatus.NotExists,
                            _indexStateProvider.GetIndexStatus("indexstatebehavior", "TestPerson"));

            _indexStateProvider.RecreateIndex("indexstatebehavior", "TestPerson");

            Assert.AreEqual(IndexStatus.Exists, _indexStateProvider.GetIndexStatus("indexstatebehavior", "TestPerson"));

            //создаем новую версию типа

            _indexStateProvider.CreateIndexType("indexstatebehavior", "TestPerson");

            Assert.AreEqual(IndexStatus.Exists, _indexStateProvider.GetIndexStatus("indexstatebehavior", "TestPerson"));

            //проверям, что в инедксе существуют 2 версии, со старым маппингом и с новым
            IEnumerable<IndexToTypeAccordance> typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior"},
                                                                                     new[] {"testPerson"});

            Assert.AreEqual(1, typeMappings.Count());
            Assert.True(typeMappings.First().TypeNames.Contains("testperson_typeschema_0"));
            Assert.True(typeMappings.First().TypeNames.Contains("testperson_typeschema_1"));

            //проверяем создание другого типа в том же индексе
            _indexStateProvider.CreateIndexType("indexstatebehavior", "TestPerson1");

            //проверям, что в индексе существуют 2 типа
            typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior"}, new[] {"testPerson", "TestPerson1"});

            Assert.AreEqual(2, typeMappings.Count());
            IndexToTypeAccordance[] mappings = typeMappings.ToArray();
            Assert.True(mappings[0].TypeNames.Contains("testperson_typeschema_0"));
            Assert.True(mappings[0].TypeNames.Contains("testperson_typeschema_1"));
            Assert.True(mappings[1].TypeNames.Contains("testperson1_typeschema_0"));

            //проверяем удаление типа из индекса
            _indexStateProvider.DeleteIndexType("indexstatebehavior", "TestPerson");

            //проверяем, что один из типов удалился
            typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior"}, new[] {"testPerson"});
            Assert.AreEqual(0, typeMappings.Count());

            //проверяем, что второй тип существует
            typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior"}, new[] {"testPerson1"});
            Assert.AreEqual(1, typeMappings.Count());
            Assert.AreEqual("indexstatebehavior", typeMappings.First().IndexName);
            Assert.AreEqual("testperson1", typeMappings.First().BaseTypeName);
            Assert.AreEqual(1, typeMappings.First().TypeNames.Count());
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.First().TypeNames.First());

            //проверяем создание типа с таким же именем в другом индексе
            _indexStateProvider.CreateIndexType("indexstatebehavior1", "TestPerson1");

            typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior1"}, new[] {"testPerson1"});
            Assert.AreEqual(1, typeMappings.Count());
            Assert.AreEqual("indexstatebehavior1", typeMappings.First().IndexName);
            Assert.AreEqual("testperson1", typeMappings.First().BaseTypeName);
            Assert.AreEqual(1, typeMappings.First().TypeNames.Count());
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.First().TypeNames.First());

            //проверяем повторное создание того же типа
            _indexStateProvider.CreateIndexType("indexstatebehavior", "TestPerson");
            typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior"}, new[] {"testPerson"});

            Assert.AreEqual(1, typeMappings.Count());
            Assert.AreEqual(1, typeMappings.First().TypeNames.Count());
            //проверяем, что создалась версия именно с нулевым индексом
            Assert.True(typeMappings.First().TypeNames.Contains("testperson_typeschema_0"));

            //проверяем удаление индекса целиком
            _indexStateProvider.DeleteIndex("indexstatebehavior");
            typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior"}, new[] {"testPerson"});
            Assert.AreEqual(0, typeMappings.Count());

            //проверяем, что другой индекс не удалился

            typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior1"}, new[] {"testPerson1"});
            Assert.AreEqual(1, typeMappings.Count());

            //проверяем удаление предыдущей версии маппинга при создании новой
            _indexStateProvider.CreateIndexType("indexstatebehavior1", "testPerson1", deleteExistingVersion: true,
                                                mappingUpdates: null);

            typeMappings = connection.GetAllTypes(new[] {"indexstatebehavior1"}, new[] {"testPerson1"});
            Assert.AreEqual(1, typeMappings.Count());
            Assert.AreEqual("testperson1_typeschema_1", typeMappings.First().TypeNames.First());


            _indexStateProvider.DeleteIndex("indexstatebehavior1");
        }
    }
}