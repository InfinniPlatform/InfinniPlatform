using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;

using Nest;

namespace InfinniPlatform.SystemConfig.Transactions
{
    /// <summary>
    /// Предоставляет методы управления транзакцией.
    /// </summary>
    [LoggerName("TransactionScope")]
    internal sealed class DocumentTransactionScope : IDocumentTransactionScope
    {
        public DocumentTransactionScope(ITenantProvider tenantProvider, ElasticConnection elasticConnection, ElasticTypeManager elasticTypeManager, IPerformanceLog performanceLog)
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


        private bool _needRefresh = true;


        public IEnumerable<object> GetDocuments(string documentType, IEnumerable<object> documents)
        {
            foreach (dynamic document in documents)
            {
                object actualDocument = document;

                object documentId = document.Id;

                if (documentId != null)
                {
                    var transactionCommand = _transactionLog.GetEntry(documentType, documentId);

                    if (transactionCommand != null)
                    {
                        if (transactionCommand.Action == DocumentTransactionAction.Save)
                        {
                            actualDocument = transactionCommand.Document;
                        }
                        else if (transactionCommand.Action == DocumentTransactionAction.Delete)
                        {
                            actualDocument = null;
                        }
                    }
                }

                if (actualDocument != null)
                {
                    yield return actualDocument;
                }
            }
        }

        public void SaveDocument(string documentType, object documentId, object document)
        {
            var saveCommand = DocumentTransactionCommand.SaveCommand(documentType, documentId, document);

            _transactionLog.EnqueueEntry(saveCommand);
        }

        public void DeleteDocument(string documentType, object documentId)
        {
            var deleteCommand = DocumentTransactionCommand.DeleteCommand(documentType, documentId);

            _transactionLog.EnqueueEntry(deleteCommand);
        }

        public void Synchronous(bool value = true)
        {
            _needRefresh = value;
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

                        var saveResponse = _elasticConnection.Bulk(d =>
                                                                   {
                                                                       foreach (var command in saveCommands)
                                                                       {
                                                                           var indexTypeName = _elasticTypeManager.GetActualTypeName(command.DocumentType);

                                                                           var indexObjectId = CreateIndexObjectId(command.DocumentId);
                                                                           var indexObject = CreateIndexObject(tenantId, indexObjectId, command.Document);

                                                                           d.Index<IndexObject>(i => i.Type(indexTypeName)
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

                        var indexObjectIds = deleteCommands.Select(i => CreateIndexObjectId(i.DocumentId));

                        var searchResponse = _elasticConnection.Search<IndexObject>(d => d.AllTypes()
                                                                                          .Filter(f => f.Terms(i => i.Id, indexObjectIds) && f.Term(i => i.TenantId, tenantId))
                                                                                          .Size(deleteCommands.Length)
                                                                                          .Source(false));

                        CheckDatabaseResponse(searchResponse);

                        if (searchResponse.Total > 0 && searchResponse.Hits != null)
                        {
                            var deleteResponse = _elasticConnection.Bulk(d =>
                                                                         {
                                                                             foreach (var hit in searchResponse.Hits)
                                                                             {
                                                                                 d.Delete<IndexObject>(i => i.Type(hit.Type)
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

                    _performanceLog.Log("Complete", startTime);
                }
                catch (Exception exception)
                {
                    _performanceLog.Log("Complete", startTime, exception);

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

        private static void CheckDatabaseResponse(IResponse response)
        {
            if (!response.IsValid)
            {
                string elasticMessage = null;

                if (response.ConnectionStatus != null && response.ConnectionStatus.OriginalException != null)
                {
                    elasticMessage = response.ConnectionStatus.OriginalException.Message;
                }
                else
                {
                    var balkResponse = response as IBulkResponse;

                    if (balkResponse != null && balkResponse.ItemsWithErrors != null)
                    {
                        var errorItem = balkResponse.ItemsWithErrors.FirstOrDefault();

                        if (errorItem != null)
                        {
                            elasticMessage = errorItem.Error;
                        }
                    }
                }

                throw new InvalidOperationException($"Cannot complete transaction. {elasticMessage}");
            }
        }
    }
}