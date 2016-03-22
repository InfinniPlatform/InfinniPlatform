namespace InfinniPlatform.Sdk.RestApi.Documents
{
    /// <summary>
    /// Запрос на удаление документа.
    /// </summary>
    public sealed class DocumentDeleteQueryClient
    {
        /// <summary>
        /// Правило фильтрации документов.
        /// </summary>
        public string Filter { get; set; }
    }
}