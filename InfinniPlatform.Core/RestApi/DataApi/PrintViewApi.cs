using System;

using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.RestApi;

namespace InfinniPlatform.Core.RestApi.DataApi
{
    public sealed class PrintViewApi : IPrintViewApi
    {
        public PrintViewApi(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public dynamic GetPrintView(string configId, string documentId, string printViewId, string printViewType,
                                    int pageNumber, int pageSize, Action<FilterBuilder> filter)
        {
            var filterBuilder = new FilterBuilder();

            return _restQueryApi.QueryPostJsonRaw("SystemConfig", "Reporting", "GetPrintView", null, new
                                                                                                     {
                                                                                                         ConfigId = configId,
                                                                                                         DocumentId = documentId,
                                                                                                         PrintViewId = printViewId,
                                                                                                         PrintViewType = printViewType,
                                                                                                         PageNumber = pageNumber,
                                                                                                         PageSize = pageSize,
                                                                                                         Filter = filterBuilder.GetFilter()
                                                                                                     });
        }
    }
}