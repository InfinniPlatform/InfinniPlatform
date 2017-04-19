using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.DocumentStorage.Abstractions.Transactions;

namespace InfinniPlatform.DocumentStorage.MongoDB.Transactions
{
    internal class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDocumentStorageFactory storageFactory, UnitOfWork parentScope = null)
        {
            _storageFactory = storageFactory;
            _parentScope = parentScope;
            _unitOfWorkLog = new UnitOfWorkLog();
        }


        private readonly IDocumentStorageFactory _storageFactory;
        private readonly UnitOfWork _parentScope;
        private readonly UnitOfWorkLog _unitOfWorkLog;


        private bool _isOrdered;


        /// <summary>
        /// Начинает работу и возвращает ее экземпляр.
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет реализовать двухуровневую вложенность экземпляров <see cref="IUnitOfWork"/>. Экземпляры
        /// <see cref="IUnitOfWork"/> делятся на два типа: родительские и дочерние. С одним родительским экземпляром может
        /// быть связано несколько дочерних. Вызов метода <see cref="Commit"/> у родительского экземпляра производит
        /// сохранение накопленных изменений в хранилище документов. Вызов метода <see cref="Commit"/>, у дочернего
        /// экземпляра производит копирование накопленных изменений в родительский экземпляр. Вызов метода
        /// <see cref="Dispose"/> отменяет действия только того экземпляра, у которого он был вызван. Таким
        /// образом обеспечивается изоляция дочерних экземпляров. Основной недостаток заключается в том, что
        /// данный подход не учитывает уровень вложенности дочерних экземпляров друг относительно друга.
        /// </remarks>
        public IUnitOfWork Begin()
        {
            return BeginParentScope() ? this : new UnitOfWork(_storageFactory, this);
        }


        /// <summary>
        /// Подтверждает все действия.
        /// </summary>
        public void Commit(bool isOrdered = false)
        {
            // Выполнение команд по порядку имеет наивысший приоритет
            isOrdered |= _isOrdered;

            try
            {
                // Получение всех действий
                var items = _unitOfWorkLog.Dequeue();

                if (_parentScope == null)
                {
                    // Применение действий к хранилищу
                    CommitToStorage(items, isOrdered);
                }
                else
                {
                    // Копирование действий в родительский контекст
                    CommitToParent(items, isOrdered);
                }
            }
            finally
            {
                // После завершения порядок вновь необязателен
                _isOrdered = false;
            }
        }

        /// <summary>
        /// Подтверждает все действия.
        /// </summary>
        public async Task CommitAsync(bool isOrdered = false)
        {
            // Выполнение команд по порядку имеет наивысший приоритет
            isOrdered |= _isOrdered;

            try
            {
                // Получение всех действий
                var items = _unitOfWorkLog.Dequeue();

                if (_parentScope == null)
                {
                    // Применение действий к хранилищу
                    await CommitToStorageAsync(items, isOrdered);
                }
                else
                {
                    // Копирование действий в родительский контекст
                    CommitToParent(items, isOrdered);
                }
            }
            finally
            {
                // После завершения порядок вновь необязателен
                _isOrdered = false;
            }
        }

        /// <summary>
        /// Завершает транзакцию.
        /// </summary>
        public void Dispose()
        {
            try
            {
                _unitOfWorkLog.Dequeue();

                if (_parentScope == null)
                {
                    DisposeParentScope();
                }
            }
            finally
            {
                _isOrdered = false;
            }
        }


        private int _isParentScope;

        /// <summary>
        /// Начинает работу родительского экземпляра.
        /// </summary>
        private bool BeginParentScope()
        {
            return (Interlocked.CompareExchange(ref _isParentScope, 1, 0) == 0);
        }

        /// <summary>
        /// Завершает работу родительского экземпляра.
        /// </summary>
        private void DisposeParentScope()
        {
            Interlocked.CompareExchange(ref _isParentScope, 0, 1);
        }


        /// <summary>
        /// Сохраняет накопленные изменения в родительский экземпляр.
        /// </summary>
        private void CommitToParent(IEnumerable<UnitOfWorkItem> items, bool isOrdered)
        {
            _parentScope._isOrdered |= isOrdered;
            _parentScope._unitOfWorkLog.Enqueue(items);
        }

        /// <summary>
        /// Сохраняет накопленные изменения в хранилище документов.
        /// </summary>
        private void CommitToStorage(IEnumerable<UnitOfWorkItem> items, bool isOrdered)
        {
            var actionGroups = items.GroupBy(i => new { i.Type, i.Name }, i => i.Action);

            foreach (var actionGroup in actionGroups)
            {
                var bulkExecutor = GetBulkExecutor(actionGroup.Key.Type, actionGroup.Key.Name);

                bulkExecutor.Bulk(builder =>
                                  {
                                      foreach (var action in actionGroup)
                                      {
                                          action(builder);
                                      }
                                  }, isOrdered);
            }
        }

        /// <summary>
        /// Сохраняет накопленные изменения в хранилище документов.
        /// </summary>
        private async Task CommitToStorageAsync(IEnumerable<UnitOfWorkItem> items, bool isOrdered)
        {
            var actionGroups = items.GroupBy(i => new { i.Type, i.Name }, i => i.Action);

            foreach (var actionGroup in actionGroups)
            {
                var bulkExecutor = GetBulkExecutor(actionGroup.Key.Type, actionGroup.Key.Name);

                await bulkExecutor.BulkAsync(builder =>
                                             {
                                                 foreach (var action in actionGroup)
                                                 {
                                                     action(builder);
                                                 }
                                             }, isOrdered);
            }
        }


        private IDocumentStorageBulkExecutor GetBulkExecutor(Type documentType, string documentTypeName)
        {
            var storage = (documentType == null)
                ? _storageFactory.GetStorage(documentTypeName)
                : _storageFactory.GetStorage(documentType, documentTypeName);

            return (IDocumentStorageBulkExecutor)storage;
        }


        public void InsertOne(string documentType, DynamicWrapper document)
        {
            _unitOfWorkLog.Enqueue(b => b.InsertOne(document), documentType);
        }

        public void InsertOne<TDocument>(string documentType, TDocument document) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.InsertOne(document), documentType);
        }

        public void InsertOne<TDocument>(TDocument document) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.InsertOne(document));
        }


        public void InsertMany(string documentType, IEnumerable<DynamicWrapper> documents)
        {
            foreach (var document in documents)
            {
                _unitOfWorkLog.Enqueue(b => b.InsertOne(document), documentType);
            }
        }

        public void InsertMany<TDocument>(string documentType, IEnumerable<TDocument> documents) where TDocument : Document
        {
            foreach (var document in documents)
            {
                _unitOfWorkLog.Enqueue<TDocument>(b => b.InsertOne(document), documentType);
            }
        }

        public void InsertMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : Document
        {
            foreach (var document in documents)
            {
                _unitOfWorkLog.Enqueue<TDocument>(b => b.InsertOne(document));
            }
        }


        public void UpdateOne(string documentType, Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _unitOfWorkLog.Enqueue(b => b.UpdateOne(update, filter, insertIfNotExists), documentType);
        }

        public void UpdateOne<TDocument>(string documentType, Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.UpdateOne(update, filter, insertIfNotExists), documentType);
        }

        public void UpdateOne<TDocument>(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.UpdateOne(update, filter, insertIfNotExists));
        }


        public void UpdateMany(string documentType, Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _unitOfWorkLog.Enqueue(b => b.UpdateMany(update, filter, insertIfNotExists), documentType);
        }

        public void UpdateMany<TDocument>(string documentType, Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.UpdateMany(update, filter, insertIfNotExists), documentType);
        }

        public void UpdateMany<TDocument>(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.UpdateMany(update, filter, insertIfNotExists));
        }


        public void ReplaceOne(string documentType, DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _unitOfWorkLog.Enqueue(b => b.ReplaceOne(replacement, filter, insertIfNotExists), documentType);
        }

        public void ReplaceOne<TDocument>(string documentType, TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.ReplaceOne(replacement, filter, insertIfNotExists), documentType);
        }

        public void ReplaceOne<TDocument>(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.ReplaceOne(replacement, filter, insertIfNotExists));
        }


        public void DeleteOne(string documentType, Func<IDocumentFilterBuilder, object> filter = null)
        {
            _unitOfWorkLog.Enqueue(b => b.DeleteOne(filter), documentType);
        }

        public void DeleteOne<TDocument>(string documentType, Expression<Func<TDocument, bool>> filter = null) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.DeleteOne(filter), documentType);
        }

        public void DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter = null) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.DeleteOne(filter));
        }


        public void DeleteMany(string documentType, Func<IDocumentFilterBuilder, object> filter = null)
        {
            _unitOfWorkLog.Enqueue(b => b.DeleteMany(filter), documentType);
        }

        public void DeleteMany<TDocument>(string documentType, Expression<Func<TDocument, bool>> filter = null) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.DeleteMany(filter), documentType);
        }

        public void DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter = null) where TDocument : Document
        {
            _unitOfWorkLog.Enqueue<TDocument>(b => b.DeleteMany(filter));
        }
    }
}