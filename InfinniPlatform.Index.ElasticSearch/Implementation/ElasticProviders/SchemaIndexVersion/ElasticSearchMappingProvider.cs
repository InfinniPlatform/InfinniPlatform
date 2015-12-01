using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    /// <summary>
    /// Есть подозрение, что все эти методы есть в текущей версии NEST или ElasticSearch.Net.
    /// И, очевидно стоит использовать их, так как вручную делать опрос узлов совершенно
    /// неудачное решение. Видимо, ручная работа с REST была сделана от безысходности?!
    /// </summary>
    internal sealed class ElasticSearchMappingProvider
    {
        public ElasticSearchMappingProvider(ElasticSearchSettings settings)
        {
            var nodeUrls = settings.Nodes.Select(url => new Uri(url.Trim()));

            _nodePool = new ElasticSearchNodePool(nodeUrls);

            _httpClient = new HttpClient();

            if (!string.IsNullOrEmpty(settings.Login) && !string.IsNullOrEmpty(settings.Password))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Login}:{settings.Password}")));
            }
        }


        private readonly ElasticSearchNodePool _nodePool;
        private readonly HttpClient _httpClient;


        public IEnumerable<IndexToTypeAccordance> FillIndexMappings(IEnumerable<string> indexNames, IEnumerable<string> typeNames)
        {
            List<IndexToTypeAccordance> result = null;

            var nodeAddresses = _nodePool.GetActualNodes();

            foreach (var nodeAddress in nodeAddresses)
            {
                if (TryFillIndexMappings(nodeAddress, indexNames, typeNames, out result))
                {
                    _nodePool.NodeIsWork(nodeAddress);
                    break;
                }

                _nodePool.NodeIsNotWork(nodeAddress);
            }

            return result ?? new List<IndexToTypeAccordance>();
        }

        private bool TryFillIndexMappings(Uri nodeAddress, IEnumerable<string> indexNames, IEnumerable<string> typeNames, out List<IndexToTypeAccordance> result)
        {
            result = null;

            var indexNameList = string.Join(",", indexNames).ToLowerInvariant();
            var typeNameList = string.Join(",", typeNames.Select(t => t + "*")).ToLowerInvariant();

            var elasticQueryMappings = new Uri(nodeAddress, string.Format("{0}/{1}/_mapping", indexNameList, typeNameList));

            // Запрос возвращает схему маппинга для всех индексов и типов индексов в следующем виде:
            //
            // testperson_schema_0": {  -- наименование индекса
            //   "indexobjects": { ... }, -- наименование типа
            //   "testperson_schema_1": { ... } -- наименование типа
            // }

            // получаем список маппингов
            var response = _httpClient.GetAsync(elasticQueryMappings).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK || responseContent == "{}")
            {
                return false;
            }

            dynamic resultMappings = responseContent.ToDynamic();

            // проверяем, что получили маппинги индексов и типов
            if (resultMappings.error != null)
            {
                return false;
            }

            result = new List<IndexToTypeAccordance>();

            //проходим по каждому индексу, заполняем типы
            foreach (var index in resultMappings)
            {
                // "testperson_indexschema_0": {
                //   "testperson_typeschema_0": { ... },
                //   "testperson_typeschema_1": { ... }
                // }

                // derivedtypenames:
                //   "testperson_typeschema_0": { ... },
                //   "testperson_typeschema_1": { ... }

                var derivedTypes = index.Value.mappings;

                var derivedTypeNames = new Dictionary<string, object>();

                foreach (var derivedTypeName in derivedTypes)
                {
                    derivedTypeNames.Add(derivedTypeName.Key, derivedTypeName.Value);
                }

                var baseTypeNames = derivedTypeNames.Select(d => d.Key).GetBaseTypeNames();
                foreach (var baseTypeName in baseTypeNames)
                {
                    var accordance = new IndexToTypeAccordance(index.Key, baseTypeName);
                    result.Add(accordance);
                    var schemaTypes = derivedTypeNames.Select(d => d.Key).GetDerivedTypeNames(baseTypeName);
                    foreach (var schemaType in schemaTypes)
                    {
                        accordance.RegisterSchemaType(schemaType, derivedTypeNames[schemaType]);
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Получение маппинга конкретного типа в заданном индексе.
        /// </summary>
        public IList<PropertyMapping> GetIndexTypeMapping(string indexName, string typeName)
        {
            List<PropertyMapping> result = null;

            var nodeAddresses = _nodePool.GetActualNodes();

            foreach (var nodeAddress in nodeAddresses)
            {
                if (TryGetIndexTypeMapping(nodeAddress, indexName, typeName, out result))
                {
                    _nodePool.NodeIsWork(nodeAddress);
                    break;
                }

                _nodePool.NodeIsNotWork(nodeAddress);
            }

            return result ?? new List<PropertyMapping>();
        }

        private bool TryGetIndexTypeMapping(Uri nodeAddress, string indexName, string typeName, out List<PropertyMapping> result)
        {
            result = null;

            indexName = indexName.ToLowerInvariant();
            typeName = typeName.ToLowerInvariant();

            var elasticQueryMappings = new Uri(nodeAddress, string.Format("{0}/{1}/_mapping", indexName, typeName));

            // Запрос возвращает схему маппинга для всех индексов и типов индексов в следующем виде:
            //
            // testperson_schema_0": {  -- наименование индекса
            //   "indexobjects": { ... }, -- наименование типа
            //   "testperson_schema_1": { ... } -- наименование типа
            // }

            // получаем список маппингов
            var response = _httpClient.GetAsync(elasticQueryMappings).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK || responseContent == "{}")
            {
                return false;
            }

            dynamic resultMappings = responseContent.ToDynamic();

            // проверяем, что получили маппинг типа
            if (resultMappings.error != null)
            {
                return false;
            }

            var mappingProperties = resultMappings[indexName].mappings[typeName].properties;

            result = new List<PropertyMapping>();

            if (mappingProperties != null &&
                mappingProperties.Values != null &&
                mappingProperties.Values.properties != null)
            {
                foreach (var mappingProperty in mappingProperties.Values.properties)
                {
                    result.Add(ExtractProperty(mappingProperty));
                }
            }

            return true;
        }

        private static PropertyMapping ExtractProperty(dynamic property)
        {
            if (property.Value.properties != null)
            {
                var propertiesList = new List<PropertyMapping>();

                foreach (var prop in property.Value.properties)
                {
                    propertiesList.Add(ExtractProperty(prop));
                }

                return new PropertyMapping(property.Key, propertiesList);
            }

            PropertyDataType dataType;
            string type = property.Value.type;

            switch (type)
            {
                case "string":
                    dataType = PropertyDataType.String;
                    break;
                case "date":
                    dataType = PropertyDataType.Date;
                    break;
                case "binary":
                    dataType = PropertyDataType.Object; //в эластике храним только ссылку на бинарные данные, сами бинарные данные хранятся в Cassandra
                    break;
                case "boolean":
                    dataType = PropertyDataType.Boolean;
                    break;
                case "double":
                case "float":
                    dataType = PropertyDataType.Float;
                    break;
                case "integer":
                case "long":
                    dataType = PropertyDataType.Integer;
                    break;
                case "object":
                    dataType = PropertyDataType.Object;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var hasSortingFiled = property.Value.fields != null && property.Value.fields.sort != null;

            return new PropertyMapping(property.Key, dataType, hasSortingFiled);
        }


        public IEnumerable<Uri> GetActualNodes()
        {
            return _nodePool.GetActualNodes();
        }
    }
}