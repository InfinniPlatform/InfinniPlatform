using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;

using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
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
        /// �������� ������� ����� ����� � ������� �� ���� ������
        /// </summary>
        /// <param name="derivedTypesNames">����� ������ ����� � �������</param>
        /// <returns>������ ������������ ������� ����� � �������</returns>
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
        /// ����� ������������ � ��������� ������������� ���� � ��������
        /// </summary>
        /// <param name="accordances">������ �������������</param>
        /// <param name="typeName">������������ ���� ��� ������</param>
        /// <returns>������������� ���� � ��������</returns>
        public static IndexToTypeAccordance GetIndexToTypeAccordance(this IEnumerable<IndexToTypeAccordance> accordances,
                                                                     string typeName)
        {
            typeName = typeName.ToLowerInvariant();
            return accordances.FirstOrDefault(a => a.BaseTypeName.ToLowerInvariant() == typeName.ToLowerInvariant());
        }

        /// <summary>
        /// �������� ���������� ������ ����� ���� ���������
        /// </summary>
        /// <param name="accordances">������ ������������ ������������ �������� � ���������� � ��� ������ ����� ������</param>
        /// <param name="typeName">������������ ����, ���������� ������ �������� ��������� ��������</param>
        /// <returns>���������� ������ ����</returns>
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