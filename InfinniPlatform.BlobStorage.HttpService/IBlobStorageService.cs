using InfinniPlatform.Http;

namespace InfinniPlatform.BlobStorage
{
    public interface IBlobStorageService
    {
        StreamHttpResponse GetFileResponse(string blobId);
    }
}
