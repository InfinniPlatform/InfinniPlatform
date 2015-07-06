using InfinniPlatform.Api.Index;
using InfinniPlatform.Sdk.Environment.Index;
using Nest;

using System;
using System.Collections.Generic;
using System.Linq;
using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions
{
    public static class IndexTypeMapper
    {
        public const string MappingTypeVersionPattern = "_typeschema_";

        /// <summary>
        /// Задание схемы для документов, хранимых в индексе
        /// </summary>
        public static void ApplyIndexTypeMapping(
            ElasticClient elasticClient,
            string indexName,
            string schemaversionname,
            IList<PropertyMapping> properties,
            SearchAbilityType searchAbility = SearchAbilityType.KeywordBasedSearch)
        {
            indexName = indexName.ToLowerInvariant();

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

            var props = properties ?? new List<PropertyMapping>();

            var propertiesDictionary =
                props.ToDictionary<PropertyMapping, PropertyNameMarker, IElasticType>(property => property.Name,
                    GetElasticTypeByMapping);

            var mappingResult = elasticClient.Map<dynamic>(
                m => m
                    .Index(indexName)
                    .Type(schemaversionname)
                    .SearchAnalyzer("string_lowercase")
                    .IndexAnalyzer(searchAbility.ToString().ToLowerInvariant())
					.RoutingField(r => r.Required())
                    .Properties( p => p.Object<dynamic>(
                        od => od.Name("Values").Properties(ps => ps.AddProperties(propertiesDictionary)))));
        }

        /// <summary>
        /// Рекурсивный метод получения значения свойства по схеме
        /// </summary>
        private static IElasticType GetElasticTypeByMapping(PropertyMapping mapping)
        {
            IElasticType resultType;

            switch (mapping.DataType)
            {
                case PropertyDataType.String:
                    if (mapping.AddSortField)
                    {
                        var multifield = new MultiFieldMapping {Type = "string"};
                        multifield.Fields.Add("sort", new StringMapping {Index = FieldIndexOption.NotAnalyzed});
                        resultType = multifield;
                    }
                    else
                    {
                        resultType = new StringMapping();
                    }
                    break;
                case PropertyDataType.Integer:
                    if (mapping.AddSortField)
                    {
                        var multifield = new MultiFieldMapping { Type = "integer" };
                        multifield.Fields.Add("sort", new NumberMapping{Type = "integer"});
                        resultType = multifield;
                    }
                    else
                    {
                        resultType = new NumberMapping {Type = "integer"};
                    }
                    break;
                case PropertyDataType.Float:
                    if (mapping.AddSortField)
                    {
                        var multifield = new MultiFieldMapping { Type = "float" };
                        multifield.Fields.Add("sort", new NumberMapping{Type = "float"});
                        resultType = multifield;
                    }
                    else
                    {
                        resultType = new NumberMapping {Type = "float"};
                    }
                    break;
                case PropertyDataType.Date:
                    if (mapping.AddSortField)
                    {
                        var multifield = new MultiFieldMapping { Type = "date" };
                        multifield.Fields.Add("sort", new DateMapping());
                        resultType = multifield;
                    }
                    else
                    {
                        resultType = new DateMapping();
                    }
                    break;
                case PropertyDataType.Boolean:
                    if (mapping.AddSortField)
                    {
                        var multifield = new MultiFieldMapping { Type = "boolean" };
                        multifield.Fields.Add("sort", new BooleanMapping());
                        resultType = multifield;
                    }
                    else
                    {
                        resultType = new BooleanMapping();
                    }
                    break;
				case PropertyDataType.Binary:
					resultType = new ObjectMapping
					{
						Properties = new Dictionary<PropertyNameMarker, IElasticType>()
					};
					(resultType as ObjectMapping).Properties.Add("Info",new ObjectMapping());
					break;
                case PropertyDataType.Object:
				                    
                    // Поле является контейнером для других полей
                    resultType = new ObjectMapping
                    {
                        Properties = new Dictionary<PropertyNameMarker, IElasticType>()
                    };

                    foreach (var property in mapping.ChildProperties)
                    {
                        (resultType as ObjectMapping).Properties.Add(property.Name, GetElasticTypeByMapping(property));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            resultType.Name = mapping.Name;
            return resultType;
        }
    }
}
