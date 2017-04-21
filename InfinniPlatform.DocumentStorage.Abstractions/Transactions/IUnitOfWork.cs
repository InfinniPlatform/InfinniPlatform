using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.DocumentStorage.Transactions
{
    /// <summary>
    /// Предоставляет интерфейс, реализующий шаблон "Единица работы" ("Unit of Work").
    /// </summary>
    /// <remarks>
    /// http://martinfowler.com/eaaCatalog/unitOfWork.html
    /// </remarks>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="document">Документ для вставки.</param>
        void InsertOne(string documentType, DynamicWrapper document);

        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="document">Документ для вставки.</param>
        void InsertOne<TDocument>(string documentType, TDocument document) where TDocument : Document;

        /// <summary>
        /// Вставляет один документ в хранилище или возвращает исключение, если хранилище уже содержит указанный документ.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="document">Документ для вставки.</param>
        void InsertOne<TDocument>(TDocument document) where TDocument : Document;


        /// <summary>
        /// Вставляет набор документов хранилище или возвращает исключение, если хранилище уже содержит один из указанных документов.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="documents">Список документов для вставки.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        void InsertMany(string documentType, IEnumerable<DynamicWrapper> documents);

        /// <summary>
        /// Вставляет набор документов хранилище или возвращает исключение, если хранилище уже содержит один из указанных документов.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="documents">Список документов для вставки.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        void InsertMany<TDocument>(string documentType, IEnumerable<TDocument> documents) where TDocument : Document;

        /// <summary>
        /// Вставляет набор документов хранилище или возвращает исключение, если хранилище уже содержит один из указанных документов.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documents">Список документов для вставки.</param>
        /// <exception cref="DocumentStorageWriteException"></exception>
        void InsertMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : Document;


        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void UpdateOne(string documentType, Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void UpdateOne<TDocument>(string documentType, Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document;

        /// <summary>
        /// Обновляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void UpdateOne<TDocument>(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document;


        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void UpdateMany(string documentType, Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void UpdateMany<TDocument>(string documentType, Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document;

        /// <summary>
        /// Обновляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="update">Оператор обновления документов.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void UpdateMany<TDocument>(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document;


        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void ReplaceOne(string documentType, DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false);

        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void ReplaceOne<TDocument>(string documentType, TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document;

        /// <summary>
        /// Заменяет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="replacement">Документ замены.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        /// <param name="insertIfNotExists">Следует ли создать документ, если ничего не найдено.</param>
        void ReplaceOne<TDocument>(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document;


        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        void DeleteOne(string documentType, Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        void DeleteOne<TDocument>(string documentType, Expression<Func<TDocument, bool>> filter = null) where TDocument : Document;

        /// <summary>
        /// Удаляет первый найденный документ, удовлетворяющий указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="filter">Фильтр для поиска документов.</param>
        void DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter = null) where TDocument : Document;


        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        void DeleteMany(string documentType, Func<IDocumentFilterBuilder, object> filter = null);

        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="filter">Фильтр для поиска документов.</param>
        void DeleteMany<TDocument>(string documentType, Expression<Func<TDocument, bool>> filter = null) where TDocument : Document;

        /// <summary>
        /// Удаляет все документы, удовлетворяющие указанному фильтру.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="filter">Фильтр для поиска документов.</param>
        void DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter = null) where TDocument : Document;


        /// <summary>
        /// Подтверждает все действия.
        /// </summary>
        /// <param name="isOrdered">Обязательно ли выполнять команды по порядку.</param>
        void Commit(bool isOrdered = false);

        /// <summary>
        /// Подтверждает все действия.
        /// </summary>
        /// <param name="isOrdered">Обязательно ли выполнять команды по порядку.</param>
        Task CommitAsync(bool isOrdered = false);
    }
}