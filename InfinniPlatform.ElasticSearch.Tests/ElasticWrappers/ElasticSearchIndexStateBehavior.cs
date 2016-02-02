using System.Linq;

using InfinniPlatform.ElasticSearch.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.ElasticSearch.Tests.Builders;

using NUnit.Framework;

namespace InfinniPlatform.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class ElasticSearchIndexStateBehavior
    {
        [Test]
        public void ShouldMakeCompleteIndexingProcess()
        {
            var elasticTypeManager = ElasticFactoryBuilder.ElasticTypeManager.Value;

            elasticTypeManager.DeleteIndex();

            Assert.IsFalse(elasticTypeManager.TypeExists("TestPerson"));

            elasticTypeManager.CreateType("TestPerson");

            Assert.IsTrue(elasticTypeManager.TypeExists("TestPerson"));

            //создаем новую версию типа

            elasticTypeManager.CreateType("TestPerson");

            // проверяем, что в индексе существуют 2 версии, со старым маппингом и с новым
            var typeMappings = elasticTypeManager.GetTypeMappings("TestPerson").ToArray();

            Assert.AreEqual(2, typeMappings.Length);
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_1"));

            // проверяем создание другого типа в том же индексе
            elasticTypeManager.CreateType("TestPerson1");

            // проверяем, что в индексе существуют 2 типа
            typeMappings = elasticTypeManager.GetTypeMappings("TestPerson")
                                             .Union(elasticTypeManager.GetTypeMappings("TestPerson1"))
                                             .ToArray();

            Assert.AreEqual(3, typeMappings.Length);
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson1_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_1"));

            // проверяем удаление типа из индекса
            elasticTypeManager.DeleteType("TestPerson");

            // проверяем, что один из типов удалился
            typeMappings = elasticTypeManager.GetTypeMappings("TestPerson").ToArray();

            Assert.AreEqual(0, typeMappings.Length);

            // проверяем, что второй тип существует
            typeMappings = elasticTypeManager.GetTypeMappings("TestPerson1").ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            Assert.True(typeMappings.GetMappingsBaseTypeNames().Contains("testperson1"));
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.GetMappingsTypeNames().First());

            // проверяем удаление предыдущей версии маппинга при создании новой
            elasticTypeManager.CreateType("testPerson1", deleteExistingVersion: true);

            typeMappings = elasticTypeManager.GetTypeMappings("testPerson1").ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            Assert.AreEqual("testperson1_typeschema_1", typeMappings.GetMappingsTypeNames().First());

            elasticTypeManager.DeleteIndex();
        }
    }
}