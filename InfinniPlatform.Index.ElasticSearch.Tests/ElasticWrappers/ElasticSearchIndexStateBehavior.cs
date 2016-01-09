using System.Linq;

using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Environment.Index;

using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class ElasticSearchIndexStateBehavior
    {
        [Test]
        public void ShouldMakeCompleteIndexingProcess()
        {
            var elasticTypeManager = ElasticFactoryBuilder.ElasticTypeManager.Value;

            elasticTypeManager.DeleteIndex("indexstatebehavior");
            elasticTypeManager.DeleteIndex("indexstatebehavior1");

            Assert.IsFalse(elasticTypeManager.TypeExists("indexstatebehavior", "TestPerson"));

            elasticTypeManager.DeleteType("indexstatebehavior", "TestPerson");
            elasticTypeManager.CreateType("indexstatebehavior", "TestPerson");

            Assert.IsTrue(elasticTypeManager.TypeExists("indexstatebehavior", "TestPerson"));

            //создаем новую версию типа

            elasticTypeManager.CreateType("indexstatebehavior", "TestPerson");

            Assert.IsTrue(elasticTypeManager.TypeExists("indexstatebehavior", "TestPerson"));

            //проверям, что в инедксе существуют 2 версии, со старым маппингом и с новым
            var typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior", "testPerson").ToArray();

            Assert.AreEqual(2, typeMappings.Length);
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_1"));

            //проверяем создание другого типа в том же индексе
            elasticTypeManager.CreateType("indexstatebehavior", "TestPerson1");

            //проверям, что в индексе существуют 2 типа
            typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior", "testPerson")
                                             .Union(elasticTypeManager.GetTypeMappings("indexstatebehavior", "TestPerson1"))
                                             .ToArray();

            Assert.AreEqual(3, typeMappings.Length);
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson1_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_1"));

            //проверяем удаление типа из индекса
            elasticTypeManager.DeleteType("indexstatebehavior", "TestPerson");
            //проверяем, что один из типов удалился
            typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior", "testPerson").ToArray();

            Assert.AreEqual(0, typeMappings.Length);

            //проверяем, что второй тип существует
            typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior", "testPerson1").ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            Assert.True(typeMappings.GetMappingsBaseTypeNames().Contains("testperson1"));
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.GetMappingsTypeNames().First());

            //проверяем создание типа с таким же именем в другом индексе
            elasticTypeManager.CreateType("indexstatebehavior1", "TestPerson1");

            typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior1","testPerson1").ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            Assert.AreEqual("testperson1", typeMappings.GetMappingsBaseTypeNames().First());
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.GetMappingsTypeNames().First());

            //проверяем повторное создание того же типа
            elasticTypeManager.CreateType("indexstatebehavior", "TestPerson");
            typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior", "testPerson").ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            //проверяем, что создалась версия именно с нулевым индексом
            Assert.True(typeMappings.GetMappingsTypeNames().Contains("testperson_typeschema_0"));

            //проверяем удаление индекса целиком
            elasticTypeManager.DeleteIndex("indexstatebehavior");
            typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior", "testPerson").ToArray();

            Assert.AreEqual(0, typeMappings.Length);

            //проверяем, что другой индекс не удалился
            typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior1", "testPerson1").ToArray();

            Assert.AreEqual(1, typeMappings.Length);

            //проверяем удаление предыдущей версии маппинга при создании новой
            elasticTypeManager.CreateType("indexstatebehavior1", "testPerson1", deleteExistingVersion: true);

            typeMappings = elasticTypeManager.GetTypeMappings("indexstatebehavior1", "testPerson1").ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            Assert.AreEqual("testperson1_typeschema_1", typeMappings.GetMappingsTypeNames().First());

            elasticTypeManager.DeleteIndex("indexstatebehavior1");
        }
    }
}