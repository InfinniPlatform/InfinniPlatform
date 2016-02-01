using System.Collections.Generic;

namespace InfinniPlatform.Core.Documents
{
    public interface ISetDocumentExecutor
    {
        DocumentExecutorResult SaveDocument(string configuration, string documentType, object documentInstance);

        DocumentExecutorResult SaveDocuments(string configuration, string documentType, IEnumerable<object> documentInstances);

        DocumentExecutorResult DeleteDocument(string configuration, string documentType, object documentId);

        DocumentExecutorResult DeleteDocuments(string configuration, string documentType, IEnumerable<object> documentIds);
    }
}