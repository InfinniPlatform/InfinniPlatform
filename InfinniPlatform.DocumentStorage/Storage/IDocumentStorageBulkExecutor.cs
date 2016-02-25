using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal interface IDocumentStorageBulkExecutor
    {
        DocumentBulkResult Bulk(Action<object> documentBulkBuilderInitializer, bool isOrdered = false);

        Task<DocumentBulkResult> BulkAsync(Action<object> documentBulkBuilderInitializer, bool isOrdered = false);
    }
}