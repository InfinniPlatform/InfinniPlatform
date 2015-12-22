using System.Collections.Generic;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public interface ISetDocumentExecutor
    {
        dynamic SetDocument(string configuration, string documentType, object documentInstance);

        dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documentInstances);
    }
}