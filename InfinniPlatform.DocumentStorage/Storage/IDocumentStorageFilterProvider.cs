using System;
using System.Linq.Expressions;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal interface IDocumentStorageFilterProvider
    {
        Func<IDocumentFilterBuilder, object> AddSystemFilter(Func<IDocumentFilterBuilder, object> filter);

        Expression<Func<TDocument, bool>> AddSystemFilter<TDocument>(Expression<Func<TDocument, bool>> filter) where TDocument : Document;
    }
}