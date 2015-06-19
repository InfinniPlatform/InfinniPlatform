using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.ContextComponents
{
    public sealed class BlobStorageComponent : IBlobStorageComponent
    {
        private readonly IBlobStorage _blobStorage;

        public BlobStorageComponent(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public IBlobStorage GetBlobStorage()
        {
            return _blobStorage;
        }
    }
}