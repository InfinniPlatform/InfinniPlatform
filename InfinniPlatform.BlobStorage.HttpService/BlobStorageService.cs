using System;
using InfinniPlatform.BlobStorage.Properties;
using InfinniPlatform.Http;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorage _blobStorage;
        private readonly ILogger _logger;

        public BlobStorageService(IBlobStorage blobStorage, ILogger<BlobStorageService> logger)
        {
            _blobStorage = blobStorage;
            _logger = logger;
        }

        public StreamHttpResponse GetFileResponse(string blobId)
        {
            try
            {
                var blobData = _blobStorage.GetBlobData(blobId);

                if (blobData != null)
                {
                    var fileResponse = new StreamHttpResponse(blobData.Data, blobData.Info.Type)
                    {
                        FileName = blobData.Info.Name,
                        LastWriteTimeUtc = blobData.Info.Time,
                        ContentLength = blobData.Info.Size
                    };

                    fileResponse.SetContentDispositionAttachment();

                    return fileResponse;
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(Resources.RequestProcessedWithException, e);
                throw;
            }
        }
    }
}
