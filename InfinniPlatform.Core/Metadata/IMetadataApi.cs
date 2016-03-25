using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Metadata
{
    public interface IMetadataApi
    {
        IEnumerable<string> GetNames(string startsWithMask);

        dynamic GetMetadata(string metadataName);


        IEnumerable<string> GetDocumentNames();

        dynamic GetDocumentSchema(string documentName);

        dynamic GetDocumentEvents(string documentName);

        IEnumerable<object> GetDocumentIndexes(string documentName);
    }
}