using System;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Sdk.Documents.Obsolete
{
    public interface IDocumentApi
    {
        /// <summary>
        /// Получить документ по указанному идентификатору
        /// </summary>
        /// <param name="documentType">Тип документа</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Документ с указанным идентификатором</returns>
        dynamic GetDocumentById(string documentType, string documentId);

        /// <summary>
        /// Получить документы по указанным фильтрам
        /// </summary>
        /// <param name="documentType">Тип документа</param>
        /// <param name="filter">Выражение для фильтрации документов</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sorting">Выражение для сортировки документов</param>
        /// <returns>Список документов, удовлетворяющих указанному фильтру</returns>
        IEnumerable<dynamic> GetDocument(string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null);

        /// <summary>
        /// Получить документы по указанным фильтрам
        /// </summary>
        /// <param name="documentType">Тип документа</param>
        /// <param name="filter">Выражение для фильтрации документов</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sorting">Выражение для сортировки документов</param>
        /// <returns>Список документов, удовлетворяющих указанному фильтру</returns>
        IEnumerable<dynamic> GetDocuments(string documentType, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null);

        /// <summary>
        /// Вставить или полностью заменить существующий документ
        /// </summary>
        /// <param name="documentType">Тип документа</param>
        /// <param name="document">Экземпляр сохраняемого документа</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        dynamic SetDocument(string documentType, object document);

        /// <summary>
        /// Вставить или полностью заменить документы в переданном списке
        /// </summary>
        /// <param name="documentType">Тип документа</param>
        /// <param name="documents">Список сохраняемых документов</param>
        /// <returns>Идентификатор сохраненного документа</returns>
        dynamic SetDocuments(string documentType, IEnumerable<object> documents);

        /// <summary>
        /// Удалить документ
        /// </summary>
        /// <param name="documentType">Идентификатор типа документа</param>
        /// <param name="instanceId">Идентификатор экземпляра документа</param>
        /// <returns>Результат удаления документа</returns>
        dynamic DeleteDocument(string documentType, string instanceId);

        /// <summary>
        /// Получить количество документов по указанному фильтру
        /// </summary>
        /// <param name="documentType">Идентификатор типа документа</param>
        /// <param name="filter">Фильтр документов</param>
        /// <returns>Количество документов</returns>
        long GetNumberOfDocuments(string documentType, Action<FilterBuilder> filter);

        /// <summary>
        /// Получить количество документов по указанному фильтру
        /// </summary>
        /// <param name="documentType">Идентификатор типа документа</param>
        /// <param name="filter">Фильтр документов</param>
        /// <returns>Количество документов</returns>
        long GetNumberOfDocuments(string documentType, IEnumerable<FilterCriteria> filter);

        /// <summary>
        /// Прикрепляет файл к свойству документа.
        /// </summary>
        void AttachFile(string documentType, string documentId, string fileProperty, string fileName, string fileType, Stream fileStream);
    }
}