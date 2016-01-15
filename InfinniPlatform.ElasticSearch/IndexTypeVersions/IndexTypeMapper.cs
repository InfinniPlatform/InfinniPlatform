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
                                           PropertyDataType.String, propertyMapping => propertyMapping.AddSortField
                                                                                           ? new MultiFieldMapping
                                                                                             {
                                                                                                 Type = propertyMapping.DataType.ToString(),
                                                                                                 Fields = new Dictionary<PropertyNameMarker, IElasticCoreType> { { SortFieldName, new StringMapping { Index = FieldIndexOption.NotAnalyzed } } }
                                                                                             }
                                                                                           : new StringMapping()
                                       },
                                       {
                                           PropertyDataType.Binary, propertyMapping => new ObjectMapping
                                                                                       {
                                                                                           Properties = new Dictionary<PropertyNameMarker, IElasticType> { { "Info", new ObjectMapping() } }
                                                                                       }
                                       },
                                       {
                                           PropertyDataType.Object, propertyMapping =>
                                                                    {
                                                                        // Поле является контейнером для других полей
                                                                        return new ObjectMapping
                                                                               {
                                                                                   Properties = propertyMapping.ChildProperties
                                                                                                               .ToDictionary<PropertyMapping, PropertyNameMarker, IElasticType>(property => property.Name, GetElasticType)
                                                                               };
                                                                    }
                                       },
                                       { PropertyDataType.Integer, propertyMapping => CorrectElasticFieldMapping(new NumberMapping { Type = PropertyDataType.Integer.ToString() }, propertyMapping) },
                                       { PropertyDataType.Float, propertyMapping => CorrectElasticFieldMapping(new NumberMapping { Type = PropertyDataType.Float.ToString() }, propertyMapping) },
                                       { PropertyDataType.Date, propertyMapping => CorrectElasticFieldMapping(new DateMapping(), propertyMapping) },
                                       { PropertyDataType.Boolean, propertyMapping => CorrectElasticFieldMapping(new BooleanMapping(), propertyMapping) }
                                   };

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
        /// Коректирует маппинг поля соответственно типу данных и необходимости сортировки.
        /// </summary>
        /// <param name="propertyMapping">Обобщенный маппинг поля документа</param>
        /// <param name="elasticFieldMapping">Маппинг поля в elasticsearch</param>
        private static IElasticType CorrectElasticFieldMapping(IElasticCoreType elasticFieldMapping, PropertyMapping propertyMapping)
        {
            return propertyMapping.AddSortField
                       ? (IElasticType)new MultiFieldMapping
                                       {
                                           Type = propertyMapping.DataType.ToString(),
                                           Fields = new Dictionary<PropertyNameMarker, IElasticCoreType> { { SortFieldName, elasticFieldMapping } }
                                       }
                       : elasticFieldMapping;
        }

        /// <summary>
        /// Получает маппинг поля соответственно типу данных поля.
        /// </summary>
        /// <param name="propertyMapping">Обобщенный маппинг поля документа</param>
        /// <returns>Маппинг поля в elasticsearch</returns>
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
    }
}