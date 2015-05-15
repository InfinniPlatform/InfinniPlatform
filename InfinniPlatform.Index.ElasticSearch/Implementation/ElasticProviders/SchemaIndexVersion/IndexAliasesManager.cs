using System;
using System.Linq;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    /// <summary>
    /// Вспомогальный класс, позволяющий управлять алиасами для индексов.
    /// Обладаем возможностями по добавлению и удалению алиасов для существующих индексов
    /// </summary>
    internal sealed class IndexAliasesManager
    {
        private readonly ElasticClient _elasticClient;

        private const string SearchingAliasEnding = "_searching_alias";

        private const string IndexingAliasEnding = "_indexing_alias";

        public IndexAliasesManager(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// Создаёт обертку над логической совокупностью индексов по её имени. 
        /// Для успешной работы метода необходимо, чтобы алиасы для индекса были созданы.
        /// Возращает null, если алиасов для индекса нет
        /// </summary>
        public AliasedIndex GetAliasedIndexByName(string aliasedIndexName)
        {
            var searchingIndeces =
                _elasticClient.GetIndicesPointingToAlias(aliasedIndexName + SearchingAliasEnding).ToArray();

            // алиаса для добавления может не существовать (например, если aliasedIndexName используется только для поиска данных)
            var activeIndex =
                _elasticClient.GetIndicesPointingToAlias(aliasedIndexName + IndexingAliasEnding).ToArray();

            if (searchingIndeces.Length == 0)
            {
                // Это означает, что для индекса не было создано алиасов
                return null;
            }

            return new AliasedIndex(
                aliasedIndexName,
                searchingIndeces,
                activeIndex.Length > 0 ? activeIndex[0] : null,
                aliasedIndexName + SearchingAliasEnding,
                activeIndex.Length > 0 ? aliasedIndexName + IndexingAliasEnding : aliasedIndexName + SearchingAliasEnding);
        }

        /// <summary>
        /// Актуализирует алиасы после создания нового индекса в рамках единой логической совокупности индексов.
        /// Создается новый алиас для индексирования, указывающий на новый индекс.
        /// Алиас для поиска расширяется путем добавления ссылки на новый индекс (ссылки на старые индексы сохраняются)
        /// </summary>
        /// <param name="aliasedIndexName">Имя логической совокупности индексов (без суффикса с версией)</param>
        /// <param name="versionSpecifier">Суффикс с наименованием текущей версии индекса</param>
        public void CreateAliasesForActualIndexVersion(string aliasedIndexName, string versionSpecifier)
        {
            // необходимо убедиться, что индекс существует поскольку если индекса нет,
            // создать алиасы не получится
            if (!_elasticClient.IndexExists(aliasedIndexName + versionSpecifier).Exists)
            {
                throw new Exception("Type " + aliasedIndexName + versionSpecifier + " doesn't exist");
            }

            // Удаляем алиас для индекса, являвшегося актуальным ранее
            foreach (string previousActualIndexName in _elasticClient.GetIndicesPointingToAlias(aliasedIndexName + IndexingAliasEnding))
            {
                _elasticClient.RemoveAlias(previousActualIndexName, aliasedIndexName + IndexingAliasEnding);
            }

            _elasticClient.Alias(aliasedIndexName + versionSpecifier, aliasedIndexName + IndexingAliasEnding);
            _elasticClient.Alias(aliasedIndexName + versionSpecifier, aliasedIndexName + SearchingAliasEnding);
        }

        /// <summary>
        /// Удаляет существующие алиасы для индекса
        /// </summary>
        /// <param name="aliasedIndexName">Имя логической совокупности индексов (без суффикса с версией)</param>
        public void DeleteAliasesForIndex(string aliasedIndexName)
        {
            foreach (string indexName in _elasticClient.GetIndicesPointingToAlias(aliasedIndexName + SearchingAliasEnding))
            {
                _elasticClient.RemoveAlias(indexName, aliasedIndexName + SearchingAliasEnding);
            }

            foreach (string indexName in _elasticClient.GetIndicesPointingToAlias(aliasedIndexName + IndexingAliasEnding))
            {
                _elasticClient.RemoveAlias(indexName, aliasedIndexName + IndexingAliasEnding);
            }
        }
    }
}