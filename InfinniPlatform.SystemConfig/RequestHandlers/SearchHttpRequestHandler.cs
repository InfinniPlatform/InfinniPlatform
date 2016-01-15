using System;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class SearchHttpRequestHandler : SimpleHttpRequestHandler
    {
        public SearchHttpRequestHandler(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        protected override object ActionResult(IHttpRequest request)
        {
            string configuration = request.Parameters.Configuration;
            string documentType = request.Parameters.DocumentType;

            string filterString = request.Query.Filter;
            int pageNumber = request.Query.PageNumber;
            int pageSize = request.Query.PageSize;

            FilterCriteria[] filter = null;

            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = Uri.UnescapeDataString(filterString);
                filter = JsonObjectSerializer.Default.Deserialize<FilterCriteria[]>(filterString);
            }

            pageNumber = Math.Max(pageNumber, 0);
            pageSize = Math.Min(pageSize, 1000);

            return (pageSize > 0) ? _documentApi.GetDocuments(configuration, documentType, filter, pageNumber, pageSize) : null;
        }
    }
}