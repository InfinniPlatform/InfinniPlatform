using System.Linq;

using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Sdk.Environment.Index;

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

            _indexStateProvider = new ElasticFactory().BuildIndexStateProvider();
            _indexStateProvider.DeleteIndex("indexstatebehavior");
            _indexStateProvider.DeleteIndex("indexstatebehavior1");

            Assert.AreEqual(IndexStatus.NotExists, _indexStateProvider.GetIndexStatus("indexstatebehavior", "TestPerson"));

            _indexStateProvider.RecreateIndex("indexstatebehavior", "TestPerson");

            Assert.AreEqual(IndexStatus.Exists, _indexStateProvider.GetIndexStatus("indexstatebehavior", "TestPerson"));

            //создаем новую версию типа

            _indexStateProvider.CreateIndexType("indexstatebehavior", "TestPerson");

            Assert.AreEqual(IndexStatus.Exists, _indexStateProvider.GetIndexStatus("indexstatebehavior", "TestPerson"));

            //проверям, что в инедксе существуют 2 версии, со старым маппингом и с новым
            var typeMappings = connection.GetAllTypesNest("indexstatebehavior", new[] { "testPerson" });

            Assert.AreEqual(1, typeMappings.Count);
            Assert.True(typeMappings.First().Value.Any(x => x.TypeName == "testperson_typeschema_0"));
            Assert.True(typeMappings.First().Value.Any(x => x.TypeName == "testperson_typeschema_1"));

            //проверяем создание другого типа в том же индексе
            _indexStateProvider.CreateIndexType("indexstatebehavior", "TestPerson1");

            //проверям, что в индексе существуют 2 типа
            typeMappings = connection.GetAllTypesNest("indexstatebehavior", new[] { "testPerson", "TestPerson1" });

            Assert.AreEqual(1, typeMappings.Count);
            Assert.AreEqual(3, typeMappings.First().Value.Count);

            var mappings = typeMappings.First().Value.ToArray();

            Assert.AreEqual("testperson1_typeschema_0", mappings[0].TypeName);
            Assert.AreEqual("testperson_typeschema_0", mappings[1].TypeName);
            Assert.AreEqual("testperson_typeschema_1", mappings[2].TypeName);

            //проверяем удаление типа из индекса
            _indexStateProvider.DeleteIndexType("indexstatebehavior", "TestPerson");
            //проверяем, что один из типов удалился
            typeMappings = connection.GetAllTypesNest("indexstatebehavior", new[] { "testPerson" });

            Assert.AreEqual(0, typeMappings.First().Value.Count);

            //проверяем, что второй тип существует
            typeMappings = connection.GetAllTypesNest("indexstatebehavior", new[] { "testPerson1" });

            Assert.AreEqual(1, typeMappings.Count);
            Assert.AreEqual("indexstatebehavior", typeMappings.First().Key);
            Assert.True(typeMappings.First().GetMappingsBaseTypeNames().Contains("testperson1"));
            Assert.AreEqual(1, typeMappings.First().Value.Count);
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.First().GetMappingsTypeNames().First());

            //проверяем создание типа с таким же именем в другом индексе
            _indexStateProvider.CreateIndexType("indexstatebehavior1", "TestPerson1");

            typeMappings = connection.GetAllTypesNest("indexstatebehavior1", new[] { "testPerson1" });

            Assert.AreEqual(1, typeMappings.Count);
            Assert.AreEqual("indexstatebehavior1", typeMappings.First().Key);
            Assert.AreEqual("testperson1", typeMappings.First().GetMappingsBaseTypeNames().First());
            Assert.AreEqual(1, typeMappings.First().Value.Count);
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.First().GetMappingsTypeNames().First());

            //проверяем повторное создание того же типа
            _indexStateProvider.CreateIndexType("indexstatebehavior", "TestPerson");
            typeMappings = connection.GetAllTypesNest("indexstatebehavior", new[] { "testPerson" });

            Assert.AreEqual(1, typeMappings.Count);
            Assert.AreEqual(1, typeMappings.First().Value.Count);
            //проверяем, что создалась версия именно с нулевым индексом
            Assert.True(typeMappings.First().GetMappingsTypeNames().Contains("testperson_typeschema_0"));

            //проверяем удаление индекса целиком
            _indexStateProvider.DeleteIndex("indexstatebehavior");
            typeMappings = connection.GetAllTypesNest("indexstatebehavior", new[] { "testPerson" });

            Assert.AreEqual(0, typeMappings.Count);

            //проверяем, что другой индекс не удалился
            typeMappings = connection.GetAllTypesNest("indexstatebehavior1", new[] { "testPerson1" });

            Assert.AreEqual(1, typeMappings.Count);

            //проверяем удаление предыдущей версии маппинга при создании новой
            _indexStateProvider.CreateIndexType("indexstatebehavior1", "testPerson1", true);

            typeMappings = connection.GetAllTypesNest("indexstatebehavior1", new[] { "testPerson1" });

            Assert.AreEqual(1, typeMappings.Count);
            Assert.AreEqual("testperson1_typeschema_1", typeMappings.First().GetMappingsTypeNames().First());

            _indexStateProvider.DeleteIndex("indexstatebehavior1");
        }
    }
}