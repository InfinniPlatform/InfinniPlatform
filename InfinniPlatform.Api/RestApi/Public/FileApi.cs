using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class FileApi : IFileApi
    {
        public dynamic UploadFile(string application, string documentType, string instanceId, string fieldName, string fileName,
            Stream fileStream)
        {
            return new UploadApi().UploadBinaryContent(application, documentType, instanceId,
                fieldName, fileName, fileStream);
        }

        public dynamic DownloadFile(string contentId)
        {
            return new UploadApi().DownloadBinaryContent(contentId);
        }
    }
}
