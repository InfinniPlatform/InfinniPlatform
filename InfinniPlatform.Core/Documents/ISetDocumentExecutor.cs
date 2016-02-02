using System.Collections.Generic;

namespace InfinniPlatform.Core.Documents
{
    public interface ISetDocumentExecutor
    {
        DocumentExecutorResult SaveDocument(string documentType, object documentInstance);

        DocumentExecutorResult SaveDocuments(string documentType, IEnumerable<object> documentInstances);

        DocumentExecutorResult DeleteDocument(string documentType, object documentId);

        DocumentExecutorResult DeleteDocuments(string documentType, IEnumerable<object> documentIds);
    }
}