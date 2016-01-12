using System;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;

using Nest;

namespace InfinniPlatform.SystemConfig.Executors
{
    /// <summary>
    /// Предоставляет методы управления транзакцией.
    /// </summary>
    internal sealed class ElasticDocumentTransactionScope : IDocumentTransactionScope
    {
        private const string PerformanceLogComponent = "TransactionScope";
        private const string PerformanceLogComplete = "Complete";


        public ElasticDocumentTransactionScope(ITenantProvider tenantProvider, ElasticConnection elasticConnection, ElasticTypeManager elasticTypeManager, IPerformanceLog performanceLog)
        {
            _tenantProvider = tenantProvider;
            _elasticConnection = elasticConnection;
            _elasticTypeManager = elasticTypeManager;
            _performanceLog = performanceLog;
            _transactionLog = new DocumentTransactionLog();
        }


        private readonly ITenantProvider _tenantProvider;
        private readonly ElasticConnection _elasticConnection;
        private readonly ElasticTypeManager _elasticTypeManager;
        private readonly IPerformanceLog _performanceLog;
        private readonly DocumentTransactionLog _transactionLog;


        private bool _needRefresh;


        public void SaveDocument(string configuration, string documentType, object documentId, object document)
        {
            var saveCommand = DocumentTransactionCommand.SaveCommand(configuration, documentType, documentId, document);

            _transactionLog.EnqueueEntry(saveCommand);
        }

        public void DeleteDocument(string configuration, string documentType, object documentId)
        {
            var deleteCommand = DocumentTransactionCommand.DeleteCommand(configuration, documentType, documentId);

            _transactionLog.EnqueueEntry(deleteCommand);
        }

        public void Synchronous()
        {
            _needRefresh = true;
        }


        public void Complete()
        {
            var transactionEntries = _transactionLog.DequeueEntries();

            if (transactionEntries.Length > 0)
            {
                var startTime = DateTime.Now;

                try
                {
                    var tenantId = _tenantProvider.GetTenantId();

                    var saveCommands = transactionEntries
                        .Where(i => i.Action == DocumentTransactionAction.Save)
                        .ToArray();

                    var deleteCommands = transactionEntries
                        .Where(i => i.Action == DocumentTransactionAction.Delete)
                        .ToArray();

                    if (saveCommands.Length > 0)
                    {
                        // Сохранение документов можно сделать за один запрос, так как имя типа для каждого документа известно

                        var saveResponse = _elasticConnection
                            .Client.Bulk(d =>
                                         {
                                             foreach (var command in saveCommands)
                                             {
                                                 var indexName = _elasticConnection.GetIndexName(command.Configuration);
                                                 var indexTypeName = _elasticTypeManager.GetActualTypeName(command.Configuration, command.DocumentType);

                                                 var indexObjectId = CreateIndexObjectId(command.DocumentId);
                                                 var indexObject = CreateIndexObject(tenantId, indexObjectId, command.Document);

                                                 d.Index<IndexObject>(i => i.Index(indexName)
                                                                            .Type(indexTypeName)
                                                                            .Document(indexObject));
                                             }

                                             if (_needRefresh)
                                             {
                                                 d.Refresh();
                                             }

                                             return d;
                                         });

                        CheckDatabaseResponse(saveResponse);
                    }

                    if (deleteCommands.Length > 0)
                    {
                        // Удаление документов вынуждено делается за два этапа: определение типа удаляемого документа, затем удаление

                        var indexNames = deleteCommands.Select(i => _elasticConnection.GetIndexName(i.Configuration));
                        var indexObjectIds = deleteCommands.Select(i => CreateIndexObjectId(i.DocumentId));

                        var searchResponse = _elasticConnection
                            .Client.Search<IndexObject>(d => d.Indices(indexNames)
                                                              .AllTypes()
                                                              .Filter(f => f.Terms(i => i.Id, indexObjectIds) && f.Term(i => i.TenantId, tenantId))
                                                              .Size(deleteCommands.Length)
                                                              .Source(false));

                        CheckDatabaseResponse(searchResponse);

                        if (searchResponse.Total > 0 && searchResponse.Hits != null)
                        {
                            var deleteResponse = _elasticConnection
                                .Client.Bulk(d =>
                                             {
                                                 foreach (var hit in searchResponse.Hits)
                                                 {
                                                     d.Delete<IndexObject>(i => i.Index(hit.Index)
                                                                                 .Type(hit.Type)
                                                                                 .Id(hit.Id));
                                                 }

                                                 if (_needRefresh)
                                                 {
                                                     d.Refresh();
                                                 }

                                                 return d;
                                             });

                            CheckDatabaseResponse(deleteResponse);
                        }
                    }

                    _performanceLog.Log(PerformanceLogComponent, PerformanceLogComplete, startTime, null);
                }
                catch (Exception exception)
                {
                    _performanceLog.Log(PerformanceLogComponent, PerformanceLogComplete, startTime, exception.GetMessage());

                    throw;
                }
            }
        }


        public void Rollback()
        {
            _transactionLog.DequeueEntries();
        }


        private static string CreateIndexObjectId(object documentId)
        {
            return documentId.ToString().ToLower();
        }

        private static IndexObject CreateIndexObject(string tenantId, string indexObjectId, object document)
        {
            // TODO: Нужно переработать структуру заголовка документа
            // TODO: Нужно избавиться от этой эвристики определения TenantId
            tenantId = TryGetDocumentTenantId(document) ?? tenantId;

            return new IndexObject
            {
                Id = indexObjectId,
                TenantId = tenantId,
                TimeStamp = DateTime.Now,
                Status = "valid",
                Values = document
            };
        }

        private static string TryGetDocumentTenantId(object document)
        {
            try
            {
                return ObjectHelper.GetProperty(document, "TenantId") as string;
            }
            catch
            {
                return null;
            }
        }

        private static void CheckDatabaseResponse(IResponse databaseResponse)
        {
            if (!databaseResponse.IsValid)
            {
                var elasticMessage = databaseResponse.ConnectionStatus?.OriginalException?.Message;

                throw new InvalidOperationException($"Cannot complete transaction. {elasticMessage}");
            }
        }
    }
}