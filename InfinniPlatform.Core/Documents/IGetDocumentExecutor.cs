using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.Documents
{
    public interface IGetDocumentExecutor
    {
        object GetDocumentById(string documentType, string id);

        int GetNumberOfDocuments(string documentType, IEnumerable<FilterCriteria> filter);

        int GetNumberOfDocuments(string documentType, Action<FilterBuilder> filter);

        IEnumerable<object> GetDocument(string documentName, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null, IEnumerable<dynamic> ignoreResolve = null);

        IEnumerable<object> GetDocument(string documentName, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null, IEnumerable<object> ignoreResolve = null);
    }
}