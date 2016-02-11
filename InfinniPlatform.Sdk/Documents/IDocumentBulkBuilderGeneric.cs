using System;
using System.Linq.Expressions;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Предоставляет методы создания набора операций изменения документов.
    /// </summary>
    public interface IDocumentBulkBuilder<TDocument>
    {
        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        IDocumentBulkBuilder<TDocument> InsertOne(TDocument document);

        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        IDocumentBulkBuilder<TDocument> UpdateOne(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        IDocumentBulkBuilder<TDocument> UpdateMany(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        IDocumentBulkBuilder<TDocument> ReplaceOne(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentBulkBuilder<TDocument> DeleteOne(Expression<Func<TDocument, bool>> filter = null);

        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentBulkBuilder<TDocument> DeleteMany(Expression<Func<TDocument, bool>> filter = null);
    }
}