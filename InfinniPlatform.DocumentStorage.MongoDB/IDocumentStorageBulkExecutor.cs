using System;
using System.Threading.Tasks;

namespace InfinniPlatform.DocumentStorage
{
    public interface IDocumentStorageBulkExecutor
    {
        DocumentBulkResult Bulk(Action<object> documentBulkBuilderInitializer, bool isOrdered = false);

        Task<DocumentBulkResult> BulkAsync(Action<object> documentBulkBuilderInitializer, bool isOrdered = false);
    }
}