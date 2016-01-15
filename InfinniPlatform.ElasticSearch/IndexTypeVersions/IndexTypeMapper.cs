using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;

using Nest;

using PropertyMapping = InfinniPlatform.Core.Index.PropertyMapping;

namespace InfinniPlatform.ElasticSearch.IndexTypeVersions
{
    public static class IndexTypeMapper
    {
        public const string MappingTypeVersionPattern = "_typeschema_";
        private const string SortFieldName = "sort";

        private static readonly Dictionary<PropertyDataType, Func<PropertyMapping, IElasticType>>
            ElasticTypeFactories = new Dictionary<PropertyDataType, Func<PropertyMapping, IElasticType>>
                                   {
                                       {
                                           PropertyDataType.String, GetStringElasticFieldMapping
                                       },
                                       {
                                           PropertyDataType.Binary, mapping => new ObjectMapping
                                                                               {
                                                                                   Properties = new Dictionary<PropertyNameMarker, IElasticType> { { "Info", new ObjectMapping() } }
                                                                               }
                                       },
                                       {
                                           PropertyDataType.Object, mapping =>
                                                                    {
                                                                        // Поле является контейнером для других полей
                                                                        return new ObjectMapping
                                                                               {
                                                                                   Properties = mapping.ChildProperties
                                                                                                       .ToDictionary<PropertyMapping, PropertyNameMarker, IElasticType>(property => property.Name, GetElasticType)
                                                                               };
                                                                    }
                                       },
                                       { PropertyDataType.Integer, propertyMapping => GetElasticFieldMapping(propertyMapping, new NumberMapping { Type = PropertyDataType.Integer.ToString() }) },
                                       { PropertyDataType.Float, propertyMapping => GetElasticFieldMapping(propertyMapping, new NumberMapping { Type = PropertyDataType.Float.ToString() }) },
                                       { PropertyDataType.Date, propertyMapping => GetElasticFieldMapping(propertyMapping, new DateMapping()) },
                                       { PropertyDataType.Boolean, propertyMapping => GetElasticFieldMapping(propertyMapping, new BooleanMapping()) }
                                   };

        private static MultiFieldMapping GetStringElasticFieldMapping(PropertyMapping mapping)
        {
            return mapping.AddSortField
                       ? new MultiFieldMapping
                         {
                             Type = mapping.DataType.ToString(),
                             Fields = new Dictionary<PropertyNameMarker, IElasticCoreType> { { SortFieldName, new StringMapping { Index = FieldIndexOption.NotAnalyzed } } }
                         }
                       : new StringMapping();
        }

        /// <summary>
        /// Задание схемы для документов, хранимых в индексе
        /// </summary>
        public static void ApplyIndexTypeMapping(ElasticClient elasticClient, string indexName, string schemaversionname, IList<PropertyMapping> properties, SearchAbilityType searchAbility = SearchAbilityType.KeywordBasedSearch)
        {
            indexName = indexName.ToLowerInvariant();

            //TODO: Это можно делать и непосредственно при создании индекса
            // Для корректной работы маппера в настройках ElasticSearch должны быть
            // определены необходимые фильтры и анализаторы:
            /*             
            index:
                analysis:
                    analyzer:
            #стандартный анализатор, использующийся для поиска по умолчанию
                        string_lowercase:
                            filter: lowercase
                            tokenizer: keyword
            #стандартный анализатор, использующийся для индексирования по умолчанию
                        keywordbasedsearch: 
                            filter: lowercase
                            tokenizer: keyword
            #полнотекстовый анализатор, использующийся для индексирования
                        fulltextsearch:  
                            filter: lowercase
                            tokenizer: standard
            #Анализатор с разбиением на слова, использующийся для полнотекстового поиска
                        fulltextquery: 
                            filter: lowercase
                            tokenizer: whitespace
             */

            var propertiesDictionary = properties?.ToDictionary<PropertyMapping, PropertyNameMarker, IElasticType>(property => property.Name, GetElasticType) ?? new Dictionary<PropertyNameMarker, IElasticType>();

            elasticClient.Map<dynamic>(m => m.Index(indexName)
                                             .Type(schemaversionname)
                                             .SearchAnalyzer("string_lowercase")
                                             .IndexAnalyzer(searchAbility.ToString().ToLowerInvariant())
                                             .Properties(p => p.Object<dynamic>(od => od.Name("Values").Properties(ps => ps.AddProperties(propertiesDictionary)))));
        }

        /// <summary>
        /// Рекурсивный метод получения значения свойства по схеме
        /// </summary>
        private static IElasticType GetElasticType(PropertyMapping propertyMapping)
        {
            Func<PropertyMapping, IElasticType> elasticTypeFactory;
            if (!ElasticTypeFactories.TryGetValue(propertyMapping.DataType, out elasticTypeFactory))
            {
                throw new ArgumentOutOfRangeException();
            }

            var elasticType = elasticTypeFactory.Invoke(propertyMapping);
            elasticType.Name = propertyMapping.Name;

            return elasticType;
        }

        private static IElasticType GetElasticFieldMapping(PropertyMapping propertyMapping, IElasticCoreType elasticFieldMapping)
        {
            return propertyMapping.AddSortField
                       ? (IElasticType)new MultiFieldMapping
                                       {
                                           Type = propertyMapping.DataType.ToString(),
                                           Fields = new Dictionary<PropertyNameMarker, IElasticCoreType> { { SortFieldName, elasticFieldMapping } }
                                       }
                       : elasticFieldMapping;
        }
    }
}