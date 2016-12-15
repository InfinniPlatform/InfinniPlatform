using System;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Contract;

namespace InfinniPlatform.DocumentStorage.Storage
{
    public interface IDocumentStorageBulkExecutor
    {
        DocumentBulkResult Bulk(Action<object> documentBulkBuilderInitializer, bool isOrdered = false);

        Task<DocumentBulkResult> BulkAsync(Action<object> documentBulkBuilderInitializer, bool isOrdered = false);
    }
}