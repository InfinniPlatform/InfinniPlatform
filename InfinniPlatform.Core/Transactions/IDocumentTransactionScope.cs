using System.Collections.Generic;

namespace InfinniPlatform.Core.Transactions
{
    /// <summary>
    /// Предоставляет методы управления транзакцией.
    /// </summary>
    public interface IDocumentTransactionScope
    {
        /// <summary>
        /// Возвращает актуальный список документов.
        /// </summary>
        /// <param name="configuration">Имя конфигурации.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="documents">Список документов.</param>
        IEnumerable<object> GetDocuments(string configuration, string documentType, IEnumerable<object> documents);

        /// <summary>
        /// Регистрирует запрос сохранения документа.
        /// </summary>
        /// <param name="configuration">Имя конфигурации.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <param name="document">Экземпляр документа.</param>
        void SaveDocument(string configuration, string documentType, object documentId, object document);

        /// <summary>
        /// Регистрирует запрос удаления документа.
        /// </summary>
        /// <param name="configuration">Имя конфигурации.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        void DeleteDocument(string configuration, string documentType, object documentId);

        /// <summary>
        /// Устанавливает синхронный режим транзакции.
        /// </summary>
        void Synchronous();

        /// <summary>
        /// Подтверждает все операции.
        /// </summary>
        void Complete();

        /// <summary>
        /// Отменяет все операции.
        /// </summary>
        void Rollback();
    }
}