using System;

using InfinniPlatform.Core.Dynamic;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет методы создания набора команд изменения документов.
    /// </summary>
    public interface IDocumentBulkBuilder
    {
        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        IDocumentBulkBuilder InsertOne(DynamicWrapper document);

        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        IDocumentBulkBuilder UpdateOne(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        IDocumentBulkBuilder UpdateMany(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        IDocumentBulkBuilder ReplaceOne(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentBulkBuilder DeleteOne(Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentBulkBuilder DeleteMany(Func<IDocumentFilterBuilder, object> filter = null);
    }
}