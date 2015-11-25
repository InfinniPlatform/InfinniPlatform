using System;

using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class PrintViewApi : IPrintViewApi
    {
        public PrintViewApi()
        {
            _printViewApi = new DataApi.PrintViewApi();
        }


        private readonly DataApi.PrintViewApi _printViewApi;


        public dynamic GetPrintView(string configId, string documentId, string printViewId, string printViewType, int pageNumber, int pageSize, Action<FilterBuilder> filter)
        {
            return _printViewApi.GetPrintView(configId, documentId, printViewId, printViewType, pageNumber, pageSize, filter);
        }
    }
}