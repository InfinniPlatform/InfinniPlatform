using System;
using NUnit.Framework;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    /// <summary>
    ///     Выполняет чистку хранилища ElasticSearch (удаляются все индексы).
    ///     Рекомендумется запускать этот тест перед всеми остальными тестами.
    /// </summary>
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    [Ignore("Очистка всех индексов")]
    public sealed class ElasticSearchInitialization
    {
        [Test]
        public void CleanElasticByDeletingAllIndecesSearch()
        {
            var elasticSettings = new ConnectionSettings(new Uri("http://127.0.0.1:9200"));
            var _client = new ElasticClient(elasticSettings);

            if (!_client.Connection.HeadSync(new Uri("http://127.0.0.1:9200")).Success)
            {
                throw new ArgumentException("connection error");
            }

            _client.DeleteIndex(i => i.Index("_all"));

            _client.Flush(f => f.Force());
        }
    }
}