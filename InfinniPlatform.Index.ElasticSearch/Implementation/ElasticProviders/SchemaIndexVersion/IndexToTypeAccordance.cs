using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;

using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{

    /// <summary>
    ///   Соответствие индексов и хранящихся в них типов
    ///   Например:
    ///   Индекс: product_schema_0
    ///   Типы:      customer_schema_0
    ///              customer_schema_1
    ///              customer_schema_2
    ///           product_schema_1
    ///              customer_schema_3
    ///              product_schema_0
    ///              order_schema_0
    ///              order_schema_1      
    /// </summary>
    public sealed class IndexToTypeAccordance
    {
	    private readonly string _indexName;

	    /// <summary>
		///   Базовое наименование типа данных (например product, order, etc)
		/// для которого существуют производные типы с собственными маппингами (product_schema_0, order_schema_1, etc)
		/// </summary>
	    private readonly string _baseTypeName;

		private readonly Dictionary<string, object> _typeNames = new Dictionary<string, object>();

	    public IndexToTypeAccordance(string indexName, string baseTypeName)
	    {
		    _indexName = indexName;
		    _baseTypeName = baseTypeName;
	    }

	    public string IndexName
	    {
		    get { return _indexName; }
	    }

	    /// <summary>
	    ///   Наименования схем типов, отсортированные в порядке убывания (product_schema_1, product_schema_0, etc)
	    /// </summary>
	    public IEnumerable<string> TypeNames
	    {
		    get { return _typeNames.Select(t => t.Key).ToList(); }
	    }

	    /// <summary>
	    ///   Базовое наименование типа данных (например product, order, etc)
	    /// для которого существуют производные типы с собственными маппингами (product_schema_0, order_schema_1, etc)
	    /// </summary>
	    public string BaseTypeName
	    {
		    get { return _baseTypeName; }
	    }

	    /// <summary>
	    ///   Добавить версию схемы типа
	    /// (например product_schema_1)
	    /// </summary>
	    /// <param name="schemaTypeVersion">Наименование версии схемы типа</param>
	    /// <param name="mapping">Схема маппинга версии типа</param>
	    public void RegisterSchemaType(string schemaTypeVersion, object mapping)
	    {
		    _typeNames.Add(schemaTypeVersion,mapping);
	    }


    }

    public static class IndexToTypeAccordanceExtensions
    {
        public static IEnumerable<string> GetDerivedTypeNames(this IEnumerable<string> derivedTypesNames,
                                                              string baseTypeName)
        {
            baseTypeName = baseTypeName.ToLowerInvariant();
            var result = new List<string>();
            foreach (var derivedTypeName in derivedTypesNames)
            {
                var posSchema = derivedTypeName.IndexOf(IndexTypeMapper.MappingTypeVersionPattern, System.StringComparison.Ordinal);
                if (posSchema > -1)
                {
                    var indexBase = derivedTypeName.Substring(0, posSchema);
                    if (indexBase == baseTypeName)
                    {
                        result.Add(derivedTypeName);
                    }
                }
            }
            return result;
        } 

        /// <summary>
        ///   Получить базовые имена типов в индексе из имен версий
        /// </summary>
        /// <param name="derivedTypesNames">Имена версий типов в индексе</param>
        /// <returns>Список наименований базовых типов в индексе</returns>
        public static IEnumerable<string> GetBaseTypeNames(this IEnumerable<string> derivedTypesNames)
        {
            var result = new List<string>();
            foreach (var derivedTypeName in derivedTypesNames)
            {
                var posSchema = derivedTypeName.IndexOf(IndexTypeMapper.MappingTypeVersionPattern, System.StringComparison.Ordinal);
                if (posSchema > -1)
                {
                    result.Add(derivedTypeName.Substring(0, posSchema));
                }
            }
            return result.Distinct();
        }

        /// <summary>
		///   Найти существующее в коллекции сопоставление типа и индексов
		/// </summary>
		/// <param name="accordances">Список сопоставлений</param>
		/// <param name="typeName">Наименование типа для поиска</param>
		/// <returns>Сопоставление типа и индексов</returns>
		public static IndexToTypeAccordance GetIndexToTypeAccordance(this IEnumerable<IndexToTypeAccordance> accordances,
		                                                      string typeName)
        {
            typeName = typeName.ToLowerInvariant();
			return accordances.FirstOrDefault(a => a.BaseTypeName.ToLowerInvariant() == typeName.ToLowerInvariant());
		}

        /// <summary>
        ///  Получить актуальный версию схемы типа документа
        /// </summary>
        /// <param name="accordances">Список соответствий наименований индексов и хранящихся в них версий типов данных</param>
        /// <param name="typeName">Наименование типа, актуальную версию которого требуется получить</param>
        /// <returns>Актуальная версия типа</returns>
        public static string GetActualTypeName(this IEnumerable<IndexToTypeAccordance> accordances, string typeName)
        {
            typeName = typeName.ToLowerInvariant();
            
            var indexToTypeAccordances = accordances as IndexToTypeAccordance[] ?? accordances.ToArray();

            if (accordances == null || !indexToTypeAccordances.Any())
            {
                return string.Empty;
            }
            
            var actualIndexAccordance = indexToTypeAccordances.Where(a => a.TypeNames.Any(t => t.Contains(typeName))).OrderByDescending(a => a.IndexName).FirstOrDefault();
            
            if (actualIndexAccordance != null)
            {
                return GetActualType(actualIndexAccordance.TypeNames);
            }
            
            throw new ArgumentException($"no type \"{typeName}\"exists in actual index ");
        }

        public static string GetActualTypeNameNest(this Dictionary<string, IList<TypeMapping>> accordances, string typeName)
        {
            var max = 0;
            string actualTypeName = null;

            var typeMappings = accordances.FirstOrDefault().Value;

            foreach (var typeMapping in typeMappings)
            {
                var version = typeMapping.GetTypeVersion().GetValueOrDefault();

                if (version >= max)
                {
                    actualTypeName = typeMapping.TypeName;
                    max = version;
                }
            }

            return actualTypeName;
        }

        private static string GetActualType(IEnumerable<string> types)
        {
            string resultType = null;

            var maxTypeNumber = -1;

            foreach (var indexType in types)
            {
                var indexTypeNumber = int.Parse(indexType.Split('_').Last());

                if (indexTypeNumber > maxTypeNumber)
                {
                    resultType = indexType;
                    maxTypeNumber = indexTypeNumber;
                }
            }

            return resultType;
        }

    } 
}
