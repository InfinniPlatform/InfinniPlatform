using System;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal interface IDocumentStorageHeaderProvider
    {
        void SetInsertHeader(DynamicWrapper document);

        void SetReplaceHeader(DynamicWrapper document);

        Action<IDocumentUpdateBuilder> SetUpdateHeader(Action<IDocumentUpdateBuilder> update);

        Action<IDocumentUpdateBuilder> SetDeleteHeader();
    }
}