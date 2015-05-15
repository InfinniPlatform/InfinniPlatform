using System.Collections.Generic;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    /// Представляет собой абстракцию над индексом (индекс с псевдонимами).
    /// Может объединять несколько индексов для поиска данных 
    /// (например, позволяет делать запросы к user_v1, user_v2, ...), 
    /// однако запись ведется всегда только в один индекс с актуальной версией
    /// </summary>
    public class AliasedIndex
    {
        public AliasedIndex(
            string name,
            IEnumerable<string> searchingIndexes, 
            string activeIndex, 
            string searchingAlias, 
            string indexingAlias)
        {
            Name = name;
            IndexingAlias = indexingAlias;
            SearchingAlias = searchingAlias;
            ActiveIndex = activeIndex;
            SearchingIndexes = searchingIndexes;
        }

        /// <summary>
        /// Возвращает имя логической совокупности индексов
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Возвращает имена индексов, по которым производится поиск
        /// </summary>
        public IEnumerable<string> SearchingIndexes { get; private set; }

        /// <summary>
        /// Возвращает имя индекса, в который производится вставка
        /// (индекс в последней редакции)
        /// </summary>
        public string ActiveIndex { get; private set; }

        /// <summary>
        /// Возвращает alias к совокупности индексов,
        /// по которым можно искать документы
        /// </summary>
        public string SearchingAlias { get; private set; }

        /// <summary>
        /// Возвращает alias, адресованный к индексу для
        /// вставки данных
        /// </summary>
        public string IndexingAlias { get; private set; }
    }
}