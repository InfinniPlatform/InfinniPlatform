using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.RestApi.DataApi
{
    public interface IGetDocumentExecutor
    {
        dynamic GetDocument(string id);

        int GetNumberOfDocuments(string configurationName, string documentType, dynamic filter);

        int GetNumberOfDocuments(string configurationName, string documentType, Action<FilterBuilder> filter);
        
        IEnumerable<object> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null);

        IEnumerable<object> GetDocument(string configurationName, string documentType, IEnumerable<FilterBuilder.CriteriaBuilder.CriteriaFilter> filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, IEnumerable<CriteriaSorting> sorting = null);

        IEnumerable<object> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, IEnumerable<object> ignoreResolve, Action<SortingBuilder> sorting = null);
    }
}