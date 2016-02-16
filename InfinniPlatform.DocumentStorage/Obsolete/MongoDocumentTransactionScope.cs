using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.DocumentStorage.Obsolete
{
    /// <summary>
    /// Предоставляет методы управления транзакцией.
    /// </summary>
    [LoggerName("TransactionScope")]
    internal sealed class MongoDocumentTransactionScope : IDocumentTransactionScope
    {
        public MongoDocumentTransactionScope(Func<string, IDocumentStorage> documentStorageFactory, IPerformanceLog performanceLog)
        {
            _documentStorageFactory = documentStorageFactory;
            _performanceLog = performanceLog;
            _transactionLog = new DocumentTransactionLog();
        }


        private readonly Func<string, IDocumentStorage> _documentStorageFactory;
        private readonly IPerformanceLog _performanceLog;
        private readonly DocumentTransactionLog _transactionLog;


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
        }


        public void Complete()
        {
            var transactionEntries = _transactionLog.DequeueEntries();

            if (transactionEntries.Length > 0)
            {
                var startTime = DateTime.Now;

                try
                {
                    var storageCommands = transactionEntries
                        .GroupBy(i => i.DocumentType)
                        .ToArray();

                    foreach (var commands in storageCommands)
                    {
                        var documentStorage = _documentStorageFactory.Invoke(commands.Key);

                        documentStorage.Bulk(bulk =>
                                             {
                                                 foreach (var command in commands)
                                                 {
                                                     switch (command.Action)
                                                     {
                                                         case DocumentTransactionAction.Save:
                                                             bulk.ReplaceOne((DynamicWrapper)command.Document, f => f.Eq("_id", command.DocumentId), true);
                                                             break;
                                                         case DocumentTransactionAction.Delete:
                                                             bulk.DeleteOne(f => f.Eq("_id", command.DocumentId));
                                                             break;
                                                     }
                                                 }
                                             });
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
    }
}