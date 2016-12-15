using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.DocumentStorage.Contract
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
        [SerializerVisible]
        public DocumentHeader _header { get; internal set; }


        // ReSharper restore InconsistentNaming
    }
}