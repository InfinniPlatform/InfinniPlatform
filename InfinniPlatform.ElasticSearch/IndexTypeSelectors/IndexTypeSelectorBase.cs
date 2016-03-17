﻿using System.Collections.Generic;
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
        /// <param name="typeNames">Наименование типа данных для выборки из индекса</param>
        /// <returns>Настроенный дескриптор</returns>
        public static SearchDescriptor<dynamic> BuildSearchForType(this SearchDescriptor<dynamic> searchDescriptor, IEnumerable<string> typeNames)
        {
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
        /// <param name="typeNames">Наименование типа данных для выборки из индекса</param>
        /// <returns>Настроенный дескриптор</returns>
        public static CountDescriptor<dynamic> BuildSearchForType(this CountDescriptor<dynamic> countDescriptor, IEnumerable<string> typeNames)
        {
            var types = typeNames as string[] ?? typeNames.ToArray();
            if (typeNames != null && types.Any())
            {
                countDescriptor = countDescriptor.Types(types);
            }

            return countDescriptor;
        }
    }
}