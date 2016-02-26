using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Documents.Transactions;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Documents
{
    public static class DocumentStorageExtensions
    {
        /// <summary>
        /// Свойство, в которое помещается значение релевантности документа при полнотекстовом поиске.
        /// </summary>
        public const string DefaultTextScoreProperty = "textScore";


        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <param name="document">Документ для сохранения.</param>
        public static DocumentUpdateResult SaveOne(this IDocumentStorage target, DynamicWrapper document)
        {
            return target.ReplaceOne(document, f => f.Eq("_id", document["_id"]), true);
        }

        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <param name="document">Документ для сохранения.</param>
        public static Task<DocumentUpdateResult> SaveOneAsync(this IDocumentStorage target, DynamicWrapper document)
        {
            return target.ReplaceOneAsync(document, f => f.Eq("_id", document["_id"]), true);
        }

        /// <summary>
        /// Вставляет набор документов в хранилище или заменяет их, если они уже существуют.
        /// </summary>
        /// <param name="documents">Документы для сохранения.</param>
        public static DocumentBulkResult SaveMany(this IDocumentStorage target, IEnumerable<DynamicWrapper> documents)
        {
            return target.Bulk(b => b.SaveMany(documents));
        }

        /// <summary>
        /// Вставляет набор документов в хранилище или заменяет их, если они уже существуют.
        /// </summary>
        /// <param name="documents">Документы для сохранения.</param>
        public static Task<DocumentBulkResult> SaveManyAsync(this IDocumentStorage target, IEnumerable<DynamicWrapper> documents)
        {
            return target.BulkAsync(b => b.SaveMany(documents));
        }


        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <param name="document">Документ для сохранения.</param>
        public static DocumentUpdateResult SaveOne<TDocument>(this IDocumentStorage<TDocument> target, TDocument document) where TDocument : Document
        {
            return target.ReplaceOne(document, i => i._id == document._id, true);
        }

        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <param name="document">Документ для сохранения.</param>
        public static Task<DocumentUpdateResult> SaveOneAsync<TDocument>(this IDocumentStorage<TDocument> target, TDocument document) where TDocument : Document
        {
            return target.ReplaceOneAsync(document, i => i._id == document._id, true);
        }

        /// <summary>
        /// Вставляет набор документов в хранилище или заменяет их, если они уже существуют.
        /// </summary>
        /// <param name="documents">Документы для сохранения.</param>
        public static DocumentBulkResult SaveMany<TDocument>(this IDocumentStorage<TDocument> target, IEnumerable<TDocument> documents) where TDocument : Document
        {
            return target.Bulk(b => b.SaveMany(documents));
        }

        /// <summary>
        /// Вставляет набор документов в хранилище или заменяет их, если они уже существуют.
        /// </summary>
        /// <param name="documents">Документы для сохранения.</param>
        public static Task<DocumentBulkResult> SaveManyAsync<TDocument>(this IDocumentStorage<TDocument> target, IEnumerable<TDocument> documents) where TDocument : Document
        {
            return target.BulkAsync(b => b.SaveMany(documents));
        }


        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <param name="document">Документ для сохранения.</param>
        public static IDocumentBulkBuilder SaveOne(this IDocumentBulkBuilder target, DynamicWrapper document)
        {
            return target.ReplaceOne(document, f => f.Eq("_id", document["_id"]), true);
        }

        /// <summary>
        /// Вставляет набор документов в хранилище или заменяет их, если они уже существуют.
        /// </summary>
        /// <param name="documents">Документы для сохранения.</param>
        public static IDocumentBulkBuilder SaveMany(this IDocumentBulkBuilder target, IEnumerable<DynamicWrapper> documents)
        {
            foreach (var document in documents)
            {
                target = target.SaveOne(document);
            }

            return target;
        }


        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <param name="document">Документ для сохранения.</param>
        public static IDocumentBulkBuilder<TDocument> SaveOne<TDocument>(this IDocumentBulkBuilder<TDocument> target, TDocument document) where TDocument : Document
        {
            return target.ReplaceOne(document, i => i._id == document._id, true);
        }

        /// <summary>
        /// Вставляет набор документов в хранилище или заменяет их, если они уже существуют.
        /// </summary>
        /// <param name="documents">Документы для сохранения.</param>
        public static IDocumentBulkBuilder<TDocument> SaveMany<TDocument>(this IDocumentBulkBuilder<TDocument> target, IEnumerable<TDocument> documents) where TDocument : Document
        {
            foreach (var document in documents)
            {
                target = target.SaveOne(document);
            }

            return target;
        }


        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="document">Документ для сохранения.</param>
        public static void SaveOne(this IUnitOfWork target, string documentType, DynamicWrapper document)
        {
            target.ReplaceOne(documentType, document, f => f.Eq("_id", document["_id"]), true);
        }

        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="document">Документ для сохранения.</param>
        public static void SaveOne<TDocument>(this IUnitOfWork target, string documentType, TDocument document) where TDocument : Document
        {
            target.ReplaceOne(documentType, document, i => i._id == document._id, true);
        }

        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="document">Документ для сохранения.</param>
        public static void SaveOne<TDocument>(this IUnitOfWork target, TDocument document) where TDocument : Document
        {
            target.ReplaceOne(document, i => i._id == document._id, true);
        }

        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="documents">Документы для сохранения.</param>
        public static void SaveMany(this IUnitOfWork target, string documentType, IEnumerable<DynamicWrapper> documents)
        {
            foreach (var document in documents)
            {
                target.SaveOne(documentType, document);
            }
        }

        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="documents">Документы для сохранения.</param>
        public static void SaveMany<TDocument>(this IUnitOfWork target, string documentType, IEnumerable<TDocument> documents) where TDocument : Document
        {
            foreach (var document in documents)
            {
                target.SaveOne(documentType, document);
            }
        }

        /// <summary>
        /// Вставляет один документ в хранилище или заменяет его, если он уже существует.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documents">Документы для сохранения.</param>
        public static void SaveMany<TDocument>(this IUnitOfWork target, IEnumerable<TDocument> documents) where TDocument : Document
        {
            foreach (var document in documents)
            {
                target.SaveOne(document);
            }
        }
    }
}