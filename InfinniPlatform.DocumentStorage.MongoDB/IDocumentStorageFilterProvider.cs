using System;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Abstractions;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal interface IDocumentStorageFilterProvider
    {
        Func<IDocumentFilterBuilder, object> AddSystemFilter(Func<IDocumentFilterBuilder, object> filter);

        Expression<Func<TDocument, bool>> AddSystemFilter<TDocument>(Expression<Func<TDocument, bool>> filter) where TDocument : Document;
    }
}