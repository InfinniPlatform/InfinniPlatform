using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Abstractions.Transactions;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Abstractions
{
    public static class DocumentStorageExtensions
    {
        /// <summary>
        /// Свойство, в которое помещается значение релевантности документа при полнотекстовом поиске.
        /// </summary>
        public const string DefaultTextScoreProperty = "textScore";


        /// <summary>
        /// Возвращает имя типа документа по умолчанию.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <returns>Имя типа документа.</returns>
        public static string GetDefaultDocumentTypeName<TDocument>()
        {
            var type = typeof(TDocument);

            return type.GetTypeInfo().GetAttributeValue<DocumentTypeAttribute, string>(i => i.Name, type.NameOf());
        }


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


        /// <summary>
        /// Инвертирует выражение фильтрации с использованием логического оператора НЕ.
        /// </summary>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> value)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(value.Body), value.Parameters[0]);
        }

        /// <summary>
        /// Объединяет два выражения фильтрации с использованием логического оператора И.
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return Compose(first, second, Expression.AndAlso);
        }

        /// <summary>
        /// Объединяет два выражения фильтрации с использованием логического оператора ИЛИ.
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return Compose(first, second, Expression.OrElse);
        }

        /// <summary>
        /// Объединяет два выражения с использованием указанной функции.
        /// </summary>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> composeFunc)
        {
            if (first != null && second != null)
            {
                // Таблица соответствий параметров первого и второго выражений
                var secondToFirstMap = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

                // Переименование параметров второго выражения
                var newSecondBody = new LambdaExpressionComposer(secondToFirstMap).Visit(second.Body);

                // Объединение выражений заданной операцией
                return Expression.Lambda<T>(composeFunc(first.Body, newSecondBody), first.Parameters);
            }

            if (first != null)
            {
                return first;
            }

            return second;
        }


        /// <summary>
        /// Предоставляет интерфейс для объединения выражений.
        /// </summary>
        private class LambdaExpressionComposer : ExpressionVisitor
        {
            public LambdaExpressionComposer(Dictionary<ParameterExpression, ParameterExpression> secondToFirstParameterMap)
            {
                _secondToFirstParameterMap = secondToFirstParameterMap ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }


            private readonly Dictionary<ParameterExpression, ParameterExpression> _secondToFirstParameterMap;


            protected override Expression VisitParameter(ParameterExpression secondParameter)
            {
                ParameterExpression firstParameter;

                if (_secondToFirstParameterMap.TryGetValue(secondParameter, out firstParameter))
                {
                    secondParameter = firstParameter;
                }

                return base.VisitParameter(secondParameter);
            }
        }
    }
}