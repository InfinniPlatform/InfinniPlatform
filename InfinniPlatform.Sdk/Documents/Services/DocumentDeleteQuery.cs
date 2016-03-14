namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Запрос на удаление документа.
    /// </summary>
    public class DocumentDeleteQuery
    {
        /// <summary>
        /// Идентификатор документа.
        /// </summary>
        public object DocumentId { get; set; }
    }
}