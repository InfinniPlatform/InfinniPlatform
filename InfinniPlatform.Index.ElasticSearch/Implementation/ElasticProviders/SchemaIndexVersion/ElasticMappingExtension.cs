using System;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Sdk.Environment.Index;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    public static class ElasticMappingExtension
    {
        public static IEnumerable<IndexToTypeAccordance> FillIndexMappings(
            string server,
            int port,
            IEnumerable<string> typeNames,
            IEnumerable<string> searchingIndeces)
        {
            var indexNames = string.Join(",", searchingIndeces).ToLowerInvariant();
            var types = string.Join(",", typeNames.Select(t => t + "*")).ToLowerInvariant();

            var elasticQueryMappings = string.Format("http://{0}:{1}/{2}/{3}/_mapping", server, port, indexNames, types);

            /* Запрос возвращает схему маппинга для всех индексов и типов индексов в следующем виде:
			 
			  testperson_schema_0": {  -- наименование индекса
					"indexobjects": {...}, -- наименование типа
					"testperson_schema_1": {...} -- наименование типа
					}
				},			  			  			  
			 */

            //получаем список маппингов
            var restClient = new RestClient(elasticQueryMappings);
            var response = restClient.Get(new RestRequest {RequestFormat = DataFormat.Json}).Content;
            if (string.IsNullOrEmpty(response))
            {
                return new List<IndexToTypeAccordance>();
            }
            dynamic resultMappings = response.ToDynamic();

            //проверяем, что получили маппинги индексов и типов
            if (resultMappings.error != null && resultMappings.error != null &&
                ((string) resultMappings.error).ToLowerInvariant().Contains("missing"))
            {
                return new List<IndexToTypeAccordance>();
            }

            var result = new List<IndexToTypeAccordance>();
            
            //проходим по каждому индексу, заполняем типы
            foreach (var index in resultMappings)
            {
                //"testperson_indexschema_0": {
                //	"testperson_typeschema_0": { … },
                //	"testperson_typeschema_1": { … }
                //},

                //derivedtypenames:
                //	"testperson_typeschema_0": { … },
                //	"testperson_typeschema_1": { … }

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

            return result;
        }

        /// <summary>
        /// Получение маппинга конкретного типа в заданном индексе
        /// </summary>
        public static IList<PropertyMapping> GetIndexTypeMapping(
            string server,
            int port,
            string indexName,
            string typeName)
        {
            indexName = indexName.ToLowerInvariant();
            typeName = typeName.ToLowerInvariant();

            var elasticQueryMappings = string.Format("http://{0}:{1}/{2}/{3}/_mapping", server, port, indexName,
                typeName);

            /* Запрос возвращает схему маппинга для всех индексов и типов индексов в следующем виде:
			 
              testperson_schema_0": {  -- наименование индекса
                    "indexobjects": {...}, -- наименование типа
                    "testperson_schema_1": {...} -- наименование типа
                    }
                },			  			  			  
             */

            //получаем список маппингов
            var restClient = new RestClient(elasticQueryMappings);
            var response = restClient.Get(new RestRequest {RequestFormat = DataFormat.Json}).Content;
            if (string.IsNullOrEmpty(response))
            {
                return new List<PropertyMapping>();
            }
            dynamic resultMappings = response.ToDynamic();

            //проверяем, что получили маппинг типа
            if (resultMappings.error != null && resultMappings.error != null &&
                ((string) resultMappings.error).ToLowerInvariant().Contains("missing"))
            {
                return new List<PropertyMapping>();
            }

            var mappingProperties = resultMappings[indexName].mappings[typeName].properties;

            var result = new List<PropertyMapping>();

            if (mappingProperties != null &&
                mappingProperties.Values != null && 
                mappingProperties.Values.properties != null)
            {
                foreach (var mappingProperty in mappingProperties.Values.properties)
                {
                    result.Add(ExtractProperty(mappingProperty));
                }
            }

            return result;
        }

        /// <summary>
        /// Рекурсивный метод для извлечения одного свойства
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
                    dataType = PropertyDataType.Object;//в эластике храним только ссылку на бинарные данные, сами бинарные данные хранятся в Cassandra
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

            var hasSortingFiled = property.Value.fields != null &&
                                  property.Value.fields.sort != null;

            return new PropertyMapping(property.Key, dataType, hasSortingFiled);
        }
    }
}