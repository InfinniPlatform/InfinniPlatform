using System;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal interface IDocumentStorageFilterProvider
    {
        Func<IDocumentFilterBuilder, object> AddSystemFilter(Func<IDocumentFilterBuilder, object> filter);
    }
}