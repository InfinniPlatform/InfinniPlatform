using System;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Реализует REST-клиент для FileApi.
    /// </summary>
    public sealed class FileApiClient : BaseRestClient
    {
        public FileApiClient(string server, int port, IJsonObjectSerializer serializer = null) : base(server, port, serializer)
        {
        }

        public dynamic DownloadFile(string contentId)
        {
            var requestUri = BuildRequestUri("/RestfulApi/UrlEncodedData/configuration/DownloadBinaryContent");

            var requestData = new DynamicWrapper
                              {
                                  ["ContentId"] = contentId
                              };

            var pathArguments = $"/?Form={Uri.EscapeDataString(Serializer.ConvertToString(requestData))}";

            return RequestExecutor.GetDownload(requestUri + pathArguments);
        }
    }
}