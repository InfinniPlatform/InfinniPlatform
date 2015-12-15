using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Environment.Index;

using Nest;

using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    /// <summary>
    /// Есть подозрение, что все эти методы есть в текущей версии NEST или ElasticSearch.Net.
    /// И, очевидно стоит использовать их, так как вручную делать опрос узлов совершенно
    /// неудачное решение. Видимо, ручная работа с REST была сделана от безысходности?!
    /// </summary>
    internal sealed class ElasticSearchMappingProvider
    {
        public ElasticSearchMappingProvider(Lazy<ElasticClient> elasticClient)
        {
            _elasticClient = elasticClient.Value;
        }

        private readonly ElasticClient _elasticClient;
        
        public Dictionary<string, IList<TypeMapping>> FillIndexMappingsNest(string indexName, IEnumerable<string> typeNames)
        {
            indexName = indexName.ToLower();

            var baseTypeNames = typeNames.Select(x => x.GetTypeBaseName()).ToArray();

            var typeNameFilter = string.Join(",", baseTypeNames.Select(t => t.ToLower() + "*")).ToLowerInvariant();

            var nestResult = _elasticClient.GetMapping(new GetMappingRequest(new IndexNameMarker(), new TypeNameMarker()) { Index = indexName, Type = typeNameFilter });

            if (nestResult != null && nestResult.Mappings.ContainsKey(indexName))
            {
                baseTypeNames = baseTypeNames.Select(t => t.ToLowerInvariant()).ToArray();

                var mappingsInIndex = nestResult.Mappings[indexName];

                var fullTypeNames = mappingsInIndex.Where(x => baseTypeNames.Contains(x.GetTypeBaseName()))
                                                   .Select(x => x.TypeName);

                var filteredMappings = mappingsInIndex.Where(x => fullTypeNames.Contains(x.TypeName)).ToList();

                nestResult.Mappings[indexName] = filteredMappings;

                return nestResult.Mappings;
            }

            return nestResult?.Mappings;
        }

        public List<PropertyMapping> GetIndexTypeMappingNest(string indexName, string typeName)
        {
            //TODO Рассмотреть возможность использования типов данных из NEST.
            var resultNest = new List<PropertyMapping>();

            var fillIndexMappingsNest = FillIndexMappingsNest(indexName, new[] { typeName })?[indexName.ToLower()];
            if (fillIndexMappingsNest != null)
            {
                var mappingNest = (fillIndexMappingsNest.OrderByDescending(mapping => mapping.TypeName.GetTypeVersion()).First()?.Mapping.Properties["Values"] as ObjectMapping)?.Properties;

                if (mappingNest != null)
                {
                    resultNest.AddRange(mappingNest.Select(ExtractProperty));
                }
            }

            return resultNest;
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

                if (objectMapping != null && objectMapping.Properties != null)
                {
                    var propertiesList = new List<PropertyMapping>();

                    foreach (var prop in objectMapping.Properties)
                    {
                        propertiesList.Add(ExtractProperty(prop));
                    }

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
    }
}