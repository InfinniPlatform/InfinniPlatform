using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class PrintViewApi : IPrintViewApi
    {
        public dynamic GetPrintView(string configId, string documentId, string printViewId, string printViewType, int pageNumber,
            int pageSize, Action<FilterBuilder> filter)
        {
            return new DataApi.PrintViewApi().GetPrintView(configId, documentId, printViewId, printViewType, pageNumber,
                pageSize, filter);
        }
    }
}
