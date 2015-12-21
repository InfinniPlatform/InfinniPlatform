using System.Collections.Generic;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public interface ISetDocumentExecutor
    {
        dynamic SetDocument(string configuration, string documentType, dynamic documentInstance, bool ignoreWarnings = false, bool allowNonSchemaProperties = false);

        dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documentInstances, int batchSize = 200, bool allowNonSchemaProperties = false);
    }
}