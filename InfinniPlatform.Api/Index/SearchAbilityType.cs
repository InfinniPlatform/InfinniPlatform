namespace InfinniPlatform.Api.Index
{
    /// <summary>
    ///     Определяет поддерживаемый тип поиска по индексу
    /// </summary>
    public enum SearchAbilityType
    {
        /// <summary>
        ///     Поиск только по ключевым словам целиком (используется по умолчанию).
        ///     В основе лежит Keyword Tokenizer: A tokenizer of type keyword that emits the entire input as a single output
        /// </summary>
        KeywordBasedSearch = 0,

        /// <summary>
        ///     Полнотекстовый поиск по любой части слова.
        ///     Тип поиска должен быть применен только к хранилищам, по которым будет
        ///     осуществляться полнотекстовый поиск введенных пользователем данных.
        /// </summary>
        FullTextSearch = 1
    }
}