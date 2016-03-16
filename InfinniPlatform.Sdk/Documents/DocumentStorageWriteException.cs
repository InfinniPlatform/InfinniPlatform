using System;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Исключение при выполнении команды изменения документа в хранилище.
    /// </summary>
    [Serializable]
    public sealed class DocumentStorageWriteException : Exception
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="writeResult">Результат выполнения команды изменения документа в хранилище.</param>
        public DocumentStorageWriteException(string documentType, DocumentStorageWriteResult writeResult)
        {
            Data.Add("DocumentType", documentType);
            Data.Add("WriteResult", writeResult);
        }

        /// <summary>
        /// Результат выполнения команды изменения документа в хранилище.
        /// </summary>
        public DocumentStorageWriteResult WriteResult { get; set; }
    }
}