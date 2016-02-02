using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Реализует REST-клиент для PrintViewApi.
    /// </summary>
    public class PrintViewApiClient : BaseRestClient
    {
        public PrintViewApiClient(string server, int port) : base(server, port)
        {
        }

        public Stream GetPrintView(string documentId, string printViewId, string printViewType, int pageNumber, int pageSize, Action<FilterBuilder> filter = null)
        {
            IEnumerable<FilterCriteria> filterCriterias = null;

            if (filter != null)
            {
                var filterBuilder = new FilterBuilder();
                filter(filterBuilder);

                filterCriterias = filterBuilder.CriteriaList;
            }

            var requestUri = BuildRequestUri("/SystemConfig/UrlEncodedData/reporting/GetPrintView");

            var requestData = new DynamicWrapper
                              {
                                  ["DocumentId"] = documentId,
                                  ["PrintViewId"] = printViewId,
                                  ["PrintViewType"] = printViewType,
                                  ["PageNumber"] = pageNumber,
                                  ["PageSize"] = pageSize,
                                  ["Filter"] = filterCriterias
                              };

            return RequestExecutor.PostDownload(requestUri, requestData);
        }
    }
}