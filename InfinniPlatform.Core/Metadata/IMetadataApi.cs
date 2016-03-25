using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Metadata
{
    public interface IMetadataApi
    {
        IEnumerable<string> GetMetadataItemNames(string partOfFullName);

        dynamic GetMetadata(string metadataName);

        dynamic GetDocumentSchema(string documentName);

        dynamic GetDocumentEvents(string documentName);

        IEnumerable<object> GetDocumentIndexes(string documentName);
    }
}