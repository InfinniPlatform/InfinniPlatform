using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.DocumentStorage.Contract.Interceptors;

namespace InfinniPlatform.DocumentStorage.Storage
{
    [DebuggerDisplay("DocumentType = {" + nameof(DocumentType) + "}")]
    internal class DocumentStorageImpl<TDocument> : IDocumentStorage<TDocument>, IDocumentStorageBulkExecutor where TDocument : Document
    {
        public DocumentStorageImpl(IDocumentStorageProviderFactory storageProviderFactory,
                                   IDocumentStorageIdProvider storageIdProvider,
                                   IDocumentStorageHeaderProvider storageHeaderProvider,
                                   IDocumentStorageFilterProvider storageFilterProvider,
                                   IDocumentStorageInterceptorProvider storageInterceptorProvider,
                                   string documentType = null)
        {
            if (string.IsNullOrEmpty(documentType))
            {
                documentType = DocumentStorageExtensions.GetDefaultDocumentTypeName<TDocument>();
            }

            DocumentType = documentType;

            _storageProvider = new Lazy<IDocumentStorageProvider<TDocument>>(() => storageProviderFactory.GetStorageProvider<TDocument>(documentType));
            _storageIdProvider = storageIdProvider;
            _storageHeaderProvider = storageHeaderProvider;
            _storageFilterProvider = storageFilterProvider;
            _storageInterceptor = storageInterceptorProvider.GetInterceptor<TDocument>(documentType);
        }


        private readonly Lazy<IDocumentStorageProvider<TDocument>> _storageProvider;
        private readonly IDocumentStorageIdProvider _storageIdProvider;
        private readonly IDocumentStorageHeaderProvider _storageHeaderProvider;
        private readonly IDocumentStorageFilterProvider _storageFilterProvider;
        private readonly IDocumentStorageInterceptor<TDocument> _storageInterceptor;


        public string DocumentType { get; }


        public long Count(Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Count(filter);
        }

        public Task<long> CountAsync(Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.CountAsync(filter);
        }


        public IDocumentCursor<TProperty> Distinct<TProperty>(Expression<Func<TDocument, TProperty>> property, Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Distinct(property, filter);
        }

        public Task<IDocumentCursor<TProperty>> DistinctAsync<TProperty>(Expression<Func<TDocument, TProperty>> property, Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.DistinctAsync(property, filter);
        }


        public IDocumentCursor<TItem> Distinct<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Distinct(arrayProperty, filter);
        }

