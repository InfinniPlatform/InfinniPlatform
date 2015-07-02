using Nest;

using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeSelectors
{
    public static class IndexTypeSelectorBase 
    {
	    /// <summary>
	    ///   Настроить дескриптор поиска для выборки данных указанных в переданном типе
	    /// </summary>
	    /// <param name="searchDescriptor">Дескриптор поиска</param>
	    /// <param name="indexNames">Имена индексов, по которым производить поиск</param>
	    /// <param name="typeName">Наименование типа данных для выборки из индекса</param>
	    /// <param name="searchInAllIndeces">Искать по всем инлдексам</param>
	    /// <param name="searchInAllTypes">Искать по всем типам</param>
	    /// <returns>Настроенный дескриптор</returns>
	    public static SearchDescriptor<dynamic> BuildSearchForType(
            this SearchDescriptor<dynamic> searchDescriptor,
            IEnumerable<string> indexNames,
            IEnumerable<string> typeName,
            bool searchInAllIndeces,
            bool searchInAllTypes)
        {
            if (indexNames == null || !indexNames.Any())
            {
                if (searchInAllIndeces)
                {
                    searchDescriptor = searchDescriptor.AllIndices();
                }
            }
            else
            {
                searchDescriptor = indexNames.Count() == 1 ?
                    searchDescriptor.Index(indexNames.First()) :
                    searchDescriptor.Indices(indexNames);
            }


            if (typeName == null || !typeName.Any())
            {
                if (searchInAllTypes)
                {
                    searchDescriptor = searchDescriptor.AllTypes();
                }
            }
            else
            {
                searchDescriptor = searchDescriptor.Types(typeName);
            }

	        return searchDescriptor;
        }

        /// <summary>
        ///   Настроить дескриптор поиска для выборки данных указанных в переданном типе
        /// </summary>
        /// <param name="countDescriptor">Дескриптор поиска</param>
        /// <param name="indexNames">Имена индексов, по которым производить поиск</param>
        /// <param name="typeName">Наименование типа данных для выборки из индекса</param>
        /// <param name="searchInAllIndeces">Искать по всем инлдексам</param>
        /// <param name="searchInAllTypes">Искать по всем типам</param>
        /// <returns>Настроенный дескриптор</returns>
        public static CountDescriptor<dynamic> BuildSearchForType(
            this CountDescriptor<dynamic> countDescriptor,
            IEnumerable<string> indexNames,
            IEnumerable<string> typeName,
            bool searchInAllIndeces,
            bool searchInAllTypes)
        {
            if (indexNames == null || !indexNames.Any())
            {
                if (searchInAllIndeces)
                {
                    countDescriptor = countDescriptor.AllIndices();
                }
            }
            else
            {
                countDescriptor = indexNames.Count() == 1 ?
                    countDescriptor.Index(indexNames.First()) :
                    countDescriptor.Indices(indexNames);
            }


            if (typeName == null || !typeName.Any())
            {
                if (searchInAllTypes)
                {
                    countDescriptor = countDescriptor.AllTypes();
                }
            }
            else
            {
                countDescriptor = countDescriptor.Types(typeName);
            }
            
            return countDescriptor;
        }
    }
}
