using System;

using InfinniPlatform.Sdk.BlobStorage;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Реализует REST-сервис для FileApi.
    /// </summary>
    internal sealed class FileApiHttpService : IHttpService
    {
        public FileApiHttpService(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        private readonly IBlobStorage _blobStorage;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/RestfulApi/UrlEncodedData/configuration/DownloadBinaryContent"] = DownloadFile;
        }

        private object DownloadFile(IHttpRequest request)
        {
            string formString = request.Query.Form;

            dynamic form = null;

            if (!string.IsNullOrWhiteSpace(formString))
            {
                formString = Uri.UnescapeDataString(formString);
                form = JsonObjectSerializer.Default.Deserialize(formString);
            }

            if (form != null)
            {
                string contentId = form.ContentId;

                if (!string.IsNullOrEmpty(contentId))
                {
                    var blobData = _blobStorage.GetBlobData(contentId);

                    if (blobData != null)
                    {
                        return new StreamHttpResponse(blobData.Data, blobData.Info?.Type);
                    }
                }
            }

            return null;
        }
    }
}