using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

using RestSharp;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    /// <summary>
    /// Есть подозрение, что все эти методы есть в текущей версии NEST или ElasticSearch.Net.
    /// И, очевидно стоит использовать их, так как вручную делать опрос узлов совершенно
    /// неудачное решение. Видимо, ручная работа с REST была сделана от безысходности?!
    /// </summary>
    internal static class ElasticMappingExtension
    {
        public static IEnumerable<IndexToTypeAccordance> FillIndexMappings(NodePool nodePool, IEnumerable<string> indexNames, IEnumerable<string> typeNames)
        {
            List<IndexToTypeAccordance> result = null;

            var nodeAddresses = nodePool.GetActualNodes();

            foreach (var nodeAddress in nodeAddresses)
            {
                if (TryFillIndexMappings(nodeAddress, indexNames, typeNames, out result))
                {
                    nodePool.NodeIsWork(nodeAddress);
                    break;
                }

                nodePool.NodeIsNotWork(nodeAddress);
            }

            return result ?? new List<IndexToTypeAccordance>();
        }

        private static bool TryFillIndexMappings(Uri nodeAddress, IEnumerable<string> indexNames, IEnumerable<string> typeNames, out List<IndexToTypeAccordance> result)
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
            var restClient = new RestClient(elasticQueryMappings);
            var response = restClient.Get(new RestRequest { RequestFormat = DataFormat.Json });
            var responseContent = response.Content;

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
        public static IList<PropertyMapping> GetIndexTypeMapping(NodePool nodePool, string indexName, string typeName)
        {
            List<PropertyMapping> result = null;

            var nodeAddresses = nodePool.GetActualNodes();

            foreach (var nodeAddress in nodeAddresses)
            {
                if (TryGetIndexTypeMapping(nodeAddress, indexName, typeName, out result))
                {
                    nodePool.NodeIsWork(nodeAddress);
                    break;
                }

                nodePool.NodeIsNotWork(nodeAddress);
            }

            return result ?? new List<PropertyMapping>();
        }

        public static bool TryGetIndexTypeMapping(Uri nodeAddress, string indexName, string typeName, out List<PropertyMapping> result)
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
            var restClient = new RestClient(elasticQueryMappings);
            var response = restClient.Get(new RestRequest { RequestFormat = DataFormat.Json });
            var responseContent = response.Content;

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


        /// <summary>
        /// Рекурсивный метод для извлечения одного свойства.
        /// </summary>
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
    }
}