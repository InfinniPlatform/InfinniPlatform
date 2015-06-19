namespace InfinniPlatform.Api.ModelRepository.MetadataObjectModel
{
    /// <summary>
    ///     Ссылка на документ
    /// </summary>
    public class DocumentLink
    {
        /// <summary>
        ///     Идентификатор конфигурации
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        ///     Идентификатор документа
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        ///     Встроенный документ
        /// </summary>
        public bool Inline { get; set; }
    }
}