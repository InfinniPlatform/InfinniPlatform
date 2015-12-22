using System;
using System.Collections.Generic;

using InfinniPlatform.Api.SearchOptions.Builders;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public interface IGetDocumentExecutor
    {
        IEnumerable<object> GetDocumentByQuery(string queryText, bool denormalizeResult = false);

        dynamic GetDocument(string id);

        int GetNumberOfDocuments(string configuration, string metadata, dynamic filter);

        int GetNumberOfDocuments(string configuration, string metadata, Action<FilterBuilder> filter);

        IEnumerable<object> GetDocument(string configuration, string metadata, dynamic filter, int pageNumber, int pageSize, IEnumerable<object> ignoreResolve = null, dynamic sorting = null);

        IEnumerable<object> GetDocument(string configuration, string metadata, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null);

        IEnumerable<object> GetDocument(string configuration, string metadata, Action<FilterBuilder> filter, int pageNumber, int pageSize, IEnumerable<object> ignoreResolve, Action<SortingBuilder> sorting = null);

        IEnumerable<object> GetDocumentUnfolded(string configuration, string metadata, dynamic filter, int pageNumber, int pageSize, IEnumerable<object> ignoreResolve = null, dynamic sorting = null);
    }
}