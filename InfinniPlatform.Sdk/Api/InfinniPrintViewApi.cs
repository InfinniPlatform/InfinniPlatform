using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Api
{
    public class InfinniPrintViewApi : BaseApi, IPrintViewApi
    {
        private readonly InfinniCustomServiceApi _customServiceApi;

        public InfinniPrintViewApi(string server, string port, string route) : base(server, port, route)
        {
            _customServiceApi = new InfinniCustomServiceApi(server, port, route);
        }


        public dynamic GetPrintView(string configId, string documentId, string printViewId, string printViewType, int pageNumber,
            int pageSize, Action<FilterBuilder> filter)
        {
            var filterBuilder = new FilterBuilder();

            return _customServiceApi.ExecuteAction("SystemConfig", "Reporting", "GetPrintView", new
            {
                ConfigId = configId,
                DocumentId = documentId,
                PrintViewId = printViewId,
                PrintViewType = printViewType,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Filter = filterBuilder.GetFilter()
            }).ToDynamic();
        }
    }
}
