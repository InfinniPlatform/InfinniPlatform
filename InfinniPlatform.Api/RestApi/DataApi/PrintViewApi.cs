using System;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class PrintViewApi : IPrintViewApi
    {
        public dynamic GetPrintView(string configId, string documentId, string printViewId, string printViewType,
            int pageNumber, int pageSize, Action<Sdk.FilterBuilder> filter)
        {
            var filterBuilder = new FilterBuilder();

            return RestQueryApi.QueryPostJsonRaw("SystemConfig", "Reporting", "GetPrintView", null, new
                {
                    ConfigId = configId,
                    DocumentId = documentId,
                    PrintViewId = printViewId,
                    PrintViewType = printViewType,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Filter = filterBuilder.GetFilter()
                }).Content;
        }
    }
}