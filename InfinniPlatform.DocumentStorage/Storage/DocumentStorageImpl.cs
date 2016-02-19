using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Interceptors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal sealed class DocumentStorageImpl : IDocumentStorage
    {
        public DocumentStorageImpl(string documentType,
                                   Func<string, IDocumentStorageProvider> storageProviderFactory,
                                   IDocumentStorageIdProvider storageIdProvider,
                                   IDocumentStorageHeaderProvider storageHeaderProvider,
                                   IDocumentStorageFilterProvider storageFilterProvider,
                                   IDocumentStorageInterceptorProvider storageInterceptorProvider)
        {
            _storageProvider = new Lazy<IDocumentStorageProvider>(() => storageProviderFactory(documentType));
            _storageIdProvider = storageIdProvider;
            _storageHeaderProvider = storageHeaderProvider;
            _storageFilterProvider = storageFilterProvider;
            _storageInterceptor = storageInterceptorProvider.GetInterceptor(documentType);
        }


        private readonly Lazy<IDocumentStorageProvider> _storageProvider;
        private readonly IDocumentStorageIdProvider _storageIdProvider;
        private readonly IDocumentStorageHeaderProvider _storageHeaderProvider;
        private readonly IDocumentStorageFilterProvider _storageFilterProvider;
        private readonly IDocumentStorageInterceptor _storageInterceptor;


        public long Count(Func<IDocumentFilterBuilder, object> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Count(filter);
        }

        public Task<long> CountAsync(Func<IDocumentFilterBuilder, object> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.CountAsync(filter);
        }


        public IDocumentCursor<TProperty> Distinct<TProperty>(string property, Func<IDocumentFilterBuilder, object> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Distinct<TProperty>(property, filter);
        }

        public Task<IDocumentCursor<TProperty>> DistinctAsync<TProperty>(string property, Func<IDocumentFilterBuilder, object> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.DistinctAsync<TProperty>(property, filter);
        }


        public IDocumentFindCursor Find(Func<IDocumentFilterBuilder, object> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Find(filter);
        }

        public IDocumentFindCursor FindText(string search, string language = null, bool caseSensitive = false, bool diacriticSensitive = false, Func<IDocumentFilterBuilder, object> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.FindText(search, language, caseSensitive, diacriticSensitive, filter);
        }


        public IDocumentAggregateCursor Aggregate(Func<IDocumentFilterBuilder, object> filter = null)
        {
            filter = _storageFilterProvider.AddSystemFilter(filter);
            return _storageProvider.Value.Aggregate(filter);
        }


        public void InsertOne(DynamicWrapper document)
        {
            _storageInterceptor.ExecuteCommand(
                new DocumentInsertOneCommand(document),
                command =>
                {
                    _storageIdProvider.SetDocumentId(command.Document);
                    _storageHeaderProvider.SetInsertHeader(command.Document);

                    _storageProvider.Value.InsertOne(command.Document);
                },
                command => _storageInterceptor.OnBeforeInsertOne(command),
                (command, result, error) => _storageInterceptor.OnAfterInsertOne(command, result, error));
        }

        public Task InsertOneAsync(DynamicWrapper document)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentInsertOneCommand(document),
                command =>
                {
                    _storageIdProvider.SetDocumentId(command.Document);
                    _storageHeaderProvider.SetInsertHeader(command.Document);

                    return _storageProvider.Value.InsertOneAsync(command.Document);
                },
                command => _storageInterceptor.OnBeforeInsertOne(command),
                (command, result, error) => _storageInterceptor.OnAfterInsertOne(command, result, error));
        }


        public void InsertMany(IEnumerable<DynamicWrapper> documents)
        {
            _storageInterceptor.ExecuteCommand(
                new DocumentInsertManyCommand(documents),
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

        public Task InsertManyAsync(IEnumerable<DynamicWrapper> documents)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentInsertManyCommand(documents),
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


        public DocumentUpdateResult UpdateOne(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentUpdateOneCommand(update, filter, insertIfNotExists),
                command =>
                {
                    command.Update = _storageHeaderProvider.SetUpdateHeader(command.Update);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.UpdateOne(command.Update, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeUpdateOne(command),
                (command, result, error) => _storageInterceptor.OnAfterUpdateOne(command, result, error));
        }

        public Task<DocumentUpdateResult> UpdateOneAsync(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentUpdateOneCommand(update, filter, insertIfNotExists),
                command =>
                {
                    command.Update = _storageHeaderProvider.SetUpdateHeader(command.Update);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.UpdateOneAsync(command.Update, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeUpdateOne(command),
                (command, result, error) => _storageInterceptor.OnAfterUpdateOne(command, result, error));
        }


        public DocumentUpdateResult UpdateMany(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentUpdateManyCommand(update, filter, insertIfNotExists),
                command =>
                {
                    command.Update = _storageHeaderProvider.SetUpdateHeader(command.Update);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.UpdateMany(command.Update, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeUpdateMany(command),
                (command, result, error) => _storageInterceptor.OnAfterUpdateMany(command, result, error));
        }

        public Task<DocumentUpdateResult> UpdateManyAsync(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentUpdateManyCommand(update, filter, insertIfNotExists),
                command =>
                {
                    command.Update = _storageHeaderProvider.SetUpdateHeader(command.Update);
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    return _storageProvider.Value.UpdateManyAsync(command.Update, command.Filter, command.InsertIfNotExists);
                },
                command => _storageInterceptor.OnBeforeUpdateMany(command),
                (command, result, error) => _storageInterceptor.OnAfterUpdateMany(command, result, error));
        }


        public DocumentUpdateResult ReplaceOne(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentReplaceOneCommand(replacement, filter, insertIfNotExists),
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

        public Task<DocumentUpdateResult> ReplaceOneAsync(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentReplaceOneCommand(replacement, filter, insertIfNotExists),
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


        public long DeleteOne(Func<IDocumentFilterBuilder, object> filter = null)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentDeleteOneCommand(filter),
                command =>
                {
                    var delete = _storageHeaderProvider.SetDeleteHeader();
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    var result = _storageProvider.Value.UpdateOne(delete, command.Filter);

                    return result.ModifiedCount;
                },
                command => _storageInterceptor.OnBeforeDeleteOne(command),
                (command, result, error) => _storageInterceptor.OnAfterDeleteOne(command, result, error));
        }

        public Task<long> DeleteOneAsync(Func<IDocumentFilterBuilder, object> filter = null)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentDeleteOneCommand(filter),
                async command =>
                      {
                          var delete = _storageHeaderProvider.SetDeleteHeader();
                          command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                          var result = await _storageProvider.Value.UpdateOneAsync(delete, command.Filter);

                          return result.ModifiedCount;
                      },
                command => _storageInterceptor.OnBeforeDeleteOne(command),
                (command, result, error) => _storageInterceptor.OnAfterDeleteOne(command, result, error));
        }


        public long DeleteMany(Func<IDocumentFilterBuilder, object> filter = null)
        {
            return _storageInterceptor.ExecuteCommand(
                new DocumentDeleteManyCommand(filter),
                command =>
                {
                    var delete = _storageHeaderProvider.SetDeleteHeader();
                    command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                    var result = _storageProvider.Value.UpdateMany(delete, command.Filter);

                    return result.ModifiedCount;
                },
                command => _storageInterceptor.OnBeforeDeleteMany(command),
                (command, result, error) => _storageInterceptor.OnAfterDeleteMany(command, result, error));
        }

        public Task<long> DeleteManyAsync(Func<IDocumentFilterBuilder, object> filter = null)
        {
            return _storageInterceptor.ExecuteCommandAsync(
                new DocumentDeleteManyCommand(filter),
                async command =>
                      {
                          var delete = _storageHeaderProvider.SetDeleteHeader();
                          command.Filter = _storageFilterProvider.AddSystemFilter(command.Filter);

                          var result = await _storageProvider.Value.UpdateManyAsync(delete, command.Filter);

                          return result.ModifiedCount;
                      },
                command => _storageInterceptor.OnBeforeDeleteMany(command),
                (command, result, error) => _storageInterceptor.OnAfterDeleteMany(command, result, error));
        }


        public DocumentBulkResult Bulk(Action<IDocumentBulkBuilder> requests, bool isOrdered = false)
        {
            var bulkInterceptor = new DocumentStorageBulkBuilderInterceptor(this, requests, isOrdered);

            return _storageInterceptor.ExecuteCommand(
                bulkInterceptor.CreateBulkCommand(),
                command => _storageProvider.Value.Bulk(bulkInterceptor.AddBulkCommands, command.IsOrdered),
                command => _storageInterceptor.OnBeforeBulk(command),
                (command, result, error) => _storageInterceptor.OnAfterBulk(command, result, error));
        }

        public Task<DocumentBulkResult> BulkAsync(Action<IDocumentBulkBuilder> requests, bool isOrdered = false)
        {
            var bulkInterceptor = new DocumentStorageBulkBuilderInterceptor(this, requests, isOrdered);

            return _storageInterceptor.ExecuteCommandAsync(
                bulkInterceptor.CreateBulkCommand(),
                command => _storageProvider.Value.BulkAsync(bulkInterceptor.AddBulkCommands, command.IsOrdered),
                command => _storageInterceptor.OnBeforeBulk(command),
                (command, result, error) => _storageInterceptor.OnAfterBulk(command, result, error));
        }


        private sealed class DocumentStorageBulkBuilderInterceptor : IDocumentBulkBuilder
        {

            public DocumentStorageBulkBuilderInterceptor(DocumentStorageImpl storage, Action<IDocumentBulkBuilder> requests, bool isOrdered = false)
            {
                _storage = storage;
                _requests = requests;
                _isOrdered = isOrdered;
            }


            private readonly DocumentStorageImpl _storage;
            private readonly Action<IDocumentBulkBuilder> _requests;
            private readonly bool _isOrdered;


            private readonly Dictionary<IDocumentWriteCommand, Action<IDocumentBulkBuilder>> _commands
                = new Dictionary<IDocumentWriteCommand, Action<IDocumentBulkBuilder>>();


            IDocumentBulkBuilder IDocumentBulkBuilder.InsertOne(DynamicWrapper document)
            {
                var command = new DocumentInsertOneCommand(document);

                _commands.Add(command, bulk =>
                                       {
                                           _storage._storageIdProvider.SetDocumentId(command.Document);
                                           _storage._storageHeaderProvider.SetInsertHeader(command.Document);

                                           bulk.InsertOne(command.Document);
                                       });

                return this;
            }

            IDocumentBulkBuilder IDocumentBulkBuilder.UpdateOne(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter, bool insertIfNotExists)
            {
                var command = new DocumentUpdateOneCommand(update, filter, insertIfNotExists);

                _commands.Add(command, bulk =>
                                       {
                                           command.Update = _storage._storageHeaderProvider.SetUpdateHeader(command.Update);
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.UpdateOne(command.Update, command.Filter, command.InsertIfNotExists);
                                       });

                return this;
            }

            IDocumentBulkBuilder IDocumentBulkBuilder.UpdateMany(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter, bool insertIfNotExists)
            {
                var command = new DocumentUpdateManyCommand(update, filter, insertIfNotExists);

                _commands.Add(command, bulk =>
                                       {
                                           command.Update = _storage._storageHeaderProvider.SetUpdateHeader(command.Update);
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.UpdateMany(command.Update, command.Filter, command.InsertIfNotExists);
                                       });

                return this;
            }

            IDocumentBulkBuilder IDocumentBulkBuilder.ReplaceOne(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter, bool insertIfNotExists)
            {
                var command = new DocumentReplaceOneCommand(replacement, filter, insertIfNotExists);

                _commands.Add(command, bulk =>
                                       {
                                           _storage._storageIdProvider.SetDocumentId(command.Replacement);
                                           _storage._storageHeaderProvider.SetReplaceHeader(command.Replacement);
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.ReplaceOne(command.Replacement, command.Filter, command.InsertIfNotExists);
                                       });

                return this;
            }

            IDocumentBulkBuilder IDocumentBulkBuilder.DeleteOne(Func<IDocumentFilterBuilder, object> filter)
            {
                var command = new DocumentDeleteOneCommand(filter);

                _commands.Add(command, bulk =>
                                       {
                                           var delete = _storage._storageHeaderProvider.SetDeleteHeader();
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.UpdateOne(delete, command.Filter);
                                       });

                return this;
            }

            IDocumentBulkBuilder IDocumentBulkBuilder.DeleteMany(Func<IDocumentFilterBuilder, object> filter)
            {
                var command = new DocumentDeleteManyCommand(filter);

                _commands.Add(command, bulk =>
                                       {
                                           var delete = _storage._storageHeaderProvider.SetDeleteHeader();
                                           command.Filter = _storage._storageFilterProvider.AddSystemFilter(command.Filter);

                                           bulk.UpdateMany(delete, command.Filter);
                                       });

                return this;
            }


            public DocumentBulkCommand CreateBulkCommand()
            {
                _requests?.Invoke(this);

                return new DocumentBulkCommand(_commands.Keys, _isOrdered);
            }


            public void AddBulkCommands(IDocumentBulkBuilder bulk)
            {
                foreach (var addCommand in _commands.Values)
                {
                    addCommand(bulk);
                }
            }
        }
    }
}