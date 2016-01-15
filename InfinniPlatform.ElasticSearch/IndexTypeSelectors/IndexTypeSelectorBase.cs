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
        /// <param name="indexName">Имена индексов, по которым производить поиск</param>
        /// <param name="typeNames">Наименование типа данных для выборки из индекса</param>
        /// <returns>Настроенный дескриптор</returns>
        public static SearchDescriptor<dynamic> BuildSearchForType(this SearchDescriptor<dynamic> searchDescriptor,
                                                                   string indexName,
                                                                   IEnumerable<string> typeNames)
        {
            searchDescriptor.Index(indexName);

            var types = typeNames as string[] ?? typeNames.ToArray();
            if (typeNames != null && types.Any())
            {
                searchDescriptor = searchDescriptor.Types(types);
            }

            return searchDescriptor;
        }

        /// <summary>
        /// Настроить дескриптор поиска для выборки данных указанных в переданном типе
        /// </summary>
        /// <param name="countDescriptor">Дескриптор поиска</param>
        /// <param name="indexName">Имена индексов, по которым производить поиск</param>
        /// <param name="typeNames">Наименование типа данных для выборки из индекса</param>
        /// <returns>Настроенный дескриптор</returns>
        public static CountDescriptor<dynamic> BuildSearchForType(this CountDescriptor<dynamic> countDescriptor,
                                                                  string indexName,
                                                                  IEnumerable<string> typeNames)
        {
            countDescriptor.Index(indexName);

            var types = typeNames as string[] ?? typeNames.ToArray();
            if (typeNames != null && types.Any())
            {
                countDescriptor = countDescriptor.Types(types);
            }

            return countDescriptor;
        }
    }
}