using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.ElasticSearch.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.ElasticSearch.IndexTypeVersions;
using InfinniPlatform.Sdk.Environment.Index;

using Nest;

using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    /// <summary>
    /// Предоставляет методы для управления типами индексов.
    /// </summary>
    public class ElasticTypeManager
    {
        public ElasticTypeManager(ElasticConnection elasticConnection)
        {
            _elasticConnection = elasticConnection;
        }


        private readonly ElasticConnection _elasticConnection;


        // INDEXES


        public bool IndexExists(string indexName)
        {
            indexName = indexName.ToLower();

            return _elasticConnection.Client.IndexExists(i => i.Index(indexName)).Exists;
        }

        public void CreateIndex(string indexName)
        {
            indexName = indexName.ToLower();

            _elasticConnection.Client.CreateIndex(i => i.Index(indexName));

            ResetIndexMappings();
        }

        public void DeleteIndex(string indexName)
        {
            indexName = indexName.ToLower();

            _elasticConnection.Client.DeleteIndex(i => i.Index(indexName));

            ResetIndexMappings();
        }


        // TYPES


        public bool TypeExists(string indexName, string typeName)
        {
            indexName = indexName.ToLower();
            typeName = typeName.ToLower();

            var typeMappings = GetTypeMappings(indexName, typeName);

            return typeMappings.Any();
        }

        public void CreateType(string indexName, string typeName, IList<PropertyMapping> properties = null, bool deleteExistingVersion = false, SearchAbilityType searchAbility = SearchAbilityType.KeywordBasedSearch)
        {
            indexName = indexName.ToLower();
            typeName = typeName.ToLower();

            var typeMappings = GetTypeMappings(indexName, typeName);

            var schemaTypeVersionNumber = 0;

            // Если существует указанный тип
            if (typeMappings.Any() && typeMappings.Any())
            {
                // TODO: вычисляем номер следующей версии маппинга
                schemaTypeVersionNumber = typeMappings.GetTypeActualVersion(typeName) + 1;
            }

            if (typeMappings.Any() && deleteExistingVersion)
            {
                foreach (var typeMapping in typeMappings)
                {
                    _elasticConnection.Client.DeleteMapping<dynamic>(d => d.Index(indexName).Type(typeMapping.TypeName));
                }
            }

            var schemaTypeVersion = (typeName + IndexTypeMapper.MappingTypeVersionPattern + schemaTypeVersionNumber).ToLowerInvariant();

            if (!IndexExists(indexName))
            {
                CreateIndex(indexName);
            }

            _elasticConnection.Refresh();

            _elasticConnection.Client.Map<dynamic>(s => s.Index(indexName)
                                                         .Type(schemaTypeVersion)
                                                         .SearchAnalyzer("string_lowercase")
                                                         .IndexAnalyzer(searchAbility.ToString().ToLowerInvariant()));

            var mapping = _elasticConnection.Client.GetMapping<dynamic>(d => d.Index(indexName).Type(schemaTypeVersion));

            if (mapping == null)
            {
                throw new ArgumentException($"Fail to create type name mapping: \"{typeName}\"");
            }

            if (properties != null)
            {
                IndexTypeMapper.ApplyIndexTypeMapping(_elasticConnection.Client, indexName, schemaTypeVersion, properties);
            }

            ResetIndexMappings();
        }

        public void DeleteType(string indexName, string typeName)
        {
            indexName = indexName.ToLower();
            typeName = typeName.ToLower();

            var typeMappings = GetTypeMappings(indexName, typeName);

            foreach (var mapping in typeMappings)
            {
                _elasticConnection.Client.DeleteMapping<dynamic>(d => d.Index(indexName).Type(mapping.TypeName));
            }

            _elasticConnection.Refresh();

            ResetIndexMappings();
        }


        // MAPPINGS

        public IEnumerable<TypeMapping> GetTypeMappings(string indexName, string typeName)
        {
            indexName = indexName.ToLower();
            typeName = typeName.ToLower();

            var indexMappings = GetIndexMappings(indexName);

            if (indexMappings != null)
            {
                var isBaseTypes = !typeName.Contains(IndexTypeMapper.MappingTypeVersionPattern);

                return isBaseTypes
                    ? indexMappings.Where(mapping => typeName == mapping.TypeName.GetTypeBaseName())
                    : indexMappings.Where(mapping => typeName == mapping.TypeName);
            }

            return Enumerable.Empty<TypeMapping>();
        }

        public IList<PropertyMapping> GetPropertyMappings(string indexName, string typeName)
        {
            indexName = indexName.ToLower();
            typeName = typeName.ToLower();

            // TODO: Рассмотреть возможность использования типов данных из NEST.
            var propertyMappings = new List<PropertyMapping>();

            var indexMappings = GetIndexMappings(indexName);

            if (indexMappings == null)
            {
                return propertyMappings;
            }

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
                dataType = PropertyDataType.Object;
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


        public string GetActualTypeName(string configuration, string documentType)
        {
            // TODO: Данный метод должен возвращать актуальное имя типа для версии приложения.
            // Вместо этого он возвращает имя последнего созданного типа, что в свою очередь
            // нарушает работу более ранних версий приложения.

            string actualTypeName = null;

            var indexName = _elasticConnection.GetIndexName(configuration);
            var baseTypeName = _elasticConnection.GetBaseTypeName(documentType);

            var indexMappings = GetIndexMappings(indexName);

            if (indexMappings != null)
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


        private readonly object _mappingsCacheSync = new object();
        private volatile Dictionary<string, IList<TypeMapping>> _mappingsCache;

        private IList<TypeMapping> GetIndexMappings(string indexName)
        {
            IList<TypeMapping> result = null;

            var mappings = _mappingsCache;

            if (mappings == null)
            {
                lock (_mappingsCacheSync)
                {
                    mappings = _mappingsCache;

                    if (mappings == null)
                    {
                        mappings = _elasticConnection.Client.GetMapping(new GetMappingRequest("_all", "_all")).Mappings;

                        _mappingsCache = mappings;
                    }
                }
            }

            mappings?.TryGetValue(indexName, out result);

            return result;
        }

        private void ResetIndexMappings()
        {
            if (_mappingsCache != null)
            {
                lock (_mappingsCacheSync)
                {
                    _mappingsCache = null;
                }
            }
        }
    }
}