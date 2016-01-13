using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Sdk.RestApi
{
    public class InfinniPrintViewApi : BaseApi, IPrintViewApi
    {
        public InfinniPrintViewApi(string server, int port) : base(server, port)
        {
            _customServiceApi = new InfinniCustomServiceApi(server, port);
        }

        private readonly InfinniCustomServiceApi _customServiceApi;

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
                                                                                                    Filter = (IEnumerable<CriteriaFilter>)filterBuilder.CriteriaList
                                                                                                }).ToDynamic();
        }
    }
}