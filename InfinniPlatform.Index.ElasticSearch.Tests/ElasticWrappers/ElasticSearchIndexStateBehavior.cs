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
        [Test]
        public void ShouldMakeCompleteIndexingProcess()
        {
            var elasticConnection = new ElasticConnection();

            elasticConnection.DeleteIndex("indexstatebehavior");
            elasticConnection.DeleteIndex("indexstatebehavior1");

            Assert.AreEqual(IndexStatus.NotExists, elasticConnection.GetIndexStatus("indexstatebehavior", "TestPerson"));

            elasticConnection.DeleteType("indexstatebehavior", "TestPerson");
            elasticConnection.CreateType("indexstatebehavior", "TestPerson");

            Assert.AreEqual(IndexStatus.Exists, elasticConnection.GetIndexStatus("indexstatebehavior", "TestPerson"));

            //создаем новую версию типа

            elasticConnection.CreateType("indexstatebehavior", "TestPerson");

            Assert.AreEqual(IndexStatus.Exists, elasticConnection.GetIndexStatus("indexstatebehavior", "TestPerson"));

            //проверям, что в инедксе существуют 2 версии, со старым маппингом и с новым
            var typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior", new[] { "testPerson" }).ToArray();

            Assert.AreEqual(2, typeMappings.Length);
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_1"));

            //проверяем создание другого типа в том же индексе
            elasticConnection.CreateType("indexstatebehavior", "TestPerson1");

            //проверям, что в индексе существуют 2 типа
            typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior", new[] { "testPerson", "TestPerson1" }).ToArray();

            Assert.AreEqual(3, typeMappings.Length);
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson1_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_0"));
            Assert.True(typeMappings.Any(x => x.TypeName == "testperson_typeschema_1"));

            //проверяем удаление типа из индекса
            elasticConnection.DeleteType("indexstatebehavior", "TestPerson");
            //проверяем, что один из типов удалился
            typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior", new[] { "testPerson" }).ToArray();

            Assert.AreEqual(0, typeMappings.Length);

            //проверяем, что второй тип существует
            typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior", new[] { "testPerson1" }).ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            Assert.True(typeMappings.GetMappingsBaseTypeNames().Contains("testperson1"));
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.GetMappingsTypeNames().First());

            //проверяем создание типа с таким же именем в другом индексе
            elasticConnection.CreateType("indexstatebehavior1", "TestPerson1");

            typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior1", new[] { "testPerson1" }).ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            Assert.AreEqual("testperson1", typeMappings.GetMappingsBaseTypeNames().First());
            Assert.AreEqual("testperson1_typeschema_0", typeMappings.GetMappingsTypeNames().First());

            //проверяем повторное создание того же типа
            elasticConnection.CreateType("indexstatebehavior", "TestPerson");
            typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior", new[] { "testPerson" }).ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            //проверяем, что создалась версия именно с нулевым индексом
            Assert.True(typeMappings.GetMappingsTypeNames().Contains("testperson_typeschema_0"));

            //проверяем удаление индекса целиком
            elasticConnection.DeleteIndex("indexstatebehavior");
            typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior", new[] { "testPerson" }).ToArray();

            Assert.AreEqual(0, typeMappings.Length);

            //проверяем, что другой индекс не удалился
            typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior1", new[] { "testPerson1" }).ToArray();

            Assert.AreEqual(1, typeMappings.Length);

            //проверяем удаление предыдущей версии маппинга при создании новой
            elasticConnection.CreateType("indexstatebehavior1", "testPerson1", deleteExistingVersion: true);

            typeMappings = elasticConnection.GetTypeMappings("indexstatebehavior1", new[] { "testPerson1" }).ToArray();

            Assert.AreEqual(1, typeMappings.Length);
            Assert.AreEqual("testperson1_typeschema_1", typeMappings.GetMappingsTypeNames().First());

            elasticConnection.DeleteIndex("indexstatebehavior1");
        }
    }
}