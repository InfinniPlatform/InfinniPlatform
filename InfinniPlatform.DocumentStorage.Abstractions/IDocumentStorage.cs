using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Core.Dynamic;

namespace InfinniPlatform.DocumentStorage.Abstractions
{
    /// <summary>
    /// Предоставляет методы для работы с данными хранилища документов.
    /// </summary>
    public interface IDocumentStorage
    {
        /// <summary>
        /// Имя типа документа.
        /// </summary>
        string DocumentType { get; }


        /// <summary>
        /// Возвращает количество документов, удовлетворяющих указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        long Count(Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Возвращает количество документов, удовлетворяющих указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        Task<long> CountAsync(Func<IDocumentFilterBuilder, object> filter = null);


        /// <summary>
        /// Возвращает список уникальных значений свойства документа для указанного фильтра.
        /// </summary>
        /// <param name="property">Свойство документа.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentCursor<TProperty> Distinct<TProperty>(string property, Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Возвращает список уникальных значений свойства документа для указанного фильтра.
        /// </summary>
        /// <param name="property">Свойство документа.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        Task<IDocumentCursor<TProperty>> DistinctAsync<TProperty>(string property, Func<IDocumentFilterBuilder, object> filter = null);


        /// <summary>
        /// Осуществляет поиск по указанному фильтру и возвращает указатель на результат поиска.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentFindCursor Find(Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Осуществляет полнотекстовый поиск по указанной строке и возвращает указатель на результат поиска.
        /// </summary>
        /// <param name="search">Строка для полнотекстового поиска.</param>
        /// <param name="language">Язык для поиска.</param>
        /// <param name="caseSensitive">Чувствительность к регистру символов.</param>
        /// <param name="diacriticSensitive">Чувствительность к диакритическим символам.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentFindCursor FindText(string search, string language = null, bool caseSensitive = false, bool diacriticSensitive = false, Func<IDocumentFilterBuilder, object> filter = null);


        /// <summary>
        /// Возвращает интерфейс для построения запроса агрегации.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentAggregateCursor Aggregate(Func<IDocumentFilterBuilder, object> filter = null);


        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        void InsertOne(DynamicWrapper document);

        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        Task InsertOneAsync(DynamicWrapper document);

        /// <summary>
        /// Вставляет набор документов хранилище или возвращает исключение, если хранилище уже содержит один из указанных документов.
        /// </summary>
        /// <param name="documents">Список документов для вставки.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        void InsertMany(IEnumerable<DynamicWrapper> documents);

        /// <summary>
        /// Вставляет набор документов хранилище или возвращает исключение, если хранилище уже содержит один из указанных документов.
        /// </summary>
        /// <param name="documents">Список документов для вставки.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        Task InsertManyAsync(IEnumerable<DynamicWrapper> documents);


        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        DocumentUpdateResult UpdateOne(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        Task<DocumentUpdateResult> UpdateOneAsync(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        DocumentUpdateResult UpdateMany(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        Task<DocumentUpdateResult> UpdateManyAsync(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);


        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        DocumentUpdateResult ReplaceOne(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        Task<DocumentUpdateResult> ReplaceOneAsync(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);


        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        long DeleteOne(Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        Task<long> DeleteOneAsync(Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        long DeleteMany(Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        Task<long> DeleteManyAsync(Func<IDocumentFilterBuilder, object> filter = null);


        /// <summary>
        /// Выполняет набор команд изменения документов в рамках одного запроса.
        /// </summary>
        /// <param name="requests">Набор команд изменения документов.</param>
        /// <param name="isOrdered">Обязательно ли выполнять команды по порядку.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        DocumentBulkResult Bulk(Action<IDocumentBulkBuilder> requests, bool isOrdered = false);

        /// <summary>
        /// Выполняет набор команд изменения документов в рамках одного запроса.
        /// </summary>
        /// <param name="requests">Набор команд изменения документов.</param>
        /// <param name="isOrdered">Обязательно ли выполнять команды по порядку.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        Task<DocumentBulkResult> BulkAsync(Action<IDocumentBulkBuilder> requests, bool isOrdered = false);
    }
}