        public Task<IDocumentCursor<TItem>> DistinctAsync<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.DistinctAsync(arrayProperty, filter);
        }


        public IDocumentFindCursor<TDocument, TDocument> Find(Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Find(filter);
        }

        public IDocumentFindCursor<TDocument, TDocument> FindText(string search, string language = null, bool caseSensitive = false, bool diacriticSensitive = false, Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.FindText(search, language, caseSensitive, diacriticSensitive, filter);
        }


        public IDocumentAggregateCursor<TDocument> Aggregate(Expression<Func<TDocument, bool>> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Aggregate(filter);
        }


        public void InsertOne(TDocument document)
        {
            _storageInterceptor.ExecuteCommand(
                new DocumentInsertOneCommand<TDocument>(document),
                command =>
                {
                    _storageIdProvider.SetDocumentId(command.Document);
                    _storageHeaderProvider.SetInsertHeader(command.Document);

                    _storageProvider.Value.InsertOne(command.Document);
                },
                command => _storageInterceptor.OnBeforeInsertOne(command),
                (command, result, error) => _storageInterceptor.OnAfterInsertOne(command, result, error));
        }

        public Task InsertOneAsync(TDocument document)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentInsertOneCommand<TDocument>(document),
                command =>
                {
                    _storageIdProvider.SetDocumentId(command.Document);
                    _storageHeaderProvider.SetInsertHeader(command.Document);

                    return _storageProvider.Value.InsertOneAsync(command.Document);
                },
                command => _storageInterceptor.OnBeforeInsertOne(command),
                (command, result, error) => _storageInterceptor.OnAfterInsertOne(command, result, error));
        }


        public void InsertMany(IEnumerable<TDocument> documents)
        {
            _storageInterceptor.ExecuteCommand(
                new DocumentInsertManyCommand<TDocument>(documents),
                command =>
                {
                    foreach (var document in command.Documents)
                    {
                        _storageIdProvider.SetDocumentId(document);
                        _storageHeaderProvider.SetInsertHeader(document);
                    }

                    _storageProvider.Value.InsertMany(command.Documents);
                },
                command => _storageInterceptor.OnBeforeInsertMany(command),
                (command, result, error) => _storageInterceptor.OnAfterInsertMany(command, result, error));
        }

        public Task InsertManyAsync(IEnumerable<TDocument> documents)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentInsertManyCommand<TDocument>(documents),
                command =>
                {
                    foreach (var document in command.Documents)
                    {
                        _storageIdProvider.SetDocumentId(document);
                        _storageHeaderProvider.SetInsertHeader(document);
                    }

                    return _storageProvider.Value.InsertManyAsync(command.Documents);
                },
                command => _storageInterceptor.OnBeforeInsertMany(command),
                (command, result, error) => _storageInterceptor.OnAfterInsertMany(command, result, error));
        }


        public DocumentUpdateResult UpdateOne(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentUpdateOneCommand<TDocument>(update, filter, insertIfNotExists),
                command =>
                {
                    command.Update = _storageHeaderProvider.SetUpdateHeader(command.Update);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.UpdateOne(command.Update, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeUpdateOne(command),
                (command, result, error) => _storageInterceptor.OnAfterUpdateOne(command, result, error));
        }

        public Task<DocumentUpdateResult> UpdateOneAsync(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentUpdateOneCommand<TDocument>(update, filter, insertIfNotExists),
                command =>
                {
                    command.Update = _storageHeaderProvider.SetUpdateHeader(command.Update);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.UpdateOneAsync(command.Update, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeUpdateOne(command),
                (command, result, error) => _storageInterceptor.OnAfterUpdateOne(command, result, error));
        }


        public DocumentUpdateResult UpdateMany(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentUpdateManyCommand<TDocument>(update, filter, insertIfNotExists),
                command =>
                {
                    command.Update = _storageHeaderProvider.SetUpdateHeader(command.Update);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.UpdateMany(command.Update, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeUpdateMany(command),
                (command, result, error) => _storageInterceptor.OnAfterUpdateMany(command, result, error));
        }

        public Task<DocumentUpdateResult> UpdateManyAsync(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentUpdateManyCommand<TDocument>(update, filter, insertIfNotExists),
                command =>
                {
                    command.Update = _storageHeaderProvider.SetUpdateHeader(command.Update);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.UpdateManyAsync(command.Update, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeUpdateMany(command),
                (command, result, error) => _storageInterceptor.OnAfterUpdateMany(command, result, error));
        }


        public DocumentUpdateResult ReplaceOne(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentReplaceOneCommand<TDocument>(replacement, filter, insertIfNotExists),
                command =>
                {
                    _storageIdProvider.SetDocumentId(command.Replacement);
                    _storageHeaderProvider.SetReplaceHeader(command.Replacement);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.ReplaceOne(command.Replacement, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeReplaceOne(command),
                (command, result, error) => _storageInterceptor.OnAfterReplaceOne(command, result, error));
        }

        public Task<DocumentUpdateResult> ReplaceOneAsync(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentReplaceOneCommand<TDocument>(replacement, filter, insertIfNotExists),
                command =>
                {
                    _storageIdProvider.SetDocumentId(command.Replacement);
                    _storageHeaderProvider.SetReplaceHeader(command.Replacement);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.ReplaceOneAsync(command.Replacement, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeReplaceOne(command),
                (command, result, error) => _storageInterceptor.OnAfterReplaceOne(command, result, error));
        }


        public long DeleteOne(Expression<Func<TDocument, bool>> filter = null)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentDeleteOneCommand<TDocument>(filter),
                command =>
                {
                    var delete = _storageHeaderProvider.SetDeleteHeader<TDocument>();
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    var result = _storageProvider.Value.UpdateOne(delete, command.Filter);

                    return result.ModifiedCount;
                },
                command => _storageInterceptor.OnBeforeDeleteOne(command),
                (command, result, error) => _storageInterceptor.OnAfterDeleteOne(command, result, error));
        }

        public Task<long> DeleteOneAsync(Expression<Func<TDocument, bool>> filter = null)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentDeleteOneCommand<TDocument>(filter),
                async command =>
                      {
                          var delete = _storageHeaderProvider.SetDeleteHeader<TDocument>();
                          command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                          var result = await _storageProvider.Value.UpdateOneAsync(delete, command.Filter);

                          return result.ModifiedCount;
                      },
                command => _storageInterceptor.OnBeforeDeleteOne(command),
                (command, result, error) => _storageInterceptor.OnAfterDeleteOne(command, result, error));
        }


        public long DeleteMany(Expression<Func<TDocument, bool>> filter = null)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentDeleteManyCommand<TDocument>(filter),
                command =>
                {
                    var delete = _storageHeaderProvider.SetDeleteHeader<TDocument>();
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    var result = _storageProvider.Value.UpdateMany(delete, command.Filter);

                    return result.ModifiedCount;
                },
                command => _storageInterceptor.OnBeforeDeleteMany(command),
                (command, result, error) => _storageInterceptor.OnAfterDeleteMany(command, result, error));
        }

        public Task<long> DeleteManyAsync(Expression<Func<TDocument, bool>> filter = null)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentDeleteManyCommand<TDocument>(filter),
                async command =>
                      {
                          var delete = _storageHeaderProvider.SetDeleteHeader<TDocument>();
                          command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                          var result = await _storageProvider.Value.UpdateManyAsync(delete, command.Filter);

                          return result.ModifiedCount;
                      },
                command => _storageInterceptor.OnBeforeDeleteMany(command),
                (command, result, error) => _storageInterceptor.OnAfterDeleteMany(command, result, error));
        }

        public DocumentBulkResult Bulk(Action<IDocumentBulkBuilder<TDocument>> requests, bool isOrdered = false)
        {
            var bulkInterceptor = new DocumentStorageBulkBuilderInterceptor(this, requests, isOrdered);

            return _storageInterceptor.ExecuteCommand(
                bulkInterceptor.CreateBulkCommand(),
                command => _storageProvider.Value.Bulk(bulkInterceptor.AddBulkCommands, command.IsOrdered),
                command => _storageInterceptor.OnBeforeBulk(command),
                (command, result, error) => _storageInterceptor.OnAfterBulk(command, result, error));
        }

        public Task<DocumentBulkResult> BulkAsync(Action<IDocumentBulkBuilder<TDocument>> requests, bool isOrdered = false)
        {
            var bulkInterceptor = new DocumentStorageBulkBuilderInterceptor(this, requests, isOrdered);

            return _storageInterceptor.ExecuteCommandAsync(
                bulkInterceptor.CreateBulkCommand(),
                command => _storageProvider.Value.BulkAsync(bulkInterceptor.AddBulkCommands, command.IsOrdered),
                command => _storageInterceptor.OnBeforeBulk(command),
                (command, result, error) => _storageInterceptor.OnAfterBulk(command, result, error));
        }


        DocumentBulkResult IDocumentStorageBulkExecutor.Bulk(Action<object> documentBulkBuilderInitializer, bool isOrdered)
        {
            return Bulk(documentBulkBuilderInitializer, isOrdered);
        }

        Task<DocumentBulkResult> IDocumentStorageBulkExecutor.BulkAsync(Action<object> documentBulkBuilderInitializer, bool isOrdered)
        {
            return BulkAsync(documentBulkBuilderInitializer, isOrdered);
        }


        private sealed class DocumentStorageBulkBuilderInterceptor : IDocumentBulkBuilder<TDocument>
        {
            public DocumentStorageBulkBuilderInterceptor(DocumentStorageImpl<TDocument> storage, Action<IDocumentBulkBuilder<TDocument>> requests, bool isOrdered = false)
            {
                _storage = storage;
                _requests = requests;
                _isOrdered = isOrdered;
            }


            private readonly DocumentStorageImpl<TDocument> _storage;
            private readonly Action<IDocumentBulkBuilder<TDocument>> _requests;
            private readonly bool _isOrdered;


            private readonly Dictionary<IDocumentWriteCommand<TDocument>, Action<IDocumentBulkBuilder<TDocument>>> _commands
                = new Dictionary<IDocumentWriteCommand<TDocument>, Action<IDocumentBulkBuilder<TDocument>>>();


            IDocumentBulkBuilder<TDocument> IDocumentBulkBuilder<TDocument>.InsertOne(TDocument document)
            {
                var command = new DocumentInsertOneCommand<TDocument>(document);

                _commands.Add(command, bulk =>
                                       {
                                           _storage._storageIdProvider.SetDocumentId(command.Document);
                                           _storage._storageHeaderProvider.SetInsertHeader(command.Document);

                                           bulk.InsertOne(command.Document);
                                       });

                return this;
            }

            IDocumentBulkBuilder<TDocument> IDocumentBulkBuilder<TDocument>.UpdateOne(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter, bool insertIfNotExists)
            {
                var command = new DocumentUpdateOneCommand<TDocument>(update, filter, insertIfNotExists);

                _commands.Add(command, bulk =>
                                       {
                                           command.Update = _storage._storageHeaderProvider.SetUpdateHeader(command.Update);
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.UpdateOne(command.Update, command.Filter, command.InsertIfNotExists);
                                       });

                return this;
            }

            IDocumentBulkBuilder<TDocument> IDocumentBulkBuilder<TDocument>.UpdateMany(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter, bool insertIfNotExists)
            {
                var command = new DocumentUpdateManyCommand<TDocument>(update, filter, insertIfNotExists);

                _commands.Add(command, bulk =>
                                       {
                                           command.Update = _storage._storageHeaderProvider.SetUpdateHeader(command.Update);
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.UpdateMany(command.Update, command.Filter, command.InsertIfNotExists);
                                       });

                return this;
            }

            IDocumentBulkBuilder<TDocument> IDocumentBulkBuilder<TDocument>.ReplaceOne(TDocument replacement, Expression<Func<TDocument, bool>> filter, bool insertIfNotExists)
            {
                var command = new DocumentReplaceOneCommand<TDocument>(replacement, filter, insertIfNotExists);

                _commands.Add(command, bulk =>
                                       {
                                           _storage._storageIdProvider.SetDocumentId(command.Replacement);
                                           _storage._storageHeaderProvider.SetReplaceHeader(command.Replacement);
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.ReplaceOne(command.Replacement, command.Filter, command.InsertIfNotExists);
                                       });

                return this;
            }

            IDocumentBulkBuilder<TDocument> IDocumentBulkBuilder<TDocument>.DeleteOne(Expression<Func<TDocument, bool>> filter)
            {
                var command = new DocumentDeleteOneCommand<TDocument>(filter);

                _commands.Add(command, bulk =>
                                       {
                                           var delete = _storage._storageHeaderProvider.SetDeleteHeader<TDocument>();
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.UpdateOne(delete, command.Filter);
                                       });

                return this;
            }

            IDocumentBulkBuilder<TDocument> IDocumentBulkBuilder<TDocument>.DeleteMany(Expression<Func<TDocument, bool>> filter)
            {
                var command = new DocumentDeleteManyCommand<TDocument>(filter);

                _commands.Add(command, bulk =>
                                       {
                                           var delete = _storage._storageHeaderProvider.SetDeleteHeader<TDocument>();
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.UpdateMany(delete, command.Filter);
                                       });

                return this;
            }


            public DocumentBulkCommand<TDocument> CreateBulkCommand()
            {
                _requests?.Invoke(this);

                return new DocumentBulkCommand<TDocument>(_commands.Keys, _isOrdered);
            }


            public void AddBulkCommands(IDocumentBulkBuilder<TDocument> bulk)
            {
                foreach (var addCommand in _commands.Values)
                {
                    addCommand(bulk);
                }
            }
        }
    }
}