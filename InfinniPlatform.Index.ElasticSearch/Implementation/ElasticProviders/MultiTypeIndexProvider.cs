using InfinniPlatform.Index.ElasticSearch.Implementation.Extensions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Versioning.IndexStrategies;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Nest.Resolvers;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
	/// Провайдер операций с индексом ElasticSearch, поддерживающий хранение нескольких
	/// типов в одном индексе. В данном случае, индекс является аналогом пространства имен.
	/// 
	/// В реализации провайдера не реализованы концепции, характерные для других провайдеров
	/// (версионность, использование алиасов)
	/// 
	/// </summary>
    public sealed class MultiTypeIndexProvider : IMultiTypeIndexProvider
    {
	    private readonly ElasticConnection _connection;
        
        public MultiTypeIndexProvider()
		{
		    _connection = new ElasticConnection();
		}

        /// <summary>
        /// Помещает документы указанного типа в индекс
        /// </summary>
        /// <param name="indexName">Имя индекса</param>
        /// <param name="typeName">Имя типа</param>
        /// <param name="documents">Документы, которые необходимо поместить</param>
        /// <param name="indexTypeMapping"></param>
        public void SetDocuments(
            string indexName, 
            string typeName, 
            IEnumerable<object> documents, 
            IIndexTypeMapping indexTypeMapping = null)
        {
            if (documents == null)
            {
                return;
            }

            indexName = indexName.ToLowerInvariant();
            typeName = typeName.ToLowerInvariant();

            if (indexTypeMapping != null && 
                _connection.IndexingClient.GetMapping(indexName, typeName) == null)
            {
                if (_connection.IndexingClient.IndexExists(indexName).Exists == false)
                {
                    _connection.IndexingClient.CreateIndex(indexName, new IndexSettings());
                }

                ApplyIndexTypeMapping(indexTypeMapping, indexName, typeName);
            }
            
            _connection.IndexingClient.IndexMany(
                documents.Select(item => IndexObjectExtension.ToIndexObject(item, new UpdateItemStrategy())),
                indexName,
                typeName);
        }

        /// <summary>
        /// Осуществляет поиск документа по критерию
        /// </summary>
        /// <param name="indexName">Имя индекса</param>
        /// <param name="typeName">Имя типа</param>
        /// <param name="id">Идентификатор запрашиваемого документа</param>
        /// <returns>Результат поиска</returns>
        public dynamic GetDocument(string indexName, string typeName, string id)
        {
            return _connection.IndexingClient.Get<IndexObject>(
                indexName.ToLowerInvariant(), 
                typeName.ToLowerInvariant(), 
                id).Values;
        }

        /// <summary>
        /// Осуществляет поиск документа по критерию
        /// </summary>
        /// <param name="indexName">Имя индекса</param>
        /// <param name="typeName">Имя типа</param>
        /// <param name="ids">Идентификаторы запрашиваемых документов</param>
        /// <returns>Результат поиска</returns>
        public IEnumerable<dynamic> GetDocuments(string indexName, string typeName, IEnumerable<string> ids)
        {
            return _connection.IndexingClient.MultiGet<IndexObject>(
                indexName.ToLowerInvariant(),
                typeName.ToLowerInvariant(),
                ids).Select(v => v.Values);
        }

        /// <summary>
        /// Задание схемы для документов, хранимых в индексе
        /// </summary>
        private void ApplyIndexTypeMapping(IIndexTypeMapping indexTypeMapping, string indexName, string typeName)
        {
            var objectMapping = new RootObjectMapping
            {
                IndexAnalyzer = indexTypeMapping.IndexAnalyzer,
                Properties = new Dictionary<string, IElasticType>(),
                TypeNameMarker = new TypeNameMarker
                {
                    Name = typeName,
                }
            };

            var valuesType = new ObjectMapping { Name = "Values", Properties = new Dictionary<string, IElasticType>() };

            foreach (var property in indexTypeMapping.Properties)
            {
                valuesType.Properties.Add(property.Name, IndexStateProvider.GetElasticTypeByMapping(property));
            }

            objectMapping.Properties.Add("Values", valuesType);

            _connection.IndexingClient.Map(objectMapping, indexName, typeName);
        }
    }
}