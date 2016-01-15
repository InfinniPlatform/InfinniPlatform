using System;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// REST-клиент для BlobStorageApi.
    /// </summary>
    public sealed class InfinniFileApi : BaseApi
    {
        public InfinniFileApi(string server, int port) : base(server, port)
        {
        }

        public dynamic DownloadFile(string contentId)
        {
            var requestUri = BuildRequestUri("/SystemConfig/UrlEncodedData/configuration/DownloadBinaryContent");

            var requestData = new DynamicWrapper
                              {
                                  ["ContentId"] = contentId
                              };

            var pathArguments = $"/?Form={Uri.EscapeDataString(JsonObjectSerializer.Default.ConvertToString(requestData))}";

            return RequestExecutor.GetDownload(requestUri + pathArguments);
        }
    }
}