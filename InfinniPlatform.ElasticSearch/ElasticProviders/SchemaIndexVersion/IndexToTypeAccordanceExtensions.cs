using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.ElasticSearch.IndexTypeVersions;

namespace InfinniPlatform.ElasticSearch.ElasticProviders.SchemaIndexVersion
{
    public static class IndexToTypeAccordanceExtensions
    {
        public static IEnumerable<string> GetDerivedTypeNames(this IEnumerable<string> derivedTypesNames,
                                                              string baseTypeName)
        {
            baseTypeName = baseTypeName.ToLowerInvariant();
            var result = new List<string>();
            foreach (var derivedTypeName in derivedTypesNames)
            {
                var posSchema = derivedTypeName.IndexOf(IndexTypeMapper.MappingTypeVersionPattern, StringComparison.Ordinal);
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
        /// Получить базовые имена типов в индексе из имен версий
        /// </summary>
        /// <param name="derivedTypesNames">Имена версий типов в индексе</param>
        /// <returns>Список наименований базовых типов в индексе</returns>
        public static IEnumerable<string> GetBaseTypeNames(this IEnumerable<string> derivedTypesNames)
        {
            var result = new List<string>();
            foreach (var derivedTypeName in derivedTypesNames)
            {
                var posSchema = derivedTypeName.IndexOf(IndexTypeMapper.MappingTypeVersionPattern, StringComparison.Ordinal);
                if (posSchema > -1)
                {
                    result.Add(derivedTypeName.Substring(0, posSchema));
                }
            }
            return result.Distinct();
        }

        /// <summary>
        /// Найти существующее в коллекции сопоставление типа и индексов
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
        /// Получить актуальный версию схемы типа документа
        /// </summary>
        /// <param name="accordances">Список соответствий наименований индексов и хранящихся в них версий типов данных</param>
        /// <param name="typeName">Наименование типа, актуальную версию которого требуется получить</param>
        /// <returns>Актуальная версия типа</returns>
        public static string GetActualTypeName(this IEnumerable<IndexToTypeAccordance> accordances, string typeName)
        {
            typeName = typeName.ToLowerInvariant();

            var indexToTypeAccordances = accordances.ToArray();

            if (accordances == null || !indexToTypeAccordances.Any())
            {
                return string.Empty;
            }

            var actualIndexAccordance = indexToTypeAccordances.Where(a => a.TypeNames.Any(t => t.Contains(typeName)))
                                                              .OrderByDescending(a => a.IndexName)
                                                              .FirstOrDefault();

            if (actualIndexAccordance != null)
            {
                return GetActualType(actualIndexAccordance.TypeNames);
            }

            throw new ArgumentException($"no type \"{typeName}\"exists in actual index ");
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