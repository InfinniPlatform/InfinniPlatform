using System.Collections.Generic;
using System.Linq;

using Nest;

namespace InfinniPlatform.ElasticSearch.IndexTypeSelectors
{
    public static class IndexTypeSelectorBase
    {
        /// <summary>
        /// Настроить дескриптор поиска для выборки данных указанных в переданном типе
        /// </summary>
        /// <param name="searchDescriptor">Дескриптор поиска</param>
        /// <param name="indexNames">Имена индексов, по которым производить поиск</param>
        /// <param name="typeName">Наименование типа данных для выборки из индекса</param>
        /// <returns>Настроенный дескриптор</returns>
        public static SearchDescriptor<dynamic> BuildSearchForType(this SearchDescriptor<dynamic> searchDescriptor,
                                                                   IEnumerable<string> indexNames,
                                                                   IEnumerable<string> typeName)
        {
            var indices = indexNames as string[] ?? indexNames.ToArray();
            if (indexNames != null && indices.Any())
            {
                searchDescriptor.Indices(indices);
            }

            var types = typeName as string[] ?? typeName.ToArray();
            if (typeName != null && types.Any())
            {
                searchDescriptor = searchDescriptor.Types(types);
            }

            return searchDescriptor;
        }

        /// <summary>
        /// Настроить дескриптор поиска для выборки данных указанных в переданном типе
        /// </summary>
        /// <param name="countDescriptor">Дескриптор поиска</param>
        /// <param name="indexNames">Имена индексов, по которым производить поиск</param>
        /// <param name="typeName">Наименование типа данных для выборки из индекса</param>
        /// <returns>Настроенный дескриптор</returns>
        public static CountDescriptor<dynamic> BuildSearchForType(this CountDescriptor<dynamic> countDescriptor,
                                                                  IEnumerable<string> indexNames,
                                                                  IEnumerable<string> typeName)
        {
            var indices = indexNames as string[] ?? indexNames.ToArray();
            if (indexNames != null && indices.Any())
            {
                countDescriptor.Indices(indices);
            }

            var types = typeName as string[] ?? typeName.ToArray();
            if (typeName != null && types.Any())
            {
                countDescriptor = countDescriptor.Types(types);
            }

            return countDescriptor;
        }
    }
}