using System;
using System.Threading.Tasks;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Bulk document storage operations executor.
    /// </summary>
    public interface IDocumentStorageBulkExecutor
    {
        /// <summary>
        /// Executes bulk operation.
        /// </summary>
        /// <param name="documentBulkBuilderInitializer">Action that contains bulk operations.</param>
        /// <param name="isOrdered">Flag indicating if execution operations should preserve order.</param>
        DocumentBulkResult Bulk(Action<object> documentBulkBuilderInitializer, bool isOrdered = false);

        /// <summary>
        /// Executes bulk operation.
        /// </summary>
        /// <param name="documentBulkBuilderInitializer">Action that contains bulk operations.</param>
        /// <param name="isOrdered">Flag indicating if execution operations should preserve order.</param>
        Task<DocumentBulkResult> BulkAsync(Action<object> documentBulkBuilderInitializer, bool isOrdered = false);
    }
}