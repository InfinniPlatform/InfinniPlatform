using System;
using System.Collections.Generic;
using System.Linq;

using Elasticsearch.Net.ConnectionPool;

using InfinniPlatform.Api.Settings;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;
using InfinniPlatform.Index.ElasticSearch.Properties;
using InfinniPlatform.Sdk.Environment.Index;

using Nest;
using Nest.Resolvers;

using IndexStatus = InfinniPlatform.Sdk.Environment.Index.IndexStatus;
using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    /// Соединение с ElasticSearch.
    /// </summary>
    public sealed class ElasticConnection : IElasticConnection
    {
        private static readonly Lazy<ElasticClient> ElasticClient;
        private static Dictionary<string, IList<TypeMapping>> _mappingsCache;

        static ElasticConnection()
        {
            // TODO: Избавиться от статического конструктора и нормально зарегистрировать все зависимости

            /* Remember: The client is thread-safe, so you can use a single client, in which case, passing a per request configuration
             * is the only way to pass state local to the request. Instantiating a client each time is also supported. In this case each
             * client instance could hold a different ConnectionSettings object with their own set of basic authorization credentials.
             * Do note that if you new a client each time (or your IoC does), they all should use the same IConnectionPool instance.
             * https://www.elastic.co/blog/nest-and-elasticsearch-net-1-3
             */

            // TODO: Избавиться от прямого обращения к AppConfiguration.Instance

            var settings = AppConfiguration.Instance.GetSection<ElasticSearchSettings>(ElasticSearchSettings.SectionName);

            ElasticClient = new Lazy<ElasticClient>(() => CreatElasticClient(settings));
            ResetMappingsCache();
        }

        /// <summary>
        /// Возвращает клиента для работы с индексом.
        /// </summary>
        public ElasticClient Client => ElasticClient.Value;

        private static void ResetMappingsCache()
        {
            var elasticMappings = ElasticClient.Value.GetMapping(new GetMappingRequest("_all", "_all")).Mappings;

            _mappingsCache = new Dictionary<string, IList<TypeMapping>>(elasticMappings, StringComparer.OrdinalIgnoreCase);
        }

        private static ElasticClient CreatElasticClient(ElasticSearchSettings settings)
        {
            var nodeAddresses = settings.Nodes.Select(node => new Uri(node));

            var connectionPool = new SniffingConnectionPool(nodeAddresses);

            var connectionSettings = new ConnectionSettings(connectionPool);

            if (!string.IsNullOrEmpty(settings.Login) && !string.IsNullOrEmpty(settings.Password))
            {
                connectionSettings.SetBasicAuthentication(settings.Login, settings.Password);
            }

            connectionSettings.SetDefaultPropertyNameInferrer(i => i);
            connectionSettings.SetJsonSerializerSettingsModifier(m => m.ContractResolver = new ElasticContractResolver(connectionSettings));

            var client = new ElasticClient(connectionSettings);

            return client;
        }

        public void Refresh()
        {
            // TODO: От этого нужно избавиться!!!

            Client.Refresh(i => i.Force());
        }

        /// <summary>
        /// Возвращает все типы, являющиеся версиями указанного типа в индексе
        /// </summary>
        /// <param name="indexName">Наименование индекса, по которым выполняем поиск.</param>
        /// <param name="typeNames">Типы данных, для которого получаем все версии типов в индексе.</param>
        /// <returns>Список всех версий типов данных в указанных индексах.</returns>
        /// <remarks>
        /// Например product_schema_0, product_schema_1 являются версиями типа product и искать данные можно по всем этим типам.
        /// </remarks>
        public IList<TypeMapping> GetIndexMappings(string indexName)
        {
            indexName = indexName.ToLower();

            //            var actualMappings = ElasticClient.Value.GetMapping(new GetMappingRequest("_all", "_all")).Mappings;
            if (_mappingsCache.ContainsKey(indexName))
            {
                return _mappingsCache[indexName];
            }

            return new List<TypeMapping>();
        }

        public IEnumerable<TypeMapping> GetTypeMappings(string indexName, IEnumerable<string> typeNames)
        {
            indexName = indexName.ToLower();
            var typeNamesArray = typeNames.Select(typeName => typeName.ToLower()).ToArray();



            if (typeNames == null || !typeNamesArray.Any())
            {
                throw new ArgumentException("Type name for index should not be empty.");
            }
            //            var actualMappings = ElasticClient.Value.GetMapping(new GetMappingRequest("_all", "_all")).Mappings;
            if (_mappingsCache.ContainsKey(indexName))
            {
                var isBaseTypes = !typeNamesArray.All(s => s.Contains(IndexTypeMapper.MappingTypeVersionPattern));

                return isBaseTypes
                           ? _mappingsCache[indexName].Where(mapping => typeNamesArray.Contains(mapping.TypeName.GetTypeBaseName()))
                           : _mappingsCache[indexName].Where(mapping => typeNamesArray.Contains(mapping.TypeName));
            }

            return Enumerable.Empty<TypeMapping>();
        }


        public string GetIndexName(string configuration)
        {
            var indexName = configuration.ToLower();

            return indexName;
        }

        public string GetBaseTypeName(string documentType)
        {
            var baseTypeName = documentType.ToLower() + IndexTypeMapper.MappingTypeVersionPattern;

            return baseTypeName;
        }

        public string GetActualTypeName(string configuration, string documentType)
        {
            // TODO: Данный метод должен возвращать актуальное имя типа для версии приложения.
            // Вместо этого он возвращает имя последнего созданного типа, что в свою очередь
            // нарушает работу более ранних версий приложения.

            string actualTypeName = null;

            var indexName = GetIndexName(configuration);
            var baseTypeName = GetBaseTypeName(documentType);

            IList<TypeMapping> indexMappings;

            if (_mappingsCache.TryGetValue(indexName, out indexMappings))
            {
                var maxVersion = 0;

                foreach (var typeMapping in indexMappings)
                {
                    if (typeMapping.TypeName.StartsWith(baseTypeName))
                    {
                        var version = typeMapping.GetTypeVersion().GetValueOrDefault();

                        if (version >= maxVersion)
                        {
                            actualTypeName = typeMapping.TypeName;
                            maxVersion = version;
                        }
                    }
                }
            }

            return actualTypeName;
        }


        /// <summary>
        /// Возвращает актуальную схему для документов, хранимых в типе.
        /// </summary>
        public IList<PropertyMapping> GetPropertyMappings(string indexName, string typeName)
        {
            indexName = indexName.ToLower();
            typeName = typeName.ToLower();

            //TODO Рассмотреть возможность использования типов данных из NEST.
            var propertyMappings = new List<PropertyMapping>();

            //            var actualMappings = ElasticClient.Value.GetMapping(new GetMappingRequest("_all", "_all")).Mappings;
            if (!_mappingsCache.ContainsKey(indexName))
            {
                return propertyMappings;
            }

            var indexMappings = _mappingsCache[indexName];

            var isBaseType = !typeName.Contains(IndexTypeMapper.MappingTypeVersionPattern);

            var mappingsByVersion = isBaseType
                                        ? indexMappings.Where(mapping => mapping.TypeName.GetTypeBaseName() == typeName)
                                        : indexMappings.Where(mapping => mapping.TypeName == typeName);

            var typeMapping = mappingsByVersion.OrderByDescending(mapping => mapping.TypeName.GetTypeVersion());
            var rootObjectMapping = typeMapping.FirstOrDefault()?.Mapping;

            if (rootObjectMapping != null && rootObjectMapping.Properties.ContainsKey("Values"))
            {
                var properties = (rootObjectMapping.Properties["Values"] as ObjectMapping)?.Properties;

                if (properties != null)
                {
                    propertyMappings.AddRange(properties.Select(ExtractProperty));
                }

                return propertyMappings;
            }

            return propertyMappings;
        }

        /// <summary>
        /// Получение статуса кластера.
        /// </summary>
        public string GetStatus()
        {
            var settings = AppConfiguration.Instance.GetSection<ElasticSearchSettings>(ElasticSearchSettings.SectionName);

            var health = ElasticClient.Value.ClusterHealth();
            var nodeAddresses = string.Join("; ", settings.Nodes);

            return $"cluster name - {health.ClusterName}, status - {health.Status}, number of nodes: {health.NumberOfNodes}, configured nodes: {nodeAddresses}";
        }

        private static PropertyMapping ExtractProperty(KeyValuePair<PropertyNameMarker, IElasticType> property)
        {
            PropertyDataType dataType;
            var propertyType = property.Value.Type.Name;
            var propertyName = property.Key.Name;

            if (propertyType == "string")
            {
                dataType = PropertyDataType.String;
            }
            else if (propertyType == "date")
            {
                dataType = PropertyDataType.Date;
            }
            else if (propertyType == "binary")
            {
                dataType = PropertyDataType.Object; //в эластике храним только ссылку на бинарные данные, сами бинарные данные хранятся в Cassandra
            }
            else if (propertyType == "boolean")
            {
                dataType = PropertyDataType.Boolean;
            }
            else if (propertyType == "double" || propertyType == "float")
            {
                dataType = PropertyDataType.Float;
            }
            else if (propertyType == "integer" || propertyType == "long")
            {
                dataType = PropertyDataType.Integer;
            }
            else if (propertyType == "object")
            {
                var objectMapping = property.Value as ObjectMapping;

                if (objectMapping?.Properties != null)
                {
                    var propertiesList = objectMapping.Properties.Select(ExtractProperty).ToList();

                    return new PropertyMapping(propertyName, propertiesList);
                }

                dataType = PropertyDataType.Object;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            var hasSortingFiled = (property.Value as MultiFieldMapping)?.Fields.Any() == true;

            return new PropertyMapping(propertyName, dataType, hasSortingFiled);
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

            var typesNest = GetTypeMappings(indexName, new[] { typeName });

            return typesNest.Any()
                       ? IndexStatus.Exists
                       : IndexStatus.NotExists;
        }

        /// <summary>
        /// Удалить индекс с указанным наименованием
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа в индексе</param>
        public void DeleteType(string indexName, string typeName)
        {
            indexName = indexName.ToLowerInvariant();

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(Resources.EmptyIndexTypeName);
            }

            var typeMappings = GetTypeMappings(indexName, new[] { typeName });

            foreach (var mapping in typeMappings)
            {
                //var mappingIndexName = mappings.Key;
                //удаляем только маппинги типов в индексе
                Client.DeleteMapping<dynamic>(d => d.Index(indexName).Type(mapping.TypeName));
            }

            Refresh();
            ResetMappingsCache();
        }

        /// <summary>
        /// Создает новую версию индекса с указанными настройками маппинга объектов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">
        /// Тип документа в индексе, для которого создаем новую версию.
        /// создается новая версия типа внутри индекса
        /// </param>
        /// <param name="properties">Список изменений в маппинге</param>
        /// <param name="deleteExistingVersion">Удалить существующую версию</param>
        /// <param name="searchAbility">Возможности поиска по индексу</param>
        public string CreateType(string indexName, string typeName, IList<PropertyMapping> properties = null, bool deleteExistingVersion = false, SearchAbilityType searchAbility = SearchAbilityType.KeywordBasedSearch)
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

            var typeMappings = GetTypeMappings(indexName, new[] { typeName });

            var schemaTypeVersionNumber = 0;

            //Если существует указанный тип
            if (typeMappings.Any() && typeMappings.Any())
            {
                //TODO вычисляем номер следующей версии маппинга
                schemaTypeVersionNumber = typeMappings.GetTypeActualVersion(typeName) + 1;
            }

            if (typeMappings.Any() && deleteExistingVersion)
            {
                foreach (var typeMapping in typeMappings)
                {
                    Client.DeleteMapping<dynamic>(d => d.Index(indexName).Type(typeMapping.TypeName));
                }
            }

            var schemaTypeVersion = (typeName + IndexTypeMapper.MappingTypeVersionPattern + schemaTypeVersionNumber).ToLowerInvariant();

            //если индекса не существует, вначале создаем сам индекс
            if (!Client.IndexExists(i => i.Index(indexName)).Exists)
            {
                var result = Client.CreateIndex(i => i.Index(indexName));

                if (!result.ConnectionStatus.Success)
                {
                    if (result.ConnectionStatus.OriginalException != null &&
                        !result.ConnectionStatus.OriginalException.Message.ToLowerInvariant().Contains("already exists"))
                    {
                        throw new Exception($"fail to create index version \"{indexName}\" ");
                    }
                }
            }

            Refresh();

            Client.Map<dynamic>(s => s.Index(indexName)
                                      .Type(schemaTypeVersion)
                                      .SearchAnalyzer("string_lowercase")
                                      .IndexAnalyzer(searchAbility.ToString().ToLowerInvariant()));

            var mapping = Client.GetMapping<dynamic>(d => d.Index(indexName).Type(schemaTypeVersion));

            if (mapping == null)
            {
                throw new ArgumentException($"Fail to create type name mapping: \"{typeName}\"");
            }

            if (properties != null)
            {
                IndexTypeMapper.ApplyIndexTypeMapping(Client, indexName, schemaTypeVersion, properties);
            }

            ResetMappingsCache();

            return schemaTypeVersion;
        }

        public void DeleteIndex(string indexName)
        {
            Client.DeleteIndex(i => i.Index(indexName));
            ResetMappingsCache();
        }
    }
}