using InfinniPlatform.Api.Index;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Properties;


using System;
using System.Linq;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions
{
    public sealed class MultipleTypeIndex
    {
        private readonly ElasticConnection _connection;

        public MultipleTypeIndex()
        {
            _connection = new ElasticConnection();
        }

        /// <summary>
        /// Получить состояние индекса
        /// </summary>
        /// <param name="indexName">Наименование индекса, состояние которого получаем</param>
        /// <param name="typeName">Наименование типа, состояние которого получаем</param>
        /// <returns>Состояние индекса</returns>
        public IndexStatus GetIndexStatus(string indexName, string typeName)
        {

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(Resources.EmptyIndexTypeName);
            }

            indexName = indexName.ToLowerInvariant();

            var types = _connection.GetAllTypes(new[] { indexName }, new[] { typeName });

            if (!types.Any())
            {
                return IndexStatus.NotExists;
            }

            return IndexStatus.Exists;
        }

        /// <summary>
        ///  Удалить индекс с указанным наименованием
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа в индексе</param>
        public void DeleteIndexType(string indexName, string typeName)
        {
            indexName = indexName.ToLowerInvariant();

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(Resources.EmptyIndexTypeName);
            }

            var types = _connection.GetAllTypes(new [] {indexName}, new[] { typeName });

            foreach (var indexToTypeAccordance in types)
            {
                //удаляем только маппинги типов в индексе
                foreach (var derivedTypeName in indexToTypeAccordance.TypeNames)
                {
                    _connection.Client.DeleteMapping<dynamic>(d=>d.Index(indexToTypeAccordance.IndexName).Type(derivedTypeName));    
                }
            }

            _connection.Refresh();
        }

        /// <summary>
        ///  Создает новую версию индекса с указанными настройками маппинга объектов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Тип документа в индексе, для которого создаем новую версию. 
        ///     создается новая версия типа внутри индекса
        /// </param>
        /// <param name="deleteExistingVersion">Удалить существующую версию</param>
        /// <param name="searchAbility">Возможности поиска по индексу</param>
        public string CreateIndexType(
            string indexName, 
            string typeName, 
            bool deleteExistingVersion = false,
            SearchAbilityType searchAbility = SearchAbilityType.KeywordBasedSearch)
        {
            if (string.IsNullOrEmpty(indexName))
            {
                throw new ArgumentException("Index name for creation index can't be empty.");
            }

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("type name for creation type index can't be empty.");
            }

            indexName = indexName.ToLowerInvariant();

            var schemaTypes = _connection.GetAllTypes(new[] {indexName}, new[] {typeName});

            var schemaTypeVersionNumber = 0;

            //Если существует указанный тип
            if (schemaTypes.Any())
            {
                //вычисляем номер следующей версии маппинга
                schemaTypeVersionNumber =
                    int.Parse(
                        schemaTypes.First()
                            .TypeNames.OrderByDescending(s => s.ToLowerInvariant()).First()
                            .Substring((typeName + IndexTypeMapper.MappingTypeVersionPattern).Length)) + 1;
            }

            if (schemaTypes.Any() && deleteExistingVersion)
            {
                foreach (var indexToTypeAccordance in schemaTypes)
                {
                    foreach (var name in indexToTypeAccordance.TypeNames)
                    {
                        IndexToTypeAccordance accordance = indexToTypeAccordance;
                        _connection.Client.DeleteMapping<dynamic>(d => d.Index(accordance.IndexName).Type(name));
                    }
                }
            }

            string schemaTypeVersion = (typeName + IndexTypeMapper.MappingTypeVersionPattern + schemaTypeVersionNumber).ToLowerInvariant();
            
            //если индекса не существует, вначале создаем сам индекс
            if (!_connection.Client.IndexExists(i => i.Index(indexName)).Exists)
            {
                var result = _connection.Client.CreateIndex(i => i.Index(indexName));

                if (!result.ConnectionStatus.Success)
                {
                    if (result.ConnectionStatus.OriginalException != null &&
                        !result.ConnectionStatus.OriginalException.Message.ToLowerInvariant().Contains("already exists"))
                    {
                        throw new Exception(string.Format("fail to create index version \"{0}\" ", indexName));
                    }
                }
            }

            _connection.Refresh();

            _connection.Client.Map<dynamic>(s => s
                .Index(indexName)
                .Type(schemaTypeVersion)
                .SearchAnalyzer("string_lowercase")
                .IndexAnalyzer(searchAbility.ToString().ToLowerInvariant())
				);

            var mapping = _connection.Client.GetMapping<dynamic>(d => d.Index(indexName).Type(schemaTypeVersion));
            if (mapping == null)
            {
                throw new ArgumentException(string.Format("Fail to create type name mapping: \"{0}\"", typeName));
            }

            _connection.Refresh();
            return schemaTypeVersion;
        }


        public void DeleteIndex(string indexName)
        {
            _connection.Client.DeleteIndex(i=>i.Index(indexName));
        }
    }
}
