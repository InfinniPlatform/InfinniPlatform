using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions
{
    /// <summary>
    ///   Провайдер связки между индексом и типом, в которой одному индексу сопоставлен один тип
    /// </summary>
    public sealed class TypeToIndexOneTypeInIndex : ITypeToIndexReference
    {
        private readonly ElasticConnection _connection;
        private readonly IndexAliasesManager _aliasesManager;

        public TypeToIndexOneTypeInIndex()
        {
            _connection = new ElasticConnection();
            _aliasesManager = new IndexAliasesManager(_connection.IndexingClient);
        }


        /// <summary>
        /// Получить состояние индекса
        /// </summary>
        /// <param name="aliasedIndexName">Наименование логической совокупности индексов</param>
        /// <param name="typeName"></param>
        /// <returns>Состояние индекса</returns>
        public IndexStatus GetIndexStatus(string aliasedIndexName, string typeName)
        {
            aliasedIndexName = aliasedIndexName.ToLowerInvariant();

            if (_aliasesManager.GetAliasedIndexByName(aliasedIndexName) == null)
            {
                return IndexStatus.NotExists;
            }

            return IndexStatus.Exists;
        }

        /// <summary>
        ///  Удалить индекс с указанным наименованием
        /// </summary>
        /// <param name="aliasedIndexName">Наименование логической совокупности индексов</param>
        /// <param name="typeName">Наименование типа в индексе</param>
        public void DeleteIndexType(string aliasedIndexName, string typeName)
        {
            aliasedIndexName = aliasedIndexName.ToLowerInvariant();

            var aliasedIndex = _aliasesManager.GetAliasedIndexByName(aliasedIndexName);

            if (aliasedIndex != null)
            {
                // Возможны ситуации, когда у индекса есть только алиас для поиска
                if (!string.IsNullOrEmpty(aliasedIndex.ActiveIndex))
                {
                    _connection.IndexingClient.DeleteIndex(aliasedIndex.ActiveIndex);
                }

                foreach (var index in aliasedIndex.SearchingIndexes)
                {
                    _connection.IndexingClient.DeleteIndex(index);
                }
            }

            _aliasesManager.DeleteAliasesForIndex(aliasedIndexName);

            _connection.IndexingClient.Refresh();
        }


        /// <summary>
        ///  Создает новую версию индекса с указанными настройками маппинга объектов
        /// </summary>
        /// <param name="aliasedIndexName">Наименование логической совокупности индексов</param>
        /// <param name="typeName">Тип документа в индексе, для которого создаем новую версию. 
        ///     Если не указано, создается новая версия всего индекса
        ///     Иначе создается новая версия типа внутри индекса
        /// </param>
        /// <param name="deleteExistingVersion">Удалить существующую версию</param>
        /// <param name="indexTypeMapper"></param>
        public void CreateIndexType(string aliasedIndexName, string typeName, bool deleteExistingVersion = false, IIndexTypeMapper indexTypeMapper = null)
        {
            aliasedIndexName = aliasedIndexName.ToLowerInvariant();

            var aliasedVersion = 0;

            var aliasedIndex = _aliasesManager.GetAliasedIndexByName(aliasedIndexName);
            if (aliasedIndex != null && aliasedIndex.ActiveIndex.Contains(IndexTypeMapper.MappingVersionPattern))
            {
                aliasedVersion =
                    int.Parse(aliasedIndex.ActiveIndex.Substring((aliasedIndexName + IndexTypeMapper.MappingVersionPattern).Length)) + 1;
            }

            if (aliasedIndex != null && deleteExistingVersion)
            {
                _connection.IndexingClient.DeleteIndex(aliasedIndex.ActiveIndex);
            }

            // Получаем номер версии документа, актуальной в настоящее время
            // Паттерн для имени индекса: aliased_index_name_version_5 . Удаляем из этой
            // строки все символы, кроме номера версии и инкрементируем версию на единицу

            var indexAnalyzer = "string_lowercase";
            if (indexTypeMapper != null)
            {
                indexAnalyzer = indexTypeMapper.IndexAnalyzer;
            }

            _connection.IndexingClient.CreateIndex(
                   aliasedIndexName + IndexTypeMapper.MappingVersionPattern + aliasedVersion,
                   c => c.AddMapping<IndexObject>(m => m.MapFromAttributes().IndexAnalyzer(indexAnalyzer))
             );

            // Обновляем алиасы для индексов
            _aliasesManager.CreateAliasesForActualIndexVersion(aliasedIndexName, IndexTypeMapper.MappingVersionPattern + aliasedVersion);

            if (indexTypeMapper != null)
            {
                indexTypeMapper.ApplyIndexTypeMapping(aliasedIndexName + IndexTypeMapper.MappingVersionPattern + aliasedVersion);
            }

            _connection.IndexingClient.Refresh();
        }

    }
}
