using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.RestApi.DataApi
{
    public interface IGetDocumentExecutor
    {
        dynamic GetDocument(string id);

        int GetNumberOfDocuments(string configurationName, string documentType, IEnumerable<FilterCriteria> filter);

        int GetNumberOfDocuments(string configurationName, string documentType, Action<FilterBuilder> filter);

        IEnumerable<object> GetDocument(string configurationName, string documentType, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null, IEnumerable<dynamic> ignoreResolve = null);

        IEnumerable<object> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null, IEnumerable<object> ignoreResolve = null);
    }
}