using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;

using Nest;

namespace InfinniPlatform.ElasticSearch.IndexTypeVersions
{
    public static class IndexTypeMapper
    {
        public const string MappingTypeVersionPattern = "_typeschema_";
        private const string SortFieldName = "sort";

        private static readonly Dictionary<FieldType, Func<PropertyMapping, IElasticType>>
            ElasticTypeFactories = new Dictionary<FieldType, Func<PropertyMapping, IElasticType>>
                                   {
                                       {
                                           FieldType.String, propertyMapping => propertyMapping.AddSortField
                                                                                           ? new MultiFieldMapping
                                                                                             {
                                                                                                 Type = propertyMapping.DataType.ToString(),
                                                                                                 Fields = new Dictionary<PropertyNameMarker, IElasticCoreType> { { SortFieldName, new StringMapping { Index = FieldIndexOption.NotAnalyzed } } }
                                                                                             }
                                                                                           : new StringMapping()
                                       },
                                       {
                                           FieldType.Binary, propertyMapping => new ObjectMapping
                                                                                       {
                                                                                           Properties = new Dictionary<PropertyNameMarker, IElasticType> { { "Info", new ObjectMapping() } }
                                                                                       }
                                       },
                                       {
                                           FieldType.Object, propertyMapping =>
                                                                    {
                                                                        // Поле является контейнером для других полей
                                                                        return new ObjectMapping
                                                                               {
                                                                                   Properties = propertyMapping.ChildProperties
                                                                                                               .ToDictionary<PropertyMapping, PropertyNameMarker, IElasticType>(property => property.Name, GetElasticType)
                                                                               };
                                                                    }
                                       },
                                       { FieldType.Integer, propertyMapping => CorrectElasticFieldMapping(new NumberMapping { Type = FieldType.Integer.ToString() }, propertyMapping) },
                                       { FieldType.Float, propertyMapping => CorrectElasticFieldMapping(new NumberMapping { Type = FieldType.Float.ToString() }, propertyMapping) },
                                       { FieldType.Date, propertyMapping => CorrectElasticFieldMapping(new DateMapping(), propertyMapping) },
                                       { FieldType.Boolean, propertyMapping => CorrectElasticFieldMapping(new BooleanMapping(), propertyMapping) }
                                   };

        /// <summary>
        /// Задание схемы для документов, хранимых в индексе
        /// </summary>
        public static void ApplyIndexTypeMapping(ElasticClient elasticClient, string indexName, string schemaversionname, IList<PropertyMapping> properties, SearchAbilityType searchAbility = SearchAbilityType.KeywordBasedSearch)
        {
            var propertiesDictionary = properties?.ToDictionary<PropertyMapping, PropertyNameMarker, IElasticType>(property => property.Name, GetElasticType) ?? new Dictionary<PropertyNameMarker, IElasticType>();

            elasticClient.Map<dynamic>(m => m.Index(indexName.ToLowerInvariant())
                                             .Type(schemaversionname)
                                             .SearchAnalyzer("string_lowercase")
                                             .IndexAnalyzer(searchAbility.ToString().ToLowerInvariant())
                                             .Properties(p => p.Object<dynamic>(od => od.Name("Values").Properties(ps => ps.AddProperties(propertiesDictionary)))));
        }

        /// <summary>
        /// Корректирует маппинг поля соответственно типу данных и необходимости сортировки.
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