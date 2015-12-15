using System;
using System.Collections.Generic;
using System.Linq;

using Elasticsearch.Net.ConnectionPool;

using InfinniPlatform.Api.Settings;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;

using Nest;
using Nest.Resolvers;

using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    /// Соединение с ElasticSearch.
    /// </summary>
    public sealed class ElasticConnection
    {
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

            _elasticClient = new Lazy<ElasticClient>(() => CreatElasticClient(settings));
            _mappingProvider = new ElasticSearchMappingProvider(settings, _elasticClient);
        }


        private static readonly ElasticSearchMappingProvider _mappingProvider;
        private static readonly Lazy<ElasticClient> _elasticClient;


        private static ElasticClient CreatElasticClient(ElasticSearchSettings settings)
        {
            var nodeAddresses = GetActualNodes(settings);

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


        /// <summary>
        /// Возвращает клиента для работы с индексом.
        /// </summary>
        public ElasticClient Client
        {
            get { return _elasticClient.Value; }
        }


        public void Refresh()
        {
            // TODO: От этого нужно избавиться!!!

            Client.Refresh(i => i.Force());
        }


        /// <summary>
        /// Возвращает все типы, являющиеся версиями указанного типа в индексе
        /// </summary>
        /// <param name="indexNames">Наименования индексов, по которым выполняем поиск.</param>
        /// <param name="typeNames">Типы данных, для которого получаем все версии типов в индексе.</param>
        /// <returns>Список всех версий типов данных в указанных индексах.</returns>
        /// <remarks>
        /// Например product_schema_0, product_schema_1 являются версиями типа product и искать данные можно по всем этим типам.
        /// </remarks>
//        [Obsolete]
//        public IEnumerable<IndexToTypeAccordance> GetAllTypes(IEnumerable<string> indexNames, IEnumerable<string> typeNames)
//        {
//            if (typeNames == null || !typeNames.Any())
//            {
//                throw new ArgumentException("Type name for index should not be empty.");
//            }
//
//            // Ищем все маппинги для указанных типов (используем низкоуровневый вызов GetMapping).
//            // Ищем по всем типам и всем указанным индексам.
//            var indexToTypeAccordances = GetTypeVersions(indexNames, typeNames);
//            
//            return indexToTypeAccordances;
//        }

        public Dictionary<string, IList<TypeMapping>> GetAllTypesNest(string indexName, IEnumerable<string> typeNames)
        {
            var typeNamesArray = typeNames.ToArray();
            if (typeNames == null || !typeNamesArray.Any())
            {
                throw new ArgumentException("Type name for index should not be empty.");
            }

            var mappingsNest = _mappingProvider.FillIndexMappingsNest(indexName, typeNamesArray);
            // Ищем все маппинги для указанных типов (используем низкоуровневый вызов GetMapping).
            // Ищем по всем типам и всем указанным индексам.
            
            return mappingsNest;
        }

        /// <summary>
        /// Возвращает актуальную схему для документов, хранимых в типе.
        /// </summary>
        public IList<PropertyMapping> GetIndexTypeMapping(string indexName, string typeName)
        {
//            var schemaTypes = GetAllTypes(new[] { indexName }, new[] { typeName });
            var schemaTypesNest = GetAllTypesNest(indexName , new[] { typeName });

            if (schemaTypesNest == null || !schemaTypesNest.Any())
            {
                return new PropertyMapping[] { };
            }

//            var actualType = schemaTypes.GetActualTypeName(typeName);
            var actualTypeNest = schemaTypesNest.GetActualTypeNameNest(typeName);

//            var indexTypeMapping = _mappingProvider.GetIndexTypeMapping(indexName, actualType);
            var indexTypeMappingNest = _mappingProvider.GetIndexTypeMappingNest(indexName, actualTypeNest);

            //            return indexTypeMapping;
            return indexTypeMappingNest;
        }

        /// <summary>
        /// Получение статуса кластера.
        /// </summary>
        public string GetStatus()
        {
            var settings = AppConfiguration.Instance.GetSection<ElasticSearchSettings>(ElasticSearchSettings.SectionName);

            var health = _elasticClient.Value.ClusterHealth();
            var nodeAddresses = string.Join("; ", GetActualNodes(settings).Select(i => i.ToString()));

            return $"cluster name - {health.ClusterName}, status - {health.Status}, number of nodes: {health.NumberOfNodes}, configured nodes: {nodeAddresses}";
        }

        /// <summary>
        /// Возвращает список версий указанных типов в указанных индексах.
        /// </summary>
        /// <param name="indexNames">Список индексов для поиска</param>
        /// <param name="typeNames">Список типов, для которых ищем версии схем</param>
        /// <returns>Список версий схем</returns>
        private IEnumerable<IndexToTypeAccordance> GetTypeVersions(IEnumerable<string> indexNames, IEnumerable<string> typeNames)
        {
            // собираем маппинги по всем индексам и типам
            var mappings = _mappingProvider.FillIndexMappings(indexNames, typeNames);
            
            typeNames = typeNames.Select(t => t.ToLowerInvariant()).ToList();
            indexNames = indexNames.Select(s => s.ToLowerInvariant()).ToList();

            var indexToTypeAccordances = mappings.Where(m => indexNames.Contains(m.IndexName) && typeNames.Contains(m.BaseTypeName))
                                                 .OrderBy(m => m.IndexName)
                                                 .ThenBy(m => m.BaseTypeName)
                                                 .ToList();

            return indexToTypeAccordances;
        }

        //TODO REMOVE
//        private List<KeyValuePair<string, IList<TypeMapping>>> GetTypeVersionsNest(IEnumerable<string> indexNames, IEnumerable<string> typeNames)
//        {
//            // собираем маппинги по всем индексам и типам
//            var mappingsNest = _mappingProvider.FillIndexMappingsNest(indexNames, typeNames);
//
//            typeNames = typeNames.Select(t => t.ToLowerInvariant()).ToList();
//            indexNames = indexNames.Select(s => s.ToLowerInvariant()).ToList();
//
//            var indexToTypeAccordances = mappingsNest.Mappings.Where(m => indexNames.Contains(m.Key) && typeNames.Contains(m.Value[0].TypeName.Split('_').First()))
//                                                     .ToList();
//
//            return indexToTypeAccordances;
//        }


        //TODO duplicated from InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.ElasticSearchNodePool. BEGIN
        private readonly Dictionary<Uri, NodeInfo> _nodeList;
        
        private static IEnumerable<Uri> GetActualNodes(ElasticSearchSettings settings)
        {
            var nodeUrls = settings.Nodes.Select(url => new Uri(url.Trim()));

            var nodeList = new Dictionary<Uri, NodeInfo>();

            if (nodeUrls != null)
            {
                foreach (var nodeAddress in nodeUrls)
                {
                    nodeList[nodeAddress] = new NodeInfo(nodeAddress);
                }
            }

            var result = nodeList.Values
                              .OrderByDescending(i => i.IsWork)
                              .Select(i => i.Address)
                              .ToArray();

            return result;
        }

        private class NodeInfo
        {
            public NodeInfo(Uri address)
            {
                Address = address;
                IsWork = true;
            }

            public readonly Uri Address;

            public bool IsWork;
        }
        //TODO duplicated from InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.ElasticSearchNodePool. END
    }
}