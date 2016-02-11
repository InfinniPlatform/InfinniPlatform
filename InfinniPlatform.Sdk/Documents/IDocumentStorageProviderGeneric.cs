using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Предоставляет методы для работы с данными хранилища документов.
    /// </summary>
    public interface IDocumentStorageProvider<TDocument>
    {
        /// <summary>
        /// Возвращает количество документов, удовлетворяющих указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        long Count(Expression<Func<TDocument, bool>> filter = null);

        /// <summary>
        /// Возвращает количество документов, удовлетворяющих указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        Task<long> CountAsync(Expression<Func<TDocument, bool>> filter = null);


        /// <summary>
        /// Возвращает список уникальных значений свойства документа для указанного фильтра.
        /// </summary>
        /// <param name="field">Свойство документа.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentCursor<TProperty> Distinct<TProperty>(Expression<Func<TDocument, TProperty>> field, Expression<Func<TDocument, bool>> filter = null);

        /// <summary>
        /// Возвращает список уникальных значений свойства документа для указанного фильтра.
        /// </summary>
        /// <param name="field">Свойство документа.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        Task<IDocumentCursor<TProperty>> DistinctAsync<TProperty>(Expression<Func<TDocument, TProperty>> field, Expression<Func<TDocument, bool>> filter = null);


        /// <summary>
        /// Возвращает интерфейс для построения поискового запроса.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentFindCursor<TDocument, TDocument> Find(Expression<Func<TDocument, bool>> filter = null);


        /// <summary>
        /// Возвращает интерфейс для построения запроса агрегации.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentAggregateCursor<TDocument> Aggregate(Expression<Func<TDocument, bool>> filter = null);


        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        void InsertOne(TDocument document);

        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        Task InsertOneAsync(TDocument document);

        /// <summary>
        /// Вставляет набор документов хранилище или возвращает исключение, если хранилище уже содержит один из указанных документов.
        /// </summary>
        /// <param name="documents">Список документов для вставки.</param>
        void InsertMany(IEnumerable<TDocument> documents);

        /// <summary>
        /// Вставляет набор документов хранилище или возвращает исключение, если хранилище уже содержит один из указанных документов.
        /// </summary>
        /// <param name="documents">Список документов для вставки.</param>
        Task InsertManyAsync(IEnumerable<TDocument> documents);


        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        DocumentUpdateResult UpdateOne(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        Task<DocumentUpdateResult> UpdateOneAsync(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        DocumentUpdateResult UpdateMany(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        Task<DocumentUpdateResult> UpdateManyAsync(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);


        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        DocumentUpdateResult ReplaceOne(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        Task<DocumentUpdateResult> ReplaceOneAsync(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);


        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        long DeleteOne(Expression<Func<TDocument, bool>> filter = null);

        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        Task<long> DeleteOneAsync(Expression<Func<TDocument, bool>> filter = null);

        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        long DeleteMany(Expression<Func<TDocument, bool>> filter = null);

        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        Task<long> DeleteManyAsync(Expression<Func<TDocument, bool>> filter = null);


        /// <summary>
        /// Выполняет набор операций изменения документов в рамках одного запроса.
        /// </summary>
        /// <param name="bulk">Набор операций изменения документов.</param>
        /// <param name="isOrdered">Обязательно ли выполнять операции по порядку.</param>
        DocumentBulkResult Bulk(Action<IDocumentBulkBuilder<TDocument>> bulk, bool isOrdered = false);

        /// <summary>
        /// Выполняет набор операций изменения документов в рамках одного запроса.
        /// </summary>
        /// <param name="bulk">Набор операций изменения документов.</param>
        /// <param name="isOrdered">Обязательно ли выполнять операции по порядку.</param>
        Task<DocumentBulkResult> BulkAsync(Action<IDocumentBulkBuilder<TDocument>> bulk, bool isOrdered = false);
    }
}