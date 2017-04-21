namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Базовый класс документов.
    /// </summary>
    public class Document
    {
        // ReSharper disable InconsistentNaming


        /// <summary>
        /// Идентификатор документа.
        /// </summary>
        public object _id { get; set; }

        /// <summary>
        /// Заголовок документа.
        /// </summary>
        public DocumentHeader _header { get; set; }


        // ReSharper restore InconsistentNaming
    }
}