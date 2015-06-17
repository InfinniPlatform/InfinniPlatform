using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    ///   Провайдер административных операций с индексом ElasticSearch
    /// 
    ///   Предназначен для абстрагирования от процесса создания маппингов индексов ElasticSearch
    /// </summary>
    public sealed class IndexStateProvider : IIndexStateProvider
    {
        private readonly ElasticConnection _connection;
       

        public IndexStateProvider()
        {
            _connection = new ElasticConnection();            
        }

        /// <summary>
        /// Получить состояние индекса
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа данных</param>
        /// <returns>Состояние индекса</returns>
        public IndexStatus GetIndexStatus(string indexName, string typeName)
        {
            return new MultipleTypeIndex().GetIndexStatus(indexName, typeName);
	    }

	    /// <summary>
        ///   В ходе выполнения операции удалятся все данные и маппинга всех типов из индекса.
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование удаляемого типа из индекса (Удаляются все версии типа из всех индексов, относящихся к совокупности (алиасу))</param>
        public void RecreateIndex(string indexName, string typeName)
        {
            var index = new MultipleTypeIndex();
            index.DeleteIndexType(indexName, typeName);
            index.CreateIndexType(indexName, typeName);
        }
        
        /// <summary>
        /// Обновление позволяет делать запросы к только что добавленным данным
        /// </summary>
        public void Refresh()
        {
            _connection.Client.Refresh(f => f.Force());
        }

        /// <summary>
        ///  Удалить индекс с указанным наименованием
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа в индексе</param>
        public void DeleteIndexType(string indexName, string typeName)
        {
            var index = new MultipleTypeIndex();
            index.DeleteIndexType(indexName, typeName);
        }

        /// <summary>
        ///  Создает новую версию индекса с указанными настройками маппинга объектов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Тип документа в индексе, для которого создаем новую версию. 
        ///     Если не указано, создается новая версия всего индекса
        ///     Иначе создается новая версия типа внутри индекса
        /// </param>
        /// <param name="deleteExistingVersion">Удалить существующую версию</param>
        /// <param name="indexTypeMapping">Список изменений в маппинге</param>
        public void CreateIndexType(
            string indexName, 
            string typeName, 
            bool deleteExistingVersion = false, 
            IIndexTypeMapping indexTypeMapping = null)
        {
            var index = new MultipleTypeIndex();

            var schemaVersionName = index.CreateIndexType(indexName, typeName, deleteExistingVersion);

            if (indexTypeMapping != null)
            {
                IndexTypeMapper.ApplyIndexTypeMapping(
                    _connection.Client, 
                    indexName, 
                    schemaVersionName, 
                    indexTypeMapping.Properties);
            }
        }

        /// <summary>
        ///   Удалить весь индекс для всех типов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        public void DeleteIndex(string indexName)
        {
            var index = new MultipleTypeIndex();
            index.DeleteIndex(indexName);
        }
      
    }
}