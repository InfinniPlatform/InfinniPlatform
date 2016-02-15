using System;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal class DocumentStorageIdProvider : IDocumentStorageIdProvider
    {
        public void SetDocumentId(DynamicWrapper document)
        {
            if (document["_id"] == null)
            {
                document["_id"] = Guid.NewGuid().ToString("N");
            }
        }
    }
}