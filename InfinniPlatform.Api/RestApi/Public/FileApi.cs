using System.IO;

using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class FileApi : IFileApi
    {
        public FileApi(DataApi.UploadApi uploadApi)
        {
            _uploadApi = uploadApi;
        }


        private readonly DataApi.UploadApi _uploadApi;


        public dynamic UploadFile(string application,
                                  string documentType,
                                  string documentId,
                                  string fieldName,
                                  string fileName,
                                  Stream fileStream)
        {
            return _uploadApi.UploadBinaryContent(application, documentType, documentId, fieldName, fileName, fileStream);
        }

        public dynamic DownloadFile(string contentId)
        {
            return _uploadApi.DownloadBinaryContent(contentId);
        }
    }
}